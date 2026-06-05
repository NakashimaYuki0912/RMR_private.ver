// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardAbility_hubertDiceLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardAbility_hubertDiceLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[2]
      {
        "Weak_Keyword",
        "Bleeding_Keyword"
      };
    }
  }

  public override void OnSucceedAttack()
  {
    this.behavior.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Weak, 2, this.owner);
    this.behavior.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 10, this.owner);
  }
}
}
