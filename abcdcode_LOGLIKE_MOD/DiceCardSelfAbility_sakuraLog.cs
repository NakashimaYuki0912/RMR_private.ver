// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_sakuraLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_sakuraLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_sakuraLog</summary>

public class DiceCardSelfAbility_sakuraLog : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    if (this.owner.allyCardDetail.GetHand().Count < 4)
      return;
    this.owner.allyCardDetail.DiscardACardRandomlyByAbility(4);
    this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
    {
      min = 4
    });
  }
}
}
