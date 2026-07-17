// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood36
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood36.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;
using RogueLike_Mod_Reborn;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood36 : ShopPickUpModel
    {
        public PickUpModel_ShopGood36()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570036));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90036);
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override string[] Keywords
        {
            get => new string[1] { "LogueLikeMod_LuckyBuf_Page" };
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood36.MenualPeneEffect());
        }

        /// <summary>MenualPeneEffect</summary>

        public class MenualPeneEffect : MenualGlobalEffect
        {
            public override Sprite GetSprite()
            {
                return MenualGlobalEffect.CurEffect == this ? LogLikeMod.ArtWorks["ShopPassive36_on"] : LogLikeMod.ArtWorks["ShopPassive36"];
            }

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570036));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570036));
            }

            public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
            {
                if (MenualGlobalEffect.CurEffect != this || behavior.owner.faction != Faction.Player || behavior.behaviourInCard.Detail != BehaviourDetail.Penetrate)
                    return;
                BattleUnitBuf_RMR_Luck.ChangeDiceResult(behavior, 1, ref diceResult);
            }
        }
    }
}
