// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_tomerry2Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_tomerry2Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_tomerry2Log</summary>

public class DiceCardSelfAbility_tomerry2Log : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
    {
      power = 3
    });
    this.owner.TakeDamage(5, DamageType.Card_Ability, this.owner);
  }
}
}
