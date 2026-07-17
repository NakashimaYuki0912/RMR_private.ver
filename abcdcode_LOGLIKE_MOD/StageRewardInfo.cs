// -----------------------------------------------------------------------------
// Data model / enum / XML root: StageRewardInfo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\StageRewardInfo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Xml.Serialization;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>StageRewardInfo</summary>

public class StageRewardInfo
{
  [XmlElement("workshopid")]
  public string Dropworkshopid;
  [XmlElement("id")]
  public int Dropid;
  [XmlElement("type")]
  public DropType rewardType;
}
}
