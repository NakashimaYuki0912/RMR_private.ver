// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch3_4
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

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
