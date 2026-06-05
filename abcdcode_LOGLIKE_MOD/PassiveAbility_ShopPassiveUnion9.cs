// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveUnion9
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassiveUnion9 : PassiveAbilityBase
{
  public override string debugDesc => "흐트러짐 상태가 되면 현재 체력의 25%를 잃고 흐트러짐 상태에서 벗어남";

  public override bool OnBreakGageZero()
  {
    this.owner.breakDetail.ResetGauge();
    this.owner.LoseHp((int) this.owner.hp / 4);
    return true;
  }
}
}
