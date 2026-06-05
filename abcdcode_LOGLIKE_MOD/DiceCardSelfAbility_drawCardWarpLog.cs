// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_drawCardWarpLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_drawCardWarpLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[3]
      {
        "Quickness_Keyword",
        "WarpCharge",
        "DrawCard_Keyword"
      };
    }
  }

  public override void OnUseCard()
  {
    this.owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 2, this.owner);
    if (!(this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.WarpCharge) is BattleUnitBuf_warpCharge activatedBuf) || activatedBuf.stack < 3)
      return;
    activatedBuf.UseStack(3, true);
    this.owner.allyCardDetail.DrawCards(2);
  }
}
}
