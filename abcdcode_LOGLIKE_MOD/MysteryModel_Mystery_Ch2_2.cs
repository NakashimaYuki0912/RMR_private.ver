// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch2_2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch2_2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Mystery node model: MysteryModel_Mystery_Ch2_2</summary>

    public class MysteryModel_Mystery_Ch2_2 : MysteryBase
    {
        public override void SwapFrame(int id) => base.SwapFrame(id);

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 1)
            {
                RewardInfo rewardInfo = new RewardInfo()
                {
                    grade = ChapterGrade.Grade2,
                    rewards = new List<RewardPassiveInfo>()
                };
                rewardInfo.rewards.Add(Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 210003)));
                LogLikeMod.rewards_passive.Add(rewardInfo);
                MysteryBase.RemoveStageList((Predicate<LogueStageInfo>)(x => x.type == StageType.Boss), ChapterGrade.Grade2);
                LogueStageInfo stageInfo = Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 220006));
                stageInfo.type = StageType.Boss;
                stageInfo.stageid = 20006;
                MysteryBase.AddStageList(stageInfo, ChapterGrade.Grade2);
            }
            if (this.curFrame.FrameID == 2)
            {
                RewardInfo rewardInfo = new RewardInfo()
                {
                    grade = ChapterGrade.Grade2,
                    rewards = new List<RewardPassiveInfo>()
                };
                rewardInfo.rewards.Add(Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 210006)));
                LogLikeMod.rewards_passive.Add(rewardInfo);
                MysteryBase.RemoveStageList((Predicate<LogueStageInfo>)(x => x.type == StageType.Boss), ChapterGrade.Grade2);
                LogueStageInfo stageInfo = Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 220005));
                stageInfo.type = StageType.Boss;
                stageInfo.stageid = 20005;
                MysteryBase.AddStageList(stageInfo, ChapterGrade.Grade2);
            }
            base.OnClickChoice(choiceid);
        }
    }
}
