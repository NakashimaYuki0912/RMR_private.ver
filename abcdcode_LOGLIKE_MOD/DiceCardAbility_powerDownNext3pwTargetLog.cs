// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_powerDownNext3pwTargetLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_powerDownNext3pwTargetLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_powerDownNext3pwTargetLog</summary>

public class DiceCardAbility_powerDownNext3pwTargetLog : DiceCardAbilityBase
{
  public override void OnWinParrying()
  {
    this.card.target?.currentDiceAction?.AddDiceAdder(DiceMatch.NextDice, -3);
  }
}
}
