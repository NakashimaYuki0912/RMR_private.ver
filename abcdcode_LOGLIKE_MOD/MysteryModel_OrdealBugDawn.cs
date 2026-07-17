// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_OrdealBugDawn
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_OrdealBugDawn.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Mystery node model: MysteryModel_OrdealBugDawn</summary>

public class MysteryModel_OrdealBugDawn : MysteryModel_OrdealBase
{
  public override void SwapFrame(int id)
  {
    Color color;
    ColorUtility.TryParseHtmlString("#FE960BFF", out color);
    this.ShowOrdealEndInfo("BugDawnEnd", color, "Bug_End");
  }
}
}
