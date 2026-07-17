// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_BossReward6
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_BossReward6.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using RogueLike_Mod_Reborn;
using System;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_BossReward6 : PickUpModelBase
    {
        public override string KeywordId => "GlobalEffect_TwoLeafClover";
        public override string KeywordIconId => "BossReward6";
        public PickUpModel_BossReward6():base()
        {

        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find(x => x is PickUpModel_ShopGood23.Shop23Effect) != null && Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find(x => x is PickUpModel_BossReward6.FourCloverEffect) == null;
        }

        public override void OnPickUp()
        {
            base.OnPickUp();
            if (Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find(x => x is PickUpModel_ShopGood23.Shop23Effect) == null)
                return;
            Singleton<GlobalLogueEffectManager>.Instance.RemoveEffect(Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find(x => x is PickUpModel_ShopGood23.Shop23Effect));
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_BossReward6.FourCloverEffect());
        }

        /// <summary>FourCloverEffect</summary>

        public class FourCloverEffect : GlobalLogueEffectBase
        {
            public override string KeywordId => "GlobalEffect_TwoLeafClover";
            public override string KeywordIconId => "BossReward6";

            public static Rarity ItemRarity = Rarity.Unique;

            public override int ChangeSuccCostValue() => 1;

            public override void OnRoundStart(StageController stage)
            {
                base.OnRoundStart(stage);
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
                {
                    alive.bufListDetail.AddKeywordBufThisRoundByEtc(RoguelikeBufs.RMRLuck, 1);
                    SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfileAll();
                }
            }
        }
    }
}
