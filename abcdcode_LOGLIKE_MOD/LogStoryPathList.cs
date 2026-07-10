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
using System.Text;
using System.Xml.Serialization;
using UI;
using UnityEngine;
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
            string lang = TextDataModel.CurrentLanguage != null
                ? TextDataModel.CurrentLanguage.ToString()
                : "cn";

            // Workshop layout historically stores Chinese under StoryInfo/cn/, while
            // LoadStoryFile only looked at StoryInfo/Localize/{lang}/ (en/kr only).
            // Missing Localize/cn opened an empty story scene → all-tofu dialogue.
            bool isSatellite = pathInfo.pid != LogLikeMod.ModId;
            string storyRoot = isSatellite
                ? $"{modPath}/Assemblies/Roguedlls/StoryInfo"
                : $"{modPath}/Assemblies/dlls/StoryInfo";
            string effectRoot = storyRoot + "/EffectInfo";

            string storyPath = ResolveStoryLocalizePath(storyRoot, lang, pathInfo.localizepath);
            string effectPath = Path.Combine(effectRoot, pathInfo.effectpath);

            if (string.IsNullOrEmpty(storyPath) || !File.Exists(storyPath))
            {
                Debug.LogError($"[RMR Story] Story file missing for id={id.id} lang={lang} (checked Localize/{lang} and {lang}/ under StoryInfo).");
                return;
            }

            bool loaded = LogStoryPathList.LoadStoryFile(storyPath, effectPath, modPath);
            if (!loaded)
            {
                Debug.LogError($"[RMR Story] Failed to deserialize story '{storyPath}'.");
                return;
            }

            Debug.Log($"[RMR Story] Loaded story id={id.id} lang={lang} path={storyPath}");
            StorySerializer.curEpisodeIdx = pathInfo.episode;
            StorySerializer.curgroupidx = pathInfo.group;
            if (!OpenStory)
                return;
            if (endFunc == null)
                endFunc = DefaultStoryEnd;
            this.ActivateStoryScene(endFunc);
        }

        /// <summary>
        /// Prefer Localize/{lang}/file, then {lang}/file (cn lives here), then Localize/en, then en.
        /// </summary>
        private static string ResolveStoryLocalizePath(string storyRoot, string language, string fileName)
        {
            if (string.IsNullOrEmpty(storyRoot) || string.IsNullOrEmpty(fileName))
                return null;

            string lang = (language ?? "cn").Trim();
            // Normalize common aliases
            string langKey = lang.ToLowerInvariant();
            if (langKey == "zh" || langKey == "zh-cn" || langKey == "chs" || langKey == "cn")
                lang = "cn";
            else if (langKey == "ko" || langKey == "kr")
                lang = "kr";
            else if (langKey == "ja" || langKey == "jp")
                lang = "jp";
            else if (langKey == "en" || langKey.StartsWith("en"))
                lang = "en";

            string[] candidates =
            {
                Path.Combine(storyRoot, "Localize", lang, fileName),
                Path.Combine(storyRoot, lang, fileName),
                // Chinese install often has only StoryInfo/cn + Localize/en|kr
                Path.Combine(storyRoot, "cn", fileName),
                Path.Combine(storyRoot, "Localize", "cn", fileName),
                Path.Combine(storyRoot, "Localize", "en", fileName),
                Path.Combine(storyRoot, "en", fileName),
            };

            foreach (string path in candidates)
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    return path;
            }
            return candidates[0];
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
                if (!File.Exists(storyPath))
                    return false;

                // Files are UTF-8 Chinese; system-default StreamReader can corrupt on some installs.
                using (StreamReader streamReader = new StreamReader(storyPath, Encoding.UTF8, detectEncodingFromByteOrderMarks: true))
                {
                    ScenarioRoot scenarioRoot = new XmlSerializer(typeof(ScenarioRoot)).Deserialize((TextReader)streamReader) as ScenarioRoot;
                    if (scenarioRoot == null || scenarioRoot.groups == null || scenarioRoot.groups.Count == 0
                        || scenarioRoot.groups[0] == null
                        || scenarioRoot.groups[0].episodes == null || scenarioRoot.groups[0].episodes.Count == 0)
                    {
                        Debug.LogError("[RMR Story] ScenarioRoot empty or invalid: " + storyPath);
                        return false;
                    }
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
                    using (StreamReader streamReader = new StreamReader(effectPath, Encoding.UTF8, detectEncodingFromByteOrderMarks: true))
                    {
                        StorySerializer.effectDefinition = (SceneEffect)new XmlSerializer(typeof(SceneEffect)).Deserialize((TextReader)streamReader);
                        StorySerializer.isMod = true;
                        StorySerializer.curModPath = modPath;
                        StorySerializer.customEffectFilePath = effectPath;
                    }
                }
                else
                {
                    Debug.LogWarning("[RMR Story] Effect file missing (story still loaded): " + effectPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("[RMR Story] LoadStoryFile exception: " + ex);
                return false;
            }
        }
    }
}
