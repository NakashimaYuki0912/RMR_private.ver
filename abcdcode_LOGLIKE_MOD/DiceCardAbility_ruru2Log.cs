// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_ruru2Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_ruru2Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_ruru2Log</summary>

public class DiceCardAbility_ruru2Log : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Burn_Keyword" };
  }

  public override void OnSucceedAttack()
  {
    this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 3, this.owner);
    this.owner.TakeDamage(2, DamageType.Card_Ability);
  }
}
}
