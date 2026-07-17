// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGoodStigma1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGoodStigma1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGoodStigma1 : ShopPickUpModel
    {
        public PickUpModel_ShopGoodStigma1() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8582001));
            this.id = new LorId(LogLikeMod.ModId, 82001);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new PickUpModel_ShopGoodStigma1.Stigma1Effect());
        }
        public override string KeywordId => "GlobalEffect_SlowBurn";
        public override string KeywordIconId => "ShopPassiveStigma1";

        /// <summary>Stigma1Effect</summary>

        public class Stigma1Effect : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Unique;

            public override string KeywordId => "GlobalEffect_SlowBurn";
            public override string KeywordIconId => "ShopPassiveStigma1";
        }
    }
}
