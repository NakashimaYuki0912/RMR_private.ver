// -----------------------------------------------------------------------------
// Data model / enum / XML root: RewardPassiveInfo
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\RewardPassiveInfo.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Xml.Serialization;
using UnityEngine;

namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>RewardPassiveInfo</summary>

    public class RewardPassiveInfo
    {
        [XmlIgnore]
        public string workshopID = "";
        [XmlAttribute("PassiveType")]
        public RewardPassiveType passivetype;
        [XmlAttribute("RewardType")]
        public RewardType rewardtype = RewardType.Passive;
        [XmlAttribute("ShopType")]
        public ShopRewardType shoptype = ShopRewardType.Eternal;
        [XmlAttribute("ShopType2")]
        public ShopRewardType2 shoptype2 = ShopRewardType2.Default;
        [XmlAttribute("Rarity")]
        public Rarity passiverarity;
        [XmlAttribute("ID")]
        public int passiveid;
        [XmlAttribute("Price")]
        public int price = -1;
        [XmlAttribute("ArtWork")]
        public string artwork = "";
        [XmlAttribute("IconArtWork")]
        public string iconartwork = "";
        [XmlAttribute("TargetType")]
        public EmotionTargetType targettype = EmotionTargetType.SelectOne;
        [XmlAttribute("Script")]
        public string script = "";
        [XmlAttribute("Level")]
        public int level = 1;

        [XmlIgnore]
        public Sprite Artwork
        {
            get
            {
                try
                {
                    return this.workshopID == LogLikeMod.ModId ? LogLikeMod.ArtWorks[this.artwork] : LogLikeMod.ModdedArtWorks[(this.id.packageId, this.artwork)];
                } catch
                {
                    return null;
                }
            }
        }

        [XmlIgnore]
        public LorId id => new LorId(this.workshopID, this.passiveid);
    }
}
