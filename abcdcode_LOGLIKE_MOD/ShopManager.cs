// -----------------------------------------------------------------------------
// Library of Ruina mod script: ShopManager
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\ShopManager.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UI;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Shop component: ShopManager</summary>

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
