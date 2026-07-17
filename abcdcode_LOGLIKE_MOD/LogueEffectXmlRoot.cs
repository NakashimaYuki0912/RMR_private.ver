// -----------------------------------------------------------------------------
// LOGLIKE core UI/data: LogueEffectXmlRoot
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogueEffectXmlRoot.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml.Serialization;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>LOGLIKE type: LogueEffectXmlRoot</summary>

public class LogueEffectXmlRoot
{
  [XmlElement("LogueEffectInfo")]
  public List<LogueEffectXmlInfo> LogueEffectList;
}
}
