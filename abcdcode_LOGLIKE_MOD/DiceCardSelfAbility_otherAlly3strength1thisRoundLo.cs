// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_otherAlly3strength1thisRoundLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_otherAlly3strength1thisRoundLog : DiceCardSelfAbilityBase
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
    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
    List<BattleUnitModel> battleUnitModelList = new List<BattleUnitModel>();
    aliveList.RemoveAll((Predicate<BattleUnitModel>) (x => x == this.owner));
    for (int index = 3; aliveList.Count > 0 && index > 0; --index)
    {
      BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
      aliveList.Remove(battleUnitModel);
      battleUnitModelList.Add(battleUnitModel);
    }
    foreach (BattleUnitModel battleUnitModel in battleUnitModelList)
      battleUnitModel.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1, this.owner);
  }
}
}
