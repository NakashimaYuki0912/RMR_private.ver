using System;
using System.Collections.Generic;
using System.Linq;
using abcdcode_LOGLIKE_MOD;
using LOR_DiceSystem;

namespace RogueLike_Mod_Reborn
{

    #region Canard
    public class UpgradeModel_RMR_Track : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_TrackUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 104005);
        }
    }

    public class UpgradeModel_RMR_Chargeup : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_ChargeupUpgrade");
            this.upgradeinfo.SetAbility(0, "paralysis1atk");
            this.baseid = new LorId(LogLikeMod.ModId, 103003);
        }
    }

    public class UpgradeModel_RMR_BackstreetsDash : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_BackstreetsDashDie");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 101005);
        }
    }

    public class UpgradeModel_RMR_SkitterAway : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_SkitterAwayUpgrade");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 101006);
        }
    }

    public class UpgradeModel_RMR_Endure : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_EndureUpgrade");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 103004);
        }
    }

    public class UpgradeModel_RMR_BlowItUp : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 3, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 103001);
        }
    }

    public class UpgradeModel_RMR_YoureTooSlow : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 102002);
        }
    }

    public class UpgradeModel_RMR_Driedup : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_DriedupUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 103005);
        }
    }

    public class UpgradeModel_RMR_FendThisOff : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 104003);
        }
    }

    public class UpgradeModel_RMR_TimeForTest : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 3, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 102004);
        }
    }

    public class UpgradeModel_RMR_ChopItOff : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 103006);
        }
    }

    public class UpgradeModel_RMR_GoinFirst : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 104002);
        }
    }

    public class UpgradeModel_RMR_Struggle : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_StruggleUpgrade");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 102003);
        }
    }

    public class UpgradeModel_RMR_YouOnlyLiveOnce : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 3, 3);
            this.baseid = new LorId(LogLikeMod.ModId, 103002);
        }
    }

    public class UpgradeModel_RMR_GutHarvesting : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_GutHarvestingUpgrade");
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 101001);
        }
    }

    public class UpgradeModel_RMR_Rampage : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 104004);
        }
    }


    public class UpgradeModel_RMR_Wallop : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_breakvuln1atk");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 100003);
        }
    }

    public class UpgradeModel_RMR_Thrust : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 100002);
        }
    }

    #endregion

    #region Urban Myth

    public class UpgradeModel_RMR_GatherIntel : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_GatherIntelUpgrade");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 202009);
        }
    }

    public class UpgradeModel_RMR_Appetite : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 201007);
        }
    }

    public class UpgradeModel_RMR_TrimIngredients : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(1, "bleeding2pw");
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 201002);
        }
    }

    public class UpgradeModel_RMR_PreparedMindLulu : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_PreparedMindLuluUpgrade");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetAbility(1, "burn2atk");
            this.baseid = new LorId(LogLikeMod.ModId, 202012);
        }
    }

    public class UpgradeModel_RMR_Cruelty : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_CrueltyUpgradeDie");
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 201006);
        }
    }

    public class UpgradeModel_RMR_PreparedMindMars : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_Endurance2start");
            this.upgradeinfo.SetAbility(0, "endurance2pw");
            this.upgradeinfo.SetDice(0, 2, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 202002);
        }
    }

    public class UpgradeModel_RMR_KeepItFresh : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 201001);
        }
    }

    public class UpgradeModel_RMR_Multiblock : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_MultiblockUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 200002);
        }
    }

    public class UpgradeModel_RMR_FleetFootsteps : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 202008);
        }
    }

    public class UpgradeModel_RMR_Deflect : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 202001);
        }
    }

    public class UpgradeModel_RMR_IngredientHunt : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetAbility(1, "bleeding2atk");
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 201005);
        }
    }

    public class UpgradeModel_RMR_CookAnything : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetAbility(1, "recoverHp5atkLog");
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 201003);
        }
    }

    public class UpgradeModel_RMR_SetFire : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_SetFireUpgradeDie");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetAbility(1, "RMR_SetFireUpgradeDie");
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 202003);
        }
    }

    public class UpgradeModel_RMR_Multihit : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(1, 2, 1);
            this.upgradeinfo.SetDice(2, 2, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 200003);
        }
    }

    public class UpgradeModel_RMR_StrongStrike : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "recoverBreak3atk");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetAbility(1, "recoverBreak3atk");
            this.baseid = new LorId(LogLikeMod.ModId, 200005);
        }
    }

    public class UpgradeModel_RMR_NonstopAssault : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_NonstopAssaultUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 202007);
        }
    }

    #endregion

    #region Urban Legend
    
    public class UpgradeModel_RMR_MeatJam : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_MeatJamUpgrade");
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 2000002);
        }
    } 


    public class UpgradeModel_RMR_LawOrder : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetAbility(0, "RMR_LawOrderUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 301011);
        }
    }

    public class UpgradeModel_RMR_Diversion : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 300001);
        }
    }

    public class UpgradeModel_RMR_SharpSwipe : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.baseid = new LorId(LogLikeMod.ModId, 300002);
        }
    }

    public class UpgradeModel_RMR_Standoff : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_StandoffUpgrade");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 300004);
        }
    }

    public class UpgradeModel_RMR_Avert : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 301009);
        }
    }

    public class UpgradeModel_RMR_ArtOfSteps : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("quickness2");
            this.upgradeinfo.SetDice(0, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 302006);
        }
    }

    public class UpgradeModel_RMR_StartinLightly : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.ChangeDiceType(1, BehaviourDetail.Hit);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 303004);
        }
    }

    public class UpgradeModel_RMR_Guardian : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_GuardianUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 301007);
        } 
    }
    public class UpgradeModel_RMR_Gamble : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_GambleUpgrade");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 302003);
        }
    }

    public class UpgradeModel_RMR_BackAttack : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 300003);
        }
    }

    public class UpgradeModel_RMR_Slice : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_SliceUpgrade");
            this.upgradeinfo.ChangeDiceType(2, BehaviourDetail.Slash);
            this.baseid = new LorId(LogLikeMod.ModId, 302007);
        }
    }

    public class UpgradeModel_RMR_SilentNight : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetDice(0, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 301002);
        }
    }

    public class UpgradeModel_RMR_StayClam : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_StayClamUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 302005);
        }
    }

    public class UpgradeModel_RMR_Fence : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.baseid = new LorId(LogLikeMod.ModId, 301003);
        }

        public override DiceCardXmlInfo GetUpgradeInfo(int index, int count)
        {
            this.upxmlinfo = base.GetUpgradeInfo(index, count);

            // the way I'm doing this is probably stupid and could be using Sort() or some other function but I cannot come up with a solution and I will bruteforce it instead - Lux
            this.upxmlinfo.DiceBehaviourList.Clear();
            DiceBehaviour die = new DiceBehaviour
            {

                Min = 3,
                Dice = 6,
                Detail = BehaviourDetail.Evasion,
                Type = BehaviourType.Def,
                MotionDetail = MotionDetail.E,
                MotionDetailDefault = MotionDetail.N,
                Script = ""
            };
            DiceBehaviour die2 = new DiceBehaviour
            {

                Min = 4,
                Dice = 5,
                Detail = BehaviourDetail.Guard,
                Type = BehaviourType.Def,
                MotionDetail = MotionDetail.G,
                MotionDetailDefault = MotionDetail.N,
                Script = ""
            };
            
            DiceBehaviour die3 = new DiceBehaviour
            {

                Min = 1,
                Dice = 6,
                Detail = BehaviourDetail.Slash,
                Type = BehaviourType.Atk,
                MotionDetail = MotionDetail.J,
                MotionDetailDefault = MotionDetail.N,
                Script = ""
            };
            this.upxmlinfo.DiceBehaviourList.Add(die);
            this.upxmlinfo.DiceBehaviourList.Add(die2);
            this.upxmlinfo.DiceBehaviourList.Add(die3);
            
            return this.upxmlinfo;
        }
    }

    public class UpgradeModel_RMR_FishOnslaught : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_FishOnslaughtUpgrade");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 302001);
        }
    }

    public class UpgradeModel_RMR_DestructiveImpact : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 5);
            this.baseid = new LorId(LogLikeMod.ModId, 303006);
        }

    }

    public class UpgradeModel_RMR_SearingBlow : UpgradeBase
    {
        public override bool CanRepeatUpgrade() => true;
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.baseid = new LorId(LogLikeMod.ModId, 303005);
        }
        public override DiceCardXmlInfo GetUpgradeInfo(int index, int count)
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            if (count > 1) // custom scaling because fuck Lux
            {
                int num = 0;
                for (int i = count; i > 0; --i) // if count = 5; becomes 5 + 4 + 3 + 2 ...
                    num += i;
                this.upxmlinfo = this.CreateUpBase(index, count);
                this.upgradeinfo.SetDice(0, 2, 0);
                for (int index1 = 0; index1 < count; index1++)
                {
                    // regular min value scaling
                    foreach (KeyValuePair<int, int> keyValuePair in this.upgradeinfo.mindata)
                        this.upxmlinfo.DiceBehaviourList[keyValuePair.Key].Min += keyValuePair.Value;
                }
                // custom max value scaling
                this.upxmlinfo.DiceBehaviourList[0].Dice += num;
                return this.upxmlinfo;
            }
            this.upgradeinfo.SetDice(0, 2, 1);
            return base.GetUpgradeInfo(index, 1);
        }
    }

    public class UpgradeModel_RMR_Outburst : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetAbility(0, "RMR_allydraw1pw");
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 300005);
        }
    }

    public class UpgradeModel_RMR_Crush : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetDice(0, 0, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 303008);
        }
    }

    public class UpgradeModel_RMR_EndYou : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetAbility(0, "RMR_paralysisbind1atk");
            this.upgradeinfo.SetAbility(1, "RMR_paralysisbind1atk");
            this.baseid = new LorId(LogLikeMod.ModId, 303003);
        }
    }

    public class UpgradeModel_RMR_CutIn : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 3);
            this.upgradeinfo.SetDice(2, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 303007);
        }
    }

    public class UpgradeModel_RMR_YourShield : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 301004);
        }
    }

    public class UpgradeModel_RMR_BladeWhirl : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_BladeWhirlUpgrade");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 301006);
        }
    }

    public class UpgradeModel_RMR_HandlingWork : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_HandlingWorkUpgrade");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 301001);
        }
    }

    public class UpgradeModel_RMR_Retaliate : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 301010);
        }
    }

    #endregion

    # region Urban Plague

    public class UpgradeModel_RMR_TasteChain : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.upgradeinfo.SetDice(0, 2, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 409001);
        }
    }

    public class UpgradeModel_RMR_LowNight : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_LowNightUpgrade");
            this.upgradeinfo.SetDice(0, 2, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 411101);
        }
    }

    public class UpgradeModel_RMR_RulesBackstreets : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_bleed2draw1atk");
            this.upgradeinfo.SetDice(0, 2, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 406007);
        }
    }

    public class UpgradeModel_RMR_HandleRequest : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_PreparedMindLulu");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 403006);
        }
    }

    public class UpgradeModel_RMR_HighspeedStabbing : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_boostdiceminvalue2pl");
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 408005);
        }
    }

    public class UpgradeModel_RMR_CleanUp : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 406008);
        }
    }

    public class UpgradeModel_RMR_PrescriptOrder : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_PrescriptOrderUpgrade");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 401005);
        }
    }

    public class UpgradeModel_RMR_BizarreAttack : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, -1, -1);
            this.upgradeinfo.SetDice(1, 1, 0);

              /*  DiceBehaviour die = new DiceBehaviour
                {
                    Min = 2,
                    Dice = 4,
                    Detail = BehaviourDetail.Slash,
                    Type = BehaviourType.Atk,
                    EffectRes = "Carnival_H",
                    MotionDetail = MotionDetail.H,
                    MotionDetailDefault = MotionDetail.N,
                    Script = ""
                }
            this.upgradeinfo.AddDice(die) */
            this.upgradeinfo.AddDice(2, 4, BehaviourDetail.Hit, "break5atk", MotionDetail.H, BehaviourType.Atk, "Carnival_H", "");
            this.baseid = new LorId(LogLikeMod.ModId, 401006);
        }
    }

    public class UpgradeModel_RMR_BizarreAttack2 : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.index = 1;
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "powerUpNext2pw");
            this.upgradeinfo.SetDice(0, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 401006);
        }
    }

    public class UpgradeModel_RMR_BizarreAttack3 : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.index = 2;
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("cardPowerDown2target");
            this.baseid = new LorId(LogLikeMod.ModId, 401006);
        }
    }

    public class UpgradeModel_RMR_TargetSpotted : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 402008);
        }
    }

    public class UpgradeModel_RMR_UnavoidableGaze : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_UnavoidableGazeUpgrade");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 404008);
        }
    }

    public class UpgradeModel_RMR_YoureSoft : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.AddDice(4, 6, BehaviourDetail.Guard, "", MotionDetail.G, BehaviourType.Def, "", "");
            this.baseid = new LorId(LogLikeMod.ModId, 409002);
        }
    }

    public class UpgradeModel_RMR_Relay : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_RelayUpgrade");
            this.upgradeinfo.SetAbility(0, "vulnerable2atk");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 411102);
        }
    }

    public class UpgradeModel_RMR_DarkCloud : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_DarkCloudUpgrade");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 406009);
        }
    }

    public class UpgradeModel_RMR_UpbeatPerformance : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "break3atk");
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 407005);
        }
    }

    public class UpgradeModel_RMR_ReturnFire : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_ReturnFireUpgrade");
            this.upgradeinfo.SetDice(0, 3, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 402002);
        }
    }

    public class UpgradeModel_RMR_FrontalDodge : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 409003);
        }
    }

    public class UpgradeModel_RMR_FlankAttack : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_FlankAttackUpgrade");
            this.upgradeinfo.SetDice(0, 2, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 409004);
        }
    }

    public class UpgradeModel_RMR_RedNotes : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_RedNotesUpgrade");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 407007);
        }
    }

    public class UpgradeModel_RMR_WillBeTasty : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 401004);
        }
    }

    public class UpgradeModel_RMR_IHateCQC : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_IHATECQCUpgrade");
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 2);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 402004);
        }
    }

    public class UpgradeModel_RMR_TakeTheShot : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_TakeTheShotUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 402006);
        }
    }

    public class UpgradeModel_RMR_OpportunitySpotted : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.upgradeinfo.SetDice(3, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 404002);
        }
    }

    public class UpgradeModel_RMR_YoureHindrance : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_YoureHindranceUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 404004);
        }
    }

    public class UpgradeModel_RMR_CumulusWall : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_CumulusWallUpgrade");
            this.upgradeinfo.SetAbility(2, "RMR_bleed1twiceatk");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 406003);
        }
    }

    public class UpgradeModel_RMR_ShrineToMusic : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_ShrineToMusicUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 407004);
        }
    }

    public class UpgradeModel_RMR_FlashOfSunup : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetAbility(0, "burn1atk2times");
            this.upgradeinfo.SetAbility(2, "burn1atk2times");
            this.baseid = new LorId(LogLikeMod.ModId, 403002);
        }
    }

    public class UpgradeModel_RMR_Headshot : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 3, 4);
            this.baseid = new LorId(LogLikeMod.ModId, 402007);
        }
    }

    public class UpgradeModel_RMR_SpearedSweep : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_SpearedSweepUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetAbility(0, "RMR_SpearSweepDieUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 408004);
        }
    }

    public class UpgradeModel_RMR_FullStopToLife : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_FullStopToLifeUpgrade1");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 402009);
        }
    }

    public class UpgradeModel_RMR_FullStopToLife2 : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.index = 1;
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_FullStopToLifeUpgrade2");
            this.upgradeinfo.SetAbility(0, "RMR_1weakcrit");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 402009);
        }
    }

    public class UpgradeModel_RMR_ElectricShock : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 404006);
        }
    }

    public class UpgradeModel_RMR_ElectricShock2 : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.index = 1;
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_ElectricShockUpgrade");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 404006);
        }
    }

    public class UpgradeModel_RMR_HeresMyChance : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 409005);
        }
    }

    public class UpgradeModel_RMR_Whet : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 1);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 410003);
        }
    }

    public class UpgradeModel_RMR_StabAndDetonate : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 3);
            this.baseid = new LorId(LogLikeMod.ModId, 410002);
        }
    }

    public class UpgradeModel_RMR_FlipTheTable : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("quickall2discardLog");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 411103);
        }
    }

    public class UpgradeModel_RMR_ShuffleHands : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 411104);
        }
    }

    public class UpgradeModel_RMR_SkyClearingCut : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_add5powerpl");
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 406004);
        }
    }

    public class UpgradeModel_RMR_HeadToHead : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_HeadtoHeadUpgrade");
            this.upgradeinfo.SetDice(0, 2, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 409006);
        }
    }

    public class UpgradeModel_RMR_SunsetBlade : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 403005);
        }
    }

    public class UpgradeModel_RMR_StructuralAnalysis : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 404007);
        }
    }

    public class UpgradeModel_RMR_Feast : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_FeastUpgrade");
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 401002);
        }
    }

    public class UpgradeModel_RMR_Eject : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_EjectUpgrade");
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 403004);
        }
    }

    public class UpgradeModel_RMR_NotAnotherStep : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_NotAnotherStepUpgrade");
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 2);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 402005);
        }
    }
    /*
    public class UpgradeModel_RMR_SharpenedBlade : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_SharpenedBladeUpgrade");
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.upgradeinfo.SetAbility(1, "bleeding2atk");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 406002);
        }
    }
    */
    public class UpgradeModel_RMR_SharpenedBlade : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 1);
            this.upgradeinfo.SetDice(1, 2, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 406002);
        }
    }


    public class UpgradeModel_RMR_SilentMist : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_bleed1twiceatk");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 406006);
        }
    }

    public class UpgradeModel_RMR_WillMakeFineSilk : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(1, "carnivalLog");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 401003);
        }
    }

    public class UpgradeModel_RMR_ButterflySlash : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 403003);
        }
    }

    public class UpgradeModel_RMR_IndiscriminateShots : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.baseid = new LorId(LogLikeMod.ModId, 402003);
        }

        public override DiceCardXmlInfo GetUpgradeInfo(int index, int count)
        {
            this.upxmlinfo = base.GetUpgradeInfo(index, count);
            this.upxmlinfo.DiceBehaviourList.Clear();
            DiceBehaviour die = new DiceBehaviour
            {

                Min = 4,
                Dice = 6,
                Detail = BehaviourDetail.Penetrate,
                Type = BehaviourType.Atk,
                MotionDetail = MotionDetail.F,
                MotionDetailDefault = MotionDetail.N,
                Script = "RMR_IndiscriminateShotsDie1Upgrade"
            };
            DiceBehaviour die2 = new DiceBehaviour
            {

                Min = 3,
                Dice = 6,
                Detail = BehaviourDetail.Hit,
                Type = BehaviourType.Atk,
                MotionDetail = MotionDetail.F,
                MotionDetailDefault = MotionDetail.N,
                Script = "RMR_IndiscriminateShotsDie2"
            };
            this.upxmlinfo.DiceBehaviourList.Add(die);
            this.upxmlinfo.DiceBehaviourList.Add(die);
            this.upxmlinfo.DiceBehaviourList.Add(die);
            this.upxmlinfo.DiceBehaviourList.Add(die2);
            return this.upxmlinfo;
        }
    }

    public class UpgradeModel_RMR_BeyondTheShadow : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(1, "RMR_3bleedcrit");
            this.upgradeinfo.SetAbility(2, "RMR_3bleedcrit");
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 402010);
        }
    }

    public class UpgradeModel_RMR_Zzzzzap : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 2, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 404005);
        }
    }

    public class UpgradeModel_RMR_ScatteringSlash : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_ScatteringSlashUpgrade");
            this.upgradeinfo.SetAbility(0, "RMR_whythefuckistherenobleeding1pw");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 406005);
        }
    }

    public class UpgradeModel_RMR_UnforgettableMelody : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_UnforgettableMelodyUpgrade");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 407002);
        }
    }

    public class UpgradeModel_RMR_FaintMemories : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_FaintMemoriesUpgrade");
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 405001);
        }
    }

    public class UpgradeModel_RMR_GoingForBullseye : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_KillMotherfuckerOfChoiceUpgrade");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 402001);
        }
    }

    public class UpgradeModel_RMR_BindingArms : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_BindingArmsUpgrade");
            this.upgradeinfo.SetDice(0, 4, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 404003);
        }
    }

    public class UpgradeModel_RMR_HeavyPeaks : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.AddDice(3, 5, BehaviourDetail.Hit, "RMR_nodamage", MotionDetail.H, BehaviourType.Standby, "", "");
            this.baseid = new LorId(LogLikeMod.ModId, 407001);
        }
    }

    public class UpgradeModel_RMR_Tailoring : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "bleeding4atk");
            this.upgradeinfo.SetAbility(1, "bleeding4atk");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 401001);
        }
    }

    public class UpgradeModel_RMR_Tailoring2 : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.index = 1;
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "bleeding1atk");
            this.upgradeinfo.SetAbility(1, "bleeding1atk");
            this.upgradeinfo.SetSelfAbility("RMR_TailoringUpgrade");
            this.upgradeinfo.SetDice(0, 3, 0);
            this.upgradeinfo.SetDice(1, 3, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 401001);
        }
    }

    public class UpgradeModel_RMR_FascinatingFabric : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_FascinatingFabricUpgrade");
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 2, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 401007);
        }
    }

    public class UpgradeModel_RMR_Transpierce : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "damage7atkLog");
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.AddDice(2, 6, BehaviourDetail.Guard, "", MotionDetail.G, BehaviourType.Standby, "", "");
            this.baseid = new LorId(LogLikeMod.ModId, 408001);
        }
    }

    public class UpgradeModel_RMR_Collision : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.AddDice(5, 9, BehaviourDetail.Guard, "energy1pw", MotionDetail.G, BehaviourType.Def, "", "");
            this.baseid = new LorId(LogLikeMod.ModId, 408003);
        }
    }

    public class UpgradeModel_RMR_CrackOfDawn : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(1, "RMR_CrackOfDawnDieUpgrade");
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 403001);
        }
    }

    public class UpgradeModel_RMR_NowDie : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 409007);
        }
    }

    public class UpgradeModel_RMR_Refine : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.ChangeDiceType(1, BehaviourDetail.Penetrate);
            this.upgradeinfo.SetAbility(2, "burn3atk");
            this.baseid = new LorId(LogLikeMod.ModId, 410004);
        }
    }

    public class UpgradeModel_RMR_Sakura : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_SakuraUpgrade");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 411105);
        }
    }

    public class UpgradeModel_RMR_InkOver : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(2, "RMR_InkOverDieUpgrade");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 406001);
        }
    }

    public class UpgradeModel_RMR_Observe : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_ObserveUpgrade");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 404001);
        }
    }

    public class UpgradeModel_RMR_WrathOfTorment : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_WrathOfTormentUpgrade");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 405002);
        }
    }

    #endregion

    #region Urban Nightmare

    public class UpgradeModel_RMR_DrawSword : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 512001);
        }
    }

    public class UpgradeModel_RMR_Engagement : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_EngagementUpgrade");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 511001);
        }
    }

    public class UpgradeModel_RMR_EnGarde : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_EnGardeUpgrade");
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 511005);
        }
    }

    public class UpgradeModel_RMR_OvercomeCrisis : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_OvercomeCrisisUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 501007);
        }
    }

    public class UpgradeModel_RMR_EnergyCycle : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetSelfAbility("warpCharge4");
            this.baseid = new LorId(LogLikeMod.ModId, 506007);
        }
    }

    public class UpgradeModel_RMR_SmokingPipe : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_SmokingPipeUpgrade");
            this.upgradeinfo.SetDice(1, 2, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 507006);
        }
    }

    public class UpgradeModel_RMR_PuppetBlockade : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 502007);
        }
    }

    public class UpgradeModel_RMR_Juggling : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 3);
            this.baseid = new LorId(LogLikeMod.ModId, 503003);
        }
    }

    public class UpgradeModel_RMR_Faith : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_FaithDie");
            this.upgradeinfo.SetAbility(1, "RMR_FaithDie");
            this.baseid = new LorId(LogLikeMod.ModId, 505005);
        }
    }



    public class UpgradeModel_RMR_Leap : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_LeapUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 506006);
        }
    }

    public class UpgradeModel_RMR_Riposte : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_AdaptDice_RiposteUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 511002);
        }
    }

    public class UpgradeModel_RMR_DesperateStruggle : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_HitAndRunBlockonEvade");
            this.upgradeinfo.SetAbility(0, "RMR_HitAndRunBlockonEvade");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 501006);
        }
    }

    public class UpgradeModel_RMR_ExtractFuel : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 504005);
        }
    }

    public class UpgradeModel_RMR_NightBackstreets : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 504006);
        }
    }

    public class UpgradeModel_RMR_HiddenBlade : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_HiddenBladeUpgrade");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 507005);
        }
    }

    public class UpgradeModel_RMR_FlyingSword : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_HastePower2");
            this.upgradeinfo.SetAbility(1, "RMR_HastePower2");
            this.upgradeinfo.SetSelfAbility("RMR_FlyingSwordUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 501004);
        }
    }

    public class UpgradeModel_RMR_Gigigig : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 502005);
        }
    }

    public class UpgradeModel_RMR_InhaleSmoke : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_InhaleSmokeUpgrade");
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 507003);
        }
    }

    public class UpgradeModel_RMR_CircusKnives : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(1, "bleeding1atk");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 503002);
        }
    }

    public class UpgradeModel_RMR_WildCard : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 503004);
        }
    }

    public class UpgradeModel_RMR_Acupuncture : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(1, "RMR_critchance12pw");
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 512002);
        }
    }

    public class UpgradeModel_RMR_ProselyteBlade : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_ProselyteBladeUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 505007);
        }
    }

    public class UpgradeModel_RMR_Rewind : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(2, "recoverHp3atk");
            this.upgradeinfo.SetSelfAbility("warpCharge5");
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 506005);
        }
    }

    public class UpgradeModel_RMR_SenseQuarry : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "jaeheon_guard_pw2");
            this.upgradeinfo.SetAbility(1, "paralysis1pw");
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 505006);
        }
    }

    public class UpgradeModel_RMR_Fleche : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 511003);
        }
    }

    public class UpgradeModel_RMR_FierceCharge : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_FierceChargeUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 508011);
        }
    }

    public class UpgradeModel_RMR_ExtremeEdge : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "disarm3atk");
            this.upgradeinfo.SetDice(0, 1, 3);
            this.baseid = new LorId(LogLikeMod.ModId, 501002);
        }
    }

    public class UpgradeModel_RMR_YieldFlesh : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_YieldMyFleshUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 512003);
        }
    }

    public class UpgradeModel_RMR_PrescriptPoints : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_ToWherePrescriptUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 505001);
        }
    }

    public class UpgradeModel_RMR_DimensionalRift1 : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.baseid = new LorId(LogLikeMod.ModId, 506002);
        }

        public override DiceCardXmlInfo GetUpgradeInfo(int index, int count)
        {
            this.upxmlinfo = base.GetUpgradeInfo(index, count); base.GetUpgradeInfo(index, count);
            this.upxmlinfo.DiceBehaviourList.Clear();
            DiceBehaviour die = new DiceBehaviour
            {

                Min = 4,
                Dice = 8,
                Detail = BehaviourDetail.Slash,
                Type = BehaviourType.Atk,
                MotionDetail = MotionDetail.H,
                MotionDetailDefault = MotionDetail.N,
                Script = "",
                EffectRes = "WarpCrew_H"
            };
            DiceBehaviour die2 = new DiceBehaviour
            {

                Min = 4,
                Dice = 8,
                Detail = BehaviourDetail.Slash,
                Type = BehaviourType.Atk,
                MotionDetail = MotionDetail.J,
                MotionDetailDefault = MotionDetail.N,
                Script = "",
                EffectRes = "WarpCrew_J"
            };

            DiceBehaviour die3 = new DiceBehaviour
            {

                Min = 5,
                Dice = 8,
                Detail = BehaviourDetail.Guard,
                Type = BehaviourType.Def,
                MotionDetail = MotionDetail.G,
                MotionDetailDefault = MotionDetail.N,
                Script = "",
                EffectRes = "WarpCrew_G"
            };


            this.upxmlinfo.DiceBehaviourList.Add(die3);
            this.upxmlinfo.DiceBehaviourList.Add(die);
            this.upxmlinfo.DiceBehaviourList.Add(die2);
            return this.upxmlinfo;
        }
    }

    public class UpgradeModel_RMR_DimensionalRift2 : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.index = 1;
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            
            this.baseid = new LorId(LogLikeMod.ModId, 506002);
        }
        public override DiceCardXmlInfo GetUpgradeInfo(int index, int count)
        {
            this.upxmlinfo = base.GetUpgradeInfo(index, count);
            this.upxmlinfo.DiceBehaviourList.Clear();
            DiceBehaviour die = new DiceBehaviour
            {

                Min = 4,
                Dice = 8,
                Detail = BehaviourDetail.Slash,
                Type = BehaviourType.Atk,
                MotionDetail = MotionDetail.H,
                MotionDetailDefault = MotionDetail.N,
                Script = "",
                EffectRes = "WarpCrew_H"
            };
            DiceBehaviour die2 = new DiceBehaviour
            {

                Min = 4,
                Dice = 8,
                Detail = BehaviourDetail.Slash,
                Type = BehaviourType.Atk,
                MotionDetail = MotionDetail.J,
                MotionDetailDefault = MotionDetail.N,
                Script = "",
                EffectRes = "WarpCrew_J"
            };

            DiceBehaviour die3 = new DiceBehaviour
            {

                Min = 5,
                Dice = 8,
                Detail = BehaviourDetail.Guard,
                Type = BehaviourType.Def,
                MotionDetail = MotionDetail.G,
                MotionDetailDefault = MotionDetail.N,
                Script = "",
                EffectRes = "WarpCrew_G"
            };



            this.upxmlinfo.DiceBehaviourList.Add(die);
            this.upxmlinfo.DiceBehaviourList.Add(die3);
            this.upxmlinfo.DiceBehaviourList.Add(die2);
            return this.upxmlinfo;
        }
    }

    public class UpgradeModel_RMR_DimensionalRift3 : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.index = 2;
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.baseid = new LorId(LogLikeMod.ModId, 506002);
        }

        public override DiceCardXmlInfo GetUpgradeInfo(int index, int count)
        {
            this.upxmlinfo = base.GetUpgradeInfo(index, count);
            this.upxmlinfo.DiceBehaviourList.Clear();
            DiceBehaviour die = new DiceBehaviour
            {

                Min = 4,
                Dice = 8,
                Detail = BehaviourDetail.Slash,
                Type = BehaviourType.Atk,
                MotionDetail = MotionDetail.H,
                MotionDetailDefault = MotionDetail.N,
                Script = "",
                EffectRes = "WarpCrew_H"
            };
            DiceBehaviour die2 = new DiceBehaviour
            {

                Min = 4,
                Dice = 8,
                Detail = BehaviourDetail.Slash,
                Type = BehaviourType.Atk,
                MotionDetail = MotionDetail.J,
                MotionDetailDefault = MotionDetail.N,
                Script = "",
                EffectRes = "WarpCrew_J"
            };
            DiceBehaviour die3 = new DiceBehaviour
            {

                Min = 5,
                Dice = 8,
                Detail = BehaviourDetail.Guard,
                Type = BehaviourType.Def,
                MotionDetail = MotionDetail.G,
                MotionDetailDefault = MotionDetail.N,
                Script = "",
                EffectRes = "WarpCrew_G"
            };

            this.upxmlinfo.DiceBehaviourList.Add(die);
            this.upxmlinfo.DiceBehaviourList.Add(die2);
            this.upxmlinfo.DiceBehaviourList.Add(die3);
            return this.upxmlinfo;
        }
    }

    public class UpgradeModel_RMR_Execute : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_ExecuteUpgrade");
            this.upgradeinfo.SetAbility(1, "RMR_ExecuteUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 505003);
        }
    }

    public class UpgradeModel_RMR_FleshFillet : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 3, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 507002);
        }
    }

    public class UpgradeModel_RMR_Slay : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 2, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 512005);
        }
    }

    public class UpgradeModel_RMR_sweeperpage : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_sweeperpageUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 504001);
        }
    }

    public class UpgradeModel_RMR_RipSpace : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_draw3pw");
            this.upgradeinfo.SetAbility(1, "RMR_RipSpaceDieUpgrade");
            this.upgradeinfo.SetSelfAbility("RMR_RipSpaceUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 506001);
        }
    }

    public class UpgradeModel_RMR_EndlessBattle : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 501005);
        }
    }

    public class UpgradeModel_RMR_Manikin : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 502002);
        }
    }

    public class UpgradeModel_RMR_Creak : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 502006);
        }
    }

    public class UpgradeModel_RMR_LetShowBegin : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.AddDice(1, 5, BehaviourDetail.Hit, "", MotionDetail.H, BehaviourType.Standby, "", "");
            this.baseid = new LorId(LogLikeMod.ModId, 503011);
        }
    }

    public class UpgradeModel_RMR_TraceFumes : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_Smoke2Atk");
            this.upgradeinfo.SetAbility(1, "RMR_Smoke3Atk");
            this.upgradeinfo.SetDice(2, 2, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 507004);
        }
    }

    public class UpgradeModel_RMR_SweepBackstreets : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(2, "recoverBreak5atk");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 504003);
        }
    }

    public class UpgradeModel_RMR_UndertakePrescript : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 505002);
        }
    }

    public class UpgradeModel_RMR_FlashingStrike : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_FlashingStrikeUpgrade");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 501003);
        }
    }

    public class UpgradeModel_RMR_ForFamily : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_ForTheFamilyUpgrade");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 504004);
        }
    }

    public class UpgradeModel_RMR_TrashDisposal : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("sweeperOnlyLog");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetDice(3, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 504002);
        }
    }

    public class UpgradeModel_RMR_BoundaryOfDeath : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 501001);
        }
    }

    public class UpgradeModel_RMR_BlazingStrike : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_BlazingStrikeDieUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 508010);
        }
    }

    public class UpgradeModel_RMR_ClaimBones : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetDice(0, 1, 7);
            this.baseid = new LorId(LogLikeMod.ModId, 512006);
        }
    }

    public class UpgradeModel_RMR_DeepDrag : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_Smoke9Atk");
            this.upgradeinfo.SetDice(0, 3, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 507001);
        }
    }

    public class UpgradeModel_RMR_ForcefulGesture : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(1, "RMR_ForcefulGestureUpgrade");
            this.upgradeinfo.SetDice(1, 3, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 502004);
        }
    }

    public class UpgradeModel_RMR_Ripple : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 2, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 506003);
        }
    }

    public class UpgradeModel_RMR_Overthrow : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_OverthrowUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 512004);
        }
    }

    public class UpgradeModel_RMR_WillofthePrescript : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 505004);
        }
    }

    public class UpgradeModel_RMR_Moulinet : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 511004);
        }
    }

    public class UpgradeModel_RMR_SharpNails : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(3, 1, 1);
            this.upgradeinfo.SetDice(4, 1, 1);
            this.upgradeinfo.AddDice(4, 8, BehaviourDetail.Hit, "", MotionDetail.H, BehaviourType.Standby, "Ema_J", "");
            this.baseid = new LorId(LogLikeMod.ModId, 503001);
        }
    }

    public class UpgradeModel_RMR_Overcharge1 : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_OverchargeUpgrade1");
            this.upgradeinfo.SetDice(3, 2, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 506004);
        }
    }

    public class UpgradeModel_RMR_Overcharge2 : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.index = 1;
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_OverchargeUpgrade2");
            this.upgradeinfo.SetDice(0, 0, 3);
            this.upgradeinfo.SetDice(1, 1, 2);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetDice(3, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 506004);
        }
    }

    public class UpgradeModel_RMR_LossOfSenses : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_LossOfSensesUpgrade");
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 507007);
        }
    }

    public class UpgradeModel_RMR_AutoMovement : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(1, "paralysis1atk");
            this.upgradeinfo.SetAbility(2, "binding2atk");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 502001);
        }
    }

    public class UpgradeModel_RMR_RepressedFlesh : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_RepressedFleshUpgrade");
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.upgradeinfo.SetDice(3, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 502003);
        }
    }

    #endregion

    #region Star of the City



    #endregion

}
