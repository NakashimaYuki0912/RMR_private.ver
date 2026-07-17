// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood30
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood30.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood30 : ShopPickUpModel
    {
        public PickUpModel_ShopGood30()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570030));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90030);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood30.Shop30Effect());
        }

        /// <summary>Shop component: Shop30Effect</summary>

        public class Shop30Effect : GlobalLogueEffectBase
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive30"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570030));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570030));
            }

            public override void ChangeRestChoice(MysteryBase currest, ref List<RewardPassiveInfo> choices)
            {
                base.ChangeRestChoice(currest, ref choices);
                RewardPassiveInfo rewardPassiveInfo = choices.Find((Predicate<RewardPassiveInfo>)(x => x.id == new LorId(LogLikeMod.ModId, 800001)));
                if (rewardPassiveInfo == null)
                    return;
                int index = choices.IndexOf(rewardPassiveInfo);
                RewardPassiveInfo passiveInfo = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 800005));
                choices.Insert(index, passiveInfo);
                choices.Remove(rewardPassiveInfo);
            }
        }
    }
}
