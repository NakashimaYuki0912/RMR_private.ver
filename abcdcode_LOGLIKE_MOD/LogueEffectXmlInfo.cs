// -----------------------------------------------------------------------------
// LOGLIKE core UI/data: LogueEffectXmlInfo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogueEffectXmlInfo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Xml.Serialization;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>LOGLIKE type: LogueEffectXmlInfo</summary>

public class LogueEffectXmlInfo
{
  [XmlAttribute]
  public string Id;
  [XmlElement]
  public string Name;
  [XmlElement]
  public string Desc;
  [XmlElement]
  public string FlavorText;
  [XmlElement]
  public string CatalogDesc;

  public LogueEffectXmlInfo()
  {
    this.Id = "default";
    this.Name = "not found";
    this.Desc = "not found";
    this.FlavorText = "mmm tasty flavor text";
    this.CatalogDesc = "not found";
  }
}
}
