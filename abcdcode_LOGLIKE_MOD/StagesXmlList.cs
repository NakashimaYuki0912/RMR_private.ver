// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.StagesXmlList
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using System.Linq;


namespace abcdcode_LOGLIKE_MOD
{

    public class StagesXmlList : Singleton<StagesXmlList>
    {
        public List<StagesXmlInfo> infos;
        private StagesXmlInfo[] _allInfos;
        public void Init(List<StagesXmlInfo> info)
        {
            this.infos = info;
            foreach (StagesXmlInfo stagesXmlInfo in info)
            {
                foreach (LogueStageInfo stage in stagesXmlInfo.Stages)
                    LogLikeMod.RegisterPickUpXml(stage);
            }
            this._allInfos = new StagesXmlInfo[info.Count];
            info.CopyTo(this._allInfos);
        }

        public List<LogueStageInfo> GetChapterData(ChapterGrade chapter, bool ExceptAllGrade = false)
        {
            List<LogueStageInfo> chapterData = new List<LogueStageInfo>();
            foreach (StagesXmlInfo stagesXmlInfo in this.infos.FindAll(x =>
            {
                if (x.chapter == chapter)
                    return true;
                return !ExceptAllGrade && x.chapter == ChapterGrade.GradeAll;
            }))
                chapterData.AddRange(stagesXmlInfo.Stages);
            return chapterData;
        }

        public List<LogueStageInfo> GetChapterDataVanilla(ChapterGrade chapter, bool ExceptAllGrade = false)
        {
            return GetChapterDataSpecific(chapter, new LorId(LogLikeMod.ModId, 0), ExceptAllGrade);
        }

        /// <summary>
        /// Returns a list containing every single reception for a given chapter for a specific campaign.
        /// </summary>
        /// <param name="chapter">The chapter to filter by.</param>
        /// <param name="id">The campaign ID of the campaign.</param>
        /// <param name="ExceptAllGrade">Whether or not to exclude encounters that can show up in any chapter.</param>
        /// <returns></returns>
        public List<LogueStageInfo> GetChapterDataSpecific(ChapterGrade chapter, LorId id, bool ExceptAllGrade = false)
        {
            List<LogueStageInfo> chapterData = new List<LogueStageInfo>();
            foreach (StagesXmlInfo stagesXmlInfo in this.infos.FindAll(x =>
            {
                if (x.Id != id)
                    return false;
                if (x.chapter == chapter)
                    return true;
                return !ExceptAllGrade && x.chapter == ChapterGrade.GradeAll;
            }))
                chapterData.AddRange(stagesXmlInfo.Stages);
            return chapterData;
        }

        public LogueStageInfo GetStageInfo(LorId stageid)
        {
            foreach (StagesXmlInfo info in this.infos)
            {
                foreach (LogueStageInfo stage in info.Stages)
                {
                    if (stage.Id == stageid)
                        return stage.Copy();
                }
            }
            return (LogueStageInfo)null;
        }

        public void RestoreToDefault()
        {
            this.infos.Clear();
            this.infos.AddRange((IEnumerable<StagesXmlInfo>)((IEnumerable<StagesXmlInfo>)this._allInfos).ToList<StagesXmlInfo>());
        }
    }
}
