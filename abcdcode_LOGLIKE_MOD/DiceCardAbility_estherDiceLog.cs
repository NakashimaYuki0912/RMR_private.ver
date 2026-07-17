// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_estherDiceLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_estherDiceLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_estherDiceLog</summary>

public class DiceCardAbility_estherDiceLog : DiceCardAbilityBase
{
  public override void OnSucceedAttack()
  {
    int num = this.behavior.DiceVanillaValue * 8;
    if (num <= 0)
      return;
    this.behavior.card.target?.TakeBreakDamage(num, DamageType.Card_Ability, this.owner);
    this.behavior.card.target?.TakeDamage(num, DamageType.Card_Ability, this.owner);
  }
}
}
