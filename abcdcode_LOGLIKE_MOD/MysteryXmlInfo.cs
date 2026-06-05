// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryXmlInfo
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

 
namespace abcdcode_LOGLIKE_MOD {

public class MysteryXmlInfo
{
  [XmlAttribute("WorkShopID")]
  public string WorkShopId;
  [XmlAttribute("Title")]
  public string Title;
  [XmlAttribute("ID")]
  public int _StageId;
  [XmlElement("Frame")]
  public List<MysteryFrameInfo> Frames;
  [XmlElement("script")]
  public string script;

  public MysteryFrameInfo GetFrame(int id)
  {
    return this.Frames.Find((Predicate<MysteryFrameInfo>) (x => x.FrameID == id));
  }

  public LorId StageId => new LorId(this.WorkShopId, this._StageId);

  public MysteryXmlInfo()
  {
    this.WorkShopId = "";
    this.Title = "";
    this._StageId = -1;
    this.script = "";
    this.Frames = new List<MysteryFrameInfo>();
  }
}
}
