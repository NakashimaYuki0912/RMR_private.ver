// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_powerUp2fullCardLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_powerUp2fullCardLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_powerUp2fullCardLog</summary>

public class DiceCardSelfAbility_powerUp2fullCardLog : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    if (this.owner.allyCardDetail.GetHand().Count < 5)
      return;
    this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
    {
      power = 2
    });
  }
}
}
