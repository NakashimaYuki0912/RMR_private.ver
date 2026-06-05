// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_Seven_EnGardeLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_Seven_EnGardeLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "exhaust" };
  }

  public override void OnUseCard()
  {
    this.card.card.exhaust = true;
    this.owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 2, this.owner);
    this.card?.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 1, this.owner);
  }
}
}
