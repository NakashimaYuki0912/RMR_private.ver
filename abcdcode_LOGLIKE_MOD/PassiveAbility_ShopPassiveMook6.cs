// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveMook6
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

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
