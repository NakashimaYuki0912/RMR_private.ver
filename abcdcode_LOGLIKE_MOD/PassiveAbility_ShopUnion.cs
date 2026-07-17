// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopUnion
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopUnion.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopUnion</summary>

public class PassiveAbility_ShopUnion : PassiveAbilityBase
{
  public override void OnUnitCreated()
  {
    base.OnUnitCreated();
    Singleton<ShopManager>.Instance.OpenShop("Union");
  }
}
}
