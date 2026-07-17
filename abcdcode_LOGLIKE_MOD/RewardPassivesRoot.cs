// -----------------------------------------------------------------------------
// Data model / enum / XML root: RewardPassivesRoot
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\RewardPassivesRoot.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml.Serialization;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>RewardPassivesRoot</summary>

    public class RewardPassivesRoot
    {
        [XmlElement("ChapterList")]
        public List<RewardPassivesInfo> RewardPassivesList;
    }
}
