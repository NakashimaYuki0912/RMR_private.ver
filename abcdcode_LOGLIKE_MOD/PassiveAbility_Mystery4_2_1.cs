// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_Mystery4_2_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_Mystery4_2_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_Mystery4_2_1</summary>

public class PassiveAbility_Mystery4_2_1 : PassiveAbilityBase
{
  public override string debugDesc => "모든 주사위 위력 + 1";

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    base.BeforeRollDice(behavior);
    behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      power = 1
    });
  }
}
}
