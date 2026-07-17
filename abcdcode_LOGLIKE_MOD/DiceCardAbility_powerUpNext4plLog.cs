// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_powerUpNext4plLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_powerUpNext4plLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_powerUpNext4plLog</summary>

public class DiceCardAbility_powerUpNext4plLog : DiceCardAbilityBase
{
  public override void OnLoseParrying() => this.card.AddDiceAdder(DiceMatch.NextDice, 4);
}
}
