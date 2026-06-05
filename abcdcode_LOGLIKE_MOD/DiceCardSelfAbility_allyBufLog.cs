// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_allyBufLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_allyBufLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[4]
      {
        "bstart_Keyword",
        "Protection_Keyword",
        "BreakProtect_Keyword",
        "Strength_Keyword"
      };
    }
  }

  public override void OnStartBattle()
  {
    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
    aliveList.Remove(this.owner);
    if (aliveList.Count <= 0)
      return;
    BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
    battleUnitModel.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 2, this.owner);
    battleUnitModel.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 2, this.owner);
    battleUnitModel.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 2, this.owner);
  }
}
}
