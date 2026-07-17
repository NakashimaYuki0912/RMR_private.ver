// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_Chapter2Rat
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_Chapter2Rat.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_Chapter2Rat</summary>

public class PassiveAbility_Chapter2Rat : PassiveAbilityBase
{
  public override string debugDesc => "일방공격 시 피해량 + 1";

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    if (behavior.IsParrying())
      return;
    behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      dmg = 1
    });
  }
}
}
