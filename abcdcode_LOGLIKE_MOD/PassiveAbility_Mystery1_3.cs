// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_Mystery1_3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_Mystery1_3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_Mystery1_3</summary>

public class PassiveAbility_Mystery1_3 : PassiveAbilityBase
{
  public override void OnWaveStart()
  {
    base.OnWaveStart();
    Singleton<MysteryManager>.Instance.StartMystery(Singleton<MysteryXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 3)));
  }
}
}
