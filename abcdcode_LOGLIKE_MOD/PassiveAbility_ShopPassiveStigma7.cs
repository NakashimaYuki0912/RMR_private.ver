// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveStigma7
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveStigma7.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveStigma7</summary>

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
