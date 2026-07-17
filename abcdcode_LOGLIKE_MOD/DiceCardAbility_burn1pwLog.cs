// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_burn1pwLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_burn1pwLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_burn1pwLog</summary>

public class DiceCardAbility_burn1pwLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Burn_Keyword" };
  }

  public override void OnWinParrying()
  {
    this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 1, this.owner);
  }
}
}
