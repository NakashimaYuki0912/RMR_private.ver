// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood8
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood8.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    [HideFromItemCatalog]
    public class PickUpModel_ShopGood8 : ShopPickUpModel
    {
        public PickUpModel_ShopGood8() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570008));
            this.id = new LorId(LogLikeMod.ModId, 90008);
        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return base.IsCanPickUp(target) && !target.IsDead();
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            model.RecoverHP(model.MaxHp / 10 * 3);
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new PickUpModel_ShopGood8.HealHpBullet());
        }
        public override string KeywordId => "GlobalEffect_HpBullet";
        public override string KeywordIconId => "ShopPassive8";

        /// <summary>HealHpBullet</summary>

        public class HealHpBullet : OnceEffect
        {
            public static Rarity ItemRarity = Rarity.Rare;

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                    return;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90008));
                this.Use();
            }

            public override string KeywordId => "GlobalEffect_HpBullet";
            public override string KeywordIconId => "ShopPassive8";
        }
    }
}
