// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogueStageInfo
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Xml.Serialization;


namespace abcdcode_LOGLIKE_MOD
{

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
