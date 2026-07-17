// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassive19
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassive19.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassive19</summary>

public class PassiveAbility_ShopPassive19 : PassiveAbilityBase
{
  public override string debugDesc => "막 시작 시 무작위 내성 하나가 '면역' 상태가 됨";

  public override void OnRoundStart()
  {
    BehaviourDetail behaviourDetail = (BehaviourDetail) Random.Range(0, 3);
    bool flag = (double) Random.value > 0.5;
    this.owner.bufListDetail.AddBuf((BattleUnitBuf) new PassiveAbility_ShopPassive19.BattleUnitBuf_Immune()
    {
      Target = behaviourDetail,
      IsHp = flag
    });
  }

  /// <summary>BattleUnitBuf_Immune</summary>

  public class BattleUnitBuf_Immune : BattleUnitBuf
  {
    public bool IsHp;
    public BehaviourDetail Target = BehaviourDetail.None;

    public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
    {
      return this.IsHp && this.Target == detail ? AtkResist.Immune : base.GetResistHP(origin, detail);
    }

    public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
    {
      return !this.IsHp && this.Target == detail ? AtkResist.Immune : base.GetResistHP(origin, detail);
    }

    public override void OnRoundEnd() => this.Destroy();
  }
}
}
