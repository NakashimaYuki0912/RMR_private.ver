// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BossReward8
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

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

        public class DragonLightRoadEffect : GlobalLogueEffectBase
        {
            public override string KeywordId => "GlobalEffect_BlastFurnace";
            public override string KeywordIconId => "BossReward8";

            public static Rarity ItemRarity = Rarity.Unique;
            public override float CraftCostMultiple(CraftEffect effect) => 0.8f;
        }
    }
}
