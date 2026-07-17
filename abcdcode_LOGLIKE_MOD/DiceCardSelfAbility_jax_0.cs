// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_jax_0
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_jax_0.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_jax_0</summary>

public class DiceCardSelfAbility_jax_0 : DiceCardSelfAbilityBase
{
  public static string Desc = "[장착 시 발동] 최대 체력의 10% 만큼 피해를 입고 이번막에 힘을 2 얻음";

  public override void OnUseInstance(
    BattleUnitModel unit,
    BattleDiceCardModel self,
    BattleUnitModel targetUnit)
  {
    unit.TakeDamage(unit.MaxHp / 10 > 0 ? unit.MaxHp / 10 : 1);
    unit.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 2);
  }
}
}
