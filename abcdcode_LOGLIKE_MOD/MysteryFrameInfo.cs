// -----------------------------------------------------------------------------
// Library of Ruina mod script: MysteryFrameInfo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryFrameInfo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>MysteryFrameInfo</summary>

public class MysteryFrameInfo
{
  [XmlAttribute("ID")]
  public int FrameID;
  [XmlAttribute("FrameType")]
  public FrameType FrameType;
  [XmlElement("Dialog")]
  public List<string> Dialog;
  [XmlElement("ArtWork")]
  public string ArtWork;
  [XmlElement("Choice")]
  public List<MysteryChoiceInfo> choices;

  public MysteryChoiceInfo Getchoice(int id)
  {
    return this.choices.Find((Predicate<MysteryChoiceInfo>) (x => x.ChoiceID == id));
  }
}
}
