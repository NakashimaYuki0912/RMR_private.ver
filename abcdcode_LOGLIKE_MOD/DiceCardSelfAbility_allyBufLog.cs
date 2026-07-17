// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_allyBufLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_allyBufLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_allyBufLog</summary>

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
