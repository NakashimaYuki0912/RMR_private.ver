// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood16
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood16.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood16 : ShopPickUpModel
    {
        public PickUpModel_ShopGood16() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570016));
            this.id = new LorId(LogLikeMod.ModId, 90016);
        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return base.IsCanPickUp(target) && !target.IsDead();
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            int stack = 2;
            model.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, stack);
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(model, model.faction, model.hp, model.breakDetail.breakGauge, model.bufListDetail.GetBufUIDataList());
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood16.AckandKKang());
        }

        public override string KeywordId => "GlobalEffect_RfxStim";
        public override string KeywordIconId => "ShopPassive16";

        /// <summary>AckandKKang</summary>

        public class AckandKKang : OnceEffect
        {
            public static Rarity ItemRarity = Rarity.Uncommon;

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                    return;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90016));
                this.Use();
            }

            public override string KeywordId => "GlobalEffect_RfxStim";
            public override string KeywordIconId => "ShopPassive16";
        }
    }
}
