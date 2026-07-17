// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_MachineDawnCard2_0
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_MachineDawnCard2_0.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_MachineDawnCard2_0</summary>

public class DiceCardSelfAbility_MachineDawnCard2_0 : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    base.OnUseCard();
    if (this.card.target == null)
      return;
    int stack = this.card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable) == null ? 0 : this.card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable).stack;
    this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
    {
      power = stack
    });
  }
}
}
