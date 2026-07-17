// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_quickall2discardLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_quickall2discardLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_quickall2discardLog</summary>

public class DiceCardSelfAbility_quickall2discardLog : DiceCardSelfAbilityBase
{
  public override void OnDiscard(BattleUnitModel unit, BattleDiceCardModel self)
  {
    base.OnDiscard(unit, self);
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(unit.faction))
      alive.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 2, unit);
  }
}
}
