// -----------------------------------------------------------------------------
// Library of Ruina mod script: MysteryChoiceInfo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryChoiceInfo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Xml.Serialization;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>MysteryChoiceInfo</summary>

public class MysteryChoiceInfo
{
  [XmlAttribute("ID")]
  public int ChoiceID;
  [XmlElement("Desc")]
  public string desc = string.Empty;
  [XmlElement("Next")]
  public int next;
}
}
