// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_shyaoSelfBurnLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_shyaoSelfBurnLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_shyaoSelfBurnLog</summary>

public class DiceCardSelfAbility_shyaoSelfBurnLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[3]
      {
        "Burn_Keyword",
        "Protection_Keyword",
        "Recover_Keyword"
      };
    }
  }

  public override void OnUseCard()
  {
    if (this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.Burn) == null)
      return;
    this.owner.RecoverHP(5);
    this.owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Protection, 2, this.owner);
  }

  public override void OnStartBattle()
  {
    BattleUnitBufListDetail bufListDetail = this.owner.bufListDetail;
    BattleUnitBuf_burnDown buf = new BattleUnitBuf_burnDown();
    buf.stack = 2;
    bufListDetail.AddBuf((BattleUnitBuf) buf);
  }
}
}
