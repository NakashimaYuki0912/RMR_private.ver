// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_MemberShip
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_MemberShip.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_MemberShip</summary>

public class PassiveAbility_MemberShip : PassiveAbilityBase
{
  public override void OnWaveStart()
  {
    base.OnWaveStart();
    Singleton<MysteryManager>.Instance.StartMystery(Singleton<MysteryXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, -2)));
  }
}
}
