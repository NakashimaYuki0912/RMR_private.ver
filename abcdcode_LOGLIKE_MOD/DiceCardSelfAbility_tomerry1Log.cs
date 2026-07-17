// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_tomerry1Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_tomerry1Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_tomerry1Log</summary>

public class DiceCardSelfAbility_tomerry1Log : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    if ((double) this.owner.hp > (double) (this.owner.MaxHp / 4))
      return;
    this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
    {
      power = 3
    });
  }
}
}
