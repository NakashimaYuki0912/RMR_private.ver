// -----------------------------------------------------------------------------
// LOGLIKE core UI/data: LogStoryPathInfo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogStoryPathInfo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Xml.Serialization;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>LOGLIKE type: LogStoryPathInfo</summary>

public class LogStoryPathInfo
{
  [XmlAttribute("Id")]
  public int id;
  [XmlAttribute("WorkshopId")]
  public string pid = string.Empty;
  [XmlAttribute("Text")]
  public string localizepath;
  [XmlAttribute("Effect")]
  public string effectpath;
  [XmlAttribute("chapter")]
  public int chapter = -1;
  [XmlAttribute("group")]
  public int group = -1;
  [XmlAttribute("episode")]
  public int episode = -1;

  public LorId Id => new LorId(this.pid, this.id);
}
}
