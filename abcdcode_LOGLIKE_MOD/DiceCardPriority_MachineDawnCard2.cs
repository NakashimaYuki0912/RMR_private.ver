// -----------------------------------------------------------------------------
// Enemy AI card priority: DiceCardPriority_MachineDawnCard2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardPriority_MachineDawnCard2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>AI card priority: DiceCardPriority_MachineDawnCard2</summary>

public class DiceCardPriority_MachineDawnCard2 : DiceCardPriorityBase
{
  public override int GetPriorityBonus(BattleUnitModel owner)
  {
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(owner.faction == Faction.Enemy ? Faction.Player : Faction.Enemy))
    {
      if (alive.bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable) != null)
        return 5;
    }
    return -10;
  }
}
}
