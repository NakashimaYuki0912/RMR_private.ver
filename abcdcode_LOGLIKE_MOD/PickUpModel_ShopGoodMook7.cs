// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGoodMook7
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGoodMook7.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGoodMook7 : ShopPickUpModel
    {
        public PickUpModel_ShopGoodMook7() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8583007));
            this.id = new LorId(LogLikeMod.ModId, 83007);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGoodMook7.Mook7Effect());
        }

        public override string KeywordId => "GlobalEffect_KillingAura";
        public override string KeywordIconId => "ShopPassiveMook7";

        /// <summary>Mook7Effect</summary>

        public class Mook7Effect : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Rare;
            public override float DmgFactor(
              BattleUnitModel model,
              int dmg,
              DamageType type = DamageType.ETC,
              KeywordBuf keyword = KeywordBuf.None)
            {
                return type <= DamageType.Attack || model.faction != Faction.Enemy ? base.DmgFactor(model, dmg, type, keyword) : 2f;
            }

            public override string KeywordId => "GlobalEffect_KillingAura";
            public override string KeywordIconId => "ShopPassiveMook7";
        }
    }
}
