// -----------------------------------------------------------------------------
// Data model / enum / XML root: CardDropValueXmlRoot
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\CardDropValueXmlRoot.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml.Serialization;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>CardDropValueXmlRoot</summary>

public class CardDropValueXmlRoot
{
  [XmlElement("DropValue")]
  public List<CardDropValueXmlInfo> DropValueXmlList;
}
}
