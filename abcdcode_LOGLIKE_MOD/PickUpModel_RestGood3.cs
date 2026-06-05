// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_RestGood3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Linq;


namespace abcdcode_LOGLIKE_MOD
{

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

        public class ResistAllUp : LogStatAdder
        {
            public override AtkResist GetResist(AtkResistType type, AtkResist baseResist)
            {
                return baseResist == AtkResist.Immune || baseResist == AtkResist.Resist ? base.GetResist(type, baseResist) : baseResist + 1;
            }
        }

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
