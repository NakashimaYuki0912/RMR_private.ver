// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_240508Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_240508Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_240508Log</summary>

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
