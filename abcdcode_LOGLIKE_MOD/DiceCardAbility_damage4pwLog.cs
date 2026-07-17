// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_damage4pwLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_damage4pwLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_damage4pwLog</summary>

public class DiceCardAbility_damage4pwLog : DiceCardAbilityBase
{
  public override void OnWinParrying()
  {
    this.card.target?.TakeDamage(4, DamageType.Card_Ability, this.owner);
  }
}
}
