// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_otherAlly3strength1thisRoundLo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_otherAlly3strength1thisRoundLo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_otherAlly3strength1thisRoundLog</summary>

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
