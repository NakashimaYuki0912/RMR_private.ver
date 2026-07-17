// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_ally3protection1thisRoundLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_ally3protection1thisRoundLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_ally3protection1thisRoundLog</summary>

public class DiceCardSelfAbility_ally3protection1thisRoundLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[2]
      {
        "bstart_Keyword",
        "Protection_Keyword"
      };
    }
  }

  public override void OnStartBattle()
  {
    foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_random(this.owner.faction, 3))
      battleUnitModel.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 1, this.owner);
  }
}
}
