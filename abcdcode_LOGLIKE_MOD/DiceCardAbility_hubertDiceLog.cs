// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_hubertDiceLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_hubertDiceLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_hubertDiceLog</summary>

public class DiceCardAbility_hubertDiceLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[2]
      {
        "Weak_Keyword",
        "Bleeding_Keyword"
      };
    }
  }

  public override void OnSucceedAttack()
  {
    this.behavior.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Weak, 2, this.owner);
    this.behavior.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 10, this.owner);
  }
}
}
