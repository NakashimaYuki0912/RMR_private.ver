// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_BossReward8
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_BossReward8.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_BossReward8 : PickUpModelBase
    {
        public override string KeywordId => "GlobalEffect_BlastFurnace";
        public override string KeywordIconId => "BossReward8";
        public PickUpModel_BossReward8():base()
        {

        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find((Predicate<GlobalLogueEffectBase>)(x => x is PickUpModel_BossReward8.DragonLightRoadEffect)) == null;
        }

        public override void OnPickUp()
        {
            base.OnPickUp();
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_BossReward8.DragonLightRoadEffect());
        }

        /// <summary>DragonLightRoadEffect</summary>

        public class DragonLightRoadEffect : GlobalLogueEffectBase
        {
            public override string KeywordId => "GlobalEffect_BlastFurnace";
            public override string KeywordIconId => "BossReward8";

            public static Rarity ItemRarity = Rarity.Unique;
            public override float CraftCostMultiple(CraftEffect effect) => 0.8f;
        }
    }
}
