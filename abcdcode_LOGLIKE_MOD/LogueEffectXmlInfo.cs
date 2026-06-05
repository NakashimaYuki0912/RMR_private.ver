// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogueEffectXmlInfo
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Xml.Serialization;

 
namespace abcdcode_LOGLIKE_MOD {

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
