// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood15
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood15.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood15 : ShopPickUpModel
    {
        public PickUpModel_ShopGood15() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570015));
            this.id = new LorId(LogLikeMod.ModId, 90015);
        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return base.IsCanPickUp(target) && !target.IsDead();
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            int stack = 3;
            model.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, stack);
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(model, model.faction, model.hp, model.breakDetail.breakGauge, model.bufListDetail.GetBufUIDataList());
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood15.QuickService());
        }
        public override string KeywordId => "GlobalEffect_W1dStim";
        public override string KeywordIconId => "ShopPassive15";

        /// <summary>QuickService</summary>

        public class QuickService : OnceEffect
        {
            public static Rarity ItemRarity = Rarity.Uncommon;

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                    return;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90015));
                this.Use();
            }

            public override string KeywordId => "GlobalEffect_W1dStim";
            public override string KeywordIconId => "ShopPassive15";
        }
    }
}
