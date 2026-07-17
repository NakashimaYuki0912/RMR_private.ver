// -----------------------------------------------------------------------------
// LOGLIKE XML extension model: CardDropTableXmlInfo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD_Extension\CardDropTableXmlInfo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml.Serialization;


namespace abcdcode_LOGLIKE_MOD_Extension
{

    /// <summary>CardDropTableXmlInfo</summary>

    public class CardDropTableXmlInfo
    {
        [XmlAttribute("ID")]
        public int _id;
        [XmlAttribute("pid")]
        public string pid = "";
        [XmlElement("Card")]
        public List<LorIdXml> _cardIdList;
        [XmlIgnore]
        public List<LorId> cardIdList = new List<LorId>();
        [XmlIgnore]
        public List<int> _validCardIdList = new List<int>();
    }
}
