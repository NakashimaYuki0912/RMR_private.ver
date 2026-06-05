// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveStigma3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassiveStigma3 : PassiveAbilityBase
{
  public override string debugDesc => "자신에게 화상이 있는 동안 다른 상태이상에 면역";

  public override bool IsImmune(KeywordBuf buf)
  {
    if (buf == KeywordBuf.Burn)
      return base.IsImmune(buf);
    return this.owner.bufListDetail.HasBuf<BattleUnitBuf_burn>() || base.IsImmune(buf);
  }
}
}
