// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood35
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood35.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;
using RogueLike_Mod_Reborn;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood35 : ShopPickUpModel
    {
        public PickUpModel_ShopGood35()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570035));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90035);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override string[] Keywords
        {
            get => new string[1] { "LogueLikeMod_LuckyBuf_Page" };
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood35.MenualSlashEffect());
        }

        /// <summary>MenualSlashEffect</summary>

        public class MenualSlashEffect : MenualGlobalEffect
        {
            public override Sprite GetSprite()
            {
                return MenualGlobalEffect.CurEffect == this ? LogLikeMod.ArtWorks["ShopPassive35_on"] : LogLikeMod.ArtWorks["ShopPassive35"];
            }

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570035));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570035));
            }

            public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
            {
                if (MenualGlobalEffect.CurEffect != this || behavior.owner.faction != Faction.Player || behavior.behaviourInCard.Detail != BehaviourDetail.Slash)
                    return;
                BattleUnitBuf_RMR_Luck.ChangeDiceResult(behavior, 1, ref diceResult);
            }
        }
    }
}
