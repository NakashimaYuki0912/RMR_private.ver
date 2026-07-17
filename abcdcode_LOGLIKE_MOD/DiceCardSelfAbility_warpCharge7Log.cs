// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_warpCharge7Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_warpCharge7Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_warpCharge7Log</summary>

public class DiceCardSelfAbility_warpCharge7Log : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "WarpCharge" };
  }

  public override void OnUseCard()
  {
    this.owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.WarpCharge, 7);
  }
}
}
