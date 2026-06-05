// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveMook3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassiveMook3 : PassiveAbilityBase
{
  public override string debugDesc => "내 체력이 50% 미만일때 피해량 + 2. 적 체력이 50% 미만이면 피해량 + 2";

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    base.BeforeRollDice(behavior);
    int num = 0;
    if ((double) this.owner.hp < (double) (this.owner.MaxHp / 2))
      num += 2;
    if ((double) behavior.card.target.hp < (double) (behavior.card.target.MaxHp / 2))
      num += 2;
    if (num <= 0)
      return;
    behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      dmg = num
    });
  }
}
}
