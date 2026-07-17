// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_energySmokeLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_energySmokeLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_energySmokeLog</summary>

public class DiceCardSelfAbility_energySmokeLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[2]
      {
        "Smoke_Keyword",
        "Energy_Keyword"
      };
    }
  }

  public override void OnUseCard()
  {
    this.owner.cardSlotDetail.RecoverPlayPointByCard(1);
    this.owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Smoke, 3);
  }
}
}
