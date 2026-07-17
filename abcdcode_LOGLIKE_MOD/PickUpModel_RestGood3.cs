// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_RestGood3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_RestGood3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Linq;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_RestGood3</summary>

    public class PickUpModel_RestGood3 : RestPickUp
    {
        public PickUpModel_RestGood3()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 800003));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 800003);
            this.type = RestPickUp.RestPickUpType.Main;
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            PickUpModel_RestGood3.ResistAllUp adder = new PickUpModel_RestGood3.ResistAllUp();
            LogueBookModels.AddPlayerStat(model.UnitData, (LogStatAdder)adder);
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_RestGood3.ResistAllUpTimer());
        }

        public override void OnChoice(RestGood good) => RestPickUp.AddPassiveReward(this.id);

        /// <summary>ResistAllUp</summary>

        public class ResistAllUp : LogStatAdder
        {
            public override AtkResist GetResist(AtkResistType type, AtkResist baseResist)
            {
                return baseResist == AtkResist.Immune || baseResist == AtkResist.Resist ? base.GetResist(type, baseResist) : baseResist + 1;
            }
        }

        /// <summary>ResistAllUpTimer</summary>

        public class ResistAllUpTimer : GlobalLogueEffectBase
        {
            public bool CheckBoss;

            public override void OnStartBattle()
            {
                this.CheckBoss = false;
                base.OnStartBattle();
                if (LogLikeMod.curstagetype != StageType.Boss)
                    return;
                this.CheckBoss = true;
            }

            public override void OnEndBattle()
            {
                if (!this.CheckBoss)
                    return;
                foreach (UnitDataModel unitDataModel in LogueBookModels.playersstatadders.Keys.ToList<UnitDataModel>())
                {
                    LogueBookModels.playersstatadders[unitDataModel].RemoveAll((Predicate<LogStatAdder>)(x => x is PickUpModel_RestGood3.ResistAllUp));
                    unitDataModel.RefreshEquip();
                }
                this.Destroy();
            }
        }
    }
}
