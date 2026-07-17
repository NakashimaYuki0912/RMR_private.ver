// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_handCountpowerUpLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_handCountpowerUpLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_handCountpowerUpLog</summary>

public class DiceCardSelfAbility_handCountpowerUpLog : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    int count = this.owner.allyCardDetail.GetHand().Count;
    if (count <= 0)
      return;
    this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
    {
      power = count
    });
    this.owner.allyCardDetail.DiscardACardByAbility(this.owner.allyCardDetail.GetHand());
  }
}
}
