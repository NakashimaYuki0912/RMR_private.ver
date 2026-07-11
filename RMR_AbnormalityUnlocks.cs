using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
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
        /// <summary>script 鈫?vanilla EmotionLevel tier (1/2/3).</summary>
        private static readonly Dictionary<string, int> _vanillaEmotionTierByScript =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private static bool _vanillaEmotionTiersLoaded;
        private static bool BinahUnlockedForCurrentRoute;
        private static bool RedMistVictoryRewardsGrantedThisBattle;
        private static bool BlackSilenceClearRecordedThisBattle;
        private static bool BlueReverberationRewardsGrantedThisBattle;

        // Floor 鈫?all abnormality script roots on that floor
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
            // Do NOT bulk-UnlockPage permanent-tier abnos into the mid-battle emotion pool.
            // Permanent tiers only gate shop/reward availability (IsRewardTierAvailableForChapter).
            // Mid-battle abno picks use pages actually obtained this route (rewards/shop/emotion).
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
                // Empty route unlocks 鈥?permanent tiers still gate shop rolls only.
                return;
            }
            SaveData pages = data.GetData("Pages");
            if (pages == null)
                pages = data;
            else
                BinahUnlockedForCurrentRoute = data.GetInt("BinahUnlocked") > 0;
            // Only restore pages that were saved as route unlocks (obtained this run).
            // Never re-inject GetPermanentStartingPages bulk unlocks.
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

            // Normal RMR run: mid-battle emotion picks only from pages obtained this route
            // (battle rewards / shop / prior emotion picks) 鈥?not the entire catalog.
            var list = RouteUnlockedPages
                .Select(id => Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id))
                .Where(info => info != null && info.rewardtype == RewardType.Creature && !IsNoAbnormalityFallback(info.id))
                .ToList();
            Debug.Log($"[RMRAbnormalityUnlockManager] Mid-battle abno pool size={list.Count} (route-unlocked only).");
            return list;
        }

        /// <summary>
        /// Vanilla mapping (EmotionCard Xml &lt;EmotionLevel&gt; = page tier I/II/III):
        ///   Tier I  (EmotionLevel=1) 鈫?Team emotion 1鈥?
        ///   Tier II (EmotionLevel=2) 鈫?Team emotion 3鈥?
        ///   Tier III(EmotionLevel=3) 鈫?Team emotion 5
        /// </summary>
        public static int GetRequiredAbnoTierForTeamEmotion(int teamEmotionLevel)
        {
            if (teamEmotionLevel <= 0)
                return 1;
            if (teamEmotionLevel <= 2)
                return 1;
            if (teamEmotionLevel <= 4)
                return 2;
            return 3;
        }

        private static readonly Dictionary<string, int> VanillaEmotionTierByScript =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);


        /// <summary>
        /// Resolve vanilla abnormality tier (1/2/3) for a reward page via EmotionCardXmlList.
        /// Cached. Returns 0 if unknown.
        /// </summary>
        public static int GetVanillaAbnoTier(RewardPassiveInfo info)
        {
            if (info == null || string.IsNullOrEmpty(info.script))
                return 0;
            return GetVanillaAbnoTierForScript(info.script);
        }

        public static int GetVanillaAbnoTierForScript(string script)
        {
            if (string.IsNullOrEmpty(script))
                return 0;
            EnsureVanillaEmotionTiersLoaded();
            string key = script.Trim();
            if (_vanillaEmotionTierByScript.TryGetValue(key, out int tier) && tier > 0)
                return tier;
            // Root without trailing digit (e.g. BloodBath for BloodBath1)
            string root = GetRootScript(key);
            if (!string.IsNullOrEmpty(root) && !string.Equals(root, key, StringComparison.OrdinalIgnoreCase)
                && _vanillaEmotionTierByScript.TryGetValue(root, out tier) && tier > 0)
                return tier;
            return 0;
        }

        private static void EnsureVanillaEmotionTiersLoaded()
        {
            if (_vanillaEmotionTiersLoaded)
                return;
            _vanillaEmotionTiersLoaded = true;
            try
            {
                EmotionCardXmlList list = Singleton<EmotionCardXmlList>.Instance;
                if (list == null)
                    return;
                int loaded = 0;
                foreach (SephirahType floor in FloorAbnormalityScripts.Keys)
                {
                    for (int floorLevel = 1; floorLevel <= 6; floorLevel++)
                    {
                        List<EmotionCardXmlInfo> cards = null;
                        try { cards = list.GetDataListByLevel(floor, floorLevel); }
                        catch { continue; }
                        if (cards == null)
                            continue;
                        foreach (EmotionCardXmlInfo card in cards)
                        {
                            if (card?.Script == null)
                                continue;
                            int tier = ReadEmotionLevelField(card);
                            if (tier < 1 || tier > 3)
                                continue;
                            foreach (string s in card.Script)
                            {
                                if (string.IsNullOrEmpty(s))
                                    continue;
                                _vanillaEmotionTierByScript[s] = tier;
                                string root = GetRootScript(s);
                                if (!string.IsNullOrEmpty(root) && !_vanillaEmotionTierByScript.ContainsKey(root))
                                    _vanillaEmotionTierByScript[root] = tier;
                                loaded++;
                            }
                        }
                    }
                }
                Debug.Log($"[RMRAbnormalityUnlockManager] Loaded vanilla abno EmotionLevel tiers for {_vanillaEmotionTierByScript.Count} script keys (entries scanned={loaded}).");
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMRAbnormalityUnlockManager] EnsureVanillaEmotionTiersLoaded: " + ex.Message);
            }
        }

        private static int ReadEmotionLevelField(EmotionCardXmlInfo card)
        {
            if (card == null)
                return 0;
            try
            {
                // Public field/property on vanilla EmotionCardXmlInfo
                FieldInfo fi = AccessTools.Field(typeof(EmotionCardXmlInfo), "EmotionLevel");
                if (fi != null)
                    return Convert.ToInt32(fi.GetValue(card));
                PropertyInfo pi = AccessTools.Property(typeof(EmotionCardXmlInfo), "EmotionLevel");
                if (pi != null)
                    return Convert.ToInt32(pi.GetValue(card, null));
            }
            catch { /* ignore */ }
            return 0;
        }

        /// <summary>
        /// True if this owned page may appear when team emotion level rises to <paramref name="teamEmotionLevel"/>.
        /// </summary>
        public static bool IsOwnedPageEligibleForTeamEmotion(RewardPassiveInfo info, int teamEmotionLevel)
        {
            if (info == null || IsNoAbnormalityFallback(info.id))
                return false;
            int requiredTier = GetRequiredAbnoTierForTeamEmotion(teamEmotionLevel);
            int pageTier = GetVanillaAbnoTier(info);
            if (pageTier <= 0)
            {
                // Unknown vanilla mapping: allow only at emotion 1鈥? as safe Tier-I fallback,
                // and log once per script via debug.
                return requiredTier == 1;
            }
            return pageTier == requiredTier;
        }

        /// <summary>
        /// Filter route-owned pages for a team emotion pick, then pick up to 3.
        /// </summary>
        public static List<RewardPassiveInfo> FilterOwnedPagesForTeamEmotion(
            IEnumerable<RewardPassiveInfo> ownedPool,
            int teamEmotionLevel,
            HashSet<string> alreadySelectedKeys)
        {
            int requiredTier = GetRequiredAbnoTierForTeamEmotion(teamEmotionLevel);
            var eligible = new List<RewardPassiveInfo>();
            if (ownedPool == null)
                return eligible;
            foreach (RewardPassiveInfo info in ownedPool)
            {
                if (info == null)
                    continue;
                string key = (info.id.packageId ?? "") + ":" + info.id.id;
                if (alreadySelectedKeys != null && alreadySelectedKeys.Contains(key))
                    continue;
                if (!IsOwnedPageEligibleForTeamEmotion(info, teamEmotionLevel))
                    continue;
                eligible.Add(info);
            }
            Debug.Log($"[RMRAbnormalityUnlockManager] Emotion pick filter teamLv={teamEmotionLevel} needTier={requiredTier} eligible={eligible.Count} (owned pool filtered).");
            return eligible;
        }

        public static void EnqueueBattleClearRewards()
        {
            // Realization battles have their own reward path 鈥?skip normal Roguelike reward chains
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
        /// After a BOSS clear: one realization-exclusive abnormality 3-pick and one E.G.O. 3-pick
        /// from floors the player has already completed in realization.
        /// Atlas unlock only means "eligible for pools", NOT "already owned this run".
        /// </summary>
        private static void EnqueueBossRealizationTierRewards(ChapterGrade grade)
        {
            LoadRealizationProgress();
            PruneUnselectedRealizationPagesFromRoute();
            HashSet<SephirahType> completedFloors = GetCompletedRealizationFloorsForBossTier(grade);
            if (completedFloors.Count == 0)
            {
                Debug.Log($"[RMRAbnormalityUnlockManager] BOSS realization rewards skipped: grade={grade}, no completed floors in tier {GetTierForChapter(grade)}");
                return;
            }

            // 1) Liberation-exclusive abnormality page 鈥?always a single 3-choose-1.
            List<RewardPassiveInfo> exclusiveAbno = RollExclusiveRealizationAbnormalityChoices(completedFloors, 3);
            if (exclusiveAbno != null && exclusiveAbno.Count > 0)
            {
                LogLikeMod.rewards_passive.Add(new RewardInfo { grade = grade, rewards = exclusiveAbno });
                Debug.Log($"[RMRAbnormalityUnlockManager] BOSS exclusive abno 3-pick enqueued ({exclusiveAbno.Count} options) floors=[{string.Join(",", completedFloors)}]");
            }
            else
            {
                Debug.LogWarning($"[RMRAbnormalityUnlockManager] BOSS exclusive abno 3-pick empty for grade {grade} floors=[{string.Join(",", completedFloors)}]");
            }

            // 2) Liberation E.G.O. page 鈥?always a single 3-choose-1 (independent ego queue).
            bool egoQueued = EnqueueRealizationEgoSelection(completedFloors);
            Debug.Log($"[RMRAbnormalityUnlockManager] BOSS realization rewards grade={grade}: exclusiveAbno={exclusiveAbno?.Count ?? 0}, egoQueued={egoQueued}, floors=[{string.Join(",", completedFloors)}]");
        }

        private static HashSet<SephirahType> GetCompletedRealizationFloorsForBossTier(ChapterGrade grade)
        {
            HashSet<SephirahType> floors = GetFloorsForChapter(grade);
            floors.IntersectWith(CompletedRealizations);
            return floors;
        }

        /// <summary>
        /// True if this run already owns the E.G.O. (picked earlier or already in inventory).
        /// Atlas unlock alone is NOT ownership 鈥?that only opens the reward/shop pool.
        /// </summary>
        public static bool IsEgoOwnedOnCurrentRoute(LorId id)
        {
            if (id == null || id == LorId.None)
                return true;
            if (IsEgoUnlockedForCurrentRoute(id))
                return true;
            try
            {
                if (LogueBookModels.cardlist != null
                    && LogueBookModels.cardlist.Any(c => c != null && c.GetID() != null
                        && c.GetID().id == id.id
                        && (string.IsNullOrEmpty(c.GetID().packageId) || string.IsNullOrEmpty(id.packageId)
                            || c.GetID().packageId == id.packageId)))
                    return true;
            }
            catch { /* ignore inventory probe failures */ }
            return false;
        }

        /// <summary>
        /// Realization-exclusive abnormality pages only (Snow White / Freisch眉tz final, etc.),
        /// from completed realization floors. Does not use the wide floor pool.
        /// </summary>
        private static List<RewardPassiveInfo> RollExclusiveRealizationAbnormalityChoices(HashSet<SephirahType> floors, int count)
        {
            var candidates = new List<RewardPassiveInfo>();
            foreach (var info in GetAllCreatureRewardPages())
            {
                if (info == null || info.rewardtype != RewardType.Creature)
                    continue;
                if (IsNoAbnormalityFallback(info.id))
                    continue;
                if (!IsRealizationExclusive(info))
                    continue;
                SephirahType floor = GetRealizationFloorForScript(info.script);
                if (floor == SephirahType.None || !floors.Contains(floor))
                    continue;
                // Route ownership only 鈥?atlas unlock is pool eligibility, not run ownership.
                if (RouteUnlockedPages.Exists(id => id == info.id))
                    continue;
                if (LogueBookModels.EmotionCardList != null && LogueBookModels.EmotionCardList.Any(x => x != null && x.id == info.id))
                    continue;
                if (LogLikeMod.rewards_passive != null
                    && LogLikeMod.rewards_passive.Any(reward => reward?.rewards != null && reward.rewards.Any(queued => queued != null && queued.id == info.id)))
                    continue;
                candidates.Add(info);
            }

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
        /// Enqueues one E.G.O. 3-choose-1 from completed realization floors.
        /// Returns true if a non-empty choice set was queued.
        /// </summary>
        private static bool EnqueueRealizationEgoSelection(HashSet<SephirahType> floors)
        {
            var candidates = new List<LorId>();
            foreach (var floor in floors)
            {
                if (!RealizationEgoCardsByFloor.TryGetValue(floor, out LorId[] egoIds) || egoIds == null)
                    continue;
                foreach (LorId id in egoIds)
                {
                    if (id == null || id == LorId.None)
                        continue;
                    if (IsEgoOwnedOnCurrentRoute(id))
                        continue;
                    // Must resolve a real card XML 鈥?otherwise GetQueuedEgoRewards drops the entry and the whole 3-pick can vanish.
                    DiceCardXmlInfo card = ItemXmlDataList.instance.GetCardItem(id, true)
                        ?? ItemXmlDataList.instance.GetCardItem(id.id, true)
                        ?? ItemXmlDataList.instance.GetCardItem(new LorId(string.Empty, id.id), true);
                    if (card == null)
                        continue;
                    if (LogLikeMod.egoSelectionQueue != null
                        && LogLikeMod.egoSelectionQueue.Any(choice => choice != null && choice.Any(q => q != null && q.id == id.id)))
                        continue;
                    if (!candidates.Any(c => c != null && c.id == id.id))
                        candidates.Add(card.id != null ? card.id : id);
                }
            }

            if (candidates.Count == 0)
            {
                Debug.LogWarning($"[RMRAbnormalityUnlockManager] EnqueueRealizationEgoSelection: no unowned EGO in floors [{string.Join(",", floors)}]");
                return false;
            }

            int pickCount = Math.Min(3, candidates.Count);
            var choiceSet = new List<LorId>();
            for (int i = 0; i < pickCount; i++)
            {
                int index = UnityEngine.Random.Range(0, candidates.Count);
                choiceSet.Add(candidates[index]);
                candidates.RemoveAt(index);
            }

            if (LogLikeMod.egoSelectionQueue == null)
                LogLikeMod.egoSelectionQueue = new List<List<LorId>>();
            LogLikeMod.egoSelectionQueue.Add(choiceSet);

            Debug.Log($"[RMRAbnormalityUnlockManager] Enqueued EGO 3-pick: {choiceSet.Count} candidates (queue size {LogLikeMod.egoSelectionQueue.Count}) ids=[{string.Join(",", choiceSet.Select(x => x.id.ToString()).ToArray())}]");
            return true;
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
                // Persist into route pool so later battles this run can re-offer the page.
                UnlockPage(info.id);
                LogueBookModels.RecordAtlasAbnormalityPage(info.id);
            }
            return true;
        }

        /// <summary>
        /// Resolve vanilla EmotionEgoXmlInfo for a combat-page id (for floor _selectedEgoList / EGO hand).
        /// </summary>
        public static EmotionEgoXmlInfo FindVanillaEmotionEgo(LorId cardId)
        {
            if (cardId == null || cardId == LorId.None || Singleton<EmotionEgoXmlList>.Instance == null)
                return null;
            foreach (var pair in RealizationEgoCardsByFloor)
            {
                List<EmotionEgoXmlInfo> floorEgos = null;
                try { floorEgos = Singleton<EmotionEgoXmlList>.Instance.GetDataList(pair.Key); }
                catch { continue; }
                if (floorEgos == null)
                    continue;
                foreach (EmotionEgoXmlInfo ego in floorEgos)
                {
                    if (ego == null)
                        continue;
                    try
                    {
                        if (ego.CardId != null && ego.CardId.id == cardId.id)
                            return ego;
                        if (pair.Value != null && pair.Value.Any(x => x != null && x.id == cardId.id)
                            && ego.CardId != null && ego.CardId.id == cardId.id)
                            return ego;
                    }
                    catch { /* ignore bad entry */ }
                }
            }
            // Fallback scan: any sephirah list
            try
            {
                foreach (SephirahType floor in System.Enum.GetValues(typeof(SephirahType)))
                {
                    if (floor == SephirahType.None)
                        continue;
                    List<EmotionEgoXmlInfo> list = Singleton<EmotionEgoXmlList>.Instance.GetDataList(floor);
                    if (list == null)
                        continue;
                    foreach (EmotionEgoXmlInfo ego in list)
                    {
                        if (ego?.CardId != null && ego.CardId.id == cardId.id)
                            return ego;
                    }
                }
            }
            catch { /* ignore */ }
            return null;
        }

        /// <summary>
        /// Register a mid-battle EGO pick onto the current floor's selected list so vanilla
        /// ExistEgoCardBySelected / hand toggle treats it like a floor E.G.O. selection.
        /// </summary>
        public static bool RegisterSelectedEgoOnCurrentFloor(LorId cardId)
        {
            if (cardId == null || cardId == LorId.None)
                return false;
            try
            {
                StageLibraryFloorModel floor = Singleton<StageController>.Instance?.GetCurrentStageFloorModel();
                if (floor == null)
                    return false;
                List<EmotionEgoXmlInfo> selected =
                    ModdingUtils.GetFieldValue<List<EmotionEgoXmlInfo>>("_selectedEgoList", floor);
                if (selected == null)
                {
                    selected = new List<EmotionEgoXmlInfo>();
                    try
                    {
                        FieldInfo fi = AccessTools.Field(typeof(StageLibraryFloorModel), "_selectedEgoList");
                        fi?.SetValue(floor, selected);
                    }
                    catch { return false; }
                }
                if (selected.Any(e => e?.CardId != null && e.CardId.id == cardId.id))
                    return true;

                EmotionEgoXmlInfo real = FindVanillaEmotionEgo(cardId);
                if (real == null)
                {
                    // Build a minimal entry so CardId resolves via our Harmony if needed.
                    real = LogLikeMod.AddEmotionEgoForReward(
                        ItemXmlDataList.instance?.GetCardItem(cardId, true)
                        ?? ItemXmlDataList.instance?.GetCardItem(cardId.id, true));
                }
                if (real == null)
                    return false;
                selected.Add(real);
                Debug.Log($"[RMR] Registered floor selected EGO id={cardId.id} (selected count={selected.Count}).");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR] RegisterSelectedEgoOnCurrentFloor: " + ex.Message);
                return false;
            }
        }

        public static void UnlockEgoForCurrentRoute(LorId id)
        {
            // Track all picked EGO for this run (not only realization ids) so personalEgo grant works.
            if (id != null && id != LorId.None)
                RouteUnlockedEgoPages.Add(id);
        }

        /// <summary>All EGO ids unlocked/picked on the current route.</summary>
        public static IEnumerable<LorId> EnumerateRouteUnlockedEgoPages()
        {
            return RouteUnlockedEgoPages;
        }

        public static bool IsEgoUnlockedForCurrentRoute(LorId id)
        {
            return RouteUnlockedEgoPages.Contains(id);
        }

        public static bool HasCompletedAnyRealization()
        {
            return CompletedRealizations != null && CompletedRealizations.Count > 0;
        }

        /// <summary>
        /// Pool of E.G.O. card ids available for mid-battle picks (emotion 3/4/5 after abno),
        /// from completed floor realizations. Excludes ids already selected this reception.
        /// </summary>
        public static List<LorId> CollectMidBattleEgoCandidates(HashSet<int> alreadySelectedThisBattle)
        {
            var candidates = new List<LorId>();
            var seen = new HashSet<int>();
            void Consider(LorId id)
            {
                if (id == null || id == LorId.None)
                    return;
                if (alreadySelectedThisBattle != null && alreadySelectedThisBattle.Contains(id.id))
                    return;
                if (!seen.Add(id.id))
                    return;
                DiceCardXmlInfo card = ItemXmlDataList.instance?.GetCardItem(id, true)
                    ?? ItemXmlDataList.instance?.GetCardItem(id.id, true)
                    ?? ItemXmlDataList.instance?.GetCardItem(new LorId(string.Empty, id.id), true);
                if (card == null)
                    return;
                candidates.Add(card.id != null ? card.id : id);
            }

            if (CompletedRealizations != null)
            {
                foreach (SephirahType floor in CompletedRealizations)
                {
                    if (!RealizationEgoCardsByFloor.TryGetValue(floor, out LorId[] egoIds) || egoIds == null)
                        continue;
                    foreach (LorId id in egoIds)
                        Consider(id);
                }
            }

            // Route-picked EGO from shops/rewards that may not be on the realization table.
            foreach (LorId id in RouteUnlockedEgoPages)
                Consider(id);

            return candidates;
        }

        /// <summary>
        /// Random 3-pick (or fewer) from mid-battle EGO pool.
        /// </summary>
        public static List<LorId> RollMidBattleEgoChoiceSet(HashSet<int> alreadySelectedThisBattle)
        {
            List<LorId> candidates = CollectMidBattleEgoCandidates(alreadySelectedThisBattle);
            if (candidates.Count == 0)
                return new List<LorId>();
            int pickCount = Math.Min(3, candidates.Count);
            var choiceSet = new List<LorId>();
            for (int i = 0; i < pickCount; i++)
            {
                int index = UnityEngine.Random.Range(0, candidates.Count);
                choiceSet.Add(candidates[index]);
                candidates.RemoveAt(index);
            }
            return choiceSet;
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
            Debug.Log($"[RMRAbnormalityUnlockManager] Unknown script root: {root} 鈥?defaulting to Keter");
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

        /// <summary>
        /// Resolve the vanilla EmotionCardXmlInfo for a reward script (e.g. snowwhite1 鈫?
        /// Name "SnowWhite_Vine"). Vanilla AbnormalityCards localization is keyed by Name,
        /// not by Script, so callers that need desc text must use this Name.
        /// </summary>
        public static EmotionCardXmlInfo FindVanillaEmotionCard(string script)
        {
            if (string.IsNullOrEmpty(script))
                return null;
            SephirahType floor = GetRealizationFloorForScript(script);
            if (floor == SephirahType.None)
                floor = GetFloorForScript(script);
            if (floor == SephirahType.None)
            {
                // Scan all floors as a last resort (script roots not in the floor map).
                foreach (SephirahType candidateFloor in FloorAbnormalityScripts.Keys)
                {
                    EmotionCardXmlInfo found = FindVanillaEmotionCardOnFloor(candidateFloor, script);
                    if (found != null)
                        return found;
                }
                return null;
            }
            return FindVanillaEmotionCardOnFloor(floor, script);
        }

        private static EmotionCardXmlInfo FindVanillaEmotionCardOnFloor(SephirahType floor, string script)
        {
            for (int level = 1; level <= 6; level++)
            {
                List<EmotionCardXmlInfo> cards = Singleton<EmotionCardXmlList>.Instance.GetDataListByLevel(floor, level);
                if (cards == null)
                    continue;
                EmotionCardXmlInfo match = cards.FirstOrDefault(card =>
                    card != null
                    && card.Script != null
                    && card.Script.Any(s => ScriptMatchesRealizationEntry(s, script)));
                if (match != null)
                    return match;
            }
            return null;
        }

        public static void ApplyVanillaEmotionPresentation(RewardPassiveInfo info, EmotionCardXmlInfo virtualCard)
        {
            if (info == null || virtualCard == null || info.rewardtype != RewardType.Creature)
                return;

            EmotionCardXmlInfo vanillaCard = FindVanillaEmotionCard(info.script);
            if (vanillaCard == null)
            {
                Debug.LogWarning($"[RMRAbnormalityUnlockManager] Vanilla emotion presentation not found for {info.script}.");
                return;
            }

            // Critical: UI EmotionPassiveCardUI.SetTexts looks up AbnormalityCardDesc by Name.
            // Vanilla localization IDs are fancy names (SnowWhite_Vine), not scripts (snowwhite1).
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
        /// Grade 1-3 鈫?Malkuth/Yesod/Hod/Netzach (鍓?灞?
        /// Grade 4-5 鈫?Tiphereth/Gebura/Chesed (涓?灞?
        /// Grade 6-7 鈫?Binah/Hokma/Keter (鍚?灞?
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
            // Unicode escapes so file encoding cannot corrupt Chinese strings.
            this.Name = "\u65e0"; // 无
            this.Desc = "\u5f53\u524d\u60c5\u611f\u7b49\u7ea7\u6ca1\u6709\u5df2\u89e3\u9501\u7684\u5f02\u60f3\u4f53\u4e66\u9875\u3002"; // 当前情感等级没有已解锁的异想体书页。
            this.FlaverText = "\u8fd9\u4e00\u6b21\uff0c\u5149\u6ca1\u6709\u56de\u5e94\u3002"; // 这一次，光没有回应。
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
