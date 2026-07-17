// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood28
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood28.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood28 : ShopPickUpModel
    {
        public PickUpModel_ShopGood28()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570028));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90028);
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood28.Shop28Effect());
        }

        /// <summary>Shop component: Shop28Effect</summary>

        public class Shop28Effect : OnceEffect
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive28"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570028));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570028));
            }

            public override void OnClick()
            {
                base.OnClick();
                if (LogLikeMod.curstagetype == StageType.Boss)
                    return;
                MysteryBase.AddStageList(Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 855)), LogLikeMod.curchaptergrade);
                this.Use();
            }
        }
    }
}
