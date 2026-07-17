// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood31
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood31.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood31 : ShopPickUpModel
    {
        public PickUpModel_ShopGood31()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570031));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90031);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood31.Shop31Effect());
        }

        /// <summary>Shop component: Shop31Effect</summary>

        public class Shop31Effect : GlobalLogueEffectBase
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive31"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570031));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570031));
            }

            public override void ChangeRestChoice(MysteryBase currest, ref List<RewardPassiveInfo> choices)
            {
                base.ChangeRestChoice(currest, ref choices);
                RewardPassiveInfo passiveInfo = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 800004));
                choices.Add(passiveInfo);
            }
        }
    }
}
