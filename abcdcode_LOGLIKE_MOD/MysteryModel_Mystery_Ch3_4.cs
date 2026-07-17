// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch3_4
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch3_4.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Mystery node model: MysteryModel_Mystery_Ch3_4</summary>

public class MysteryModel_Mystery_Ch3_4 : MysteryBase
{
  public override void OnClickChoice(int choiceid)
  {
    if (this.curFrame.FrameID == 1)
    {
      RewardInfo rewardInfo = new RewardInfo()
      {
        grade = ChapterGrade.Grade3,
        rewards = new List<RewardPassiveInfo>()
      };
      rewardInfo.rewards.Add(Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 1130004)));
      LogLikeMod.rewards_passive.Insert(0, rewardInfo);
    }
    base.OnClickChoice(choiceid);
  }
}
}
