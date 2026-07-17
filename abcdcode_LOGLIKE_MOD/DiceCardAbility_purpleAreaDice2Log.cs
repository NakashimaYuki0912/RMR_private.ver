// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_purpleAreaDice2Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_purpleAreaDice2Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_purpleAreaDice2Log</summary>

public class DiceCardAbility_purpleAreaDice2Log : DiceCardAbilityBase
{
  public override void OnSucceedAttack(BattleUnitModel target)
  {
    target?.TakeBreakDamage(7, DamageType.Card_Ability, this.owner);
  }
}
}
