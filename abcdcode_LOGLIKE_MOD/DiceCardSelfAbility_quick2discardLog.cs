// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_quick2discardLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_quick2discardLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_quick2discardLog</summary>

public class DiceCardSelfAbility_quick2discardLog : DiceCardSelfAbilityBase
{
  public override void OnDiscard(BattleUnitModel unit, BattleDiceCardModel self)
  {
    base.OnDiscard(unit, self);
    unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 2, unit);
  }
}
}
