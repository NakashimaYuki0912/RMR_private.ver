// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_damage7atkLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_damage7atkLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_damage7atkLog</summary>

public class DiceCardAbility_damage7atkLog : DiceCardAbilityBase
{
  public override void OnSucceedAttack()
  {
    this.card.target?.TakeDamage(7, DamageType.Card_Ability, this.owner);
  }
}
}
