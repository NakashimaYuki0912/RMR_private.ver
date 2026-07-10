using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using abcdcode_LOGLIKE_MOD;
using GameSave;
using HarmonyLib;
using LOR_DiceSystem;
using LOR_XML;
using UnityEngine;

namespace RogueLike_Mod_Reborn
{
    public static class RMRAbnormalityUnlockManager
    {
        public const int AbnormalityBattleRewardCount = 3;
        public const int MysteryRewardCount = 1;
        public const float UrbanLegendNormalAbnormalityRewardChance = 0.5f;
        public const float UrbanPlagueNormalAbnormalityRewardChance = 0.5f;

        public const int BlackForestMysteryId = 991001;
        public const int WellMysteryId = 991002;
        public const int HandsOfLightMysteryId = 991003;

        private const string ProgressSaveName = "RMR_AbnormalityProgress";
        private const string RealizationSaveName = "RMR_FloorRealizations";
        private const string RedMistVictorySaveName = "RMR_RedMistChallengeCleared";
        private const int NoAbnormalityFallbackBaseId = 15999000;
        private const int RedMistStageId = 60020;
        private const int RedMistCorePageId = 250022;
        private const int BlackSilenceStageId = 70020;
        private const int DistortedEnsembleStageId = 70021;
        private const int DistortedEnsembleLastStageId = 70021;

        private static readonly int[] RedMistBattlePageIds =
        {
            607003,
            607004,
            607005,
            607006,
            607007
        };

        private static readonly int[] BlueReverberationBattlePageIds =
        {
            704001,
            704011,
            704012,
            704013,
            704014,
            705010,
            705011
        };

        private static readonly List<LorId> RouteUnlockedPages = new List<LorId>();
        private static readonly HashSet<LorId> RouteUnlockedEgoPages = new HashSet<LorId>();
        private static readonly HashSet<int> PermanentlyUnlockedTiers = new HashSet<int>();
        private static readonly HashSet<SephirahType> CompletedRealizations = new HashSet<SephirahType>();
        private static bool BinahUnlockedForCurrentRoute;
        private static bool RedMistVictoryRewardsGrantedThisBattle;
        private static bool BlackSilenceClearRecordedThisBattle;
        private static bool BlueReverberationRewardsGrantedThisBattle;

        // Floor → all abnormality script roots on that floor
        // Sourced from vanilla EmotionCard_*.txt <Sephirah> tags
        public static readonly Dictionary<SephirahType, string[]> FloorAbnormalityScripts = new Dictionary<SephirahType, string[]>
        {
            { SephirahType.Malkuth, new[] { "ScorchedGirl", "HappyTeddyBear", "FairyCarnival", "QueenBee", "snowwhite" } },
            { SephirahType.Yesod, new[] { "ForsakenMurderer", "LittleHelper", "SingingMachine", "Butterfly", "freischutz" } },
            { SephirahType.Hod, new[] { "ShyLookToday", "RedShoes", "SpiderBud", "Laetitia", "blackswan" } },
            { SephirahType.Netzach, new[] { "UniverseZogak", "ChildofGalaxy", "Porccubus", "Alriune", "orchestra" } },
            { SephirahType.Tiphereth, new[] { "QueenOfHatred", "KnightOfDespair", "Greed", "Angry", "clownofnihil" } },
            { SephirahType.Gebura, new[] { "Redhood", "BigBadWolf", "Mountain", "Nosferatu", "nothing" } },
            { SephirahType.Chesed, new[] { "ScareCrow", "LumberJack", "House", "Ozma", "wizard" } },
            { SephirahType.Binah, new[] { "Bigbird", "SmallBird", "LongBird", "bossbird" } },
            { SephirahType.Hokma, new[] { "Bloodytree", "Clock", "BlueStar", "onebadmanygood", "plaguedoctor", "whitenight" } },
            { SephirahType.Keter, new[] { "BloodBath", "HeartofAspiration", "Pinocchio", "TheSnowQueen", "quietKid" } },
        };

        // Final realization rewards are not granted directly. Completing a floor opens
        // these entries to the shop and reward rolls for future picks.
        public static readonly Dictionary<SephirahType, string[]> RealizationRewardScriptsByFloor = new Dictionary<SephirahType, string[]>
        {
            { SephirahType.Malkuth, new[] { "snowwhite" } },
            { SephirahType.Yesod, new[] { "SingingMachine1", "Butterfly3", "freischutz3" } },
            { SephirahType.Hod, new[] { "blackswan" } },
            { SephirahType.Netzach, new[] { "orchestra" } },
            { SephirahType.Tiphereth, new[] { "clownofnihil" } },
            { SephirahType.Gebura, new[] { "nothing" } },
            { SephirahType.Chesed, new[] { "wizard" } },
            { SephirahType.Binah, new[] { "bossbird" } },
            { SephirahType.Hokma, new[] { "whitenight", "plaguedoctor", "onebadmanygood" } },
            { SephirahType.Keter, new[] { "quietKid" } },
        };

        // Boss clear rewards use the completed floor's full vanilla abnormality pool.
        // This is intentionally wider than RealizationRewardScriptsByFloor: Binah, for
        // example, must be able to roll Bigbird/SmallBird/LongBird pages after her
        // realization, not only the final bossbird entries.
        private static readonly Dictionary<SephirahType, string[]> BossRealizationRewardScriptsByFloor = FloorAbnormalityScripts;

        public static readonly HashSet<string> RealizationExclusiveScripts = new HashSet<string>(
            RealizationRewardScriptsByFloor.SelectMany(x => x.Value), StringComparer.OrdinalIgnoreCase);

