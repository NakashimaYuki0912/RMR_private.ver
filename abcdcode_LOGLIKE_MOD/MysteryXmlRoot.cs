// -----------------------------------------------------------------------------
// Data model / enum / XML root: MysteryXmlRoot
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryXmlRoot.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml.Serialization;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>MysteryXmlRoot</summary>

public class MysteryXmlRoot
{
  [XmlElement("Mystery")]
  public List<MysteryXmlInfo> Mysterys;
}
}
