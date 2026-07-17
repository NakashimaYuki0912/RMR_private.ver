// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_rudolphDiceLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_rudolphDiceLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_rudolphDiceLog</summary>

public class DiceCardAbility_rudolphDiceLog : DiceCardAbilityBase
{
  public override void OnSucceedAttack(BattleUnitModel target)
  {
    target.TakeBreakDamage(8, DamageType.Card_Ability, this.owner);
  }
}
}
