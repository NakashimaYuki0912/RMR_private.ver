// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_drawCardWarpLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_drawCardWarpLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_drawCardWarpLog</summary>

public class DiceCardSelfAbility_drawCardWarpLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[3]
      {
        "Quickness_Keyword",
        "WarpCharge",
        "DrawCard_Keyword"
      };
    }
  }

  public override void OnUseCard()
  {
    this.owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 2, this.owner);
    if (!(this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.WarpCharge) is BattleUnitBuf_warpCharge activatedBuf) || activatedBuf.stack < 3)
      return;
    activatedBuf.UseStack(3, true);
    this.owner.allyCardDetail.DrawCards(2);
  }
}
}