        public static readonly Dictionary<SephirahType, LorId[]> RealizationEgoCardsByFloor = new Dictionary<SephirahType, LorId[]>
        {
            { SephirahType.Malkuth, new[] { new LorId(910001), new LorId(910002), new LorId(910003), new LorId(910004), new LorId(910005) } },
            { SephirahType.Yesod, new[] { new LorId(910011), new LorId(910012), new LorId(910013), new LorId(910014), new LorId(910015) } },
            { SephirahType.Hod, new[] { new LorId(910016), new LorId(910017), new LorId(910018), new LorId(910019), new LorId(910020) } },
            { SephirahType.Netzach, new[] { new LorId(910021), new LorId(910022), new LorId(910023), new LorId(910024), new LorId(910025) } },
            { SephirahType.Tiphereth, new[] { new LorId(910026), new LorId(910027), new LorId(910028), new LorId(910029), new LorId(910030) } },
            { SephirahType.Gebura, new[] { new LorId(910031), new LorId(910032), new LorId(910033), new LorId(910034), new LorId(910035) } },
            { SephirahType.Chesed, new[] { new LorId(910036), new LorId(910037), new LorId(910038), new LorId(910039), new LorId(910040) } },
            { SephirahType.Binah, new[] { new LorId(910041), new LorId(910042), new LorId(910043), new LorId(910044), new LorId(910045) } },
            { SephirahType.Hokma, new[] { new LorId(910046), new LorId(910047), new LorId(910048), new LorId(910049), new LorId(910050) } },
            { SephirahType.Keter, new[] { new LorId(910086), new LorId(910087), new LorId(910088), new LorId(910089), new LorId(910090) } },
        };
        private static readonly string[] SimpleRoots =
        {
            "ScorchedGirl", "HappyTeddyBear", "FairyCarnival", "QueenBee",
            "ForsakenMurderer", "LittleHelper", "SingingMachine", "Butterfly",
            "ShyLookToday", "RedShoes", "SpiderBud", "Laetitia",
            "UniverseZogak", "ChildofGalaxy", "Porccubus", "Alriune"
        };

        private static readonly string[] MediumRoots =
        {
            "QueenOfHatred", "KnightOfDespair", "Greed", "Angry",
            "Redhood", "BigBadWolf", "Mountain", "Nosferatu",
            "ScareCrow", "LumberJack", "House", "Ozma"
        };

        private static readonly string[] HardRoots =
        {
            "BloodBath", "HeartofAspiration", "Pinocchio", "TheSnowQueen",
            "Bigbird", "SmallBird", "LongBird",
            "Bloodytree", "Clock", "BlueStar"
        };

        public static void StartNewRoute(ChapterGrade grade)
        {
            RouteUnlockedPages.Clear();
            RouteUnlockedEgoPages.Clear();
            BinahUnlockedForCurrentRoute = false;
            LoadPermanentProgress();
            LoadRealizationProgress();
            foreach (RewardPassiveInfo info in GetPermanentStartingPages())
                UnlockPage(info.id);
        }

        public static void ResetArchiveProgress()
        {
            RouteUnlockedPages.Clear();
            RouteUnlockedEgoPages.Clear();
            BinahUnlockedForCurrentRoute = false;
            PermanentlyUnlockedTiers.Clear();
            DeleteSaveFile(ProgressSaveName);
        }

        public static void ResetAllPermanentProgress()
        {
            RouteUnlockedPages.Clear();
            RouteUnlockedEgoPages.Clear();
            BinahUnlockedForCurrentRoute = false;
            PermanentlyUnlockedTiers.Clear();
            CompletedRealizations.Clear();
            ResetRedMistChallengeBattleState();
            DeleteSaveFile(ProgressSaveName);
            DeleteSaveFile(RealizationSaveName);
            DeleteSaveFile(RedMistVictorySaveName);
        }

        public static SaveData SaveRouteUnlocks()
        {
            SaveData data = new SaveData(SaveDataType.Dictionary);
            SaveData pages = new SaveData();
            foreach (LorId id in RouteUnlockedPages)
                pages.AddToList(id.LogGetSaveData());
            data.AddData("Pages", pages);
            SaveData egoPages = new SaveData();
            foreach (LorId id in RouteUnlockedEgoPages)
                egoPages.AddToList(id.LogGetSaveData());
            data.AddData("EgoPages", egoPages);
            data.AddData("BinahUnlocked", BinahUnlockedForCurrentRoute ? 1 : 0);
            return data;
        }

        public static void LoadRouteUnlocks(SaveData data)
        {
            RouteUnlockedPages.Clear();
            RouteUnlockedEgoPages.Clear();
            BinahUnlockedForCurrentRoute = false;
            LoadPermanentProgress();
            LoadRealizationProgress();
            if (data == null)
            {
                foreach (RewardPassiveInfo info in GetPermanentStartingPages())
                    UnlockPage(info.id);
                return;
            }
            SaveData pages = data.GetData("Pages");
            if (pages == null)
                pages = data;
            else
                BinahUnlockedForCurrentRoute = data.GetInt("BinahUnlocked") > 0;
            foreach (SaveData item in pages)
            {
                LorId id = ExtensionUtils.LogLoadFromSaveData(item);
                UnlockPage(id);
            }
            SaveData egoPages = data.GetData("EgoPages");
            if (egoPages != null)
            {
                foreach (SaveData item in egoPages)
                    UnlockEgoForCurrentRoute(ExtensionUtils.LogLoadFromSaveData(item));
            }
        }

        public static List<RewardPassiveInfo> GetUnlockedEmotionCardsForBattle()
        {
            // Realization: permanent atlas pool (normal + exclusive pages already recorded).
            if (RMRRealizationManager.InRealizationBattle
                || RMRRealizationManager.IsRealizationPreparationActive)
            {
                LogueBookModels.EnsureAtlasUnlocks();
                return LogueBookModels.AtlasUnlockedAbnormalityPages
                    .Select(id => Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id))
                    .Where(info => info != null && info.rewardtype == RewardType.Creature && !IsNoAbnormalityFallback(info.id))
                    .ToList();
            }

