// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_energyAndChargeLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_energyAndChargeLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[2]{ "WarpCharge", "Energy_Keyword" };
  }

  public override void OnUseCard()
  {
    this.owner.cardSlotDetail.RecoverPlayPointByCard(1);
    this.owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.WarpCharge, 4, this.owner);
  }
}
}
