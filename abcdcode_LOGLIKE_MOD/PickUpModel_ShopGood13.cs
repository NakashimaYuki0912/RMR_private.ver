// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood13
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood13.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood13 : ShopPickUpModel
    {
        public PickUpModel_ShopGood13()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570013));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90013);
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood13.Boom());
        }

        /// <summary>Boom</summary>

        public class Boom : OnceEffect
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive13"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570013));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570013));
            }

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase || BattleObjectManager.instance.GetAliveList(Faction.Enemy).Count <= 0)
                    return;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                    alive.TakeDamage(8 + (int)LogLikeMod.curchaptergrade * 3);
                SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfileAll();
                this.Use();
            }
        }
    }
}
