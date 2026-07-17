// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveMook3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveMook3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveMook3</summary>

public class PassiveAbility_ShopPassiveMook3 : PassiveAbilityBase
{
  public override string debugDesc => "내 체력이 50% 미만일때 피해량 + 2. 적 체력이 50% 미만이면 피해량 + 2";

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    base.BeforeRollDice(behavior);
    int num = 0;
    if ((double) this.owner.hp < (double) (this.owner.MaxHp / 2))
      num += 2;
    if ((double) behavior.card.target.hp < (double) (behavior.card.target.MaxHp / 2))
      num += 2;
    if (num <= 0)
      return;
    behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      dmg = num
    });
  }
}
}