            return RouteUnlockedPages
                .Select(id => Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id))
                .Where(info => info != null && info.rewardtype == RewardType.Creature && !IsNoAbnormalityFallback(info.id))
                .ToList();
        }

        public static void EnqueueBattleClearRewards()
        {
            // Realization battles have their own reward path — skip normal Roguelike reward chains
            if (RMRRealizationManager.InRealizationBattle)
                return;

            if (IsRedMistChallengeStage())
                return;

            if (LogLikeMod.curstagetype == abcdcode_LOGLIKE_MOD.StageType.Creature)
            {
                EnqueueRewardSelections(AbnormalityBattleRewardCount, LogLikeMod.curchaptergrade);
                return;
            }

            if (LogLikeMod.curstagetype == abcdcode_LOGLIKE_MOD.StageType.Normal)
            {
                if (ShouldEnqueueNormalAbnormalityReward(LogLikeMod.curchaptergrade))
                    EnqueueRewardSelections(1, LogLikeMod.curchaptergrade);
                return;
            }

            if (LogLikeMod.curstagetype == abcdcode_LOGLIKE_MOD.StageType.Elite)
                EnqueueRewardSelections(1, LogLikeMod.curchaptergrade);

            // Boss stages: add realization-tier abnormality + EGO rewards based on chapter grade
            if (LogLikeMod.curstagetype == abcdcode_LOGLIKE_MOD.StageType.Boss)
            {
                EnqueueBossRealizationTierRewards(LogLikeMod.curchaptergrade);
            }
        }

        public static bool IsBinahUnlockedForCurrentRoute()
        {
            return BinahUnlockedForCurrentRoute;
        }

        public static void UnlockBinahForCurrentRoute()
        {
            BinahUnlockedForCurrentRoute = true;
        }

        public static void ResetRedMistChallengeBattleState()
        {
            RedMistVictoryRewardsGrantedThisBattle = false;
            BlackSilenceClearRecordedThisBattle = false;
            BlueReverberationRewardsGrantedThisBattle = false;
        }

        private static bool IsRedMistChallengeStage()
        {
            return LogLikeMod.curstageid == new LorId(LogLikeMod.ModId, RedMistStageId);
        }

        private static bool IsBlackSilenceStage()
        {
            return LogLikeMod.curstageid == new LorId(LogLikeMod.ModId, BlackSilenceStageId);
        }

        private static bool IsDistortedEnsembleStage()
        {
            return LogLikeMod.curstageid != null
                && LogLikeMod.curstageid.packageId == LogLikeMod.ModId
                && LogLikeMod.curstageid.id >= DistortedEnsembleStageId
                && LogLikeMod.curstageid.id <= DistortedEnsembleLastStageId;
        }

        private static bool IsCurrentBattleVictory()
        {
            return BattleObjectManager.instance.GetAliveListWithAvailable(Faction.Player).Count > 0
                && BattleObjectManager.instance.GetAliveListWithAvailable(Faction.Enemy).Count == 0;
        }

        public static void SuppressRedMistChallengeGenericRewards()
        {
            if (!IsRedMistChallengeStage())
                return;
            LogLikeMod.rewards?.Clear();
            LogLikeMod.rewards_lastKill?.Clear();
            LogLikeMod.rewards_passive?.Clear();
            LogLikeMod.rewards_InStage?.Clear();
            LogLikeMod.rewardsMystery?.Clear();
            LogLikeMod.egoSelectionQueue?.Clear();
        }

        public static bool IsRedMistChallengeVictoryRecorded()
        {
            try
            {
                SaveData data = Singleton<LogueSaveManager>.Instance.LoadData(RedMistVictorySaveName);
                return data != null && data.GetInt("Cleared") > 0;
            }
            catch
            {
                return false;
            }
        }

        private static void RecordRedMistChallengeVictory()
        {
            try
            {
                SaveData data = new SaveData(SaveDataType.Dictionary);
                data.AddData("Cleared", new SaveData(1));
                Singleton<LogueSaveManager>.Instance.SaveData(data, RedMistVictorySaveName);
            }
            catch (Exception e)
            {
                Debug.LogError($"[RMRAbnormalityUnlockManager] Failed to save Red Mist challenge clear flag: {e.Message}");
            }
        }

        public static void PrunePrematureRedMistChallengeRewards()
        {
            if (IsRedMistChallengeVictoryRecorded())
                return;

            bool changed = false;
            LorId redMistBookId = new LorId(LogLikeMod.ModId, RedMistCorePageId);
            ResetPrematureRedMistLoadouts(redMistBookId);

            if (LogueBookModels.booklist != null)
                changed |= LogueBookModels.booklist.RemoveAll(book => book?.ClassInfo?.id == redMistBookId) > 0;
            if (LogueBookModels.cardlist != null)
            {
                HashSet<LorId> redMistCards = GetRedMistBattleCardLorIds();
                changed |= LogueBookModels.cardlist.RemoveAll(card => card != null && redMistCards.Contains(card.GetID())) > 0;
            }

            LogueBookModels.EnsureAtlasUnlocks();
            changed |= LogueBookModels.AtlasUnlockedRoleBooks.Remove(redMistBookId);
            foreach (LorId cardId in GetRedMistBattleCardLorIds())
                changed |= LogueBookModels.AtlasUnlockedBattleCards.Remove(cardId);

            if (!changed)
                return;

            LogueBookModels.SavePermanentAtlasUnlocks();
            Debug.Log("[RMRAbnormalityUnlockManager] Removed premature Red Mist core/battle pages; rewards require clearing the Red Mist challenge.");
        }

        private static HashSet<LorId> GetRedMistBattleCardLorIds()
        {
            return new HashSet<LorId>(RedMistBattlePageIds.Select(id => new LorId(LogLikeMod.ModId, id)));
        }

        private static void ResetPrematureRedMistLoadouts(LorId redMistBookId)
        {
            if (LogueBookModels.playerBattleModel == null)
                return;
            for (int index = 0; index < LogueBookModels.playerBattleModel.Count; index++)
            {
                UnitBattleDataModel battleModel = LogueBookModels.playerBattleModel[index];
                if (battleModel?.unitData?.bookItem?.ClassInfo?.id != redMistBookId)
                    continue;
                BookXmlInfo defaultPage = Singleton<BookXmlList>.Instance.GetData(
                    new LorId(LogLikeMod.ModId, -854 - index));
                if (defaultPage != null)
                    LogueBookModels.EquipNewPage(battleModel, defaultPage, false);
            }
        }

        public static void GrantRedMistChallengeVictoryRewards()
        {
            if (RedMistVictoryRewardsGrantedThisBattle || !IsRedMistChallengeStage())
                return;
            bool victory = BattleObjectManager.instance.GetAliveListWithAvailable(Faction.Player).Count > 0
                && BattleObjectManager.instance.GetAliveListWithAvailable(Faction.Enemy).Count == 0;
            if (!victory)
                return;

            RedMistVictoryRewardsGrantedThisBattle = true;
            RecordRedMistChallengeVictory();
            SuppressRedMistChallengeGenericRewards();
            LorId redMistBookId = new LorId(LogLikeMod.ModId, RedMistCorePageId);
            bool corePageAdded = LogueBookModels.TryAddUniqueRoleBookToInventoryAndAtlas(redMistBookId);
            foreach (int pageId in RedMistBattlePageIds)
                LogueBookModels.AddCard(new LorId(LogLikeMod.ModId, pageId), 1, false);
            RMRCore.UnlockBinahAfterRedMistVictory();

            Debug.Log($"[RMRAbnormalityUnlockManager] Red Mist challenge victory: core page {redMistBookId} " +
                      $"{(corePageAdded ? "added" : "already owned")}; battle pages " +
                      $"{string.Join(", ", RedMistBattlePageIds.Select(x => x.ToString()).ToArray())} unlocked.");
        }

        public static void RecordBlackSilenceVictoryUnlock()
        {
            if (BlackSilenceClearRecordedThisBattle || !IsBlackSilenceStage())
                return;
            if (!IsCurrentBattleVictory())
                return;

            BlackSilenceClearRecordedThisBattle = true;
            RMRCore.RecordBlackSilenceStageClear();
            Debug.Log("[RMRAbnormalityUnlockManager] Black Silence clear recorded; Black Silence core page will be granted on Urban Star entry.");
        }

        public static void GrantDistortedEnsembleVictoryRewards()
        {
            if (BlueReverberationRewardsGrantedThisBattle || !IsDistortedEnsembleStage())
                return;
            if (!IsCurrentBattleVictory())
                return;

            BlueReverberationRewardsGrantedThisBattle = true;
            RMRCore.RecordDistortedEnsembleStageClear();
            LorId blueBookId = RMRCore.GetBlueReverberationCorePageLorId();
            bool corePageAdded = RMRCore.PruneLegacyBlueReverberationCorePageUnlocks();
            corePageAdded |= LogueBookModels.TryAddUniqueRoleBookToInventoryAndAtlas(blueBookId);
            foreach (int pageId in BlueReverberationBattlePageIds)
                LogueBookModels.AddCard(new LorId(pageId), 1, false);
            LogueBookModels.SavePermanentAtlasUnlocks();

            Debug.Log($"[RMRAbnormalityUnlockManager] Distorted Ensemble victory: core page {blueBookId} " +
                      $"{(corePageAdded ? "added" : "already owned")}; battle pages " +
                      $"{string.Join(", ", BlueReverberationBattlePageIds.Select(x => x.ToString()).ToArray())} unlocked; future Urban Star routes will grant these rewards automatically.");
        }

        /// <summary>
        /// Enqueues realization-victory-tier abnormality page and EGO card rewards
        /// for BOSS stage completions. The count and candidate floors follow the
        /// route grade bands, then filter down to completed realizations.
        /// If the player has not completed any matching floor realization, no realization reward
        /// is queued so the reward UI cannot open an empty selection.
        /// </summary>
        private static void EnqueueBossRealizationTierRewards(ChapterGrade grade)
        {
            int abnoCount;
            int egoCount;

            if (grade <= ChapterGrade.Grade3)
            {
                abnoCount = 1;
                egoCount = 1;
            }
            else if (grade <= ChapterGrade.Grade5)
            {
                abnoCount = 2;
                egoCount = 2;
            }
            else // Grade6+
            {
                abnoCount = 3;
                egoCount = 3;
            }

            LoadRealizationProgress();
            PruneUnselectedRealizationPagesFromRoute();
            HashSet<SephirahType> completedFloors = GetCompletedRealizationFloorsForBossTier(grade);
            if (completedFloors.Count == 0)
            {
                Debug.Log($"[RMRAbnormalityUnlockManager] BOSS realization tier rewards skipped: grade={grade}, no completed floors in tier {GetTierForChapter(grade)}");
                return;
            }

            // Enqueue abnormality page rewards (completed realization-exclusive only)
            for (int i = 0; i < abnoCount; i++)
            {
                var abnoRewards = RollRealizationAbnormalityChoices(completedFloors, 3);
                if (abnoRewards != null && abnoRewards.Count > 0)
                    LogLikeMod.rewards_passive.Add(new RewardInfo { grade = grade, rewards = abnoRewards });
                else
                    Debug.LogWarning($"[RMRAbnormalityUnlockManager] BOSS realization abno reward #{i + 1} has no candidates for grade {grade}");
            }

            // Enqueue EGO card rewards as card reward selections
            for (int i = 0; i < egoCount; i++)
            {
                EnqueueRealizationEgoSelection(completedFloors);
            }

            Debug.Log($"[RMRAbnormalityUnlockManager] Enqueued BOSS realization tier rewards: grade={grade}, abno={abnoCount}, ego={egoCount}, floors=[{string.Join(",", completedFloors)}]");
        }

        private static HashSet<SephirahType> GetCompletedRealizationFloorsForBossTier(ChapterGrade grade)
        {
            HashSet<SephirahType> floors = GetBossRealizationRewardFloorsForChapter(grade);
            floors.IntersectWith(CompletedRealizations);
            return floors;
        }

        private static HashSet<SephirahType> GetBossRealizationRewardFloorsForChapter(ChapterGrade grade)
        {
            if (grade <= ChapterGrade.Grade3)
                return new HashSet<SephirahType> { SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Hod, SephirahType.Netzach };
            if (grade <= ChapterGrade.Grade5)
                return new HashSet<SephirahType> { SephirahType.Tiphereth, SephirahType.Gebura, SephirahType.Chesed };
            return new HashSet<SephirahType> { SephirahType.Binah, SephirahType.Hokma, SephirahType.Chesed };
        }

        /// <summary>
        /// Rolls a set of realization-exclusive abnormality page reward candidates from the given floors.
        /// The caller passes only completed realization floors.
        /// Filters already-unlocked pages in the current route.
        /// </summary>
        private static List<RewardPassiveInfo> RollRealizationAbnormalityChoices(HashSet<SephirahType> floors, int count)
        {
            var candidates = new List<RewardPassiveInfo>();
            foreach (var info in GetAllCreatureRewardPages())
            {
                if (info == null || info.rewardtype != RewardType.Creature)
                    continue;
                if (IsNoAbnormalityFallback(info.id))
                    continue;
                // Boss rewards roll from the full pool of completed realization floors.
                SephirahType floor = GetBossRealizationRewardFloorForScript(info.script);
                if (floor == SephirahType.None || !floors.Contains(floor))
                    continue;
                // Skip already-unlocked in current route
                if (RouteUnlockedPages.Exists(id => id == info.id))
                    continue;
                if (LogueBookModels.IsAtlasAbnormalityPageUnlocked(info.id))
                    continue;
                // Skip already in EmotionCardList
                if (LogueBookModels.EmotionCardList != null && LogueBookModels.EmotionCardList.Any(x => x.id == info.id))
                    continue;
                if (LogLikeMod.rewards_passive != null && LogLikeMod.rewards_passive.Any(reward => reward?.rewards != null && reward.rewards.Any(queued => queued.id == info.id)))
                    continue;
                candidates.Add(info);
            }

            // Shuffle and pick
            var result = new List<RewardPassiveInfo>();
            while (candidates.Count > 0 && result.Count < count)
            {
                int index = UnityEngine.Random.Range(0, candidates.Count);
                result.Add(candidates[index]);
                candidates.RemoveAt(index);
            }
            return result;
        }

        /// <summary>
        /// Enqueues EGO card selection choices into the independent EGO queue.
        /// Each selection shows up to 3 unowned EGO card candidates from the specified floors.
        /// The queue is processed in StartPickReward before normal card drops.
        /// If no unowned candidates exist, safely skips without blocking.
        /// </summary>
        private static void EnqueueRealizationEgoSelection(HashSet<SephirahType> floors)
        {
            var candidates = new List<DiceCardXmlInfo>();
            foreach (var floor in floors)
            {
                if (!RealizationEgoCardsByFloor.TryGetValue(floor, out LorId[] egoIds))
                    continue;
                foreach (LorId id in egoIds)
                {
                    DiceCardXmlInfo card = ItemXmlDataList.instance.GetCardItem(id, true);
                    if (card == null)
                        continue;
                    if (IsEgoUnlockedForCurrentRoute(id))
                        continue;
                    if (LogueBookModels.IsAtlasEgoPageUnlocked(id))
                        continue;
                    if (LogLikeMod.egoSelectionQueue != null && LogLikeMod.egoSelectionQueue.Any(choice => choice != null && choice.Contains(id)))
                        continue;
                    if (!candidates.Exists(c => c.id == card.id))
                        candidates.Add(card);
                }
            }

            if (candidates.Count == 0)
            {
                Debug.LogWarning($"[RMRAbnormalityUnlockManager] EnqueueRealizationEgoSelection: no unowned EGO cards in floors [{string.Join(",", floors)}]");
                return;
            }

            // Pick up to 3 candidates for the selection UI
            int pickCount = Math.Min(3, candidates.Count);
            var choiceSet = new List<LorId>();
            for (int i = 0; i < pickCount; i++)
            {
                int index = UnityEngine.Random.Range(0, candidates.Count);
                choiceSet.Add(candidates[index].id);
                candidates.RemoveAt(index);
            }

            // Add to independent EGO selection queue
            if (LogLikeMod.egoSelectionQueue == null)
                LogLikeMod.egoSelectionQueue = new List<List<LorId>>();
            LogLikeMod.egoSelectionQueue.Add(choiceSet);

            Debug.Log($"[RMRAbnormalityUnlockManager] Enqueued EGO selection: {choiceSet.Count} candidates in queue position {LogLikeMod.egoSelectionQueue.Count}");
        }
        public static void EnqueueRewardSelections(int count)
        {
            EnqueueRewardSelections(count, LogLikeMod.curchaptergrade);
        }

        public static void EnqueueRewardSelections(int count, ChapterGrade grade)
        {
            for (int i = 0; i < count; i++)
            {
                List<RewardPassiveInfo> choices = RollRewardChoices(grade, 3);
                if (choices.Count > 0)
                    LogLikeMod.rewards_passive.Add(new RewardInfo { grade = grade, rewards = choices });
            }
        }

        public static bool OnEmotionPagePicked(EmotionCardXmlInfo card)
        {
            if (card == null)
                return false;
            RewardPassiveInfo info = GetPassiveInfoFromCard(card);
            if (info == null)
                return false;
            if (IsNoAbnormalityFallback(info.id))
                return true;
            if (info.rewardtype != RewardType.Creature)
                return false;
            if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.PassiveReward)
            {
                UnlockPage(info.id);
                LogueBookModels.RecordAtlasAbnormalityPage(info.id);
            }
            else if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.EmtoionChoose && !LogueBookModels.selectedEmotion.Contains(info))
            {
                LogueBookModels.selectedEmotion.Add(info);
                LogueBookModels.RecordAtlasAbnormalityPage(info.id);
            }
            return true;
        }

        public static void UnlockEgoForCurrentRoute(LorId id)
        {
            if (id != LorId.None && IsRealizationEgoCard(id))
                RouteUnlockedEgoPages.Add(id);
        }

        public static bool IsEgoUnlockedForCurrentRoute(LorId id)
        {
            return RouteUnlockedEgoPages.Contains(id);
        }

        public static EmotionCardXmlInfo GetNoAbnormalityFallback(int level)
        {
            int id = NoAbnormalityFallbackBaseId + Math.Max(1, level);
            RewardPassiveInfo info = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, id));
            return info == null ? null : LogLikeMod.GetRegisteredPickUpXml(info);
        }

        public static void RecordPermanentClear(ChapterGrade grade)
        {
            int tier = 0;
            if (grade == ChapterGrade.Grade4)
                tier = 1;
            else if (grade == ChapterGrade.Grade5)
                tier = 2;
            else if (grade >= ChapterGrade.Grade6)
                tier = 3;
            if (tier == 0)
                return;

            LoadPermanentProgress();
            if (PermanentlyUnlockedTiers.Add(tier))
                SavePermanentProgress();
        }

        public static bool IsNoAbnormalityFallback(EmotionCardXmlInfo card)
        {
            RewardPassiveInfo info = GetPassiveInfoFromCard(card);
            return info != null && IsNoAbnormalityFallback(info.id);
        }

        public static bool IsNoAbnormalityFallback(LorId id)
        {
            return id.packageId == LogLikeMod.ModId && id.id > NoAbnormalityFallbackBaseId && id.id <= NoAbnormalityFallbackBaseId + 6;
        }

        /// <summary>
        /// Check if a creature/abnormality reward page ID has been unlocked in the current route.
        /// </summary>
        public static bool IsPageUnlocked(LorId id)
        {
            return RouteUnlockedPages.Exists(x => x == id);
        }

        public static void UnlockShopAbnormalityPage(RewardPassiveInfo info)
        {
            if (info == null || info.rewardtype != RewardType.Creature)
                return;
            UnlockPage(info.id);
            LogueBookModels.RecordAtlasAbnormalityPage(info.id);
        }

        /// <summary>
        /// Returns a copy of the current route's unlocked abnormality page IDs.
        /// Used by atlas sync to record pages obtained during the run into the permanent atlas.
        /// </summary>
        public static List<LorId> GetRouteUnlockedPageIds()
        {
            return new List<LorId>(RouteUnlockedPages);
        }

        /// <summary>
        /// Returns the floor (SephirahType) that a script root belongs to, or Keter if unknown.
        /// </summary>
        public static SephirahType GetFloorForScript(string script)
        {
            string root = GetRootScript(script);
            foreach (var kvp in FloorAbnormalityScripts)
            {
                foreach (string floorRoot in kvp.Value)
                {
                    if (ScriptMatchesRoot(root, floorRoot))
                        return kvp.Key;
                }
            }
            Debug.Log($"[RMRAbnormalityUnlockManager] Unknown script root: {root} — defaulting to Keter");
            return SephirahType.None;
        }

        /// <summary>
        /// Returns true if this reward info is a final realization battle exclusive (Level 6).
        /// </summary>
        public static bool IsRealizationExclusive(RewardPassiveInfo info)
        {
            if (info == null || info.rewardtype != RewardType.Creature)
                return false;
            string root = GetRootScript(info.script);
            return GetRealizationFloorForScript(info.script) != SephirahType.None;
        }

        /// <summary>
        /// Returns true if the player has completed the realization battle for the given floor.
        /// </summary>
        public static bool IsFloorRealizationCompleted(SephirahType floor)
        {
            return CompletedRealizations.Contains(floor);
        }

        public static void RefreshRealizationProgress()
        {
            LoadRealizationProgress();
        }

        public static bool IsRealizationRewardAvailable(RewardPassiveInfo info)
        {
            if (!IsRealizationExclusive(info))
                return true;
            SephirahType floor = GetRealizationFloorForScript(info.script);
            return floor != SephirahType.None && IsFloorRealizationCompleted(floor);
        }

        public static SephirahType GetRealizationFloorForScript(string script)
        {
            foreach (var kvp in RealizationRewardScriptsByFloor)
            {
                foreach (string configuredScript in kvp.Value)
                {
                    if (ScriptMatchesRealizationEntry(script, configuredScript))
                        return kvp.Key;
                }
            }
            return SephirahType.None;
        }

        private static SephirahType GetBossRealizationRewardFloorForScript(string script)
        {
            foreach (var kvp in BossRealizationRewardScriptsByFloor)
            {
                foreach (string configuredScript in kvp.Value)
                {
                    if (ScriptMatchesRealizationEntry(script, configuredScript))
                        return kvp.Key;
                }
            }
            return SephirahType.None;
        }

        public static bool IsRealizationEgoCard(LorId id)
        {
            foreach (var kvp in RealizationEgoCardsByFloor)
            {
                if (kvp.Value.Any(x => x == id))
                    return true;
            }
            return false;
        }

        public static bool IsRealizationEgoCardUnlocked(LorId id)
        {
            foreach (var kvp in RealizationEgoCardsByFloor)
            {
                if (kvp.Value.Any(x => x == id))
                    return IsFloorRealizationCompleted(kvp.Key);
            }
            return false;
        }

        public static List<DiceCardXmlInfo> GetUnlockedRealizationEgoCardsForRewards(ChapterGrade grade)
        {
            int tier = GetTierForChapter(grade);
            List<DiceCardXmlInfo> result = new List<DiceCardXmlInfo>();
            foreach (var kvp in RealizationEgoCardsByFloor)
            {
                if (GetTierForFloor(kvp.Key) != tier || !IsFloorRealizationCompleted(kvp.Key))
                    continue;
                foreach (LorId id in kvp.Value)
                {
                    if (IsEgoUnlockedForCurrentRoute(id) || LogueBookModels.IsAtlasEgoPageUnlocked(id))
                        continue;
                    DiceCardXmlInfo card = ItemXmlDataList.instance.GetCardItem(id, true);
                    if (card != null && !result.Any(x => x.id == card.id))
                        result.Add(card);
                }
            }
            return result;
        }

        public static List<EmotionEgoXmlInfo> GetUnlockedEgoChoicesForBattle(StageLibraryFloorModel floor, int count = 3)
        {
            var selected = floor == null
                ? new List<EmotionEgoXmlInfo>()
                : ModdingUtils.GetFieldValue<List<EmotionEgoXmlInfo>>("_selectedEgoList", floor) ?? new List<EmotionEgoXmlInfo>();
            HashSet<LorId> selectedIds = new HashSet<LorId>(selected.Where(x => x != null).Select(x => x.CardId));
            List<EmotionEgoXmlInfo> candidates = new List<EmotionEgoXmlInfo>();
            bool usePermanentAtlas = RMRRealizationManager.InRealizationBattle
                || RMRRealizationManager.IsRealizationPreparationActive;
            if (usePermanentAtlas)
                LogueBookModels.EnsureAtlasUnlocks();

            foreach (var pair in RealizationEgoCardsByFloor)
            {
                List<EmotionEgoXmlInfo> floorEgos = Singleton<EmotionEgoXmlList>.Instance.GetDataList(pair.Key);
                if (floorEgos == null)
                    continue;
                foreach (EmotionEgoXmlInfo ego in floorEgos)
                {
                    if (ego == null || !pair.Value.Contains(ego.CardId))
                        continue;
                    bool unlocked = usePermanentAtlas
                        ? LogueBookModels.IsAtlasEgoPageUnlocked(ego.CardId)
                        : RouteUnlockedEgoPages.Contains(ego.CardId);
                    if (!unlocked || selectedIds.Contains(ego.CardId))
                        continue;
                    if (!candidates.Any(x => x.CardId == ego.CardId))
                        candidates.Add(ego);
                }
            }

            List<EmotionEgoXmlInfo> result = new List<EmotionEgoXmlInfo>();
            while (candidates.Count > 0 && result.Count < count)
            {
                int index = UnityEngine.Random.Range(0, candidates.Count);
                result.Add(candidates[index]);
                candidates.RemoveAt(index);
            }
            return result;
        }

        public static void ApplyVanillaEmotionPresentation(RewardPassiveInfo info, EmotionCardXmlInfo virtualCard)
        {
            if (info == null || virtualCard == null || info.rewardtype != RewardType.Creature)
                return;
            SephirahType floor = GetRealizationFloorForScript(info.script);
            if (floor == SephirahType.None)
                floor = GetFloorForScript(info.script);
            if (floor == SephirahType.None)
                return;

            EmotionCardXmlInfo vanillaCard = null;
            for (int level = 1; level <= 6 && vanillaCard == null; level++)
            {
                List<EmotionCardXmlInfo> cards = Singleton<EmotionCardXmlList>.Instance.GetDataListByLevel(floor, level);
                if (cards == null)
                    continue;
                vanillaCard = cards.FirstOrDefault(card =>
                    card != null
                    && card.Script != null
                    && card.Script.Any(script => ScriptMatchesRealizationEntry(script, info.script)));
            }
            if (vanillaCard == null)
            {
                Debug.LogWarning($"[RMRAbnormalityUnlockManager] Vanilla emotion presentation not found for {info.script} on {floor}.");
                return;
            }

            virtualCard.Name = vanillaCard.Name;
            virtualCard._artwork = vanillaCard.Artwork;
            virtualCard.Sephirah = vanillaCard.Sephirah;
            virtualCard.State = vanillaCard.State;
        }
        /// <summary>
        /// Mark a floor realization as completed (first clear only). Unlocks the floor's
        /// realization reward pool and records exclusive abnormality + E.G.O. pages into
        /// the permanent atlas. Does not grant pages to the current route inventory.
        /// </summary>
        public static void CompleteFloorRealization(SephirahType floor)
        {
            if (!CompletedRealizations.Add(floor))
                return;

            SaveRealizationProgress();

            foreach (RewardPassiveInfo info in GetAllCreatureRewardPages())
            {
                if (info == null || !IsRealizationExclusive(info))
                    continue;
                if (GetRealizationFloorForScript(info.script) != floor)
                    continue;
                LogueBookModels.RecordAtlasAbnormalityPage(info.id);
            }

            if (RealizationEgoCardsByFloor.TryGetValue(floor, out LorId[] egos) && egos != null)
            {
                foreach (LorId egoId in egos)
                    LogueBookModels.RecordAtlasEgoPage(egoId);
            }

            LogueBookModels.SavePermanentAtlasData();
            Debug.Log($"[RMRAbnormalityUnlockManager] Floor realization first clear recorded to atlas: {floor}");
        }

        /// <summary>
        /// Returns the set of floors whose pages are available for the given chapter grade.
        /// Grade 1-3 → Malkuth/Yesod/Hod/Netzach (前4层)
        /// Grade 4-5 → Tiphereth/Gebura/Chesed (中3层)
        /// Grade 6-7 → Binah/Hokma/Keter (后3层)
        /// </summary>
        public static HashSet<SephirahType> GetFloorsForChapter(ChapterGrade grade)
        {
            if (grade <= ChapterGrade.Grade3)
                return new HashSet<SephirahType> { SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Hod, SephirahType.Netzach };
            if (grade <= ChapterGrade.Grade5)
                return new HashSet<SephirahType> { SephirahType.Tiphereth, SephirahType.Gebura, SephirahType.Chesed };
            return new HashSet<SephirahType> { SephirahType.Binah, SephirahType.Hokma, SephirahType.Keter };
        }

        private static void UnlockPage(LorId id)
        {
            if (id == LorId.None || IsNoAbnormalityFallback(id))
                return;
            if (!RouteUnlockedPages.Exists(x => x == id))
                RouteUnlockedPages.Add(id);
        }

        private static List<RewardPassiveInfo> RollRewardChoices(ChapterGrade grade, int count)
        {
            List<RewardPassiveInfo> candidates = GetRewardCandidates(grade);
            List<RewardPassiveInfo> result = new List<RewardPassiveInfo>();
            while (candidates.Count > 0 && result.Count < count)
            {
                int index = UnityEngine.Random.Range(0, candidates.Count);
                result.Add(candidates[index]);
                candidates.RemoveAt(index);
            }
            return result;
        }

        private static List<RewardPassiveInfo> GetRewardCandidates(ChapterGrade grade)
        {
            return GetAllCreatureRewardPages()
                .Where(info => IsRewardTierAvailableForChapter(GetTierForScript(info.script), grade))
                .Where(info => !RouteUnlockedPages.Exists(id => id == info.id))
                .Where(info => !IsNoAbnormalityFallback(info.id))
                .Where(info => LogueBookModels.EmotionCardList == null || !LogueBookModels.EmotionCardList.Any(x => x.id == info.id))
                .Where(info => IsRealizationRewardAvailable(info))
                .ToList();
        }

        private static IEnumerable<RewardPassiveInfo> GetPermanentStartingPages()
        {
            foreach (int tier in PermanentlyUnlockedTiers)
            {
                foreach (RewardPassiveInfo info in GetAllCreatureRewardPages().Where(x => GetTierForScript(x.script) == tier && x.level >= 4 && !IsRealizationExclusive(x)))
                    yield return info;
            }
        }

        public static void PruneUnselectedRealizationPagesFromRoute()
        {
            Predicate<LorId> shouldRemove = id =>
            {
                RewardPassiveInfo info = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id);
                return info != null
                    && IsRealizationExclusive(info)
                    && !LogueBookModels.IsAtlasAbnormalityPageUnlocked(id);
            };
            RouteUnlockedPages.RemoveAll(shouldRemove);
            if (LogueBookModels.EmotionCardList != null)
                LogueBookModels.EmotionCardList.RemoveAll(info => info != null && shouldRemove(info.id));
        }

        public static List<RewardPassiveInfo> GetShopEligibleAbnormalityPages(ChapterGrade grade)
        {
            int tier = GetTierForChapter(grade);
            return GetAllCreatureRewardPages()
                .Where(info => IsRewardTierAvailableForChapter(GetTierForScript(info.script), grade))
                .Where(info => !IsNoAbnormalityFallback(info.id))
                .Where(info => IsRealizationRewardAvailable(info))
                .Where(info => LogueBookModels.EmotionCardList == null || !LogueBookModels.EmotionCardList.Any(x => x.id == info.id))
                .Where(info => !RouteUnlockedPages.Exists(id => id == info.id))
                .ToList();
        }

        private static List<RewardPassiveInfo> GetAllCreatureRewardPages()
        {
            List<RewardPassiveInfo> result = new List<RewardPassiveInfo>();
            foreach (RewardPassivesInfo info in Singleton<RewardPassivesList>.Instance.infos.Where(x => x.rewardtype == PassiveRewardListType.Custom))
            {
                if (info.RewardPassiveList == null)
                    continue;
                result.AddRange(info.RewardPassiveList.Where(x => x.rewardtype == RewardType.Creature));
            }
            return result;
        }

        private static RewardPassiveInfo GetPassiveInfoFromCard(EmotionCardXmlInfo card)
        {
            if (card == null)
                return null;
            return Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(card), card.id));
        }

        public static int GetRewardTierForCurrentChapter()
        {
            return GetTierForChapter(LogLikeMod.curchaptergrade);
        }


        public static bool IsRewardTierAvailableForChapter(int rewardTier, ChapterGrade grade)
        {
            int currentTier = GetTierForChapter(grade);
            return rewardTier > 0 && rewardTier <= currentTier;
        }

        public static float GetNormalAbnormalityRewardChance(ChapterGrade grade)
        {
            if (grade == ChapterGrade.Grade3)
                return UrbanLegendNormalAbnormalityRewardChance;
            if (grade == ChapterGrade.Grade4)
                return UrbanPlagueNormalAbnormalityRewardChance;
            return 1f;
        }

        public static bool ShouldEnqueueNormalAbnormalityReward(ChapterGrade grade)
        {
            float chance = GetNormalAbnormalityRewardChance(grade);
            return chance >= 1f || UnityEngine.Random.value < chance;
        }
        public static int GetTierForChapter(ChapterGrade grade)
        {
            if (grade <= ChapterGrade.Grade3)
                return 1;
            if (grade <= ChapterGrade.Grade5)
                return 2;
            return 3;
        }

        public static int GetTierForFloor(SephirahType floor)
        {
            if (floor == SephirahType.Malkuth || floor == SephirahType.Yesod || floor == SephirahType.Hod || floor == SephirahType.Netzach)
                return 1;
            if (floor == SephirahType.Tiphereth || floor == SephirahType.Gebura || floor == SephirahType.Chesed)
                return 2;
            if (floor == SephirahType.Binah || floor == SephirahType.Hokma || floor == SephirahType.Keter)
                return 3;
            return 0;
        }

        public static int GetTierForScript(string script)
        {
            SephirahType floor = GetFloorForScript(script);
            if (floor == SephirahType.Malkuth || floor == SephirahType.Yesod || floor == SephirahType.Hod || floor == SephirahType.Netzach)
                return 1;
            if (floor == SephirahType.Tiphereth || floor == SephirahType.Gebura || floor == SephirahType.Chesed)
                return 2;
            if (floor == SephirahType.Binah || floor == SephirahType.Hokma || floor == SephirahType.Keter)
                return 3;
            return 0;
        }

        private static bool ScriptMatchesRoot(string scriptRoot, string floorRoot)
        {
            if (string.IsNullOrEmpty(scriptRoot) || string.IsNullOrEmpty(floorRoot))
                return false;
            return string.Equals(scriptRoot, floorRoot, StringComparison.OrdinalIgnoreCase)
                   || scriptRoot.StartsWith(floorRoot, StringComparison.OrdinalIgnoreCase);
        }

        private static bool ScriptMatchesRealizationEntry(string script, string configuredScript)
        {
            if (string.IsNullOrEmpty(script) || string.IsNullOrEmpty(configuredScript))
                return false;
            if (string.Equals(script, configuredScript, StringComparison.OrdinalIgnoreCase))
                return true;
            return string.Equals(GetRootScript(script), configuredScript, StringComparison.OrdinalIgnoreCase)
                   || script.StartsWith(configuredScript, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetRootScript(string script)
        {
            if (string.IsNullOrEmpty(script))
                return string.Empty;
            int index = script.Length - 1;
            while (index >= 0 && char.IsDigit(script[index]))
                index--;
            return script.Substring(0, index + 1);
        }

        private static void LoadPermanentProgress()
        {
            PermanentlyUnlockedTiers.Clear();
            SaveData data = Singleton<LogueSaveManager>.Instance.LoadData(ProgressSaveName);
            if (data == null)
                return;
            for (int tier = 1; tier <= 3; tier++)
            {
                if (data.GetInt("Tier" + tier) > 0)
                    PermanentlyUnlockedTiers.Add(tier);
            }
        }

        private static void SavePermanentProgress()
        {
            SaveData data = new SaveData(SaveDataType.Dictionary);
            for (int tier = 1; tier <= 3; tier++)
                data.AddData("Tier" + tier, PermanentlyUnlockedTiers.Contains(tier) ? 1 : 0);
            Singleton<LogueSaveManager>.Instance.SaveData(data, ProgressSaveName);
        }

        private static void LoadRealizationProgress()
        {
            CompletedRealizations.Clear();
            SaveData data = Singleton<LogueSaveManager>.Instance.LoadData(RealizationSaveName);
            if (data == null)
                return;
            foreach (SephirahType floor in Enum.GetValues(typeof(SephirahType)))
            {
                if (data.GetData(floor.ToString()) != null && data.GetInt(floor.ToString()) > 0)
                    CompletedRealizations.Add(floor);
            }
        }

        private static void SaveRealizationProgress()
        {
            SaveData data = new SaveData(SaveDataType.Dictionary);
            foreach (SephirahType floor in Enum.GetValues(typeof(SephirahType)))
                data.AddData(floor.ToString(), CompletedRealizations.Contains(floor) ? 1 : 0);
            Singleton<LogueSaveManager>.Instance.SaveData(data, RealizationSaveName);
        }

        private static void DeleteSaveFile(string saveName)
        {
            try
            {
                string path = Path.Combine(LogueSaveManager.Saveroot, saveName);
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
            }
        }
    }

    public class PickUpModel_RMRNoAbnormality : CreaturePickUpModel
    {
        public PickUpModel_RMRNoAbnormality()
        {
            this.Name = "无";
            this.Desc = "当前情感等级没有已解锁的异想体书页。";
            this.FlaverText = "这一次，光没有回应。";
            this.ArtWork = "Stage_Rest";
        }

        public override void OnPickUp()
        {
        }

        public override void OnPickUp(BattleUnitModel model)
        {
        }
    }
}
