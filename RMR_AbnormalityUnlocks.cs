using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using abcdcode_LOGLIKE_MOD;
using GameSave;
using LOR_XML;
using UnityEngine;

namespace RogueLike_Mod_Reborn
{
    public static class RMRAbnormalityUnlockManager
    {
        public const int AbnormalityBattleRewardCount = 3;
        public const int MysteryRewardCount = 1;

        public const int BlackForestMysteryId = 991001;
        public const int WellMysteryId = 991002;
        public const int HandsOfLightMysteryId = 991003;

        private const string ProgressSaveName = "RMR_AbnormalityProgress";
        private const string RealizationSaveName = "RMR_FloorRealizations";
        private const int NoAbnormalityFallbackBaseId = 15999000;

        private static readonly List<LorId> RouteUnlockedPages = new List<LorId>();
        private static readonly HashSet<int> PermanentlyUnlockedTiers = new HashSet<int>();
        private static readonly HashSet<SephirahType> CompletedRealizations = new HashSet<SephirahType>();

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

        // Final realization battle exclusive script roots (Level 6 in vanilla EmotionCard_*.txt)
        public static readonly HashSet<string> RealizationExclusiveScripts = new HashSet<string>
        {
            "snowwhite",        // Malkuth — SnowWhite's Apple
            "freischutz",       // Yesod — Matan / Magic Bullet
            "blackswan",        // Hod — BlackSwan
            "orchestra",        // Netzach — Silent Orchestra
            "clownofnihil",     // Tiphereth — NihilClown
            "nothing",          // Gebura — NothingThere
            "wizard",           // Chesed — Oz / Wizard of Oz
            "bossbird",         // Binah — ApocalypseBird
            "whitenight",       // Hokma — WhiteNight
            "plaguedoctor",     // Hokma — WhiteNight (PlagueDoctor form)
            "onebadmanygood",   // Hokma — WhiteNight (OneBadManyGood form)
            "quietKid",         // Keter — QuietKid
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
            LoadPermanentProgress();
            LoadRealizationProgress();
            foreach (RewardPassiveInfo info in GetPermanentStartingPages())
                UnlockPage(info.id);
        }

        public static void ResetArchiveProgress()
        {
            RouteUnlockedPages.Clear();
            PermanentlyUnlockedTiers.Clear();
            RemoveLegacyProgressFile();
        }

        public static SaveData SaveRouteUnlocks()
        {
            SaveData data = new SaveData();
            foreach (LorId id in RouteUnlockedPages)
                data.AddToList(id.LogGetSaveData());
            return data;
        }

        public static void LoadRouteUnlocks(SaveData data)
        {
            RouteUnlockedPages.Clear();
            LoadPermanentProgress();
            LoadRealizationProgress();
            if (data == null)
            {
                foreach (RewardPassiveInfo info in GetPermanentStartingPages())
                    UnlockPage(info.id);
                return;
            }
            foreach (SaveData item in data)
                UnlockPage(ExtensionUtils.LogLoadFromSaveData(item));
        }

        public static List<RewardPassiveInfo> GetUnlockedEmotionCardsForBattle()
        {
            return RouteUnlockedPages
                .Select(id => Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id))
                .Where(info => info != null && info.rewardtype == RewardType.Creature && !IsNoAbnormalityFallback(info.id))
                .ToList();
        }

        public static void EnqueueBattleClearRewards()
        {
            if (LogLikeMod.curstagetype == abcdcode_LOGLIKE_MOD.StageType.Creature)
            {
                EnqueueRewardSelections(AbnormalityBattleRewardCount, LogLikeMod.curchaptergrade);
                return;
            }

            if (LogLikeMod.curstagetype == abcdcode_LOGLIKE_MOD.StageType.Normal || LogLikeMod.curstagetype == abcdcode_LOGLIKE_MOD.StageType.Elite)
                EnqueueRewardSelections(1, LogLikeMod.curchaptergrade);
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
                UnlockPage(info.id);
            else if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.EmtoionChoose && !LogueBookModels.selectedEmotion.Contains(info))
                LogueBookModels.selectedEmotion.Add(info);
            return true;
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
            return RealizationExclusiveScripts.Any(exclusiveRoot => ScriptMatchesRoot(root, exclusiveRoot));
        }

        /// <summary>
        /// Returns true if the player has completed the realization battle for the given floor.
        /// </summary>
        public static bool IsFloorRealizationCompleted(SephirahType floor)
        {
            return CompletedRealizations.Contains(floor);
        }

        /// <summary>
        /// Mark a floor realization as completed. Unlocks all that floor's abnormality pages + EGO pages permanently.
        /// Saves to persistent storage.
        /// </summary>
        public static void CompleteFloorRealization(SephirahType floor)
        {
            if (CompletedRealizations.Add(floor))
            {
                Debug.Log($"[RMRAbnormalityUnlockManager] Floor realization completed: {floor}");
                // Unlock ALL abnormality pages for this floor
                if (FloorAbnormalityScripts.TryGetValue(floor, out string[] roots))
                {
                    foreach (string root in roots)
                    {
                        foreach (RewardPassiveInfo info in GetAllCreatureRewardPages())
                        {
                            if (info != null && ScriptMatchesRoot(GetRootScript(info.script), root))
                                UnlockPage(info.id);
                        }
                    }
                }
                SaveRealizationProgress();
            }
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
            int tier = GetTierForChapter(grade);
            return GetAllCreatureRewardPages()
                .Where(info => GetTierForScript(info.script) == tier)
                .Where(info => !RouteUnlockedPages.Exists(id => id == info.id))
                .Where(info => !IsNoAbnormalityFallback(info.id))
                .Where(info => LogueBookModels.EmotionCardList == null || !LogueBookModels.EmotionCardList.Any(x => x.id == info.id))
                .Where(info => !IsRealizationExclusive(info) || IsFloorRealizationCompleted(GetFloorForScript(info.script)))
                .ToList();
        }

        private static IEnumerable<RewardPassiveInfo> GetPermanentStartingPages()
        {
            foreach (int tier in PermanentlyUnlockedTiers)
            {
                foreach (RewardPassiveInfo info in GetAllCreatureRewardPages().Where(x => GetTierForScript(x.script) == tier && x.level >= 4))
                    yield return info;
            }
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

        public static int GetTierForChapter(ChapterGrade grade)
        {
            if (grade <= ChapterGrade.Grade3)
                return 1;
            if (grade <= ChapterGrade.Grade5)
                return 2;
            return 3;
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

        private static void RemoveLegacyProgressFile()
        {
            try
            {
                string path = Path.Combine(LogueSaveManager.Saveroot, ProgressSaveName);
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
