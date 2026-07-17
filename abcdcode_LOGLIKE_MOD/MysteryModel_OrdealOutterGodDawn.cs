// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_OrdealOutterGodDawn
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_OrdealOutterGodDawn.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Mystery node model: MysteryModel_OrdealOutterGodDawn</summary>

public class MysteryModel_OrdealOutterGodDawn : MysteryModel_OrdealBase
{
  public override void SwapFrame(int id)
  {
    Color color;
    ColorUtility.TryParseHtmlString("#9F68EAFF", out color);
    this.ShowOrdealEndInfo("OutterGodDawnEnd", color, "OutterGod_End");
  }
}
}
