// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_discardAllEnergy1Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_discardAllEnergy1Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_discardAllEnergy1Log</summary>

public class DiceCardSelfAbility_discardAllEnergy1Log : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Energy_Keyword" };
  }

  public override void OnDiscard(BattleUnitModel unit, BattleDiceCardModel self)
  {
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(unit.faction))
      alive.cardSlotDetail.RecoverPlayPoint(1);
  }
}
}
