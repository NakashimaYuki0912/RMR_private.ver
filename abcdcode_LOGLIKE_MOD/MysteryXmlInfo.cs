// -----------------------------------------------------------------------------
// Data model / enum / XML root: MysteryXmlInfo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryXmlInfo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>MysteryXmlInfo</summary>

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
