// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_warpCharge9Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_warpCharge9Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_warpCharge9Log</summary>

public class DiceCardSelfAbility_warpCharge9Log : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "WarpCharge" };
  }

  public override void OnUseCard()
  {
    this.owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.WarpCharge, 9, this.owner);
  }
}
}
