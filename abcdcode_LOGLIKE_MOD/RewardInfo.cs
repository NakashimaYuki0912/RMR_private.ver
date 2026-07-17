// -----------------------------------------------------------------------------
// Library of Ruina mod script: RewardInfo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\RewardInfo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using RogueLike_Mod_Reborn;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>RewardInfo</summary>

    public class RewardInfo
    {
        public ChapterGrade grade;
        public List<RewardPassiveInfo> rewards;

        public RewardInfo()
        {
            this.grade = ChapterGrade.Grade1;
            this.rewards = new List<RewardPassiveInfo>();
        }

        public static RewardInfo GetCurChapterCommonReward(ChapterGrade grade)
        {
            return new RewardInfo()
            {
                grade = grade,
                rewards = RMRCore.CurrentGamemode.GetCurChapterCommonReward(grade)
            };
        }

        public static RewardInfo GetCurChapterEliteReward(ChapterGrade grade)
        {
            return new RewardInfo()
            {
                grade = grade,
                rewards = RMRCore.CurrentGamemode.GetCurChapterEliteReward(grade)
            };
        }

        public static RewardInfo GetCurChapterBossReward(ChapterGrade grade)
        {
            return new RewardInfo()
            {
                grade = grade,
                rewards = RMRCore.CurrentGamemode.GetCurChapterBossReward(grade)
            };
        }

        public static RewardInfo GetCurChapterCreature(ChapterGrade grade)
        {
            return new RewardInfo()
            {
                grade = grade,
                rewards = RMRCore.CurrentGamemode.GetCurChapterCreature(grade)
            };
        }

    }
}
