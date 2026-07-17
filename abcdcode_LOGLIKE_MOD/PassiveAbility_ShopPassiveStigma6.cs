// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveStigma6
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveStigma6.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveStigma6</summary>

public class PassiveAbility_ShopPassiveStigma6 : PassiveAbilityBase
{
  public override string debugDesc => "화상 상태의 적에게 공격 적중 시 즉시 화상 피해를 입힘";

  public override void OnSucceedAttack(BattleDiceBehavior behavior)
  {
    base.OnSucceedAttack(behavior);
    if (!(behavior.card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Burn) is BattleUnitBuf_burn activatedBuf))
      return;
    activatedBuf.OnRoundEnd();
  }
}
}
