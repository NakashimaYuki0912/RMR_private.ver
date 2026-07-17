// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood39
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood39.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood39 : ShopPickUpModel
    {
        public PickUpModel_ShopGood39() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570039));
            this.id = new LorId(LogLikeMod.ModId, 90039);
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            MaxDonwMinUpBuf.GiveBufThisRound(model, 2);
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood39.Shop39Effect());
        }

        public override string KeywordId => "GlobalEffect_CaffPills";
        public override string KeywordIconId => "ShopPassive39";

        /// <summary>Shop component: Shop39Effect</summary>

        public class Shop39Effect : OnceEffect
        {
            public static Rarity ItemRarity = Rarity.Rare;

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                    return;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90039));
                this.Use();
            }

            public override string KeywordId => "GlobalEffect_CaffPills";

            public override string KeywordIconId => "ShopPassive39";
        }
    }
}
