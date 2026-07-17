// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveMook6
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveMook6.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveMook6</summary>

public class PassiveAbility_ShopPassiveMook6 : PassiveAbilityBase
{
  public BattleDiceBehavior curdice;

  public override string debugDesc => "공격 주사위 하나로 최대 체력의 10% 또는 15 이상의 피해를 입히면 다음 막에 출혈 5, 마비 2 부여";

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    base.BeforeRollDice(behavior);
    this.curdice = behavior;
  }

  public override void AfterGiveDamage(int damage)
  {
    base.AfterGiveDamage(damage);
    if (this.curdice.card.target == null || damage < 15 && damage < this.curdice.card.target.MaxHp / 10)
      return;
    this.curdice.card.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 5);
    this.curdice.card.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Paralysis, 2);
  }
}
}
