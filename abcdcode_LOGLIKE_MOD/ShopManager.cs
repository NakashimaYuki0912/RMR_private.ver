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
    if ( LogLikeMod.DefFont ==  null)
    {
      LogLikeMod.DefFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
      LogLikeMod.DefFontColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
      LogLikeMod.DefFont_TMP = (SingletonBehavior<UIPopupWindowManager>.Instance.popupPanels[1] as UIOptionWindow).displayDropdown.itemText.font;
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
