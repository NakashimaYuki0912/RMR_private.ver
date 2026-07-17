// -----------------------------------------------------------------------------
// Data model / enum / XML root: StagesXmlRoot
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\StagesXmlRoot.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml.Serialization;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>StagesXmlRoot</summary>

    public class StagesXmlRoot
    {
        [XmlElement("ChapterList")]
        public List<StagesXmlInfo> ChapterList;
    }
}
