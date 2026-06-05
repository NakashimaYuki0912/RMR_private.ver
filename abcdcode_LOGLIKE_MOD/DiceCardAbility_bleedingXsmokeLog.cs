// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardAbility_bleedingXsmokeLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

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
