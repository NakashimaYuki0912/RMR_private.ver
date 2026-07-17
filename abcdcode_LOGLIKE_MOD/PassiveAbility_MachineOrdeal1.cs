// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_MachineOrdeal1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_MachineOrdeal1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_MachineOrdeal1</summary>

public class PassiveAbility_MachineOrdeal1 : PassiveAbilityBase
{
  public override void OnLoseParrying(BattleDiceBehavior behavior)
  {
    base.OnLoseParrying(behavior);
    if (behavior.card.target == null || (double) Random.value <= 0.5)
      return;
    behavior.card.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1);
  }
}
}
