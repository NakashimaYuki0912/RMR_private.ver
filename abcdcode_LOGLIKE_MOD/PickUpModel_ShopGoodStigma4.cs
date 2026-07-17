// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGoodStigma4
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGoodStigma4.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGoodStigma4 : ShopPickUpModel
    {
        public PickUpModel_ShopGoodStigma4()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8582004));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 82004);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGoodStigma4.Stigma4Effect());
        }

        /// <summary>Stigma4Effect</summary>

        public class Stigma4Effect : GlobalLogueEffectBase
        {
            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8582004));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8582004));
            }

            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassiveStigma4"];

            public override void OnDieUnit(BattleUnitModel unit)
            {
                base.OnDieUnit(unit);
                if (!(unit.bufListDetail.GetActivatedBuf(KeywordBuf.Burn) is BattleUnitBuf_burn activatedBuf))
                    return;
                int stack = activatedBuf.stack / 2 > 0 ? activatedBuf.stack / 2 : 1;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(unit.faction))
                    alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Burn, stack);
            }
        }
    }
}
