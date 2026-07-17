// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassive26
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassive26.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassive26</summary>

public class PassiveAbility_ShopPassive26 : PassiveAbilityBase
{
  public int stack;

  public override string debugDesc => "피해를 받으면 생명력을 1 회복함(막 당 최대 5)";

  public override void OnRoundStart()
  {
    base.OnRoundStart();
    this.stack = 5;
  }

  public override void AfterTakeDamage(BattleUnitModel attacker, int dmg)
  {
    base.AfterTakeDamage(attacker, dmg);
    if (this.stack <= 0)
      return;
    this.owner.RecoverHP(1);
    --this.stack;
  }
}
}
