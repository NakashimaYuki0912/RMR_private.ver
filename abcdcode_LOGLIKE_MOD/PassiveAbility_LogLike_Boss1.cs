// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_LogLike_Boss1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_LogLike_Boss1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_LogLike_Boss1</summary>

public class PassiveAbility_LogLike_Boss1 : PassiveAbilityBase
{
  public override void OnRoundStart()
  {
    base.OnRoundStart();
    this.owner.allyCardDetail.DrawCards(1);
    this.owner.cardSlotDetail.RecoverPlayPoint(1);
  }
}
}
