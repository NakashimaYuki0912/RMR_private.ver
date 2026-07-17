// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_enemy3weak1thisRoundLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_enemy3weak1thisRoundLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_enemy3weak1thisRoundLog</summary>

public class DiceCardSelfAbility_enemy3weak1thisRoundLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[2]{ "bstart_Keyword", "Weak_Keyword" };
  }

  public override void OnStartBattle()
  {
    foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_random(this.owner.faction == Faction.Enemy ? Faction.Player : Faction.Enemy, 3))
      battleUnitModel.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Weak, 1, this.owner);
  }
}
}
