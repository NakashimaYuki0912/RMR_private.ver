// -----------------------------------------------------------------------------
// Library of Ruina mod script: MysteryAnimOrderData
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryAnimOrderData.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>MysteryAnimOrderData</summary>

public class MysteryAnimOrderData
{
  public MysteryAnimOrderData.AnimType type;
  public float time;
  public List<string> targetobjgroup;
  public MysteryAnimOrderData.CustomAction action;

  public MysteryAnimOrderData(string objname)
  {
    this.time = 1f;
    this.type = MysteryAnimOrderData.AnimType.FadeIn;
    this.action = (MysteryAnimOrderData.CustomAction) null;
    this.targetobjgroup = new List<string>();
    this.targetobjgroup.Add(objname);
  }

  public MysteryAnimOrderData(List<string> objname)
  {
    this.time = 1f;
    this.type = MysteryAnimOrderData.AnimType.FadeIn;
    this.action = (MysteryAnimOrderData.CustomAction) null;
    this.targetobjgroup = new List<string>((IEnumerable<string>) objname);
  }

  public MysteryAnimOrderData(
    float time,
    MysteryAnimOrderData.AnimType type,
    string objname,
    MysteryAnimOrderData.CustomAction action = null)
  {
    this.time = time;
    this.type = type;
    this.action = action;
    this.targetobjgroup = new List<string>();
    this.targetobjgroup.Add(objname);
  }

  public MysteryAnimOrderData(
    float time,
    MysteryAnimOrderData.AnimType type,
    List<string> objnames,
    MysteryAnimOrderData.CustomAction action = null)
  {
    this.time = time;
    this.type = type;
    this.action = action;
    this.targetobjgroup = new List<string>((IEnumerable<string>) objnames);
  }

  /// <summary>enum AnimType</summary>

  public enum AnimType
  {
    FadeIn,
    FadeInGroup,
    Custom,
  }

  public delegate void CustomAction();
}
}
