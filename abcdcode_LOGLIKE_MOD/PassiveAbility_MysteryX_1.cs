// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_MysteryX_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_MysteryX_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_MysteryX_1</summary>

public class PassiveAbility_MysteryX_1 : PassiveAbilityBase
{
  public override void OnWaveStart()
  {
    base.OnWaveStart();
    Singleton<MysteryManager>.Instance.StartMystery(Singleton<MysteryXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 100001)));
  }
}
}
