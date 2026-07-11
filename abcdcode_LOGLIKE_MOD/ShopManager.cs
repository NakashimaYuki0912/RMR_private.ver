// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.ShopManager
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UI;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class ShopManager : Singleton<ShopManager>
{
  public ShopBase curshop;

  public void OpenShop(string id = "Default")
  {
    if (LogLikeMod.DefFont == null)
    {
      LogLikeMod.DefFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
      LogLikeMod.DefFontColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
    }
    // Never assign options-dropdown TMP here — it is often Latin-only and was the
    // main source of Chinese tofu in the RMR shop. Resolve language-matched CJK.
    LogLikeMod.EnsureLocalizedFonts("OpenShop", repairActiveUi: true);
    // C2: combat page ability + keyword tooltips must match game language (not leftover EN).
    try
    {
      LogLikeMod.RefreshVanillaBattleLocalize(LogLikeMod.GetActiveTextLanguage(), "OpenShop");
    }
    catch (System.Exception ex)
    {
      UnityEngine.Debug.LogWarning("[RMR Localize] OpenShop battle refresh failed: " + ex.Message);
    }
    // Extra pass: re-load effect texts only (keywords) if battle refresh partially failed.
    try
    {
      var loader = Singleton<LocalizedTextLoader>.Instance;
      if (loader != null)
        loader.LoadBattleEffectTexts(LogLikeMod.GetActiveTextLanguage());
    }
    catch (System.Exception ex)
    {
      UnityEngine.Debug.LogWarning("[RMR Localize] OpenShop EffectTexts: " + ex.Message);
    }
    ShopBase shop = ShopBase.FindShop(id);
    shop.Init();
    this.curshop = shop;
  }

  public void CloseShop() => this.curshop = (ShopBase) null;

  public void RemoveShop()
  {
    if (this.curshop == null)
      return;
    this.curshop.RemoveShop();
  }
}
}
