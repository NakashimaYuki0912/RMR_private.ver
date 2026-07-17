// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_plutoCardLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_plutoCardLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_plutoCardLog</summary>

public class DiceCardSelfAbility_plutoCardLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Energy_Keyword" };
  }

  public override void OnDiscard(BattleUnitModel unit, BattleDiceCardModel self)
  {
    unit.cardSlotDetail.RecoverPlayPointByCard(2);
  }
}
}
