// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_ChStart
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using System.Linq;
using RogueLike_Mod_Reborn;
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
            if (this.curFrame.FrameID != 0 || this.choices == null || !this.choices.ContainsKey(choiceid) || this.choices[choiceid].id != 3)
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

        public void Event5()
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_RoadlessCamelot());
        }

        public void Event6()
        {
            if (!RMRRealizationManager.InitialRelicEntryAvailable)
                return;

            var panel = Singleton<LogRealizationPanel>.Instance;
            if (panel == null)
            {
                GameObject go = new GameObject("LogRealizationPanel");
                panel = go.AddComponent<LogRealizationPanel>();
            }

            Transform parent = null;
            if (this.FrameObj != null && this.FrameObj.ContainsKey("Frame") && this.FrameObj["Frame"] != null)
                parent = this.FrameObj["Frame"].transform;
            else
            {
                try
                {
                    if (LogLikeMod.LogUIObjs != null && LogLikeMod.LogUIObjs[90] != null)
                        parent = LogLikeMod.LogUIObjs[90].transform;
                }
                catch
                {
                }
            }

            panel.Show(parent);
        }

        public void Event7()
        {
            this.SwapFrame(2);
        }

        public void Event8()
        {
            this.SwapFrame(3);
        }

        public void DataInit()
        {
            this.datas = new List<MysteryModel_ChStart.StartBoostData>();
            this.datas.Add(new MysteryModel_ChStart.StartBoostData(0, new MysteryModel_ChStart.StartBoostData.Effect(this.Event0)));
            this.datas.Add(new MysteryModel_ChStart.StartBoostData(1, new MysteryModel_ChStart.StartBoostData.Effect(this.Event1)));
            this.datas.Add(new MysteryModel_ChStart.StartBoostData(2, new MysteryModel_ChStart.StartBoostData.Effect(this.Event2)));
            this.datas.Add(new MysteryModel_ChStart.StartBoostData(3, new MysteryModel_ChStart.StartBoostData.Effect(this.Event3)));
            this.datas.Add(new MysteryModel_ChStart.StartBoostData(4, new MysteryModel_ChStart.StartBoostData.Effect(this.Event4)));
            this.datas.Add(new MysteryModel_ChStart.StartBoostData(5, new MysteryModel_ChStart.StartBoostData.Effect(this.Event5)));
            this.datas.Add(new MysteryModel_ChStart.StartBoostData(6, new MysteryModel_ChStart.StartBoostData.Effect(this.Event6)));
            this.datas.Add(new MysteryModel_ChStart.StartBoostData(7, new MysteryModel_ChStart.StartBoostData.Effect(this.Event7)));
            this.datas.Add(new MysteryModel_ChStart.StartBoostData(8, new MysteryModel_ChStart.StartBoostData.Effect(this.Event8)));
        }

        public void ListInit()
        {
            if (this.datas == null)
                this.DataInit();
            List<MysteryModel_ChStart.StartBoostData> startBoostDataList = new List<MysteryModel_ChStart.StartBoostData>();
            this.choices = new Dictionary<int, MysteryModel_ChStart.StartBoostData>();
            var realizationData = this.datas.Find(x => x.id == 6);
            var relics = this.datas
                .Where(x => x.id != 6)
                .OrderBy(x => x.id)
                .ToList<MysteryModel_ChStart.StartBoostData>();
            startBoostDataList.AddRange(relics);
            if (RMRRealizationManager.InitialRelicEntryAvailable && realizationData != null)
                startBoostDataList.Add(realizationData);

            for (int index = 0; index < this.curFrame.choices.Count; ++index)
            {
                bool active = index < startBoostDataList.Count;
                if (this.FrameObj.ContainsKey("choice_btn" + index.ToString()))
                    this.FrameObj["choice_btn" + index.ToString()].SetActive(active);
                if (!active)
                    continue;
                this.FrameObj["Desc" + index.ToString()].GetComponent<TextMeshProUGUI>().text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("MysteryChStartChoice" + startBoostDataList[index].id.ToString());
                this.choices[index] = startBoostDataList[index];
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
            {
                if (this.choices == null || !this.choices.ContainsKey(choiceid))
                    return;
                var data = this.choices[choiceid];
                data.effect();
                if (data.id == 6 || data.id == 7 || data.id == 8)
                    return;
                RMRRealizationManager.SetInitialRelicEntryAvailable(false);
            }
            else if (this.curFrame.FrameID == 2)
            {
                if (choiceid == 0)
                {
                    RMRCore.ResetAllArchiveProgress();
                    this.SwapFrame(4);
                    return;
                }
                if (choiceid == 1)
                {
                    this.SwapFrame(0);
                    return;
                }
            }
            else if ((this.curFrame.FrameID == 3 || this.curFrame.FrameID == 4) && choiceid == 0)
            {
                this.SwapFrame(0);
                return;
            }
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
