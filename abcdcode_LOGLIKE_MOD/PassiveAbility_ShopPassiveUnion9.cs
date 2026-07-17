// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveUnion9
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveUnion9.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveUnion9</summary>

public class PassiveAbility_ShopPassiveUnion9 : PassiveAbilityBase
{
  public override string debugDesc => "흐트러짐 상태가 되면 현재 체력의 25%를 잃고 흐트러짐 상태에서 벗어남";

  public override bool OnBreakGageZero()
  {
    this.owner.breakDetail.ResetGauge();
    this.owner.LoseHp((int) this.owner.hp / 4);
    return true;
  }
}
}
