// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveMook1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveMook1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveMook1</summary>

public class PassiveAbility_ShopPassiveMook1 : PassiveAbilityBase
{
  public override string debugDesc => "책장의 첫 공격 주사위 위력 + 2. 나머지 주사위 위력 - 1";

  public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
  {
    base.OnUseCard(curCard);
    curCard.ApplyDiceStatBonus(DiceMatch.NextAttackDice, new DiceStatBonus()
    {
      power = 2
    });
    curCard.ApplyDiceStatBonus(ExtensionUtils.NotFirstAttackDice(), new DiceStatBonus()
    {
      power = -1
    });
  }
}
}
