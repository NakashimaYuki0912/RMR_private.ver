// -----------------------------------------------------------------------------
// Library of Ruina mod script: RestPickUp
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\RestPickUp.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using RogueLike_Mod_Reborn;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>RestPickUp</summary>

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

        /// <summary>enum RestPickUpType</summary>

        public enum RestPickUpType
        {
            Main,
            Sub,
            Etc,
        }
    }
}
