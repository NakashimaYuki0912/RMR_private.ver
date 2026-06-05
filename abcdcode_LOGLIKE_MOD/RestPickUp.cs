// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.RestPickUp
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using RogueLike_Mod_Reborn;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    public class RestPickUp : PickUpModelBase
    {
        public RestPickUp.RestPickUpType type;
        public PassiveXmlInfo basepassive;

        public RestPickUp()
        {
            if (!string.IsNullOrEmpty(this.KeywordId))
            {
                var info = LogueEffectXmlList.Instance.GetEffectInfo(KeywordId, RMRCore.ClassIds[this.GetType().Assembly.FullName]);
                if (info != null)
                {
                    this.Name = info.Name;
                    this.Desc = info.Desc;
                    this.FlaverText = info.FlavorText;
                    this.ArtWork = KeywordIconId ?? KeywordId;
                }
            }
        }

        public virtual bool CheckCondition() => true;

        public virtual void Init()
        {
        }

        public static void AddPassiveReward(LorId id)
        {
            RewardInfo rewardInfo = new RewardInfo()
            {
                grade = ChapterGrade.GradeAll,
                rewards = new List<RewardPassiveInfo>()
            };
            rewardInfo.rewards.Add(Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id));
            LogLikeMod.rewards_InStage.Add(rewardInfo);
            if (Singleton<MysteryManager>.Instance.curMystery == null || !(Singleton<MysteryManager>.Instance.curMystery is MysteryModel_Rest))
                return;
            (Singleton<MysteryManager>.Instance.curMystery as MysteryModel_Rest).HideRest();
        }

        public virtual bool OnChoiceOther(RestGood other)
        {
            return this.type != RestPickUp.RestPickUpType.Main || other.GoodScript.type != RestPickUp.RestPickUpType.Main;
        }

        public virtual void OnChoice(RestGood good)
        {
        }

        public enum RestPickUpType
        {
            Main,
            Sub,
            Etc,
        }
    }
}
