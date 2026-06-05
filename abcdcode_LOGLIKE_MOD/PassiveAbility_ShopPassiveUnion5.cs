// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveUnion5
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassiveUnion5 : UnionWeaponPassiveAbilityBase
{
  public BattleDiceBehavior last;

  public override string debugDesc
  {
    get => "(다른 생체무기와 중복 불가) 막 시작 시 체력을 5% 잃음. 합 패배 시 현재 주사위를 재사용(주사위 당 1번)";
  }

  public override void OnLoseParrying(BattleDiceBehavior behavior)
  {
    base.OnLoseParrying(behavior);
    if (this.last == behavior)
      return;
    behavior.isBonusAttack = true;
    this.last = behavior;
  }
}
}
