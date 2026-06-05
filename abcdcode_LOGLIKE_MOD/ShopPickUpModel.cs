// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.ShopPickUpModel
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using RogueLike_Mod_Reborn;
using System;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    public class ShopPickUpModel : PickUpModelBase
    {
        public PassiveXmlInfo basepassive;

        public virtual string[] Keywords => new string[0];

        /// <summary>
        /// Do <b>NOT</b> forget to inherit this constructor on your derived <see cref="ShopPickUpModel"/>.
        /// <code>
        /// // You can do it like this:
        /// public class PickUpModel_MyCoolItem : ShopPickUpModel
        /// {
        ///     public PickUpModel_MyCoolItem() : base() // do not forget this : base() part
        ///     {
        ///         this.id = new LorId(myModInitializer.packageId, 1984);
        ///         this.rewardinfo = RewardPassivesList.Instance.GetPassiveInfo(new LorId(myModInitializer.packageId, 1984));
        ///     }
        /// }</code>
        /// </summary>
        public ShopPickUpModel()
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

        /// <summary>
        /// Adds a key page to the player's inventory.
        /// </summary>
        /// <param name="id">The LorId of the key page.</param>
        public static void AddEquipPage(LorId id)
        {
            BookModel bookModel = new BookModel(Singleton<BookXmlList>.Instance.GetData(id));
            bookModel.instanceId = LogueBookModels.nextinstanceid++;
            bookModel.TryGainUniquePassive();
            LogueBookModels.booklist.Add(bookModel);
        }

        /// <summary>
        /// Adds a passive reward immediately.
        /// </summary>
        public static void AddPassiveReward(RewardInfo info)
        {
            LogLikeMod.rewards_InStage.Add(info);
            if (Singleton<ShopManager>.Instance.curshop == null)
                return;
            Singleton<ShopManager>.Instance.curshop.HideShop();
        }

        /// <summary>
        /// Adds a passive reward immediately by the reward's LorId.
        /// </summary>
        public static void AddPassiveReward(LorId id)
        {
            RewardInfo rewardInfo = new RewardInfo()
            {
                grade = ChapterGrade.GradeAll,
                rewards = new List<RewardPassiveInfo>()
            };
            rewardInfo.rewards.Add(Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id));
            LogLikeMod.rewards_InStage.Add(rewardInfo);
            if (Singleton<ShopManager>.Instance.curshop == null)
                return;
            Singleton<ShopManager>.Instance.curshop.HideShop();
        }

        /// <summary>
        /// Edits the name of the item upon creating it in a shop.<br></br>
        /// Can be edited directly (ref keyword).
        /// </summary>
        public virtual void EditName(ref string name)
        {
        }

        /// <summary>
        /// Edits the description of the item upon creating it in a shop.<br></br>
        /// Can be edited directly (ref keyword).
        /// </summary>
        public virtual void EditDesc(ref string desc)
        {
            if (this.Keywords.Length == 0)
                return;
            for (int index = 0; index < this.Keywords.Length; ++index)
            {
                desc += Environment.NewLine;
                desc += Environment.NewLine;
                desc = desc + Singleton<BattleEffectTextsXmlList>.Instance.GetEffectTextName(this.Keywords[index]) + Environment.NewLine + Singleton<BattleEffectTextsXmlList>.Instance.GetEffectTextDesc(this.Keywords[index]);
            }
            
        }

        /// <summary>
        /// Whether the item is consumable or permanent.
        /// </summary>
        public virtual ShopRewardType GetShopType()
        {
            return Singleton<RewardPassivesList>.Instance.GetPassiveInfo(this.id).shoptype;
        }

        /// <summary>
        /// Whether the item is a key page reward or not.
        /// </summary>
        public virtual bool IsEquipReward() => false;

        /// <summary>
        /// Whether the item can show up in a shop or not.
        /// </summary>
        public virtual bool IsCanAddShop() => true;

        /// <summary>
        /// Runs upon being bought in a shop.
        /// </summary>
        public virtual void OnPickUpShop(ShopGoods good) { }

        /// <summary>
        /// Executes upon being picked up or added via ShopPickUpModel.AddPassiveReward().<br></br>
        /// Is <b>NOT</b> ran upon being bought in a shop.
        /// </summary>
        public override void OnPickUp() => base.OnPickUp();
    }
}
