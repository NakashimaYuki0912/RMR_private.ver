// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.CardDropValueXmlInfo
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Xml.Serialization;


namespace abcdcode_LOGLIKE_MOD
{

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
