// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassive26
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassive26 : PassiveAbilityBase
{
  public int stack;

  public override string debugDesc => "피해를 받으면 생명력을 1 회복함(막 당 최대 5)";

  public override void OnRoundStart()
  {
    base.OnRoundStart();
    this.stack = 5;
  }

  public override void AfterTakeDamage(BattleUnitModel attacker, int dmg)
  {
    base.AfterTakeDamage(attacker, dmg);
    if (this.stack <= 0)
      return;
    this.owner.RecoverHP(1);
    --this.stack;
  }
}
}
