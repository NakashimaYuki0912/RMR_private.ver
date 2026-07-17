// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_cardPowerDown2targetHighlander
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_cardPowerDown2targetHighlander.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_cardPowerDown2targetHighlanderLog</summary>

public class DiceCardSelfAbility_cardPowerDown2targetHighlanderLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "OnlyOne_Keyword" };
  }

  public override void OnStartParrying()
  {
    if (!this.owner.allyCardDetail.IsHighlander())
      return;
    BattleUnitModel target = this.card.target;
    if (target == null)
      return;
    BattlePlayingCardDataInUnitModel currentDiceAction = target.currentDiceAction;
    if (currentDiceAction == null)
      return;
    currentDiceAction.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
    {
      power = -2
    });
  }
}
}
