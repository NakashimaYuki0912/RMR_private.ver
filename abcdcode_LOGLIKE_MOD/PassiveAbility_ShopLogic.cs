// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopLogic
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopLogic.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopLogic</summary>

public class PassiveAbility_ShopLogic : PassiveAbilityBase
{
  public override void OnUnitCreated()
  {
    base.OnUnitCreated();
    Singleton<ShopManager>.Instance.OpenShop("Logic");
  }
}
}
