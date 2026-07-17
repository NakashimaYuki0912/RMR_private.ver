// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_recoverHp5atkLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_recoverHp5atkLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_recoverHp5atkLog</summary>

public class DiceCardAbility_recoverHp5atkLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Recover_Keyword" };
  }

  public override void OnSucceedAttack() => this.card.owner.RecoverHP(5);
}
}
