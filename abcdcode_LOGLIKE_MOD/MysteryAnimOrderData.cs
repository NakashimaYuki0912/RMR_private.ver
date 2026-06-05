// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryAnimOrderData
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

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

  public enum AnimType
  {
    FadeIn,
    FadeInGroup,
    Custom,
  }

  public delegate void CustomAction();
}
}
