// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood14
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Linq;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood14 : ShopPickUpModel
    {
        public PickUpModel_ShopGood14() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570014));
            this.id = new LorId(LogLikeMod.ModId, 90014);
        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return LogueBookModels.playerBattleModel.Find(x => x.unitData == target).isDead;
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            model.UnitData.isDead = false;
            model.UnitData.hp = (float)(model.UnitData.MaxHp / 2);
            model.OnCreated();
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(model, model.faction, model.hp, model.breakDetail.breakGauge, model.bufListDetail.GetBufUIDataList());
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood14.Revive());
        }

        public override string KeywordId => "GlobalEffect_LightFrag";
        public override string KeywordIconId => "ShopPassive14";

        public class Revive : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Rare;

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase || BattleObjectManager.instance.GetList().ToList().Find(x => x.faction == Faction.Player && x.UnitData.isDead) == null)
                    return;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90014));
                this.Destroy();
            }

            public override string KeywordId => "GlobalEffect_LightFrag";
            public override string KeywordIconId => "ShopPassive14";
        }
    }
}
