// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_MemberShip
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryModel_MemberShip : MysteryBase
    {
        public MysteryModel_MemberShip.curWorkShop curshop;

        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (id != 1)
                return;
            this.FrameObj["Dia"].GetComponent<TextMeshProUGUI>().text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("MemberShipFrame1Dia" + this.curshop.ToString());
        }

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0)
            {
                if (choiceid == 0)
                    this.curshop = MysteryModel_MemberShip.curWorkShop.Logic;
                if (choiceid == 1)
                    this.curshop = MysteryModel_MemberShip.curWorkShop.Union;
                if (choiceid == 2)
                    this.curshop = MysteryModel_MemberShip.curWorkShop.Stigma;
                if (choiceid == 3)
                    this.curshop = MysteryModel_MemberShip.curWorkShop.Mook;
            }
            if (this.curFrame.FrameID == 2)
            {
                switch (this.curshop)
                {
                    case MysteryModel_MemberShip.curWorkShop.Logic:
                        this.AddWorkShop_Logic();
                        break;
                    case MysteryModel_MemberShip.curWorkShop.Union:
                        this.AddWorkShop_Union();
                        break;
                    case MysteryModel_MemberShip.curWorkShop.Stigma:
                        this.AddWorkShop_Stigma();
                        break;
                    case MysteryModel_MemberShip.curWorkShop.Mook:
                        this.AddWorkShop_Mook();
                        break;
                }
            }
            base.OnClickChoice(choiceid);
        }

        public void AddWorkShop_Logic()
        {
            foreach (KeyValuePair<ChapterGrade, List<LogueStageInfo>> remainStage in LogueBookModels.RemainStageList)
            {
                if (remainStage.Key >= LogLikeMod.curchaptergrade)
                    remainStage.Value.Add(Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 80001)));
            }
            foreach (UnitBattleDataModel model in LogueBookModels.playerBattleModel)
                LogueBookModels.AddPlayerStat(model, (LogStatAdder)new MysteryModel_MemberShip.LogicRangeAdder());
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new MysteryModel_MemberShip.LogicRangeGlobalEffect());
        }

        public void AddWorkShop_Union()
        {
            foreach (KeyValuePair<ChapterGrade, List<LogueStageInfo>> remainStage in LogueBookModels.RemainStageList)
            {
                if (remainStage.Key >= LogLikeMod.curchaptergrade)
                    remainStage.Value.Add(Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 81001)));
            }
        }

        public void AddWorkShop_Stigma()
        {
            foreach (KeyValuePair<ChapterGrade, List<LogueStageInfo>> remainStage in LogueBookModels.RemainStageList)
            {
                if (remainStage.Key >= LogLikeMod.curchaptergrade)
                    remainStage.Value.Add(Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 82001)));
            }
        }

        public void AddWorkShop_Mook()
        {
            foreach (KeyValuePair<ChapterGrade, List<LogueStageInfo>> remainStage in LogueBookModels.RemainStageList)
            {
                if (remainStage.Key >= LogLikeMod.curchaptergrade)
                    remainStage.Value.Add(Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 83001)));
            }
        }

        public enum curWorkShop
        {
            Logic,
            Union,
            Stigma,
            Mook,
        }

        public class LogicRangeAdder : LogStatAdder
        {
            public override EquipRangeType GetRangeType(EquipRangeType cur) => EquipRangeType.Hybrid;
        }

       
        public class LogicRangeGlobalEffect : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Rare;

            public override void OnAddSubPlayer(UnitDataModel model)
            {
                LogueBookModels.AddPlayerStat(model, (LogStatAdder)new MysteryModel_MemberShip.LogicRangeAdder());
            }

            public override string KeywordId => "GlobalEffect_ThumbTrainCerti";

            public override string KeywordIconId => "GlobalEffect_MemberShipLogic";
        }
    }
}
