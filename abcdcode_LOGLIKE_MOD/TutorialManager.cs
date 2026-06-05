// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.TutorialManager
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class TutorialManager : Singleton<TutorialManager>
    {
        public bool Inited;
        public UIManualContentPanel Panel;
        public Dictionary<string, bool> IsSeeTutoDic;
        public Dictionary<string, TutorialManager.TutoInfo> TutorialDic;

        public bool IsSeeTuto(string contentname)
        {
            return this.IsSeeTutoDic.ContainsKey(contentname) && this.IsSeeTutoDic[contentname];
        }

        public void LoadTuto(string contentname)
        {
        }

        public void LoadSaveData()
        {
            SaveData saveData = Singleton<LogueSaveManager>.Instance.LoadData("Tutorial");
            if (saveData == null)
                return;
            foreach (KeyValuePair<string, SaveData> keyValuePair in saveData.GetDictionarySelf())
                this.IsSeeTutoDic[keyValuePair.Key] = keyValuePair.Value.GetIntSelf() == 1;
        }

        public SaveData CreateSaveData()
        {
            SaveData saveData = new SaveData();
            if (this.IsSeeTutoDic == null || this.IsSeeTutoDic.Count == 0)
                return saveData;
            foreach (KeyValuePair<string, bool> keyValuePair in this.IsSeeTutoDic)
                saveData.AddData(keyValuePair.Key, new SaveData(keyValuePair.Value ? 1 : 0));
            return saveData;
        }

        public TutorialManager.TutoInfo FindLogTuto(string contentname)
        {
            return this.TutorialDic.Values.ToList<TutorialManager.TutoInfo>().Find((Predicate<TutorialManager.TutoInfo>)(x => x.name == contentname));
        }

        public TutorialManager.TutoInfo FindLogTuto(UIManualScreenPage page)
        {
            return this.TutorialDic.Values.ToList<TutorialManager.TutoInfo>().Find((Predicate<TutorialManager.TutoInfo>)(x => x.Tutopage == page));
        }

        public void ConnectTuto(string prev, string next)
        {
            this.FindLogTuto(prev).Tutopage.nextid = this.FindLogTuto(next).Tutopage.currentid;
        }

        public void CreateTuto(string contentname, string ArtWorkName)
        {
            int count = this.Panel.ManualContentData[0].subtitles[0].ContentList.Count;
            UIManualScreenPage content = this.Panel.ManualContentData[0].subtitles[0].ContentList[0];
            UIManualScreenPage manualScreenPage = UnityEngine.Object.Instantiate<UIManualScreenPage>(content);
            manualScreenPage.transform.SetParent(content.transform.parent);
            manualScreenPage.transform.localPosition = content.transform.localPosition;
            manualScreenPage.transform.localScale = content.transform.localScale;
            Transform child1 = manualScreenPage.transform.GetChild(0).GetChild(1);
            Transform child2 = manualScreenPage.transform.GetChild(0).GetChild(2);
            Transform child3 = manualScreenPage.transform.GetChild(0).GetChild(3);
            UnityEngine.Object.DestroyImmediate(child1.gameObject);
            UnityEngine.Object.DestroyImmediate(child2.gameObject);
            UnityEngine.Object.DestroyImmediate(child3.gameObject);
            manualScreenPage.panel = content.panel;
            manualScreenPage.currentid.titleid = 1;
            manualScreenPage.currentid.subtitleid = 1;
            manualScreenPage.currentid.subsubtitleid = count + 1;
            manualScreenPage.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            manualScreenPage.nextid.subsubtitleid = -1;
            manualScreenPage.nextid.subtitleid = -1;
            manualScreenPage.nextid.titleid = -1;
            manualScreenPage.Init();
            manualScreenPage.OpenInit();
            this.Panel.ManualContentData[0].subtitles[0].ContentList.Add(manualScreenPage);
            this.IsSeeTutoDic.Add(contentname, false);
            this.TutorialDic.Add(contentname, new TutorialManager.TutoInfo()
            {
                ArtWork = ArtWorkName == string.Empty ? contentname : ArtWorkName,
                name = contentname,
                Tutopage = manualScreenPage
            });
        }

        public void Init(UIManualContentPanel __instance)
        {
            this.Panel = __instance;
            this.TutorialDic = new Dictionary<string, TutorialManager.TutoInfo>();
            this.IsSeeTutoDic = new Dictionary<string, bool>();
            this.CreateTuto("tutorial_EquipPage1_1new", "tutorial_EquipPage1_1new");
            this.CreateTuto("tutorial_EquipPage1_2new", "tutorial_EquipPage1_2new");
            this.ConnectTuto("tutorial_EquipPage1_1new", "tutorial_EquipPage1_2new");
            this.CreateTuto("tutorial_BattlePage1_1", "tutorial_BattlePage1_1");
            this.CreateTuto("tutorial_BattlePage1_2", "tutorial_BattlePage1_2");
            this.ConnectTuto("tutorial_BattlePage1_1", "tutorial_BattlePage1_2");
            this.CreateTuto("tutorial_EmotionPage1_1", "tutorial_EmotionPage1_1");
            this.CreateTuto("tutorial_EmotionPage1_2", "tutorial_EmotionPage1_2");
            this.ConnectTuto("tutorial_EmotionPage1_1", "tutorial_EmotionPage1_2");
            this.LoadSaveData();
            this.Inited = true;
        }

        public class TutoInfo
        {
            public UIManualScreenPage Tutopage;
            public string ArtWork;
            public string name;
        }
    }
}
