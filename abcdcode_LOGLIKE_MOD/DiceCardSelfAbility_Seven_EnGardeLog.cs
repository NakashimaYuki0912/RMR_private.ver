// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_Seven_EnGardeLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_Seven_EnGardeLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_Seven_EnGardeLog</summary>

public class DiceCardSelfAbility_Seven_EnGardeLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "exhaust" };
  }

  public override void OnUseCard()
  {
    this.card.card.exhaust = true;
    this.owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 2, this.owner);
    this.card?.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 1, this.owner);
  }
}
}
