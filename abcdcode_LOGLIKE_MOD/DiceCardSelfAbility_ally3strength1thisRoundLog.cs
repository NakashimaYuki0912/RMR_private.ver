// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_ally3strength1thisRoundLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_ally3strength1thisRoundLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[2]
      {
        "bstart_Keyword",
        "Strength_Keyword"
      };
    }
  }

  public override void OnStartBattle()
  {
    foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_random(this.owner.faction, 3))
      battleUnitModel.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1, this.owner);
  }
}
}
