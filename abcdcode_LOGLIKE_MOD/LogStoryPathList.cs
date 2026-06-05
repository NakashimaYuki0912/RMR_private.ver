// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogStoryPathList
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_XML;
using Mod;
using StoryScene;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UI;
using WorkParser;


namespace abcdcode_LOGLIKE_MOD
{

    public class LogStoryPathList : Singleton<LogStoryPathList>
    {
        public List<LogStoryPathInfo> list;

        public LogStoryPathList() => this.list = new List<LogStoryPathInfo>();

        public void AddStoryPathInfo(List<LogStoryPathInfo> infolist)
        {
            this.list.AddRange(infolist);
        }

        public LogStoryPathInfo GetPathInfo(LorId id)
        {
            return this.list.Find(x => x.Id == id);
        }

        public void LoadStoryFile(LorId id, StoryRoot.OnEndStoryFunc endFunc = null, bool OpenStory = true)
        {
            LogStoryPathInfo pathInfo = this.GetPathInfo(id);
            if (pathInfo == null)
                return;
            string modPath = ModContentManager.Instance.GetModPath(pathInfo.pid);
            string storyPath = $"{modPath}/Assemblies/dlls/StoryInfo/Localize/{TextDataModel.CurrentLanguage}/{pathInfo.localizepath}";
            string effectPath = $"{modPath}/Assemblies/dlls/StoryInfo/EffectInfo/{pathInfo.effectpath}";
            if (pathInfo.pid != LogLikeMod.ModId)
            {
                storyPath = $"{modPath}/Assemblies/Roguedlls/StoryInfo/Localize/{TextDataModel.CurrentLanguage}/{pathInfo.localizepath}";
                effectPath = $"{modPath}/Assemblies/Roguedlls/StoryInfo/EffectInfo/{pathInfo.effectpath}";
            }
            LogStoryPathList.LoadStoryFile(storyPath, effectPath, modPath);
            StorySerializer.curEpisodeIdx = pathInfo.episode;
            StorySerializer.curgroupidx = pathInfo.group;
            if (!OpenStory)
                return;
            if (endFunc == null)
                endFunc = DefaultStoryEnd;
            this.ActivateStoryScene(endFunc);
        }

        public void ActivateStoryScene(StoryRoot.OnEndStoryFunc func)
        {
            var phase = UI.UIController.Instance.CurrentUIPhase;
            if (phase == UIPhase.BattleSetting || phase == UIPhase.Invitation || phase == UIPhase.Story) // regular cutscene
            {
                GameSceneManager.Instance.uIController.gameObject.SetActive(false);
                GameSceneManager.Instance.storyRoot.gameObject.SetActive(true);
                SingletonBehavior<UIPopupWindowManager>.Instance.AllClose();
                UISoundManager.instance.SetGameStateBGM(GameCurrentState.Story);
                StoryRoot.Instance.OpenStory(func, true);
            }
            else if (SingletonBehavior<BattleSceneRoot>.Instance._battleStarted) // battle cutscene
            {
                SingletonBehavior<BattleSoundManager>.Instance.EndBgm();
                SingletonBehavior<BattleManagerUI>.Instance.ui_battleStory.OpenStory(() => func(), false, true);
            }
        }

        public static void DefaultStoryEnd()
        {
            if (SingletonBehavior<BattleManagerUI>.Instance.ui_battleStory.isActiveAndEnabled) // return BGM in battle
                SingletonBehavior<BattleSoundManager>.Instance.StartBgm();
            else // if not in battle, re-activate menu UIs
                GameSceneManager.Instance.ActivateUIController();
            // either way, do a cool little transition that'd be cool
            SingletonBehavior<UIBgScreenChangeAnim>.Instance.StartBg(UIScreenChangeType.EnterBattleSetting);
        }

        public static bool LoadStoryFile(string storyPath, string effectPath, string modPath)
        {
            try
            {
                if (File.Exists(storyPath))
                {
                    using (StreamReader streamReader = new StreamReader(storyPath))
                    {
                        ScenarioRoot scenarioRoot = new XmlSerializer(typeof(ScenarioRoot)).Deserialize((TextReader)streamReader) as ScenarioRoot;
                        StorySerializer.curChapter = scenarioRoot.chapter;
                        StorySerializer.curEpisode = scenarioRoot.groups[0].episodes[0];
                        StorySerializer.curEpisodeIdx = -1;
                        StorySerializer.curgroupidx = -1;
                        StorySerializer.curEpisodeNum = -1;
                        StorySerializer.curScenario = scenarioRoot;
                        StorySerializer.customStoryFilePath = storyPath;
                    }
                    if (File.Exists(effectPath))
                    {
                        using (StreamReader streamReader = new StreamReader(effectPath))
                        {
                            StorySerializer.effectDefinition = (SceneEffect)new XmlSerializer(typeof(SceneEffect)).Deserialize((TextReader)streamReader);
                            StorySerializer.isMod = true;
                            StorySerializer.curModPath = modPath;
                            StorySerializer.customEffectFilePath = effectPath;
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}
