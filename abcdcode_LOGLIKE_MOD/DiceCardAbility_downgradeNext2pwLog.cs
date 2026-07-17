// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_downgradeNext2pwLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_downgradeNext2pwLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_downgradeNext2pwLog</summary>

public class DiceCardAbility_downgradeNext2pwLog : DiceCardAbilityBase
{
  public override void OnWinParrying()
  {
    BattleUnitModel target = this.card.target;
    if (target == null)
      return;
    BattlePlayingCardDataInUnitModel currentDiceAction = target.currentDiceAction;
    if (currentDiceAction == null)
      return;
    currentDiceAction.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus()
    {
      max = -4
    });
  }
}
}
