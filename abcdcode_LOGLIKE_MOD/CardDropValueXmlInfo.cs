// -----------------------------------------------------------------------------
// Data model / enum / XML root: CardDropValueXmlInfo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\CardDropValueXmlInfo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Xml.Serialization;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>CardDropValueXmlInfo</summary>

    public class CardDropValueXmlInfo
    {
        [XmlAttribute("WorkShopID")]
        public string workshopID = "";
        [XmlAttribute("ID")]
        public int _id;
        [XmlAttribute("DropType")]
        public DropType droptype = DropType.Card;
        [XmlElement("CommonValue")]
        public int CommonValue = 0;
        [XmlElement("UncommonValue")]
        public int UncommonValue = 0;
        [XmlElement("RareValue")]
        public int RareValue = 0;
        [XmlElement("UniqueValue")]
        public int UniqueValue = 0;
        [XmlElement("TableId")]
        public int DropTableId;

        public static CardDropValueXmlInfo Copying(CardDropValueXmlInfo baseinfo)
        {
            return new CardDropValueXmlInfo()
            {
                workshopID = baseinfo.workshopID,
                _id = baseinfo._id,
                droptype = baseinfo.droptype,
                CommonValue = baseinfo.CommonValue,
                UncommonValue = baseinfo.UncommonValue,
                RareValue = baseinfo.RareValue,
                UniqueValue = baseinfo.UniqueValue,
                DropTableId = baseinfo.DropTableId
            };
        }

        [XmlIgnore]
        public LorId Id => new LorId(this.workshopID, this._id);
    }
}
