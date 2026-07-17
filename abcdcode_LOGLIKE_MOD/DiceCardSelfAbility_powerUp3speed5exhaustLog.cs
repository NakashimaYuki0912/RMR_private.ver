// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_powerUp3speed5exhaustLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_powerUp3speed5exhaustLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_powerUp3speed5exhaustLog</summary>

public class DiceCardSelfAbility_powerUp3speed5exhaustLog : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    if (this.card.speedDiceResultValue >= 5)
      this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
      {
        power = 3
      });
    this.card.card.ReserveExhaust();
  }
}
}
