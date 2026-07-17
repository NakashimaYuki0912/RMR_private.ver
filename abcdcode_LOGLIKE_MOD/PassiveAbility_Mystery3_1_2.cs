// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_Mystery3_1_2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_Mystery3_1_2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_Mystery3_1_2</summary>

public class PassiveAbility_Mystery3_1_2 : PassiveAbilityBase
{
  public override string debugDesc => "자신과 소속이 다른 모두를 공격 대상으로 삼음";

  public override void OnWaveStart()
  {
    base.OnWaveStart();
    this.owner.bufListDetail.AddBuf((BattleUnitBuf) new PassiveAbility_Mystery3_1_2.TargetingBuf2());
  }

  /// <summary>TargetingBuf2</summary>

  public class TargetingBuf2 : BattleUnitBuf
  {
    public override List<BattleUnitModel> GetFixedTarget()
    {
      List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList();
      aliveList.RemoveAll((Predicate<BattleUnitModel>) (x => x.passiveDetail.HasPassive<PassiveAbility_Mystery3_1_2>()));
      return aliveList;
    }
  }
}
}
