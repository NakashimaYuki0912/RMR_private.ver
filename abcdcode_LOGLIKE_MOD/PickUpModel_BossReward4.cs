// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BossReward4
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_BossReward4 : PickUpModelBase
    {
        public override string KeywordId => "GlobalEffect_SkaeniteRitual";
        public override string KeywordIconId => "BossReward4";

        public PickUpModel_BossReward4() : base()
        {

        }

        public override void OnPickUp()
        {
            base.OnPickUp();
            GlobalLogueEffectManager.Instance.AddEffects(new RMRHidden_PlayerRemover());
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            LogueBookModels.AddPlayerStat(model.UnitData, (LogStatAdder)new PickUpModel_BossReward4.BossStatAdder());
        }

        public class RMRHidden_PlayerRemover : GlobalLogueEffectBase
        {
            bool removed = false;

            public override void AfterClearBossWave()
            {
                if (removed)
                    return;
                LogLikeMod.AddPlayer = false;
                this.Destroy();
                removed = true;
            }

            public override void OnStartBattleAfter()
            {
                base.OnStartBattleAfter();
                foreach (var unit in BattleObjectManager.instance.GetAliveList(Faction.Player).FindAll(x => LogueBookModels.playersstatadders[x.UnitData.unitData].Find(y => y is BossStatAdder) != null))
                {
                    unit.allyCardDetail.DrawCards(1);
                }
            }

        }

        public class BossStatAdder : LogStatAdder
        {
            public BossStatAdder()
            {
                this.maxbreakpercent = 100;
                this.maxhppercent = 100;
                this.speeddicenum = 1;
                this.maxplaypoint = 1;
                this.startplaypoint = 1;
            }
        }
    }
}
