// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_debufsSmokeLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_debufsSmokeLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_debufsSmokeLog</summary>

public class DiceCardAbility_debufsSmokeLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[3]
      {
        "Paralysis_Keyword",
        "Weak_Keyword",
        "Smoke_Keyword"
      };
    }
  }

  public override void OnSucceedAttack()
  {
    if (!(this.card.target?.bufListDetail.GetActivatedBuf(KeywordBuf.Smoke) is BattleUnitBuf_smoke activatedBuf) || activatedBuf.stack < 6)
      return;
    this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Paralysis, 2, this.owner);
    this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Weak, 1, this.owner);
  }
}
}
