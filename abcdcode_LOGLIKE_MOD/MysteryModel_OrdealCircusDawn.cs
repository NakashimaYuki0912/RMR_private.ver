// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_OrdealCircusDawn
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_OrdealCircusDawn.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Mystery node model: MysteryModel_OrdealCircusDawn</summary>

public class MysteryModel_OrdealCircusDawn : MysteryModel_OrdealBase
{
  public override void SwapFrame(int id)
  {
    Color color;
    ColorUtility.TryParseHtmlString("#DC143CFF", out color);
    this.ShowOrdealEndInfo("CircusDawnEnd", color, "Circus_End");
  }
}
}
