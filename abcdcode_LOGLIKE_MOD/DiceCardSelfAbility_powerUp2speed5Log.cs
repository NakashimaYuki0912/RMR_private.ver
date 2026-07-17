// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_powerUp2speed5Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_powerUp2speed5Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_powerUp2speed5Log</summary>

public class DiceCardSelfAbility_powerUp2speed5Log : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    if (this.card.speedDiceResultValue < 5)
      return;
    this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
    {
      power = 2
    });
  }
}
}
