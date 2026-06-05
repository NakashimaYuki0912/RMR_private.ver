// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_shyaoSelfBurnLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

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
