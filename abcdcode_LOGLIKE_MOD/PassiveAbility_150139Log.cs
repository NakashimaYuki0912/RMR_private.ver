// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_150139Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_150139Log : PassiveAbilityBase
{
  public override bool isImmortal => true;

  public override void OnRoundStart()
  {
    if ((double) this.owner.hp > 30.0)
      return;
    this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Stun, 1);
  }

  public override void OnRoundEndTheLast() => this.CheckMaster();

  public void CheckMaster()
  {
    BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetAliveList(this.owner.faction).Find((Predicate<BattleUnitModel>) (x => x.passiveDetail.HasPassive<PassiveAbility_150051Log>()));
    if (battleUnitModel != null && !battleUnitModel.IsDead())
      return;
    this.owner.Die();
  }
}
}
