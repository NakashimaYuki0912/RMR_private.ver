// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood24
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood24.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood24 : ShopPickUpModel
    {
        public PickUpModel_ShopGood24()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570024));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90024);
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood24.Shop24Effect());
        }

        /// <summary>Shop component: Shop24Effect</summary>

        public class Shop24Effect : OnceEffect
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive24"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570024));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570024));
            }

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                    return;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
                    alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.DmgUp, 1);
                SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfileAll();
                this.Use();
            }
        }
    }
}
