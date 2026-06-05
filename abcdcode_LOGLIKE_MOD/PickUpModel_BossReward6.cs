// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BossReward6
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

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
