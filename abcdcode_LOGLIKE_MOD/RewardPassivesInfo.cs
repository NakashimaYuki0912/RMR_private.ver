// -----------------------------------------------------------------------------
// Data model / enum / XML root: RewardPassivesInfo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\RewardPassivesInfo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml.Serialization;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>RewardPassivesInfo</summary>

    public class RewardPassivesInfo
    {
        [XmlAttribute("Chapter")]
        public ChapterGrade chapter;
        [XmlAttribute("Type")]
        public PassiveRewardListType rewardtype;
        [XmlAttribute("WorkShopID")]
        public string workshopid = "";
        [XmlAttribute("ID")]
        public int listid = -1;
        [XmlElement("RewardList")]
        public List<RewardPassiveInfo> RewardPassiveList;

        public LorId Id => new LorId(this.workshopid, this.listid);
    }
}
