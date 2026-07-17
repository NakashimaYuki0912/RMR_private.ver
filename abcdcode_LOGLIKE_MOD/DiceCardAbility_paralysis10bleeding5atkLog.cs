// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_paralysis10bleeding5atkLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_paralysis10bleeding5atkLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_paralysis10bleeding5atkLog</summary>

public class DiceCardAbility_paralysis10bleeding5atkLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[2]{ "Paralysis_Keyword", "exhaust" };
  }

  public override void OnSucceedAttack()
  {
    this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Paralysis, 10, this.owner);
    this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 5, this.owner);
  }
}
}
