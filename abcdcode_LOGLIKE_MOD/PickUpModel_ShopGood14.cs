// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood14
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood14.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
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

        /// <summary>Revive</summary>

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
