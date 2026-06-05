// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD_Extension.CardDropTableXmlRoot
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;
using System.Xml.Serialization;


namespace abcdcode_LOGLIKE_MOD_Extension
{

    public class CardDropTableXmlRoot
    {
        [XmlElement("Version")]
        public string version = "1.1";
        [XmlElement("DropTable")]
        public List<CardDropTableXmlInfo> dropTableXmlList = new List<CardDropTableXmlInfo>();
    }
}
