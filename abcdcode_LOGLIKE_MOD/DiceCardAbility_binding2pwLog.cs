// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_binding2pwLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_binding2pwLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_binding2pwLog</summary>

public class DiceCardAbility_binding2pwLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Binding_Keyword" };
  }

  public override void OnWinParrying()
  {
    this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Binding, 2, this.owner);
  }
}
}
