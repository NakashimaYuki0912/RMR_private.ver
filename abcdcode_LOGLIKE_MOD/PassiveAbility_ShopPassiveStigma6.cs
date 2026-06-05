// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveStigma6
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassiveStigma6 : PassiveAbilityBase
{
  public override string debugDesc => "화상 상태의 적에게 공격 적중 시 즉시 화상 피해를 입힘";

  public override void OnSucceedAttack(BattleDiceBehavior behavior)
  {
    base.OnSucceedAttack(behavior);
    if (!(behavior.card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Burn) is BattleUnitBuf_burn activatedBuf))
      return;
    activatedBuf.OnRoundEnd();
  }
}
}
