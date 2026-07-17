// -----------------------------------------------------------------------------
// Library of Ruina mod script: DebugEffect_Damage
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DebugEffect_Damage.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>DebugEffect_Damage</summary>

public class DebugEffect_Damage : GlobalLogueEffectBase
{
  public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive_Test"];

  public override void OnClick()
  {
    base.OnClick();
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
      alive.TakeDamage(10);
    SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfileAll();
  }
}
}
