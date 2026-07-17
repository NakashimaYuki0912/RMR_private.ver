// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood20
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood20.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood20 : ShopPickUpModel
    {
        public PickUpModel_ShopGood20()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570020));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90020);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood20.Shop20Effect());
        }

        /// <summary>Shop component: Shop20Effect</summary>

        public class Shop20Effect : GlobalLogueEffectBase
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive20"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570020));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570020));
            }

            public override void OnDieUnit(BattleUnitModel unit)
            {
                base.OnDieUnit(unit);
                if (unit.faction != Faction.Enemy)
                    return;
                PassiveAbility_MoneyCheck.AddMoney(1);
            }
        }
    }
}
