// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveStigma3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveStigma3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveStigma3</summary>

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
