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
        private const int NoAbnormalityFallbackBaseId = 15999000;

        private static readonly List<LorId> RouteUnlockedPages = new List<LorId>();
        private static readonly HashSet<int> PermanentlyUnlockedTiers = new HashSet<int>();

        private static readonly string[] SimpleRoots =
        {
            "BloodBath", "ScorchedGirl", "ForsakenMurderer", "HappyTeddyBear",
            "LittleHelper", "SingingMachine", "ShyLookToday", "Redhood",
            "BigBadWolf", "UniverseZogak", "ChildofGalaxy", "Butterfly", "Laetitia"
        };

        private static readonly string[] MediumRoots =
        {
            "QueenOfHatred", "KnightOfDespair", "Greed", "Angry",
            "Mountain", "Nosferatu", "ScareCrow", "LumberJack", "House", "Ozma"
        };

        private static readonly string[] HardRoots =
        {
            "Pinocchio", "FairyCarnival", "SpiderBud", "Porccubus",
            "Bigbird", "SmallBird", "LongBird", "BlueStar", "Alriune",
            "TheSnowQueen", "QueenBee", "Clock", "Bloodytree"
        };

        public static void StartNewRoute(ChapterGrade grade)
        {
            RouteUnlockedPages.Clear();
            LoadPermanentProgress();
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
            else if (grade == ChapterGrade.Grade6)
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
            string root = GetRootScript(script);
            if (SimpleRoots.Contains(root))
                return 1;
            if (MediumRoots.Contains(root))
                return 2;
            if (HardRoots.Contains(root))
                return 3;
            return 1;
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
