// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood38
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood38.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood38 : ShopPickUpModel
    {
        public PickUpModel_ShopGood38() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570038));
            this.id = new LorId(LogLikeMod.ModId, 90038);
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            MaxUpMinDownBuf.GiveBufThisRound(model, 2);
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood38.Shop38Effect());
        }

        public override string KeywordId => "GlobalEffect_G1sSteroids";
        public override string KeywordIconId => "ShopPassive38";

        /// <summary>Shop component: Shop38Effect</summary>

        public class Shop38Effect : OnceEffect
        {
            public static Rarity ItemRarity = Rarity.Rare;

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                    return;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90038));
                this.Use();
            }

            public override string KeywordId => "GlobalEffect_G1sSteroids";
            public override string KeywordIconId => "ShopPassive38";
        }
    }
}
