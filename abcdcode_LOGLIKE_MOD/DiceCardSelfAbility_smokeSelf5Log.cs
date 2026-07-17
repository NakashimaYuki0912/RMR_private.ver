// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_smokeSelf5Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_smokeSelf5Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_smokeSelf5Log</summary>

public class DiceCardSelfAbility_smokeSelf5Log : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Smoke_Keyword" };
  }

  public override void OnUseCard()
  {
    this.owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Smoke, 5);
  }
}
}
