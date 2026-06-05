using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;

namespace RogueLike_Mod_Reborn
{
    public class MysteryModel_RMR_ChStartNew : MysteryBase
    {
        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0)
            {
                switch (choiceid)
                {
                    case 0:
                        Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_IronHeart());
                        break;
                    case 1:
                        Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_HunterCloak());
                        break;
                    case 2:
                        Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_StrangeOrb());
                        break;
                    case 3:
                        Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_ViciousGlasses());
                        break;
                    case 4:
                        Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_LightsGuidance());
                        break;
                    case 5:
                        Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_RoadlessCamelot());
                        break;
                    default:
                        break;
                }
            }
            base.OnClickChoice(choiceid);
        }

        public override void OnEnterChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0)
            {
                switch (choiceid)
                {
                    case 0:
                        this.ShowOverlayOverButton(new RMREffect_IronHeart(), choiceid);
                        break;
                    case 1:
                        this.ShowOverlayOverButton(new RMREffect_HunterCloak(), choiceid);
                        break;
                    case 2:
                        this.ShowOverlayOverButton(new RMREffect_StrangeOrb(), choiceid);
                        break;
                    case 3:
                        this.ShowOverlayOverButton(new RMREffect_ViciousGlasses(), choiceid);
                        break;
                    case 4:
                        this.ShowOverlayOverButton(new RMREffect_LightsGuidance(), choiceid);
                        break;
                    case 5:
                        this.ShowOverlayOverButton(new RMREffect_RoadlessCamelot(), choiceid);
                        break;
                    default:
                        break;
                }
            }
            base.OnEnterChoice(choiceid);
        }

        public override void OnExitChoice(int choiceid)
        {
            base.OnExitChoice(choiceid);
            this.CloseOverlayOverButton();
        }
    }


    public class MysteryModel_RMR_CopleyIndex1 : MysteryBase
    {
        public override void OnClickChoice(int choiceid)
        {
            switch (this.curFrame.FrameID)
            {
                case 0:
                    switch (choiceid)
                    {
                        case 0:
                            if (!LogueBookModels.RemainStageList[ChapterGrade.Grade5].Exists((LogueStageInfo x) => x.Id == new LorId(LogLikeMod.ModId, 150003)))
                            {
                                LogueStageInfo stageInfo = Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 150003));
                                stageInfo.type = abcdcode_LOGLIKE_MOD.StageType.Mystery;
                                stageInfo.stageid = 150003;
                                MysteryBase.AddStageList(stageInfo, ChapterGrade.Grade5);
                            }
                            LogueBookModels.AddMoney(20);
                            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_Prescript());
                            break;
                    }
                    break;
            }
            base.OnClickChoice(choiceid);
        }

        public override void OnEnterChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0)
            {
                switch (choiceid)
                {
                    case 0:
                        this.ShowOverlayOverButton(new RMREffect_Prescript(), choiceid);
                        break;
                    default:
                        break;
                }
            }
            base.OnEnterChoice(choiceid);
        }

        public override void OnExitChoice(int choiceid)
        {
            base.OnExitChoice(choiceid);
            this.CloseOverlayOverButton();
        }
    }

    public class PassiveAbility_RMR_FraudIndexEvent : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            Singleton<MysteryManager>.Instance.StartMystery(new LorId(LogLikeMod.ModId, 10003));
        }
    }

    public class PassiveAbility_RMR_RealIndexEvent : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            Singleton<MysteryManager>.Instance.StartMystery(new LorId(LogLikeMod.ModId, 110003));
        }
    }

    public class MysteryModel_RMR_CopleyIndex2 : MysteryBase
    {
        public override void OnClickChoice(int choiceid)
        {
            switch (this.curFrame.FrameID)
            {
                case 0:
                    switch (choiceid)
                    {
                        case 0:
                            DisablePrescript();
                            MysteryBase.SetNextStageCustom(new LorId(LogLikeMod.ModId, 1500031), abcdcode_LOGLIKE_MOD.StageType.Normal);
                            break;
                        case 1:
                            MysteryBase.SetNextStageCustom(new LorId(LogLikeMod.ModId, 1500032), abcdcode_LOGLIKE_MOD.StageType.Normal);
                            break;
                        case 2:
                            LosePrescript();
                            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList(Faction.Player))
                            {
                                battleUnitModel.TakeDamage(battleUnitModel.MaxHp / 5, DamageType.ETC);
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            base.OnClickChoice(choiceid);
        }

        public static void DisablePrescript()
        {
            (Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find(x => x is RMREffect_Prescript) as RMREffect_Prescript).disable = true;
        }

        public static int LosePrescript()
        {
            int count = 0;
            List<GlobalLogueEffectBase> list = Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().FindAll(x => x is RMREffect_Prescript);
            if (list.Count > 0)
            {
                foreach (GlobalLogueEffectBase item in list)
                {
                    count++;
                    Singleton<GlobalLogueEffectManager>.Instance.RemoveEffect(item);
                }
            }
            return count;
        }
    }


    public class MysteryModel_RMR_LiuTraining : MysteryBase
    {
        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (id == 0)
            {
                if (LogueBookModels.GetMoney() < 30)
                    this.DisableButton(1);
                if (LogueBookModels.GetMoney() < 20)
                    this.DisableButton(0);
            }
        }
        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0)
            {
                switch (choiceid)
                {
                    case 0:
                        LogueBookModels.SubMoney(20);
                        GlobalLogueEffectManager.Instance.AddEffects(new RMREffect_IronMountain());
                        break;
                    case 1:
                        LogueBookModels.SubMoney(30);
                        GlobalLogueEffectManager.Instance.AddEffects(new RMREffect_DragonFist());
                        break;
                }
                this.CloseOverlayOverButton();
            }
            else if (this.curFrame.FrameID == 3 && choiceid == 0)
            {
                GlobalLogueEffectManager.Instance.AddEffects(new RMREffect_IronMountain());
                GlobalLogueEffectManager.Instance.AddEffects(new RMREffect_DragonFist());
                MysteryBase.SetNextStageCustom(new LorId(RMRCore.packageId, 1400041), abcdcode_LOGLIKE_MOD.StageType.Normal);
            }
            base.OnClickChoice(choiceid);
        }
        public override void OnEnterChoice(int choiceid)
        {
            base.OnEnterChoice(choiceid);
            if (this.curFrame.FrameID == 0)
            {
                switch (choiceid) { 
                    case 0:
                        this.ShowOverlayOverButton(new RMREffect_IronMountain(), choiceid);
                        break;
                    case 1:
                        this.ShowOverlayOverButton(new RMREffect_DragonFist(), choiceid);
                        break;
                }
            }
        }

        public override void OnExitChoice(int choiceid)
        {
            base.OnExitChoice(choiceid);
            this.CloseOverlayOverButton();
        }

    }
}