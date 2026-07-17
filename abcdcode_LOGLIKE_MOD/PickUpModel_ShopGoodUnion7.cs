// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGoodUnion7
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGoodUnion7.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGoodUnion7 : ShopPickUpModel
    {
        public PickUpModel_ShopGoodUnion7()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8581007));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 81007);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGoodUnion7.Union7Effect());
        }

        /// <summary>Union7Effect</summary>

        public class Union7Effect : GlobalLogueEffectBase
        {
            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8581007));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8581007));
            }

            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassiveUnion7"];

            public override void OnDieUnit(BattleUnitModel unit)
            {
                base.OnDieUnit(unit);
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                    alive.RecoverHP(alive.MaxHp / 20 > 6 ? 6 : alive.MaxHp / 20);
            }
        }
    }
}
