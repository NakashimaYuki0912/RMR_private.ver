// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_purpleAreaDice1Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_purpleAreaDice1Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_purpleAreaDice1Log</summary>

public class DiceCardAbility_purpleAreaDice1Log : DiceCardAbilityBase
{
  public override void OnSucceedAttack(BattleUnitModel target)
  {
    target?.TakeDamage(7, DamageType.Card_Ability, this.owner);
  }
}
}
