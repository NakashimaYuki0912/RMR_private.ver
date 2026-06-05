// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveStigma7
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassiveStigma7 : PassiveAbilityBase
{
  public override string debugDesc => "전투책장 사용 시 해당 전투책장의 원래 비용만큼 적에게 화상을 부여(비용이 0인 책장이라면 대신 1 부여)";

  public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
  {
    base.OnUseCard(curCard);
    int stack = curCard.card.GetOriginCost();
    if (stack == 0)
      stack = 1;
    curCard.target.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Burn, stack);
  }
}
}
