// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_Stigma8_0
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_Stigma8_0.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_Stigma8_0</summary>

public class DiceCardSelfAbility_Stigma8_0 : DiceCardSelfAbilityBase
{
  public static string Desc = "[장착 시 발동] 대상에게 화상 5 부여";

  public override bool IsTargetableAllUnit() => true;

  public override void OnUseInstance(
    BattleUnitModel unit,
    BattleDiceCardModel self,
    BattleUnitModel targetUnit)
  {
    targetUnit.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Burn, 5);
  }
}
}
