// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood23
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood23.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using RogueLike_Mod_Reborn;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood23 : ShopPickUpModel
    {
        public PickUpModel_ShopGood23()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570023));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90023);
        }

        public override string[] Keywords
        {
            get => new string[1] { "LogueLikeMod_LuckyBuf_Page" };
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood23.Shop23Effect());
        }

        /// <summary>Shop component: Shop23Effect</summary>

        public class Shop23Effect : GlobalLogueEffectBase
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive23"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570023));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570023));
            }

            public override void OnRoundStart(StageController stage)
            {
                base.OnRoundStart(stage);
                var unit = BattleObjectManager.instance.GetAliveList(Faction.Player).SelectOneRandom();
                if (unit != null)
                {
                    unit.bufListDetail.AddKeywordBufThisRoundByEtc(RoguelikeBufs.RMRLuck, 1, unit);
                    SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfileAll();
                }
            }
        }
    }
}
