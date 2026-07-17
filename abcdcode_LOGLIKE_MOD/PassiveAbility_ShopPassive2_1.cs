// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassive2_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassive2_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassive2_1</summary>

public class PassiveAbility_ShopPassive2_1 : PassiveAbilityBase
{
  public override string debugDesc => "적을 처치하면 모든 아군의 생명력 3 회복";

  public override void OnKill(BattleUnitModel target)
  {
    base.OnKill(target);
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction))
      alive.RecoverHP(3);
  }
}
}
