// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.RewardPassivesList
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using System.Linq;


namespace abcdcode_LOGLIKE_MOD
{

    public class RewardPassivesList : Singleton<RewardPassivesList>
    {
        public List<RewardPassivesInfo> infos;
        private RewardPassivesInfo[] _allInfos;

        public void Init(List<RewardPassivesInfo> info)
        {
            this.infos = info;
            foreach (RewardPassivesInfo rewardPassivesInfo in info)
            {
                foreach (RewardPassiveInfo rewardPassive in rewardPassivesInfo.RewardPassiveList)
                    LogLikeMod.RegisterPickUpXml(rewardPassive);
            }
            this._allInfos = new RewardPassivesInfo[info.Count];
            info.CopyTo(this._allInfos);
        }

        public List<RewardPassiveInfo> GetChapterData(Predicate<RewardPassivesInfo> predicate)
        {
            List<RewardPassiveInfo> chapterData = new List<RewardPassiveInfo>();
            foreach (RewardPassivesInfo rewardPassivesInfo in this.infos.FindAll(predicate))
                chapterData.AddRange(rewardPassivesInfo.RewardPassiveList);
            return chapterData;
        }

        public List<RewardPassiveInfo> GetChapterData(
          ChapterGrade chapter,
          PassiveRewardListType type,
          LorId id,
          bool ExceptionAll = false)
        {
            List<RewardPassiveInfo> rewardPassiveInfoList = new List<RewardPassiveInfo>();
            List<RewardPassivesInfo> all = this.infos.FindAll(x =>
            {
                if (x.chapter != chapter && (x.chapter != ChapterGrade.GradeAll || ExceptionAll) || x.rewardtype != type)
                    return false;
                return id == -1 || x.Id == id;
            });
            List<RewardPassiveInfo> chapterData;
            if (all.Count == 0)
            {
                chapterData = null;
            }
            else
            {
                foreach (RewardPassivesInfo rewardPassivesInfo in all)
                    rewardPassiveInfoList.AddRange(rewardPassivesInfo.RewardPassiveList);
                chapterData = rewardPassiveInfoList;
            }
            return chapterData;
        }

        public List<RewardPassiveInfo> GetChapterData(ChapterGrade chapter)
        {
            List<RewardPassiveInfo> chapterData = new List<RewardPassiveInfo>();
            foreach (RewardPassivesInfo rewardPassivesInfo in this.infos.FindAll((Predicate<RewardPassivesInfo>)(x => x.chapter == chapter || x.chapter == ChapterGrade.GradeAll)))
                chapterData.AddRange((IEnumerable<RewardPassiveInfo>)rewardPassivesInfo.RewardPassiveList);
            return chapterData;
        }

        public RewardPassiveInfo GetPassiveInfo(LorId passiveid)
        {
            foreach (RewardPassivesInfo info in this.infos)
            {
                foreach (RewardPassiveInfo rewardPassive in info.RewardPassiveList)
                {
                    if (rewardPassive.id == passiveid)
                        return rewardPassive;
                }
            }
            return (RewardPassiveInfo)null;
        }

        public RewardPassiveInfo GetPassiveInfo_inlist(
          int passiveid,
          List<RewardPassiveInfo> passiveinfos)
        {
            foreach (RewardPassiveInfo passiveinfo in passiveinfos)
            {
                if (passiveinfo.passiveid == passiveid)
                    return passiveinfo;
            }
            return (RewardPassiveInfo)null;
        }

        public void RestoreToDefault()
        {
            this.infos.Clear();
            this.infos.AddRange((IEnumerable<RewardPassivesInfo>)((IEnumerable<RewardPassivesInfo>)this._allInfos).ToList<RewardPassivesInfo>());
        }
    }
}
