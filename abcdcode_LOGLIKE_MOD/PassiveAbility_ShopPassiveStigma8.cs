// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveStigma8
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassiveStigma8 : PassiveAbilityBase
{
  public override string debugDesc => "접대 시작 시 '발화' 전투책장을 손으로 가져옴. 해당 책장 사용 시 지정한 대상에게 화상 5 부여";

  public override void OnWaveStart()
  {
    base.OnWaveStart();
    this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 8582008));
  }
}
}
