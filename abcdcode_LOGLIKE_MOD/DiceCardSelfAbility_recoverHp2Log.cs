// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_recoverHp2Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_recoverHp2Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_recoverHp2Log</summary>

public class DiceCardSelfAbility_recoverHp2Log : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Recover_Keyword" };
  }

  public override void OnUseCard() => this.owner.RecoverHP(2);
}
}
