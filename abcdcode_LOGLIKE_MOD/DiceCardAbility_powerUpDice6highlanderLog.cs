// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_powerUpDice6highlanderLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_powerUpDice6highlanderLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_powerUpDice6highlanderLog</summary>

public class DiceCardAbility_powerUpDice6highlanderLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "OnlyOne_Keyword" };
  }

  public override void BeforeRollDice()
  {
    if (!this.owner.allyCardDetail.IsHighlander())
      return;
    this.behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      power = 6
    });
  }
}
}
