// -----------------------------------------------------------------------------
// Data model / enum / XML root: StagesXmlInfo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\StagesXmlInfo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml.Serialization;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>StagesXmlInfo</summary>

    public class StagesXmlInfo
    {
        [XmlAttribute("Chapter")]
        public ChapterGrade chapter;
        [XmlElement("StageList")]
        public List<LogueStageInfo> Stages;
        [XmlAttribute("CampaignId")]
        public int id = 0;
        [XmlAttribute("CampaignPackageId")]
        public string packageId = "";

        [XmlIgnore]
        public LorId Id => new LorId(this.packageId, this.id);
    }
}
