// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_powerUp2targetHp50Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_powerUp2targetHp50Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_powerUp2targetHp50Log</summary>

public class DiceCardAbility_powerUp2targetHp50Log : DiceCardAbilityBase
{
  public override void BeforeRollDice()
  {
    BattleUnitModel target = this.behavior.card.target;
    if (target == null || (double) target.hp > (double) target.MaxHp * 0.5)
      return;
    this.behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      power = 2
    });
  }
}
}
