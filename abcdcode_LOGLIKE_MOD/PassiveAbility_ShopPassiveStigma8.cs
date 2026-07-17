// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveStigma8
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveStigma8.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveStigma8</summary>

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
