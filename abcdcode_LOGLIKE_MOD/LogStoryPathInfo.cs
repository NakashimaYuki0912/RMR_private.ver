// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogStoryPathInfo
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Xml.Serialization;

 
namespace abcdcode_LOGLIKE_MOD {

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
