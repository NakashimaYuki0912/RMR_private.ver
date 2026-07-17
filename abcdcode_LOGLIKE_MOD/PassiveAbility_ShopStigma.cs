// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopStigma
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopStigma.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopStigma</summary>

public class PassiveAbility_ShopStigma : PassiveAbilityBase
{
  public override void OnUnitCreated()
  {
    base.OnUnitCreated();
    Singleton<ShopManager>.Instance.OpenShop("Stigma");
  }
}
}
