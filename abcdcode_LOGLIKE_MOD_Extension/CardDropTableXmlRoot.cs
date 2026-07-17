// -----------------------------------------------------------------------------
// LOGLIKE XML extension model: CardDropTableXmlRoot
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD_Extension\CardDropTableXmlRoot.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml.Serialization;


namespace abcdcode_LOGLIKE_MOD_Extension
{

    /// <summary>CardDropTableXmlRoot</summary>

    public class CardDropTableXmlRoot
    {
        [XmlElement("Version")]
        public string version = "1.1";
        [XmlElement("DropTable")]
        public List<CardDropTableXmlInfo> dropTableXmlList = new List<CardDropTableXmlInfo>();
    }
}
