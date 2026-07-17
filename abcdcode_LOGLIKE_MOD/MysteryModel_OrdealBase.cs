// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_OrdealBase
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_OrdealBase.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using GameSave;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Mystery node model: MysteryModel_OrdealBase</summary>

public class MysteryModel_OrdealBase : MysteryBase
{
  public override void LoadFromSaveData(SaveData savedata)
  {
  }

  public override void SwapFrame(int id)
  {
    Singleton<MysteryManager>.Instance.EndMystery((MysteryBase) this);
  }

  public void EndOrdeal() => Singleton<MysteryManager>.Instance.EndMystery((MysteryBase) this);

  public void ShowOrdealEndInfo(string id, Color color, string audioname)
  {
    Singleton<OrdealTextManager>.Instance.SetOrdeal(id, color, audioname, new OrdealTextManager.OrdealEnd(this.EndOrdeal));
  }
}
}
