// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopMook
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopMook.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopMook</summary>

public class PassiveAbility_ShopMook : PassiveAbilityBase
{
  public override void OnUnitCreated()
  {
    base.OnUnitCreated();
    Singleton<ShopManager>.Instance.OpenShop("Mook");
  }
}
}
