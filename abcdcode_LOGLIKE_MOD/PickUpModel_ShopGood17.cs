// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood17
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood17.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood17 : ShopPickUpModel
    {
        public PickUpModel_ShopGood17() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570017));
            this.id = new LorId(LogLikeMod.ModId, 90017);
        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return base.IsCanPickUp(target) && !target.IsDead();
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            int stack1 = 3;
            model.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, stack1);
            int stack2 = 3;
            model.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.BreakProtection, stack2);
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(model, model.faction, model.hp, model.breakDetail.breakGauge, model.bufListDetail.GetBufUIDataList());
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood17.ImImmune());
        }

        public override string KeywordId => "GlobalEffect_IrnStim";
        public override string KeywordIconId => "ShopPassive17";

        /// <summary>ImImmune</summary>

        public class ImImmune : OnceEffect
        {
            public static Rarity ItemRarity = Rarity.Uncommon;

            public override Sprite GetSprite() => LogLikeMod.ArtWorks[""];

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                    return;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90017));
                this.Use();
            }

            public override string KeywordId => "GlobalEffect_IrnStim";
            public override string KeywordIconId => "ShopPassive17";
        }
    }
}
