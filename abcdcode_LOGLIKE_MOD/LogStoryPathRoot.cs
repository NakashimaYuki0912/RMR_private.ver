// -----------------------------------------------------------------------------
// LOGLIKE core UI/data: LogStoryPathRoot
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogStoryPathRoot.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml.Serialization;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>LOGLIKE type: LogStoryPathRoot</summary>

    public class LogStoryPathRoot
    {
        [XmlElement("StoryPathList")]
        public List<LogStoryPathInfo> list;
    }
}
