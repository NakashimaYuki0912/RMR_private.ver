// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassive2_1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassive2_1 : PassiveAbilityBase
{
  public override string debugDesc => "적을 처치하면 모든 아군의 생명력 3 회복";

  public override void OnKill(BattleUnitModel target)
  {
    base.OnKill(target);
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction))
      alive.RecoverHP(3);
  }
}
}
