// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_ChStart
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryModel_ChStart : MysteryBase
    {
        public Dictionary<int, MysteryModel_ChStart.StartBoostData> choices;
        public List<MysteryModel_ChStart.StartBoostData> datas;

        public override void OnEnterChoice(int choiceid)
        {
            base.OnEnterChoice(choiceid);
            if (this.curFrame.FrameID != 0 || this.choices[choiceid].id != 3)
                return;
            SingletonBehavior<UIBattleOverlayManager>.Instance.EnableBufOverlay(abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("StartBoost3Name"), abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("StartBoost3Desc"), LogLikeMod.ArtWorks["Reward_StartBoost_3"], this.FrameObj["choice_btn" + choiceid.ToString()]);
        }

        public override void OnExitChoice(int choiceid)
        {
            base.OnExitChoice(choiceid);
            SingletonBehavior<UIBattleOverlayManager>.Instance.DisableOverlay();
        }

        public override void EndMystery()
        {
            base.EndMystery();
            SingletonBehavior<UIBattleOverlayManager>.Instance.DisableOverlay();
        }

        public void Event1() => PassiveAbility_MoneyCheck.AddMoney(20);

        public void Event2()
        {
            CardDropValueXmlInfo data1 = Singleton<CardDropValueList>.Instance.GetData(new LorId(LogLikeMod.ModId, 1001));
            CardDropValueXmlInfo data2 = Singleton<CardDropValueList>.Instance.GetData(new LorId(LogLikeMod.ModId, 2001));
            List<LorId> cardidlist = new List<LorId>();
            for (int index = 0; index < 9; ++index)
            {
                if ((double)UnityEngine.Random.value > 0.7)
                {
                    LorId id = RewardingModel.GetCard(data2).id;
                    cardidlist.Add(id);
                    LogueBookModels.AddCard(id);
                }
                else
                {
                    LorId id = RewardingModel.GetCard(data1).id;
                    cardidlist.Add(id);
                    LogueBookModels.AddCard(id);
                }
            }
            MysteryModel_CardCheck.PopupCardCheck(cardidlist, MysteryModel_CardCheck.CheckDescType.RewardDesc);
        }

        public void TestDummy(MysteryModel_CardChoice choice, DiceCardItemModel pick)
        {
            LogueBookModels.DeleteCard(pick.GetID());
            Singleton<MysteryManager>.Instance.EndMystery((MysteryBase)choice);
        }

        public void Event3()
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new MysteryModel_ChStart.Event3Effect());
        }

        public void Event0()
        {
        }

        public void Event4()
        {
            LogLikeMod.rewards_passive.Add(new RewardInfo()
            {
                grade = LogLikeMod.curchaptergrade,
                rewards = Singleton<RewardPassivesList>.Instance.GetChapterData(LogLikeMod.curchaptergrade, PassiveRewardListType.CommonReward, new LorId(-1))
            });
            LogLikeMod.rewards_passive.Add(new RewardInfo()
            {
                grade = LogLikeMod.curchaptergrade,
                rewards = Singleton<RewardPassivesList>.Instance.GetChapterData(LogLikeMod.curchaptergrade, PassiveRewardListType.CommonReward, new LorId(-1))
            });
        }

        public void DataInit()
        {
            this.datas = new List<MysteryModel_ChStart.StartBoostData>();
            this.datas.Add(new MysteryModel_ChStart.StartBoostData(0, new MysteryModel_ChStart.StartBoostData.Effect(this.Event0)));
            this.datas.Add(new MysteryModel_ChStart.StartBoostData(1, new MysteryModel_ChStart.StartBoostData.Effect(this.Event1)));
            this.datas.Add(new MysteryModel_ChStart.StartBoostData(2, new MysteryModel_ChStart.StartBoostData.Effect(this.Event2)));
            this.datas.Add(new MysteryModel_ChStart.StartBoostData(4, new MysteryModel_ChStart.StartBoostData.Effect(this.Event4)));
        }

        public void ListInit()
        {
            if (this.datas == null)
                this.DataInit();
            List<MysteryModel_ChStart.StartBoostData> startBoostDataList = new List<MysteryModel_ChStart.StartBoostData>();
            this.choices = new Dictionary<int, MysteryModel_ChStart.StartBoostData>();
            this.datas = this.datas.OrderBy<MysteryModel_ChStart.StartBoostData, int>((Func<MysteryModel_ChStart.StartBoostData, int>)(x => Singleton<System.Random>.Instance.Next())).ToList<MysteryModel_ChStart.StartBoostData>();
            for (int index = 0; index < 4; ++index)
            {
                this.FrameObj["Desc" + index.ToString()].GetComponent<TextMeshProUGUI>().text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("MysteryChStartChoice" + this.datas[index].id.ToString());
                this.choices[index] = this.datas[index];
            }
        }

        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (id != 0)
                return;
            this.ListInit();
        }

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0)
                this.choices[choiceid].effect();
            int next = this.curFrame.Getchoice(choiceid).next;
            base.OnClickChoice(choiceid);
        }

        public class StartBoostData
        {
            public int id;
            public MysteryModel_ChStart.StartBoostData.Effect effect;

            public StartBoostData(int id, MysteryModel_ChStart.StartBoostData.Effect dele)
            {
                this.id = id;
                this.effect = dele;
            }

            public delegate void Effect();
        }

        public class Event3Effect : GlobalLogueEffectBase
        {
            public static bool IsRandom = true;
            public static Rarity ItemRarity = Rarity.Rare;

            public override void OnCreateLibrarian(BattleUnitModel model)
            {
                model.emotionDetail.SetEmotionLevel(1);
            }

            public override void OnStartBattle()
            {
                base.OnStartBattle();
                if (LogLikeMod.curchaptergrade == ChapterGrade.Grade1 || LogLikeMod.curchaptergrade == ChapterGrade.Grade2)
                    return;
                this.Destroy();
            }

            public override string KeywordId => "GlobalEffect_EgoWeed";

            public override string KeywordIconId => "Reward_StartBoost_3";
        }
    }
}
