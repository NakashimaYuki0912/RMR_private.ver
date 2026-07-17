// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood22
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood22.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood22 : ShopPickUpModel
    {
        public PickUpModel_ShopGood22()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570022));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90022);
        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return base.IsCanPickUp(target) && !target.IsDead();
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            model.bufListDetail.RemoveBufAll(KeywordBuf.Bleeding);
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(model, model.faction, model.hp, model.breakDetail.breakGauge, model.bufListDetail.GetBufUIDataList());
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood22.Shop22Effect());
        }

        /// <summary>Shop component: Shop22Effect</summary>

        public class Shop22Effect : OnceEffect
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive22"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570022));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570022));
            }

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                    return;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90022));
                this.Use();
            }
        }
    }
}
