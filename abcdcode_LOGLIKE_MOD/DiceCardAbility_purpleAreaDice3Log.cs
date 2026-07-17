// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_purpleAreaDice3Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_purpleAreaDice3Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_purpleAreaDice3Log</summary>

public class DiceCardAbility_purpleAreaDice3Log : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[2]
      {
        "Bleeding_Keyword",
        "Vulnerable_Keyword"
      };
    }
  }

  public override void OnSucceedAttack(BattleUnitModel target)
  {
    target?.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 4, this.owner);
    target?.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Vulnerable, 1, this.owner);
    target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 4, this.owner);
    target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Vulnerable, 1, this.owner);
  }
}
}
