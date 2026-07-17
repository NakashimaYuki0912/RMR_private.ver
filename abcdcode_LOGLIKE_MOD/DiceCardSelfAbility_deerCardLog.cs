// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_deerCardLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_deerCardLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_deerCardLog</summary>

public class DiceCardSelfAbility_deerCardLog : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    this.owner.TakeBreakDamage(3, DamageType.Card_Ability, this.owner);
  }
}
}
