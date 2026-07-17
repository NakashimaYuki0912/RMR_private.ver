// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_BossReward4
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_BossReward4.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
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

        /// <summary>RMR type: RMRHidden_PlayerRemover</summary>

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

        /// <summary>BossStatAdder</summary>

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
