// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_Mystery4_2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_Mystery4_2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_Mystery4_2</summary>

public class PassiveAbility_Mystery4_2 : PassiveAbilityBase
{
  public override void OnWaveStart()
  {
    base.OnWaveStart();
    Singleton<MysteryManager>.Instance.StartMystery(Singleton<MysteryXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 30002)));
  }
}
}
