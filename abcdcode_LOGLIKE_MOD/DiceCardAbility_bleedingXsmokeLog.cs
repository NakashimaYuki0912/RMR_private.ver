// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_bleedingXsmokeLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_bleedingXsmokeLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_bleedingXsmokeLog</summary>

public class DiceCardAbility_bleedingXsmokeLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[2]
      {
        "Smoke_Keyword",
        "Bleeding_Keyword"
      };
    }
  }

  public override void OnSucceedAttack()
  {
    if (!(this.card.target?.bufListDetail.GetActivatedBuf(KeywordBuf.Smoke) is BattleUnitBuf_smoke activatedBuf) || activatedBuf.stack < 1)
      return;
    this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, activatedBuf.stack, this.owner);
  }
}
}
