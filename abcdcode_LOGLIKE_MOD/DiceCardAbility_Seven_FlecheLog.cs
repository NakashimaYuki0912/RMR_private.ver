// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_Seven_FlecheLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_Seven_FlecheLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_Seven_FlecheLog</summary>

public class DiceCardAbility_Seven_FlecheLog : DiceCardAbilityBase
{
  public override void OnWinParrying()
  {
    BattleUnitModel target = this.card?.target;
    if (target == null)
      return;
    target.TakeBreakDamage(5, attacker: this.owner);
    target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Vulnerable, 1, this.owner);
  }
}
}
