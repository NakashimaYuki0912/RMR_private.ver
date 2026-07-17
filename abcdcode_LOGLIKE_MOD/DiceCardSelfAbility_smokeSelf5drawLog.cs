// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_smokeSelf5drawLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_smokeSelf5drawLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_smokeSelf5drawLog</summary>

public class DiceCardSelfAbility_smokeSelf5drawLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[2]
      {
        "Smoke_Keyword",
        "DrawCard_Keyword"
      };
    }
  }

  public override void OnUseCard()
  {
    this.owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Smoke, 5);
    this.owner.allyCardDetail.DrawCards(1);
  }
}
}
