// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_240508Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_240508Log : PassiveAbilityBase
{
  public override string debugDesc => "막 종료시 에마와 합쳐짐";

  public override void OnRoundEndTheLast_ignoreDead()
  {
    BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetAliveList().Find((Predicate<BattleUnitModel>) (x => x.Book.GetBookClassInfoId() == new LorId(LogLikeMod.ModId, 140008)));
    if (!this.owner.IsDeadSceneBlock || battleUnitModel != null)
      return;
    this.owner.SetDeadSceneBlock(false);
  }
}
}
