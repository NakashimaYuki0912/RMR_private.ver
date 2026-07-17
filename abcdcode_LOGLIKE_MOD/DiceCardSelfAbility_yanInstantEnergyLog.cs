// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_yanInstantEnergyLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_yanInstantEnergyLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_yanInstantEnergyLog</summary>

public class DiceCardSelfAbility_yanInstantEnergyLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Energy_Keyword" };
  }

  public override void OnUseInstance(
    BattleUnitModel unit,
    BattleDiceCardModel self,
    BattleUnitModel targetUnit)
  {
    int playPoint = unit.cardSlotDetail.PlayPoint;
    int num = unit.cardSlotDetail.RecoverPlayPointByCard(5);
    if (num <= playPoint)
      return;
    unit.cardSlotDetail.LoseWhenStartRound(num - playPoint);
  }
}
}
