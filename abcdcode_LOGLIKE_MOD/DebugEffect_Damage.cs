// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DebugEffect_Damage
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

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
