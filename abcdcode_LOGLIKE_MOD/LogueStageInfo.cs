// -----------------------------------------------------------------------------
// LOGLIKE core UI/data: LogueStageInfo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogueStageInfo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Xml.Serialization;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>LOGLIKE type: LogueStageInfo</summary>

    public class LogueStageInfo
    {
        [XmlAttribute("WorkShopId")]
        public string workshopid = "";
        [XmlAttribute("StageType")]
        public StageType type;
        [XmlAttribute("ID")]
        public int stageid = int.MinValue;
        [XmlAttribute("Script")]
        public string script = "";

        public LogueStageInfo Copy()
        {
            return new LogueStageInfo()
            {
                workshopid = this.workshopid,
                type = this.type,
                stageid = this.stageid,
                script = this.script
            };
        }

        public LorId Id => new LorId(this.workshopid, this.stageid);
    }
}
