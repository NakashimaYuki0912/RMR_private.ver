// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_OrdealMachineDawn
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_OrdealMachineDawn.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Mystery node model: MysteryModel_OrdealMachineDawn</summary>

public class MysteryModel_OrdealMachineDawn : MysteryModel_OrdealBase
{
  public override void SwapFrame(int id)
  {
    this.ShowOrdealEndInfo("MachineDawnEnd", new Color(0.4117647f, 0.6431373f, 0.282352954f), "Machine_End");
  }
}
}
