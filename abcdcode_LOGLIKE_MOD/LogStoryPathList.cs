// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogStoryPathList
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;
using LOR_XML;
using Mod;
using StoryScene;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

            // Prefer language-specific paths. Only fall back to StoryInfo/cn for Chinese
            // (or as last-chance when lang is already cn). Never feed cn into kr/jp sessions.
            var candidates = new List<string>
            {
                Path.Combine(storyRoot, "Localize", lang, fileName),
                Path.Combine(storyRoot, lang, fileName),
            };
            if (lang == "cn")
            {
                candidates.Add(Path.Combine(storyRoot, "cn", fileName));
                candidates.Add(Path.Combine(storyRoot, "Localize", "cn", fileName));
            }
            candidates.Add(Path.Combine(storyRoot, "Localize", "en", fileName));
            candidates.Add(Path.Combine(storyRoot, "en", fileName));

            foreach (string path in candidates)
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    return path;
            }
            return candidates[0];
        }

        public void ActivateStoryScene(StoryRoot.OnEndStoryFunc func)
        {
            try
            {
                bool hasUi = UI.UIController.Instance != null;
                UIPhase phase = hasUi ? UI.UIController.Instance.CurrentUIPhase : default(UIPhase);
                // Chapter intro during invitation / battle setting / story UI phase,
                // or when UI controller is not ready yet (early AfterInitialize).
                bool menuCutscene = !hasUi
                    || phase == UIPhase.BattleSetting
                    || phase == UIPhase.Invitation
                    || phase == UIPhase.Story;
                if (menuCutscene)
                {
                    if (GameSceneManager.Instance != null)
                    {
                        if (GameSceneManager.Instance.uIController != null)
                            GameSceneManager.Instance.uIController.gameObject.SetActive(false);
                        if (GameSceneManager.Instance.storyRoot != null)
                            GameSceneManager.Instance.storyRoot.gameObject.SetActive(true);
                    }
                    try { SingletonBehavior<UIPopupWindowManager>.Instance?.AllClose(); } catch { }
                    try { UISoundManager.instance?.SetGameStateBGM(GameCurrentState.Story); } catch { }
                    if (StoryRoot.Instance != null)
                        StoryRoot.Instance.OpenStory(func, true);
                    return;
                }

                // In-battle cutscene: NEVER touch BattleSceneRoot._battleStarted directly
                // (MonoMod/FieldAccessException). Use reflection.
                bool battleStarted = false;
                try
                {
                    var root = SingletonBehavior<BattleSceneRoot>.Instance;
                    if (root != null)
                    {
                        FieldInfo f = typeof(BattleSceneRoot).GetField("_battleStarted", AccessTools.all);
                        if (f != null)
                            battleStarted = (bool)f.GetValue(root);
                    }
                }
                catch { battleStarted = false; }

                if (battleStarted)
                {
                    try { SingletonBehavior<BattleSoundManager>.Instance?.EndBgm(); } catch { }
                    try
                    {
                        SingletonBehavior<BattleManagerUI>.Instance?.ui_battleStory?.OpenStory(() => func(), false, true);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning("[RMR Story] battle story open failed: " + ex.Message);
                        try { func?.Invoke(); } catch { }
                    }
                }
                else
                {
                    // Fallback: open as menu cutscene so intro is never dropped silently.
                    if (GameSceneManager.Instance != null)
                    {
                        if (GameSceneManager.Instance.uIController != null)
                            GameSceneManager.Instance.uIController.gameObject.SetActive(false);
                        if (GameSceneManager.Instance.storyRoot != null)
                            GameSceneManager.Instance.storyRoot.gameObject.SetActive(true);
                    }
                    if (StoryRoot.Instance != null)
                        StoryRoot.Instance.OpenStory(func, true);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("[RMR Story] ActivateStoryScene failed (non-fatal): " + ex);
                try { func?.Invoke(); } catch { }
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
