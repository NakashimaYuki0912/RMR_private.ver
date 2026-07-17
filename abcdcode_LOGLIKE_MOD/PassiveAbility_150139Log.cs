// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_150139Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_150139Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_150139Log</summary>

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
