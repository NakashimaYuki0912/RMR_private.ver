// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_Stigma8_0Up
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_Stigma8_0Up.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_Stigma8_0Up</summary>

public class DiceCardSelfAbility_Stigma8_0Up : DiceCardSelfAbilityBase
{
  public override bool IsTargetableAllUnit() => true;

  public override void OnUseInstance(
    BattleUnitModel unit,
    BattleDiceCardModel self,
    BattleUnitModel targetUnit)
  {
    targetUnit.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Burn, 8);
  }
}
}
