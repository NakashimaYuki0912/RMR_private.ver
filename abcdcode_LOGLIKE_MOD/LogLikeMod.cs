// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogLikeMod
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using BattleCharacterProfile;
using GameSave;
using HarmonyLib;
using LOR_DiceSystem;
using LOR_XML;
using Mod;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BattleCardEnhancedView;
using RogueLike_Mod_Reborn;
using CommonModApi;


namespace abcdcode_LOGLIKE_MOD
{

    public class LogLikeMod : ModInitializer
    {
        /// <summary>
        /// A dictionary containing several cached UI objects for ready-initialization.
        /// </summary>
        public static LogLikeMod.CacheDic<int, GameObject> LogUIObjs;
        public static bool purpleexcept;
        public static List<string> CheckExceptionModList;
        public static bool saveloading;
        public static bool see;
        /// <summary>
        /// Pauses the combat scene if set to true.<br></br>
        /// Irrelevant if there is a currently running shop, mystery event or mystery interrupt.
        /// </summary>
        public static bool PauseBool = false;
        public static List<Assembly> LogModAssemblys;
        public static EquipChangeOrder NextPlayerOrder;
        public static int curemotion;
        public static UILogCustomSelectable StageRemainPanel;
        public static TextMeshProUGUI StageRemainText;
        public static Button skipPanel;
        public static TextMeshProUGUI skipPanelText;
        public static List<EquipChangeOrder> PlayerEquipOrders;
        public static Dictionary<string, List<EmotionCardXmlInfo>> PickUpXml_Dummy_Stage;
        public static Dictionary<string, List<EmotionCardXmlInfo>> PickUpXml_Dummy_Passive;
        public static Dictionary<string, List<EmotionEgoXmlInfo>> RewardCardDic_Dummy;
        public static List<List<LorId>> egoSelectionQueue;
        /// <summary>
        /// Number of Acts the player has gone through this chapter.
        /// </summary>
        public static int curChStageStep;
        /// <summary>
        /// Number of total Acts still available to the player this chapter.
        /// </summary>
        public static int curChRemainCount => LogueBookModels.RemainStageList[LogLikeMod.curchaptergrade].Count;
        /// <summary>
        /// The currently running mystery's title- used by Discord Rich Presence.
        /// </summary>
        public static string curMysteryTitle => Singleton<MysteryManager>.Instance.curMystery.FrameObj["Title"].GetComponent<TextMeshProUGUI>().GetParsedText();
        public static int NormalRewardCool = 0;
        /// <summary>
        /// Determines the reward for any incoming CardChoice and CardList events/interrupts.<br></br>
        /// Mainly used for end-of-stage combat page rewards.
        /// </summary>
        public static List<DropBookXmlInfo> rewards;
        /// <summary>
        /// Set to the last killed enemy's book drops. Cleared on stage start.<br></br>Useful for certain effects.
        /// </summary>
        public static List<DropBookXmlInfo> rewards_lastKill;
        /// <summary>
        /// Determines end-of-act passive choices. Mainly used for key page rewards and boss rewards.
        /// </summary>
        public static List<RewardInfo> rewards_passive;
        /// <summary>
        /// Determines mid-battle rewards.<br></br>
        /// Commonly used for passing information to <see cref="PickUpModelBase.OnPickUp(BattleUnitModel)"/>.
        /// </summary>
        public static List<RewardInfo> rewards_InStage;
        /// <summary>
        /// Runs events at the end of the act. <b>These are given out *before* <see cref="LogLikeMod.rewards_passive"/>.</b><br></br>
        /// Inserting an event LorId results in an event being called  Unused in vanilla Roguelike, but still works.
        /// </summary>
        public static List<LorId> rewardsMystery;
        /// <summary>
        /// Determines the next set of stages/encounters for the playe to choose.
        /// </summary>
        public static List<EmotionCardXmlInfo> nextlist;
        /// <summary>
        /// The current chapter the player is in.
        /// </summary>
        public static ChapterGrade curchaptergrade;
        /// <summary>
        /// The current type of stage the player is in.
        /// </summary>
        public static StageType curstagetype;
        /// <summary>
        /// The current *actual* reception the player is in within Roguelike.<br></br>
        /// When checking IDs, use this instead of <see cref="StageController.GetStageModel()"/>.
        /// </summary>
        public static LorId curstageid;
        public static Font DefFont;
        public static TMP_FontAsset _DefFont_TMP;
        public static Color _DefFontColor = new Color(0.9372549f, 0.7607843f, 0.5058824f, 1f);
        public const string ModId = "abcdcodecalmmagma.LogueLikeReborn";
        public static string path;
        public static Dictionary<string, Dictionary<ActionDetail, Dictionary<GameObject, SkeletonAnimation>>> spinemotions;
        public static Dictionary<string, SpineStandingData> spinedatas;
        public static List<ModContentInfo> LogMods;
        public static LogLikeMod.CacheDic<string, UnityEngine.Object> AssetBundles;
        public static LogLikeMod.CacheDic<string, Sprite> ArtWorks;
        public static bool CreatedShopEquipPages = false;

        public static bool Temp = true;
        public static UILogCustomSelectable LogOpenButton;
        public static UILogCustomSelectable LogContinueButton;
        public static Button ChangeEmotinCardBtn;
        public static Button CraftBtn;
        public static Image CraftBtnFrame;
        public static Button CreatureBtn;
        public static Image CreatureBtnFrame;
        public static Button AtlasBtn;
        public static Image AtlasBtnFrame;
        public static Button RealizationBtn;
        public static Image RealizationBtnFrame;
        public static Button InvenBtn;
        public static Image InvenBtnFrame;
        /// <summary>
        /// Determines to whether or not the act is actually supposed to end at the end of the scene.
        /// </summary>
        public static bool EndBattle;
        /// <summary>
        /// Determines to whether or not add a new player at the end of the act.
        /// </summary>
        public static bool AddPlayer;
        /// <summary>
        /// Determines to whether or not revive all players and heal them all at the end of the act.
        /// </summary>
        public static bool RecoverPlayers;
        public static Dictionary<string, System.Type> FindPickUpCache;
        /// <summary>
        /// Contains modded artworks from mod folders [root]/Resource/CustomArtwork.<br></br>
        /// You can obtain a sprite from it like so: 
        /// <code>var sprite = LogLikeMod.ModdedArtworks[(packageId, fileName)];</code>
        /// </summary>
        public static LogLikeMod.CacheDic<(string, string), Sprite> ModdedArtWorks;
        public static bool itemCatalogActive;
        public static LogLikeHooks logLikeHooks;

        public static SaveData CreateChSaveData(ChapterGrade grade)
        {
            SaveData data = new SaveData();
            data.AddData("curstagetype", 7);
            data.AddData("curchaptergrade", (int)grade);
            data.AddData("curChStageStep", 0);
            data.AddData("curstage", new LorId(LogLikeMod.ModId, 855).LogGetSaveData());
            data.AddData("Money", 999);
            return data;
        }

        public static SaveData GetSaveData()
        {
            SaveData saveData = new SaveData();
            saveData.AddData("curstagetype", new SaveData((int)LogLikeMod.curstagetype));
            saveData.AddData("curchaptergrade", new SaveData((int)LogLikeMod.curchaptergrade));
            saveData.AddData("curChStageStep", new SaveData(LogLikeMod.curChStageStep));
            saveData.AddData("curstage", LogLikeMod.curstageid.LogGetSaveData());
            saveData.AddData("Money", new SaveData(PassiveAbility_MoneyCheck.GetMoney()));
            return saveData;
        }

        public static void LoadFromSaveData(SaveData data)
        {
            LogLikeMod.curstagetype = (StageType)data.GetData("curstagetype").GetIntSelf();
            LogLikeMod.curchaptergrade = (ChapterGrade)data.GetData("curchaptergrade").GetIntSelf();
            LogLikeMod.curChStageStep = data.GetInt("curChStageStep");
            LogLikeMod.curstageid = ExtensionUtils.LogLoadFromSaveData(data.GetData("curstage"));
            LogLikeMod.SetNextStage(LogLikeMod.curstageid, LogLikeMod.curstagetype, NextStageSetType.BySave);
            PassiveAbility_MoneyCheck.SetMoney(data.GetInt("Money"));
        }

        /// <summary>
        /// Sets the next act's reception.
        /// </summary>
        public static void SetNextStage(LorId stageid, StageType stagetype = StageType.Custom, NextStageSetType settype = NextStageSetType.Default)
        {
            StageModel stageModel = Singleton<StageController>.Instance.GetStageModel();
            StageClassInfo startStage = Singleton<StageClassInfoList>.Instance.GetData(RMRCore.CurrentGamemode.StageStart);
            if (stageModel == null)
            {
                Debug.LogError($"[RMR SetNextStage] StageModel is null. stage={stageid}, type={stagetype}");
                return;
            }
            if ((settype == NextStageSetType.BySave || stageModel.ClassInfo == null) && startStage != null)
                stageModel.Init(startStage, LibraryModel.Instance);
            StageClassInfo data = Singleton<StageClassInfoList>.Instance.GetData(stageid);
            if (data == null)
            {
                Debug.LogError($"[RMR SetNextStage] Stage not found: StageClassInfoList.GetData returned NULL for stage={stageid}, type={stagetype}");
                return;
            }
            if (data.waveList == null || data.waveList.Count == 0)
            {
                Debug.LogError($"[RMR SetNextStage] Stage has no wave data: {stageid}, type={stagetype}");
                return;
            }
            RestoreVanillaEnemyIdsForImpurityStage(data);
            FieldInfo waveListField = typeof(StageModel).GetField("_waveList", AccessTools.all);
            List<StageWaveModel> stageWaveModelList = waveListField?.GetValue(stageModel) as List<StageWaveModel>;
            if (stageWaveModelList == null)
            {
                stageWaveModelList = new List<StageWaveModel>();
                waveListField?.SetValue(stageModel, stageWaveModelList);
            }
            for (int i = 0; i < data.waveList.Count; i++)
            {
                StageWaveInfo wave = data.waveList[i];
                if (IsImpurityRouteStage(data))
                    Debug.Log($"[RMR SetNextStage] Grade7 impurity wave normalized: stage={stageid}, wave={i + 1}/{data.waveList.Count}, enemies={DescribeWaveEnemyIds(wave)}");
                StageWaveModel stageWaveModel = new StageWaveModel();
                stageWaveModel.Init(stageModel, wave);
                if (settype == NextStageSetType.BySave && i == 0 && stageWaveModelList.Count > 0)
                    stageWaveModelList[0] = stageWaveModel;
                else
                    stageWaveModelList.Add(stageWaveModel);
            }
            List<string> mapInfo = data.mapInfo;
            if ((mapInfo == null || mapInfo.Count == 0) && stageModel.ClassInfo != null && stageModel.ClassInfo.mapInfo != null && stageModel.ClassInfo.mapInfo.Count > 0)
                mapInfo = stageModel.ClassInfo.mapInfo;
            if ((mapInfo == null || mapInfo.Count == 0) && startStage != null && startStage.mapInfo != null && startStage.mapInfo.Count > 0)
                mapInfo = startStage.mapInfo;
            if (mapInfo != null && mapInfo.Count > 0)
            {
                if (stageModel.ClassInfo != null)
                {
                    stageModel.ClassInfo.mapInfo = mapInfo;
                    stageModel.SetCurrentMapInfo(0);
                }
                else
                {
                    Debug.LogWarning($"[RMR SetNextStage] Stage {stageid} resolved mapInfo but current StageModel.ClassInfo is null.");
                }
            }
            else
            {
                Debug.LogWarning($"[RMR SetNextStage] Stage {stageid} has no mapInfo and no fallback mapInfo. The game will keep the current map.");
            }
            if (settype == NextStageSetType.Default || settype == NextStageSetType.Custom)
            {
                LogLikeMod.nextlist.Clear();
                if (settype == NextStageSetType.Default)
                {
                    if (LogLikeMod.curstagetype == StageType.Boss)
                        LogueBookModels.RemoveStageInlist(stageid, LogLikeMod.curchaptergrade + 1);
                    else
                        LogueBookModels.RemoveStageInlist(stageid, LogLikeMod.curchaptergrade);
                }
                if (LogLikeMod.curstagetype == StageType.Boss)
                {
                    RMRCore.CurrentGamemode.OnClearBossWave();
                    GlobalLogueEffectManager.Instance.AfterClearBossWave();
                }
                else
                    ++LogLikeMod.curChStageStep;
            }
            LogLikeMod.curstagetype = stagetype;
            LogLikeMod.curstageid = data.id;
        }

        public static GameObject GetLogUIObj(int index)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<Transform>(SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.transform, SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.transform.parent).gameObject;
            UnityEngine.Object.Destroy(gameObject.GetComponent<LevelUpUI>());
            for (int index1 = 0; index1 < gameObject.transform.childCount; ++index1)
                UnityEngine.Object.Destroy(gameObject.transform.GetChild(index1).gameObject);
            gameObject.SetActive(true);
            gameObject.AddComponent<LogLikeMod.UIActiveChecker>();
            gameObject.transform.localPosition = SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.transform.localPosition;
            gameObject.transform.localScale = SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.transform.localScale;
            (gameObject.transform as RectTransform).sizeDelta = new Vector2(0.0f, 0.0f);
            gameObject.GetComponent<Canvas>().enabled = true;
            gameObject.GetComponent<Canvas>().sortingOrder += index;
            return gameObject;
        }

        /// <summary>
        /// Disables the money counter, ends mystery events, closes shops and disables the dropdown item inventory.
        /// </summary>
        public static void ResetUIs()
        {
            foreach (GameObject gameObject in LogLikeMod.LogUIObjs.dic.Values)
                gameObject.SetActive(false);
            LogLikeMod.BattleMoneyUI.DeActive();
            Singleton<MysteryManager>.Instance.EndMystery();
            Singleton<ShopManager>.Instance.RemoveShop();
            Singleton<GlobalLogueEffectManager>.Instance.ClearList();
        }

        /// <summary>
        /// Shorthand for getting private fields.
        /// </summary>
        /// <typeparam name="T">The type of the field you wish you get.</typeparam>
        /// <param name="obj">The object that has the field.</param>
        /// <param name="name">The name of the field.</param>
        public static T GetFieldValue<T>(object obj, string name)
        {
            return (T)obj.GetType().GetField(name, AccessTools.all | BindingFlags.FlattenHierarchy).GetValue(obj);
        }

        /// <summary>
        /// Shorthand for setting private fields.
        /// </summary>
        /// <param name="obj">The object that has the field.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value to override the field with.</param>
        public static void SetFieldValue(object obj, string name, object value)
        {
            obj.GetType().GetField(name, AccessTools.all | BindingFlags.FlattenHierarchy).SetValue(obj, value);
        }

        public static TMP_FontAsset DefFont_TMP
        {
            get
            {
                if (LogLikeMod._DefFont_TMP == null)
                    LogLikeMod._DefFont_TMP = ResolveLocalizedTmpFont();
                return LogLikeMod._DefFont_TMP;
            }
            set
            {
                if (value == null)
                    return;
                string language = NormalizeTextLanguage(TextDataModel.CurrentLanguage);
                if (!IsTmpFontCompatibleWithLanguage(value, language))
                {
                    Debug.LogWarning($"[RMR Localize] Rejected TMP font '{value.name}' for language '{language}' because it cannot render the required glyphs.");
                    return;
                }
                LogLikeMod._DefFont_TMP = value;
            }
        }

        public static Color DefFontColor
        {
            get => LogLikeMod._DefFontColor;
            set => LogLikeMod._DefFontColor = value;
        }

        /// <summary>
        /// Checks for mod incompatibilities.
        /// </summary>
        public static bool CheckExceptionModExist(out List<string> ExceptModNames)
        {
            ExceptModNames = new List<string>();
            foreach (ModContentInfo allMod in Singleton<ModContentManager>.Instance.GetAllMods())
            {
                if (allMod.activated && LogLikeMod.CheckExceptionModList.Contains(allMod.invInfo.workshopInfo.uniqueId))
                    ExceptModNames.Add(allMod.invInfo.workshopInfo.title + $" ({allMod.invInfo.workshopInfo.uniqueId})");
            }
            return ExceptModNames.Count > 0;
        }

        /// <summary>
        /// Returns a list of mod metadata for all satellite mods.
        /// </summary>
        public static List<ModContentInfo> GetLogMods()
        {
            if (LogLikeMod.LogMods != null)
                return LogLikeMod.LogMods;
            LogLikeMod.LogMods = new List<ModContentInfo>();
            foreach (ModContentInfo allMod in Singleton<ModContentManager>.Instance.GetAllMods())
            {
                if (allMod.activated && Directory.Exists(allMod.GetLogDllPath()))
                    LogLikeMod.LogMods.Add(allMod);
            }
            return LogLikeMod.LogMods;
        }

        public static Sprite GetArtWorks(DirectoryInfo dir, string name)
        {
            if (dir.GetDirectories().Length != 0)
            {
                foreach (DirectoryInfo directory in dir.GetDirectories())
                {
                    Sprite artWorks = LogLikeMod.GetArtWorks(directory, name);
                    if (artWorks != null)
                        return artWorks;
                }
            }
            foreach (System.IO.FileInfo file in dir.GetFiles())
            {
                if (Path.GetFileNameWithoutExtension(file.FullName) == name)
                {
                    Texture2D texture2D = new Texture2D(2, 2);
                    byte[] data = File.ReadAllBytes(file.FullName);
                    texture2D.LoadImage(data);
                    return Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.0f, 0.0f), 100f, 0U, SpriteMeshType.FullRect);
                }
            }
            return (Sprite)null;
        }

        public static Sprite GetArtWorks(string name)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(LogLikeMod.path + "/ArtWork");
            if (directoryInfo.GetDirectories().Length != 0)
            {
                foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
                {
                    Sprite artWorks = LogLikeMod.GetArtWorks(directory, name);
                    if (artWorks != null)
                        return artWorks;
                }
            }
            foreach (System.IO.FileInfo file in directoryInfo.GetFiles())
            {
                if (Path.GetFileNameWithoutExtension(file.FullName) == name)
                {
                    Texture2D texture2D = new Texture2D(2, 2);
                    texture2D.LoadImage(File.ReadAllBytes(file.FullName));
                    return Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.0f, 0.0f), 100f, 0U, SpriteMeshType.FullRect);
                }
            }
            return (Sprite)null;
        }

        public static SkeletonDataAsset GetAsset(
          string atlaspath,
          string jsonpath,
          Material[] materials,
          float scale = 0.01f)
        {
            TextAsset atlasText = new TextAsset(File.ReadAllText(atlaspath));
            return SkeletonDataAsset.CreateRuntimeInstance(new TextAsset(File.ReadAllText(jsonpath)), new AtlasAsset[1]
            {
      AtlasAsset.CreateRuntimeInstance(atlasText, materials, true)
            }, true, scale);
        }

        public static Material CreateMaterialForSkel(string imagepath, string name)
        {
            Shader shader = Shader.Find("UI/Default");
            Texture2D tex = new Texture2D(2, 2);
            byte[] data = File.ReadAllBytes(imagepath);
            tex.LoadImage(data);
            tex.name = name;
            return new Material(shader)
            {
                mainTexture = (Texture)tex
            };
        }

        public static string GetPickUpXmlWorkShopId_Stage(EmotionCardXmlInfo info)
        {
            if (info == null || LogLikeMod.PickUpXml_Dummy_Stage == null)
                return null;
            foreach (var entry in LogLikeMod.PickUpXml_Dummy_Stage)
            {
                if (entry.Value != null && entry.Value.Contains(info))
                    return entry.Key;
            }
            return null;
        }

        public static string GetPickUpXmlWorkShopId_Passive(EmotionCardXmlInfo info)
        {
            return LogLikeMod.PickUpXml_Dummy_Passive == null ? null : LogLikeMod.PickUpXml_Dummy_Passive.ToList().Find(x => x.Value.Find(y => y == info) != null).Key;
        }

        public static EmotionCardXmlInfo GetRegisteredPickUpXml(LogueStageInfo info)
        {
            if (info == null || LogLikeMod.PickUpXml_Dummy_Stage == null)
                return null;
            if (!LogLikeMod.PickUpXml_Dummy_Stage.TryGetValue(info.workshopid, out List<EmotionCardXmlInfo> list) || list == null)
                return null;
            return list.Find(x => x.id == info.stageid);
        }

        public static EmotionCardXmlInfo GetRegisteredPickUpXml(RewardPassiveInfo info)
        {
            if (info == null || LogLikeMod.PickUpXml_Dummy_Passive == null)
                return null;
            string packageId = info.id != null ? info.id.packageId : info.workshopID;
            if (!string.IsNullOrEmpty(packageId)
                && LogLikeMod.PickUpXml_Dummy_Passive.TryGetValue(packageId, out List<EmotionCardXmlInfo> packageList)
                && packageList != null)
                return packageList.Find(x => x.id == info.passiveid);
            if (!string.IsNullOrEmpty(info.workshopID)
                && LogLikeMod.PickUpXml_Dummy_Passive.TryGetValue(info.workshopID, out List<EmotionCardXmlInfo> workshopList)
                && workshopList != null)
                return workshopList.Find(x => x.id == info.passiveid);
            return null;
        }

        public static void RegisterPickUpXml(LogueStageInfo info)
        {
            if (LogLikeMod.PickUpXml_Dummy_Stage == null)
                LogLikeMod.PickUpXml_Dummy_Stage = new Dictionary<string, List<EmotionCardXmlInfo>>();
            string workshopid = info.workshopid;
            if (!LogLikeMod.PickUpXml_Dummy_Stage.ContainsKey(workshopid))
                LogLikeMod.PickUpXml_Dummy_Stage.Add(workshopid, new List<EmotionCardXmlInfo>());
            LogLikeMod.PickUpXml_Dummy_Stage[workshopid].Add(new EmotionCardXmlInfo()
            {
                Sephirah = SephirahType.None,
                id = info.stageid,
                Level = 1,
                TargetType = EmotionTargetType.All,
                Name = $"{info.workshopid}_{info.stageid.ToString()}",
                Locked = false,
                State = MentalState.Positive,
                Script = new List<string>(),
                EmotionLevel = 2,
                EmotionRate = 0
            });
        }

        public static void RegisterPickUpXml(RewardPassiveInfo info)
        {
            if (LogLikeMod.PickUpXml_Dummy_Passive == null)
                LogLikeMod.PickUpXml_Dummy_Passive = new Dictionary<string, List<EmotionCardXmlInfo>>();
            string packageId = info.id.packageId;
            if (!LogLikeMod.PickUpXml_Dummy_Passive.ContainsKey(packageId))
                LogLikeMod.PickUpXml_Dummy_Passive.Add(packageId, new List<EmotionCardXmlInfo>());
            EmotionCardXmlInfo emotionCardXmlInfo = new EmotionCardXmlInfo();
            emotionCardXmlInfo._artwork = info.artwork;
            emotionCardXmlInfo.Sephirah = SephirahType.None;
            emotionCardXmlInfo.id = info.id.id;
            emotionCardXmlInfo.Level = info.level;
            emotionCardXmlInfo.TargetType = info.targettype;
            emotionCardXmlInfo.Name = info.rewardtype != RewardType.EquipPage ? info.script : $"{info.workshopID}_{info.passiveid.ToString()}";
            emotionCardXmlInfo.Locked = false;
            emotionCardXmlInfo.State = MentalState.Positive;
            if (Singleton<RewardPassivesList>.Instance.infos.Find((Predicate<RewardPassivesInfo>)(x => x.RewardPassiveList.Contains(info))).rewardtype == PassiveRewardListType.Creature)
                emotionCardXmlInfo.State = MentalState.Negative;
            emotionCardXmlInfo.Script = new List<string>()
    {
      info.script
    };
            if (info.rewardtype == RewardType.EquipPage)
                emotionCardXmlInfo.Script[0] = "EquipDefault";
            emotionCardXmlInfo.EmotionLevel = info.level;
            emotionCardXmlInfo.EmotionRate = 0;
            RMRAbnormalityUnlockManager.ApplyVanillaEmotionPresentation(info, emotionCardXmlInfo);
            LogLikeMod.PickUpXml_Dummy_Passive[packageId].Add(emotionCardXmlInfo);
            if (info.rewardtype == RewardType.EquipPage)
                RewardingModel.CreateEquipRewardXmlData(info);
        }

        public static void LoadSpineAssets()
        {
            LogLikeMod.spinedatas = new Dictionary<string, SpineStandingData>();
            Material[] materials1 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/MerchantLog/Merchant.png", "Merchant")
            };
            SpineStandingData spineStandingData1 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/MerchantLog/atlas.txt", LogLikeMod.path + "/Spine/MerchantLog/json.txt", materials1, 0.03f));
            spineStandingData1.SetDic(ActionDetail.Default, "idle");
            spineStandingData1.SetDic(ActionDetail.Move, "idle");
            spineStandingData1.SetDic(ActionDetail.Standing, "idle");
            LogLikeMod.spinedatas.Add("MerchantLog", spineStandingData1);
            Material[] materials2 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/MachineDawnLog/Machine_Dawn.png", "Machine_Dawn")
            };
            SpineStandingData spineStandingData2 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/MachineDawnLog/atlas.txt", LogLikeMod.path + "/Spine/MachineDawnLog/json.txt", materials2));
            spineStandingData2.SetDic(ActionDetail.Default, "Standing");
            spineStandingData2.SetDic(ActionDetail.Move, "Walk");
            spineStandingData2.SetDic(ActionDetail.Standing, "Standing");
            spineStandingData2.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData2.SetDic(ActionDetail.Guard, "Standing");
            spineStandingData2.SetDic(ActionDetail.Slash, "Attack_02", 3.5f, false);
            spineStandingData2.SetDic(ActionDetail.Penetrate, "Attack_01", 3.5f, false);
            spineStandingData2.SetDic(ActionDetail.Hit, "Attack_02", 3.5f, false);
            spineStandingData2.SetDic(ActionDetail.Fire, "Standing");
            spineStandingData2.SetDic(ActionDetail.Aim, "Standing");
            spineStandingData2.SetDic(ActionDetail.NONE, "Standing");
            spineStandingData2.SetDic(ActionDetail.S1, "DeadScene", IsLoop: false);
            spineStandingData2.SetScale(new Vector3(-1f, 1f));
            spineStandingData2.SetScale(ActionDetail.Standing, new Vector3(1f, 1f));
            LogLikeMod.spinedatas.Add("MachineDawnLog", spineStandingData2);
            Material[] materials3 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/MachineNoonLog/machine_Noon.png", "machine_Noon")
            };
            SpineStandingData spineStandingData3 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/MachineNoonLog/atlas.txt", LogLikeMod.path + "/Spine/MachineNoonLog/json.txt", materials3, 0.015f));
            spineStandingData3.SetDic(ActionDetail.Default, "Default");
            spineStandingData3.SetDic(ActionDetail.Move, "Default");
            spineStandingData3.SetDic(ActionDetail.Standing, "Default");
            spineStandingData3.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData3.SetDic(ActionDetail.Guard, "Default");
            spineStandingData3.SetDic(ActionDetail.Slash, "Attack_01", 2f, false);
            spineStandingData3.SetDic(ActionDetail.Penetrate, "Attack_01", 2f, false);
            spineStandingData3.SetDic(ActionDetail.Hit, "Attack_01", 2f, false);
            spineStandingData3.SetDic(ActionDetail.Fire, "Walk");
            spineStandingData3.SetDic(ActionDetail.Aim, "Walk");
            spineStandingData3.SetDic(ActionDetail.NONE, "Default");
            spineStandingData3.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("MachineNoonLog", spineStandingData3);
            Material[] materials4 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/MachineDuskLog/machineDusk.png", "machineDusk")
            };
            SpineStandingData spineStandingData4 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/MachineDuskLog/atlas.txt", LogLikeMod.path + "/Spine/MachineDuskLog/json.txt", materials4, 0.015f));
            spineStandingData4.SetDic(ActionDetail.Default, "Default");
            spineStandingData4.SetDic(ActionDetail.Move, "Default");
            spineStandingData4.SetDic(ActionDetail.Standing, "Default");
            spineStandingData4.SetDic(ActionDetail.Damaged, "Dead");
            spineStandingData4.SetDic(ActionDetail.Guard, "Default");
            spineStandingData4.SetDic(ActionDetail.Slash, "Default");
            spineStandingData4.SetDic(ActionDetail.Penetrate, "Default");
            spineStandingData4.SetDic(ActionDetail.Hit, "Default");
            spineStandingData4.SetDic(ActionDetail.Fire, "Default");
            spineStandingData4.SetDic(ActionDetail.Aim, "Default");
            spineStandingData4.SetDic(ActionDetail.NONE, "Default");
            spineStandingData4.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("MachineDuskLog", spineStandingData4);
            Material[] materials5 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/MachineMidnightLog/machine_Midnight.png", "machine_Midnight")
            };
            SpineStandingData spineStandingData5 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/MachineMidnightLog/atlas.txt", LogLikeMod.path + "/Spine/MachineMidnightLog/json.txt", materials5, 0.015f));
            spineStandingData5.SetDic(ActionDetail.Default, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Move, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Standing, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Damaged, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Guard, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Slash, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Penetrate, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Hit, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Fire, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Aim, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.NONE, "Exterminate");
            spineStandingData5.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("MachineMidnightLog", spineStandingData5);
            Material[] materials6 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/OutterGodDawnLog/cosmic_dawn.png", "cosmic_dawn")
            };
            SpineStandingData spineStandingData6 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/OutterGodDawnLog/atlas.txt", LogLikeMod.path + "/Spine/OutterGodDawnLog/json.txt", materials6, 0.015f));
            spineStandingData6.SetDic(ActionDetail.Default, "Walk");
            spineStandingData6.SetDic(ActionDetail.Move, "Walk");
            spineStandingData6.SetDic(ActionDetail.Standing, "Walk");
            spineStandingData6.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData6.SetDic(ActionDetail.Guard, "Walk");
            spineStandingData6.SetDic(ActionDetail.Slash, "Walk");
            spineStandingData6.SetDic(ActionDetail.Penetrate, "Walk");
            spineStandingData6.SetDic(ActionDetail.Hit, "Walk");
            spineStandingData6.SetDic(ActionDetail.Fire, "Walk");
            spineStandingData6.SetDic(ActionDetail.Aim, "Walk");
            spineStandingData6.SetDic(ActionDetail.NONE, "Walk");
            spineStandingData6.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("OutterGodDawnLog", spineStandingData6);
            Material[] materials7 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/OutterGodNoonLog/stone.png", "stone")
            };
            SpineStandingData spineStandingData7 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/OutterGodNoonLog/atlas.txt", LogLikeMod.path + "/Spine/OutterGodNoonLog/json.txt", materials7, 0.015f));
            spineStandingData7.SetDic(ActionDetail.Default, "Default");
            spineStandingData7.SetDic(ActionDetail.Move, "Default");
            spineStandingData7.SetDic(ActionDetail.Standing, "Default");
            spineStandingData7.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData7.SetDic(ActionDetail.Guard, "Default");
            spineStandingData7.SetDic(ActionDetail.Slash, "Casting");
            spineStandingData7.SetDic(ActionDetail.Penetrate, "Casting");
            spineStandingData7.SetDic(ActionDetail.Hit, "Casting");
            spineStandingData7.SetDic(ActionDetail.Fire, "Default");
            spineStandingData7.SetDic(ActionDetail.Aim, "Default");
            spineStandingData7.SetDic(ActionDetail.NONE, "Default");
            spineStandingData7.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("OutterGodNoonLog", spineStandingData7);
            Material[] materials8 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/BugDawnLog/Bug1.png", "Bug1")
            };
            SpineStandingData spineStandingData8 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/BugDawnLog/atlas.txt", LogLikeMod.path + "/Spine/BugDawnLog/json.txt", materials8, 0.015f));
            spineStandingData8.SetDic(ActionDetail.Default, "Move");
            spineStandingData8.SetDic(ActionDetail.Move, "Move");
            spineStandingData8.SetDic(ActionDetail.Standing, "Move");
            spineStandingData8.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData8.SetDic(ActionDetail.Guard, "Move");
            spineStandingData8.SetDic(ActionDetail.Slash, "Attack", IsLoop: false);
            spineStandingData8.SetDic(ActionDetail.Penetrate, "Attack", IsLoop: false);
            spineStandingData8.SetDic(ActionDetail.Hit, "Attack", IsLoop: false);
            spineStandingData8.SetDic(ActionDetail.Fire, "Move");
            spineStandingData8.SetDic(ActionDetail.Aim, "Move");
            spineStandingData8.SetDic(ActionDetail.NONE, "Move");
            spineStandingData8.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("BugDawnLog", spineStandingData8);
            Material[] materials9 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/BugDuskLog/BugDusk.png", "BugDusk")
            };
            SpineStandingData spineStandingData9 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/BugDuskLog/atlas.txt", LogLikeMod.path + "/Spine/BugDuskLog/json.txt", materials9, 0.015f));
            spineStandingData9.SetDic(ActionDetail.Default, "Move");
            spineStandingData9.SetDic(ActionDetail.Move, "Move");
            spineStandingData9.SetDic(ActionDetail.Standing, "Move");
            spineStandingData9.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData9.SetDic(ActionDetail.Guard, "Move");
            spineStandingData9.SetDic(ActionDetail.Slash, "Eat", IsLoop: false);
            spineStandingData9.SetDic(ActionDetail.Penetrate, "Eat", IsLoop: false);
            spineStandingData9.SetDic(ActionDetail.Hit, "Eat", IsLoop: false);
            spineStandingData9.SetDic(ActionDetail.Fire, "Move");
            spineStandingData9.SetDic(ActionDetail.Aim, "Move");
            spineStandingData9.SetDic(ActionDetail.NONE, "Move");
            spineStandingData9.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("BugDuskLog", spineStandingData9);
            Material[] materials10 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/BugMidnightLog/ThirdType.png", "ThirdType")
            };
            SpineStandingData spineStandingData10 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/BugMidnightLog/atlas.txt", LogLikeMod.path + "/Spine/BugMidnightLog/json.txt", materials10, 0.015f));
            spineStandingData10.SetDic(ActionDetail.Default, "Default");
            spineStandingData10.SetDic(ActionDetail.Move, "Default");
            spineStandingData10.SetDic(ActionDetail.Standing, "Default");
            spineStandingData10.SetDic(ActionDetail.Damaged, "Default", IsLoop: false);
            spineStandingData10.SetDic(ActionDetail.Guard, "Default");
            spineStandingData10.SetDic(ActionDetail.Slash, "Default", IsLoop: false);
            spineStandingData10.SetDic(ActionDetail.Penetrate, "Default", IsLoop: false);
            spineStandingData10.SetDic(ActionDetail.Hit, "Default", IsLoop: false);
            spineStandingData10.SetDic(ActionDetail.Fire, "Default");
            spineStandingData10.SetDic(ActionDetail.Aim, "Default");
            spineStandingData10.SetDic(ActionDetail.NONE, "Default");
            spineStandingData10.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("BugMidnightLog", spineStandingData10);
            Material[] materials11 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/CircusDawnLog/tjzjtm.png", "tjzjtm")
            };
            SpineStandingData spineStandingData11 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/CircusDawnLog/atlas.txt", LogLikeMod.path + "/Spine/CircusDawnLog/json.txt", materials11));
            spineStandingData11.SetDic(ActionDetail.Default, "Default");
            spineStandingData11.SetDic(ActionDetail.Move, "Default");
            spineStandingData11.SetDic(ActionDetail.Standing, "Default");
            spineStandingData11.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData11.SetDic(ActionDetail.Guard, "Default");
            spineStandingData11.SetDic(ActionDetail.Slash, "Trick", IsLoop: false);
            spineStandingData11.SetDic(ActionDetail.Penetrate, "Trick", IsLoop: false);
            spineStandingData11.SetDic(ActionDetail.Hit, "Trick", IsLoop: false);
            spineStandingData11.SetDic(ActionDetail.Fire, "Default");
            spineStandingData11.SetDic(ActionDetail.Aim, "Default");
            spineStandingData11.SetDic(ActionDetail.NONE, "Default");
            spineStandingData11.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("CircusDawnLog", spineStandingData11);
            Material[] materials12 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/CircusNoonLog/CircusNoon.png", "CircusNoon")
            };
            SpineStandingData spineStandingData12 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/CircusNoonLog/atlas.txt", LogLikeMod.path + "/Spine/CircusNoonLog/json.txt", materials12));
            spineStandingData12.SetDic(ActionDetail.Default, "default");
            spineStandingData12.SetDic(ActionDetail.Move, "walk");
            spineStandingData12.SetDic(ActionDetail.Standing, "default");
            spineStandingData12.SetDic(ActionDetail.Damaged, "dead", IsLoop: false);
            spineStandingData12.SetDic(ActionDetail.Guard, "default");
            spineStandingData12.SetDic(ActionDetail.Slash, "attack1", IsLoop: false);
            spineStandingData12.SetDic(ActionDetail.Penetrate, "attack2", IsLoop: false);
            spineStandingData12.SetDic(ActionDetail.Hit, "attack3", IsLoop: false);
            spineStandingData12.SetDic(ActionDetail.Fire, "default");
            spineStandingData12.SetDic(ActionDetail.Aim, "default");
            spineStandingData12.SetDic(ActionDetail.NONE, "default");
            spineStandingData12.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("CircusNoonLog", spineStandingData12);
            Material[] materials13 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/CircusDuskLog/circus_night.png", "circus_night")
            };
            SpineStandingData spineStandingData13 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/CircusDuskLog/atlas.txt", LogLikeMod.path + "/Spine/CircusDuskLog/json.txt", materials13, 0.015f));
            spineStandingData13.SetDic(ActionDetail.Default, "Default");
            spineStandingData13.SetDic(ActionDetail.Move, "Attack_03", 2f);
            spineStandingData13.SetDic(ActionDetail.Standing, "Default");
            spineStandingData13.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData13.SetDic(ActionDetail.Guard, "Default");
            spineStandingData13.SetDic(ActionDetail.Slash, "Attack_01", 3f, false);
            spineStandingData13.SetDic(ActionDetail.Penetrate, "Attack_02", 3f, false);
            spineStandingData13.SetDic(ActionDetail.Hit, "Attack_02", 3f, false);
            spineStandingData13.SetDic(ActionDetail.Fire, "Default");
            spineStandingData13.SetDic(ActionDetail.Aim, "Default");
            spineStandingData13.SetDic(ActionDetail.NONE, "Default");
            spineStandingData13.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("CircusDuskLog", spineStandingData13);
        }

        public static void LoadDiceAbilityDesc(string path, string modid)
        {
            Dictionary<string, BattleCardAbilityDesc> fieldValue = LogLikeMod.GetFieldValue<Dictionary<string, BattleCardAbilityDesc>>(Singleton<BattleCardAbilityDescXmlList>.Instance, "_dictionary");
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                
                BattleCardAbilityDescRoot cardAbilityDescRoot = (BattleCardAbilityDescRoot)new XmlSerializer(typeof(BattleCardAbilityDescRoot)).Deserialize((TextReader)stringReader);
                for (int index = 0; index < cardAbilityDescRoot.cardDescList.Count; ++index)
                {
                    BattleCardAbilityDesc cardDesc = cardAbilityDescRoot.cardDescList[index];
                    fieldValue[cardDesc.id] = cardDesc;
                }
            }
        }

        public static void LoadDropBookName(string path, string modid)
        {
            string xml = File.ReadAllText(path);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (XmlNode selectNode in xmlDocument.SelectNodes("localize/text"))
            {
                string str = string.Empty;
                if (selectNode.Attributes.GetNamedItem("id") != null)
                    str = selectNode.Attributes.GetNamedItem("id").InnerText;
                string key = str;
                string innerText = selectNode.InnerText;
                try
                {
                    dictionary.Add(key, innerText);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
                KeyValuePair<string, string> keyvalue = keyValuePair;
                List<DropBookXmlInfo> all = Singleton<DropBookXmlList>.Instance.GetList().FindAll((Predicate<DropBookXmlInfo>)(x => x._targetText == keyvalue.Key));
                if (all.Count > 0)
                {
                    foreach (DropBookXmlInfo dropBookXmlInfo in all)
                        dropBookXmlInfo.workshopName = keyvalue.Value;
                }
            }
        }

        public static void LoadEnemyUnitName(string path, string modid)
        {
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                
                CharactersNameRoot charactersNameRoot = (CharactersNameRoot)new XmlSerializer(typeof(CharactersNameRoot)).Deserialize((TextReader)stringReader);
                List<EnemyUnitClassInfo> all1 = LogLikeMod.GetFieldValue<List<EnemyUnitClassInfo>>(Singleton<EnemyUnitClassInfoList>.Instance, "_list").FindAll((Predicate<EnemyUnitClassInfo>)(x => x.workshopID == modid));
                foreach (CharacterName name in charactersNameRoot.nameList)
                {
                    CharacterName desc = name;
                    List<EnemyUnitClassInfo> all2 = all1.FindAll((Predicate<EnemyUnitClassInfo>)(x => x.nameId == desc.ID));
                    if (all2.Count > 0)
                    {
                        foreach (EnemyUnitClassInfo enemyUnitClassInfo in all2)
                            enemyUnitClassInfo.name = desc.name;
                    }
                }
            }
        }

        public static void LoadBookDesc(string path, string modid)
        {
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                
                BookDescRoot bookDescRoot = (BookDescRoot)new XmlSerializer(typeof(BookDescRoot)).Deserialize((TextReader)stringReader);
                List<BookXmlInfo> list = Singleton<BookXmlList>.Instance.GetList();
                foreach (BookDesc bookDesc in bookDescRoot.bookDescList)
                {
                    BookDesc desc = bookDesc;
                    List<BookXmlInfo> all = list.FindAll((Predicate<BookXmlInfo>)(x => x.TextId == desc.bookID));
                    if (all.Count > 0)
                    {
                        foreach (BookXmlInfo bookXmlInfo in all)
                            bookXmlInfo.InnerName = desc.bookName;
                    }
                }
                Singleton<BookDescXmlList>.Instance.AddBookTextByMod(modid, bookDescRoot.bookDescList);
            }
        }

        public static void LoadCardDesc(string path, string modid)
        {
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                
                Dictionary<LorId, BattleCardDesc> dictionary = (Dictionary<LorId, BattleCardDesc>)typeof(BattleCardDescXmlList).GetField("_dictionary", AccessTools.all).GetValue(Singleton<BattleCardDescXmlList>.Instance);
                foreach (BattleCardDesc cardDesc in ((BattleCardDescRoot)new XmlSerializer(typeof(BattleCardDescRoot)).Deserialize((TextReader)stringReader)).cardDescList)
                {
                    LorId lorId = new LorId(modid, cardDesc.cardID);
                    dictionary[lorId] = cardDesc;
                    DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(lorId, true);
                    if (cardItem != null)
                        cardItem.workshopName = cardDesc.cardName;
                }
            }
        }

        public static void LoadPassiveDesc(string path, string modid)
        {
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                
                Dictionary<LorId, PassiveDesc> dictionary = (Dictionary<LorId, PassiveDesc>)typeof(PassiveDescXmlList).GetField("_dictionary", AccessTools.all).GetValue(Singleton<PassiveDescXmlList>.Instance);
                foreach (PassiveDesc desc1 in ((PassiveDescRoot)new XmlSerializer(typeof(PassiveDescRoot)).Deserialize((TextReader)stringReader)).descList)
                {
                    PassiveDesc desc = desc1;
                    desc.workshopID = modid;
                    dictionary[desc.ID] = desc;
                    BookXmlInfo bookXmlInfo = Singleton<BookXmlList>.Instance.GetAllWorkshopData()[modid].Find((Predicate<BookXmlInfo>)(x => x.id == desc.ID));
                    if (bookXmlInfo != null)
                        bookXmlInfo.InnerName = desc.name;
                }
            }
        }

        public static void LoadLocalizeFile(string path, ref Dictionary<string, string> dic)
        {
            string xml = File.ReadAllText(path);
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(xml);
            }
            catch
            {
                Debug.LogError(("path : " + path));
            }
            foreach (XmlNode selectNode in xmlDocument.SelectNodes("localize/text"))
            {
                string str = string.Empty;
                if (selectNode.Attributes.GetNamedItem("id") != null)
                    str = selectNode.Attributes.GetNamedItem("id").InnerText;
                string key = str;
                string innerText = selectNode.InnerText;
                try
                {
                    dic[key] = innerText;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
        }

        private static FileInfo[] GetLocalizeFilesIfDirectoryExists(string directoryPath, string label)
        {
            if (!Directory.Exists(directoryPath))
            {
                Debug.LogWarning($"[RMR Localize] Missing {label} localize directory, skipped: {directoryPath}");
                return Array.Empty<FileInfo>();
            }
            return new DirectoryInfo(directoryPath).GetFiles();
        }

        private static string ResolveInitialTextLanguage()
        {
            string optionLanguage = TryReadLanguageFromOptionFile();
            if (!string.IsNullOrEmpty(optionLanguage))
                return optionLanguage;

            string current = CanonicalizeTextLanguage(TextDataModel.CurrentLanguage);
            if (!string.IsNullOrEmpty(current) && TextDataModel.GetSupportedLangs().Contains(current) && current != "kr")
                return current;

            switch (Application.systemLanguage)
            {
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.ChineseTraditional:
                    return "cn";
                case SystemLanguage.Korean:
                    return "kr";
                case SystemLanguage.Japanese:
                    return "jp";
                default:
                    return "en";
            }
        }

        private static string NormalizeTextLanguage(string language)
        {
            string optionLanguage = TryReadLanguageFromOptionFile();
            if (!string.IsNullOrEmpty(optionLanguage))
                return optionLanguage;

            language = CanonicalizeTextLanguage(language);
            if (string.IsNullOrEmpty(language) || !TextDataModel.GetSupportedLangs().Contains(language))
                return ResolveInitialTextLanguage();

            return language;
        }

        private static string CanonicalizeTextLanguage(string language)
        {
            string lang = (language ?? string.Empty).Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(lang))
                return string.Empty;
            if (lang.Contains("trcn") || lang.Contains("zh-tw") || lang.Contains("zh_hant") || lang.Contains("traditional"))
                return "trcn";
            if (lang.Contains("cn") || lang.Contains("zh-cn") || lang.Contains("zh_hans") || lang.Contains("chinese"))
                return "cn";
            if (lang == "ja" || lang.Contains("jp") || lang.Contains("japanese"))
                return "jp";
            if (lang == "ko" || lang.Contains("kr") || lang.Contains("korean"))
                return "kr";
            if (lang.Contains("en") || lang.Contains("english"))
                return "en";
            return lang;
        }

        private static string ResolveModLocalizeLanguage(string language)
        {
            string requested = NormalizeTextLanguage(language);
            string localizeRoot = Path.Combine(LogLikeMod.path, "Localize");
            string requestedPath = Path.Combine(localizeRoot, requested);
            if (Directory.Exists(requestedPath))
                return requested;

            if (requested == "trcn" && Directory.Exists(Path.Combine(localizeRoot, "cn")))
                return "cn";

            if (Directory.Exists(Path.Combine(localizeRoot, "en")))
                return "en";
            if (Directory.Exists(Path.Combine(localizeRoot, "kr")))
                return "kr";
            return requested;
        }

        private static TMP_FontAsset ResolveLocalizedTmpFont()
        {
            string language = ResolveInitialTextLanguage();
            try
            {
                LocalizedFontSetter setter = SingletonBehavior<LocalizedFontSetter>.Instance;
                if (setter != null)
                {
                    foreach (FieldInfo field in setter.GetType().GetFields(AccessTools.all))
                    {
                        if (!typeof(TMP_FontAsset).IsAssignableFrom(field.FieldType))
                            continue;
                        TMP_FontAsset candidate = field.GetValue(setter) as TMP_FontAsset;
                        if (IsTmpFontCompatibleWithLanguage(candidate, language))
                        {
                            Debug.Log($"[RMR Localize] Selected TMP font '{candidate.name}' from LocalizedFontSetter for language '{language}'.");
                            return candidate;
                        }
                    }
                }

                TMP_FontAsset defaultFont = TMP_Settings.defaultFontAsset;
                if (IsTmpFontCompatibleWithLanguage(defaultFont, language))
                {
                    Debug.Log($"[RMR Localize] Selected TMP default font '{defaultFont.name}' for language '{language}'.");
                    return defaultFont;
                }

                foreach (TMP_FontAsset candidate in Resources.FindObjectsOfTypeAll<TMP_FontAsset>())
                {
                    if (IsTmpFontCompatibleWithLanguage(candidate, language))
                    {
                        Debug.Log($"[RMR Localize] Selected loaded TMP font '{candidate.name}' for language '{language}'.");
                        return candidate;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("[RMR Localize] Failed to resolve localized TMP font: " + e);
            }
            Debug.LogWarning($"[RMR Localize] Could not find a TMP font that reports support for language '{language}'.");
            return null;
        }

        private static bool IsTmpFontCompatibleWithLanguage(TMP_FontAsset font, string language)
        {
            if (font == null)
                return false;
            string probes = GetFontProbeCharacters(language);
            if (string.IsNullOrEmpty(probes))
                return true;
            HashSet<TMP_FontAsset> visited = new HashSet<TMP_FontAsset>();
            foreach (char probe in probes)
            {
                visited.Clear();
                if (!FontHasCharacterRecursive(font, probe, visited))
                    return false;
            }
            return true;
        }

        private static bool FontHasCharacterRecursive(TMP_FontAsset font, char probe, HashSet<TMP_FontAsset> visited)
        {
            if (font == null || visited.Contains(font))
                return false;
            visited.Add(font);
            try
            {
                if (font.HasCharacter(probe))
                    return true;
                if (font.fallbackFontAssetTable != null)
                {
                    foreach (TMP_FontAsset fallback in font.fallbackFontAssetTable)
                    {
                        if (FontHasCharacterRecursive(fallback, probe, visited))
                            return true;
                    }
                }
            }
            catch
            {
                return true;
            }
            return false;
        }

        private static string GetFontProbeCharacters(string language)
        {
            string lang = CanonicalizeTextLanguage(language);
            if (lang == "cn")
                return "\u56fe\u6c49\u8bed\u6d4b\u8bd5";
            if (lang == "trcn")
                return "\u5716\u6f22\u8a9e\u6e2c\u8a66";
            if (lang == "kr")
                return "\ud55c\uae00\ub3c4";
            if (lang == "jp")
                return "\u65e5\u3042\u30a2\u6f22";
            return string.Empty;
        }

        private static string TryReadLanguageFromOptionFile()
        {
            try
            {
                string optionPath = Path.Combine(Application.persistentDataPath, "option.dat");
                if (!File.Exists(optionPath))
                    return string.Empty;

                string raw = Encoding.UTF8.GetString(File.ReadAllBytes(optionPath));
                int languageIndex = raw.IndexOf("language", StringComparison.OrdinalIgnoreCase);
                if (languageIndex < 0)
                    return string.Empty;

                string tail = raw.Substring(languageIndex, Math.Min(64, raw.Length - languageIndex));
                foreach (string lang in new[] { "trcn", "cn", "en", "kr", "jp", "ja", "ko", "zh-cn", "zh-tw" })
                {
                    string canonical = CanonicalizeTextLanguage(lang);
                    if (tail.IndexOf(lang, StringComparison.OrdinalIgnoreCase) >= 0 && TextDataModel.GetSupportedLangs().Contains(canonical))
                        return canonical;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("[RMR Localize] Failed to read option.dat language: " + e);
            }
            return string.Empty;
        }

        public static void LoadTextData(string language)
        {
            language = NormalizeTextLanguage(language);
            string localizeLanguage = ResolveModLocalizeLanguage(language);
            string str = "/Localize/" + localizeLanguage;
            abcdcode_LOGLIKE_MOD_Extension.TextDataModel._currentLanguage = language;
            abcdcode_LOGLIKE_MOD_Extension.TextDataModel.textDic.Clear();
            abcdcode_LOGLIKE_MOD_Extension.TextDataModel._isLoaded = true;
            if (!IsTmpFontCompatibleWithLanguage(LogLikeMod._DefFont_TMP, language))
                LogLikeMod._DefFont_TMP = null;
            Dictionary<string, string> textDic = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.textDic;
            DirectoryInfo directoryInfo1 = !Directory.Exists(LogLikeMod.path + str) ? new DirectoryInfo(LogLikeMod.path + "/Localize/en") : new DirectoryInfo(LogLikeMod.path + str);
            Debug.Log($"[RMR Localize] Loading mod text. gameLanguage={language}, modLocalizeLanguage={localizeLanguage}, path={directoryInfo1.FullName}");
            
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
                LogLikeMod.LoadLocalizeFile(file.FullName, ref textDic);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + str);
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles())
                        LogLikeMod.LoadLocalizeFile(file.FullName, ref textDic);
                }
            }
            foreach (FileSystemInfo file in GetLocalizeFilesIfDirectoryExists(Path.Combine(directoryInfo1.FullName, "PassiveInfo"), "PassiveInfo"))
                try
                {
                    LogLikeMod.LoadPassiveDesc(file.FullName, LogLikeMod.ModId);
                } catch (Exception e)
                {
                    Debug.Log("Failed to load PassiveInfo file at " + file.FullName + ":\n" + e);
                }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo3 = new DirectoryInfo($"{logMod.GetLogDllPath()}{str}/PassiveInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo3.FullName))
                {

                    foreach (FileSystemInfo file in directoryInfo3.GetFiles())
                        try { 
                            LogLikeMod.LoadPassiveDesc(file.FullName, uniqueId);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Failed to load PassiveInfo file at " + file.FullName + ":\n" + e);
                        }
                }
            }
            foreach (FileSystemInfo file in GetLocalizeFilesIfDirectoryExists(Path.Combine(directoryInfo1.FullName, "CardInfo"), "CardInfo"))
                try { 
                LogLikeMod.LoadCardDesc(file.FullName, LogLikeMod.ModId);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load CardInfo file at " + file.FullName + ":\n" + e);
                }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo4 = new DirectoryInfo($"{logMod.GetLogDllPath()}{str}/CardInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo4.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo4.GetFiles())
                        try { 
                        LogLikeMod.LoadCardDesc(file.FullName, uniqueId);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Failed to load CardInfo file at " + file.FullName + ":\n" + e);
                        }
                }
            }
            foreach (FileSystemInfo file in GetLocalizeFilesIfDirectoryExists(Path.Combine(directoryInfo1.FullName, "BookInfo"), "BookInfo"))
                try { 
                LogLikeMod.LoadBookDesc(file.FullName, LogLikeMod.ModId);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load BookInfo file at " + file.FullName + ":\n" + e);
                }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo5 = new DirectoryInfo($"{logMod.GetLogDllPath()}{str}/BookInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo5.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo5.GetFiles())
                        try
                        {
                            LogLikeMod.LoadBookDesc(file.FullName, uniqueId);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Failed to load BookInfo file at " + file.FullName + ":\n" + e);
                        }
                }
            }
            foreach (FileSystemInfo file in GetLocalizeFilesIfDirectoryExists(Path.Combine(directoryInfo1.FullName, "EnemyNameInfo"), "EnemyNameInfo"))
                try { 
                    LogLikeMod.LoadEnemyUnitName(file.FullName, LogLikeMod.ModId);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load EnemyUnitName file at " + file.FullName + ":\n" + e);
                }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo6 = new DirectoryInfo($"{logMod.GetLogDllPath()}{str}/EnemyNameInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo6.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo6.GetFiles())
                        try { 
                            LogLikeMod.LoadEnemyUnitName(file.FullName, uniqueId);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Failed to load EnemyUnitName file at " + file.FullName + ":\n" + e);
                        }
                }
            }
            foreach (FileSystemInfo file in GetLocalizeFilesIfDirectoryExists(Path.Combine(directoryInfo1.FullName, "DropBookInfo"), "DropBookInfo"))
                try { 
                    LogLikeMod.LoadDropBookName(file.FullName, LogLikeMod.ModId);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load DropBookName file at " + file.FullName + ":\n" + e);
                }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo7 = new DirectoryInfo($"{logMod.GetLogDllPath()}{str}/DropBookInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo7.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo7.GetFiles())
                        try { 
                            LogLikeMod.LoadDropBookName(file.FullName, uniqueId);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Failed to load DropBookName file at " + file.FullName + ":\n" + e);
                        }
                }
            }
            foreach (FileSystemInfo file in GetLocalizeFilesIfDirectoryExists(Path.Combine(directoryInfo1.FullName, "DiceAbilityInfo"), "DiceAbilityInfo"))
                try {
                    LogLikeMod.LoadDiceAbilityDesc(file.FullName, LogLikeMod.ModId);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load DiceAbilityDesc file at " + file.FullName + ":\n" + e);
                }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo8 = new DirectoryInfo($"{logMod.GetLogDllPath()}{str}/DiceAbilityInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo8.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo8.GetFiles())
                        try { 
                            LogLikeMod.LoadDiceAbilityDesc(file.FullName, uniqueId);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Failed to load DiceAbilityDesc file at " + file.FullName + ":\n" + e);
                        }
                }
            }
            try
            {
                LogueEffectXmlList.Instance.Init(localizeLanguage);
                RMRCore.LoadSatelliteBattleTexts(localizeLanguage);
                RMRCore.LoadSatelliteBattleDialog(localizeLanguage);
                RogueMysteryXmlList.Instance.Init(localizeLanguage);
            }
            catch (Exception e)
            {
                Debug.Log("Unable to re-localize modded stuff: " + e);
            }
            abcdcode_LOGLIKE_MOD_Extension.TextDataModel._isLoaded = true;
            Dictionary<string, BattleEffectText> dictionary = (Dictionary<string, BattleEffectText>)typeof(BattleEffectTextsXmlList).GetField("_dictionary", AccessTools.all).GetValue(Singleton<BattleEffectTextsXmlList>.Instance);
            dictionary["LogueLikeMod_LuckyBuf"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_LuckyBuf",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_LuckyBuf_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_LuckyBuf_Desc")
            };
            dictionary["LogueLikeMod_LuckyBuf_Page"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_LuckyBuf_Page",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_LuckyBuf_Page_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_LuckyBuf_Page_Desc")
            };
            dictionary["LogueLikeMod_PuppeteerBuf"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_PuppeteerBuf",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_PuppeteerBuf_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_PuppeteerBuf_Desc")
            };
            dictionary["LogueLikeMod_MaxUpMinDownBuf"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_MaxUpMinDownBuf",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_MaxUpMinDownBuf_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_MaxUpMinDownBuf_Desc")
            };
            dictionary["LogueLikeMod_MaxDownMinUpBuf"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_MaxDownMinUpBuf",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_MaxDownMinUpBuf_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_MaxDownMinUpBuf_Desc")
            };
            dictionary["LogueLikeMod_CricusDawn1Buf"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_CricusDawn1Buf",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_CricusDawn1Buf_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_CricusDawn1Buf_Desc")
            };
            dictionary["LogueLikeMod_CricusDawn2Buf"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_CricusDawn2Buf",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_CricusDawn2Buf_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_CricusDawn2Buf_Desc")
            };
        }

        public static MysteryXmlRoot LoadMysteryInfos(string str, string modid)
        {
            MysteryXmlRoot mysteryXmlRoot;
            using (StringReader stringReader = new StringReader(str))
                mysteryXmlRoot = (MysteryXmlRoot)new XmlSerializer(typeof(MysteryXmlRoot)).Deserialize((TextReader)stringReader);
            foreach (MysteryXmlInfo mystery in mysteryXmlRoot.Mysterys)
            {
                if (mystery.WorkShopId == string.Empty)
                    mystery.WorkShopId = modid;
            }
            return mysteryXmlRoot;
        }

        public void LoadMysteryInfos()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/SpecialStaticInfo/MysteryXmlInfos");
            List<MysteryXmlInfo> info = new List<MysteryXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles("*.xml"))
            {
                MysteryXmlRoot mysteryXmlRoot = LogLikeMod.LoadMysteryInfos(File.ReadAllText(file.FullName), LogLikeMod.ModId);
                info.AddRange((IEnumerable<MysteryXmlInfo>)mysteryXmlRoot.Mysterys);
            }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/SpecialStaticInfo/MysteryXmlInfos");
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles("*.xml"))
                    {
                        MysteryXmlRoot mysteryXmlRoot = LogLikeMod.LoadMysteryInfos(File.ReadAllText(file.FullName), logMod.invInfo.workshopInfo.uniqueId);
                        info.AddRange((IEnumerable<MysteryXmlInfo>)mysteryXmlRoot.Mysterys);
                    }
                }
            }
            Singleton<MysteryXmlList>.Instance.Init(info);
        }

        public static RewardPassivesRoot LoadRewardPassiveInfos(string str, string modid)
        {
            RewardPassivesRoot rewardPassivesRoot;
            using (StringReader stringReader = new StringReader(str))
                rewardPassivesRoot = (RewardPassivesRoot)new XmlSerializer(typeof(RewardPassivesRoot)).Deserialize(stringReader);
            foreach (RewardPassivesInfo rewardPassives in rewardPassivesRoot.RewardPassivesList)
            {
                if (rewardPassives.workshopid == string.Empty)
                    rewardPassives.workshopid = modid;
                foreach (RewardPassiveInfo rewardPassive in rewardPassives.RewardPassiveList)
                {
                    rewardPassive.workshopID = rewardPassives.workshopid == "@origin"
                        ? "@origin"
                        : rewardPassives.workshopid;
                    if (rewardPassive.iconartwork == string.Empty)
                        rewardPassive.iconartwork = rewardPassive.artwork;
                }
            }
            return rewardPassivesRoot;
        }

        public void LoadRewardPassiveInfos()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/SpecialStaticInfo/RewardPassiveInfos");
            List<RewardPassivesInfo> info = new List<RewardPassivesInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                RewardPassivesRoot rewardPassivesRoot = LogLikeMod.LoadRewardPassiveInfos(File.ReadAllText(file.FullName), LogLikeMod.ModId);
                info.AddRange((IEnumerable<RewardPassivesInfo>)rewardPassivesRoot.RewardPassivesList);
            }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/SpecialStaticInfo/RewardPassiveInfos");
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles())
                    {
                        RewardPassivesRoot rewardPassivesRoot = LogLikeMod.LoadRewardPassiveInfos(File.ReadAllText(file.FullName), logMod.invInfo.workshopInfo.uniqueId);
                        info.AddRange((IEnumerable<RewardPassivesInfo>)rewardPassivesRoot.RewardPassivesList);
                    }
                }
            }
            Singleton<RewardPassivesList>.Instance.Init(info);
        }

        public static StagesXmlRoot LoadStages(string str, string modid)
        {
            StagesXmlRoot stagesXmlRoot;
            using (StringReader stringReader = new StringReader(str))
                stagesXmlRoot = (StagesXmlRoot)new XmlSerializer(typeof(StagesXmlRoot)).Deserialize((TextReader)stringReader);
            foreach (StagesXmlInfo dropValueXml in stagesXmlRoot.ChapterList)
            {
                if (dropValueXml.packageId == string.Empty)
                    dropValueXml.packageId = modid;
                foreach (LogueStageInfo stage in dropValueXml.Stages)
                {
                    if (stage.workshopid == string.Empty)
                        stage.workshopid = modid;
                }
            }
            return stagesXmlRoot;
        }

        public void LoadStages()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/SpecialStaticInfo/StagesXmlInfos");
            List<StagesXmlInfo> info = new List<StagesXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles("*.xml"))
            {
                StagesXmlRoot stagesXmlRoot = LogLikeMod.LoadStages(File.ReadAllText(file.FullName), LogLikeMod.ModId);
                info.AddRange(stagesXmlRoot.ChapterList);
            }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/SpecialStaticInfo/StagesXmlInfos");
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles("*.xml"))
                    {
                        StagesXmlRoot stagesXmlRoot = LogLikeMod.LoadStages(File.ReadAllText(file.FullName), logMod.invInfo.workshopInfo.uniqueId);
                        info.AddRange(stagesXmlRoot.ChapterList);
                    }
                }
            }
            Singleton<StagesXmlList>.Instance.Init(info);
        }

        public static CardDropValueXmlRoot LoadDropValues(string str, string modid)
        {
            CardDropValueXmlRoot dropValueXmlRoot;
            using (StringReader stringReader = new StringReader(str))
                dropValueXmlRoot = (CardDropValueXmlRoot)new XmlSerializer(typeof(CardDropValueXmlRoot)).Deserialize((TextReader)stringReader);
            foreach (CardDropValueXmlInfo dropValueXml in dropValueXmlRoot.DropValueXmlList)
            {
                if (dropValueXml.workshopID == string.Empty)
                    dropValueXml.workshopID = modid;
            }
            return dropValueXmlRoot;
        }

        public void LoadDropValues()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/SpecialStaticInfo/DropValueXmlInfos");
            List<CardDropValueXmlInfo> info = new List<CardDropValueXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                CardDropValueXmlRoot dropValueXmlRoot = LogLikeMod.LoadDropValues(File.ReadAllText(file.FullName), LogLikeMod.ModId);
                info.AddRange((IEnumerable<CardDropValueXmlInfo>)dropValueXmlRoot.DropValueXmlList);
            }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/SpecialStaticInfo/DropValueXmlInfos");
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles())
                    {
                        CardDropValueXmlRoot dropValueXmlRoot = LogLikeMod.LoadDropValues(File.ReadAllText(file.FullName), logMod.invInfo.workshopInfo.uniqueId);
                        info.AddRange((IEnumerable<CardDropValueXmlInfo>)dropValueXmlRoot.DropValueXmlList);
                    }
                }
            }
            Singleton<CardDropValueList>.Instance.Init(info);
        }

        public static LogStoryPathRoot LoadStoryPath(string str, string modid)
        {
            LogStoryPathRoot logStoryPathRoot;
            using (StringReader stringReader = new StringReader(str))
                logStoryPathRoot = (LogStoryPathRoot)new XmlSerializer(typeof(LogStoryPathRoot)).Deserialize((TextReader)stringReader);
            foreach (LogStoryPathInfo logStoryPathInfo in logStoryPathRoot.list)
            {
                if (logStoryPathInfo.pid == string.Empty)
                    logStoryPathInfo.pid = modid;
            }
            return logStoryPathRoot;
        }

        public void LoadStoryPath()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/SpecialStaticInfo/StoryPathInfos");
            List<LogStoryPathInfo> infolist = new List<LogStoryPathInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                LogStoryPathRoot logStoryPathRoot = LogLikeMod.LoadStoryPath(File.ReadAllText(file.FullName), LogLikeMod.ModId);
                infolist.AddRange((IEnumerable<LogStoryPathInfo>)logStoryPathRoot.list);
            }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/SpecialStaticInfo/StoryPathInfos");
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles())
                    {
                        LogStoryPathRoot logStoryPathRoot = LogLikeMod.LoadStoryPath(File.ReadAllText(file.FullName), logMod.invInfo.workshopInfo.uniqueId);
                        infolist.AddRange((IEnumerable<LogStoryPathInfo>)logStoryPathRoot.list);
                    }
                }
            }
            LogStoryPathList.Instance.AddStoryPathInfo(infolist);
        }

        public static EmotionEgoXmlInfo AddEmotionEgoForReward(DiceCardXmlInfo info)
        {
            if (LogLikeMod.RewardCardDic_Dummy == null)
                LogLikeMod.RewardCardDic_Dummy = new Dictionary<string, List<EmotionEgoXmlInfo>>();
            EmotionEgoXmlInfo emotionEgoXmlInfo = new EmotionEgoXmlInfo();
            emotionEgoXmlInfo._CardId = info.id.id;
            emotionEgoXmlInfo.Sephirah = SephirahType.None;
            emotionEgoXmlInfo.id = -1;
            emotionEgoXmlInfo.isLock = false;
            if (info.id.packageId == string.Empty)
                return emotionEgoXmlInfo;
            if (!LogLikeMod.RewardCardDic_Dummy.ContainsKey(info.id.packageId))
                LogLikeMod.RewardCardDic_Dummy.Add(info.id.packageId, new List<EmotionEgoXmlInfo>());
            LogLikeMod.RewardCardDic_Dummy[info.id.packageId].Add(emotionEgoXmlInfo);
            return emotionEgoXmlInfo;
        }

        public static List<Assembly> GetAssemList()
        {
            Dictionary<string, List<Assembly>> dictionary = (Dictionary<string, List<Assembly>>)typeof(AssemblyManager).GetField("_assemblyDict", AccessTools.all).GetValue(Singleton<AssemblyManager>.Instance);
            List<Assembly> assemList = new List<Assembly>();
            if (dictionary != null)
            {
                foreach (List<Assembly> assemblyList in dictionary.Values)
                {
                    foreach (Assembly assembly in assemblyList)
                    {
                        if (!assemList.Contains(assembly))
                            assemList.Add(assembly);
                    }
                }
            }
            if (LogLikeMod.LogModAssemblys != null && LogLikeMod.LogModAssemblys.Count > 0)
            {
                foreach (Assembly logModAssembly in LogLikeMod.LogModAssemblys)
                {
                    if (!assemList.Contains(logModAssembly))
                        assemList.Add(logModAssembly);
                }
            }
            return assemList;
        }

        public static void SetStagePhase(StageController __instance, StageController.StagePhase phase)
        {
            typeof(StageController).GetMethod("set_phase", AccessTools.all).Invoke(__instance, new object[1]
            {
                phase
            });
        }

        public static bool IsBattleState()
        {
            UIPhase currentUiPhase = UI.UIController.Instance.CurrentUIPhase;
            int num;
            switch (currentUiPhase)
            {
                case UIPhase.BattleSetting:
                case UIPhase.BattleResult:
                    num = 1;
                    break;
                default:
                    num = currentUiPhase == UIPhase.DUMMY ? 1 : 0;
                    break;
            }
            return num != 0;
        }

        public static bool CheckStage(bool OnBattle = false)
        {
            if (!OnBattle && RMRRealizationManager.IsRealizationPreparationActive)
                return true;

            try
            {
                UIPhase currentUiPhase = UI.UIController.Instance.CurrentUIPhase;
                int num;
                switch (currentUiPhase)
                {
                    case UIPhase.Sephirah:
                    case UIPhase.Main_ItemList:
                    case UIPhase.Librarian_CardList:
                        num = 1;
                        break;
                    default:
                        num = currentUiPhase == UIPhase.Sepiroth ? 1 : 0;
                        break;
                }
                if (num != 0)
                    return false;
                LorId stage = Singleton<StageController>.Instance.GetStageModel().ClassInfo.id;
                if (stage == RMRCore.CurrentGamemode.StageStart || LogLikeMod.saveloading)
                    return true;
                if (stage == new LorId(LogLikeMod.ModId, -855))
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        public static void ResetNextStage()
        {
            LogLikeMod.nextlist = new List<EmotionCardXmlInfo>();
            if (LogLikeMod.curstagetype == StageType.Boss)
            {
                if (LogLikeMod.curchaptergrade != ChapterGrade.Grade6 && LogueBookModels.RemainStageList.ContainsKey(LogLikeMod.curchaptergrade + 1))
                    LogLikeMod.nextlist = LogueBookModels.GetNextList(LogLikeMod.curchaptergrade + 1, true);
                else
                    LogLikeMod.nextlist.Clear();
                LogLikeMod.curChStageStep = 0;
            }
            else
                LogLikeMod.nextlist = LogueBookModels.GetNextList(LogLikeMod.curchaptergrade, LogLikeMod.curstagetype == StageType.Start);
        }

        public static void LoadPassives()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/Passives");
            List<PassiveXmlInfo> list1 = new List<PassiveXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                try { 
                    List<PassiveXmlInfo> collection = LogLikeMod.LoadPassive(file.FullName, LogLikeMod.ModId);
                    list1.AddRange(collection);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load passives from " + file.FullName + ":\n" + e);
                }
            }
            Singleton<PassiveXmlList>.Instance.AddPassivesByMod(list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<PassiveXmlInfo> list2 = new List<PassiveXmlInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/Passives");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles())
                        try
                        {
                            list2.AddRange(LogLikeMod.LoadPassive(file.FullName, uniqueId));
                        } catch (Exception e)
                        {
                            Debug.Log("Failed to load passives from " + file.FullName + ":\n" + e);
                        }
                    Singleton<PassiveXmlList>.Instance.AddPassivesByMod(list2);
                }
            }
        }

        public static List<PassiveXmlInfo> LoadPassive(string path, string modid)
        {
            List<PassiveXmlInfo> passiveXmlInfoList = new List<PassiveXmlInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                        passiveXmlInfoList = (new XmlSerializer(typeof(PassiveXmlRoot)).Deserialize((TextReader)streamReader) as PassiveXmlRoot).list;
                }
                foreach (PassiveXmlInfo passiveXmlInfo in passiveXmlInfoList)
                    passiveXmlInfo.workshopID = modid;
            }
            catch (Exception ex)
            {
                Debug.Log((ex.Message + Environment.NewLine + ex.StackTrace));
            }
            return passiveXmlInfoList;
        }

        public static void LoadDecks()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/Deck");
            List<DeckXmlInfo> list1 = new List<DeckXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                try {
                    List<DeckXmlInfo> collection = LogLikeMod.LoadDeck(file.FullName, LogLikeMod.ModId);
                    list1.AddRange(collection);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load decks from " + file.FullName + ":\n" + e);
                }
            }
            Singleton<DeckXmlList>.Instance.AddDeckByMod(list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<DeckXmlInfo> list2 = new List<DeckXmlInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/Deck");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles())
                        try
                        {
                            list2.AddRange(LogLikeMod.LoadDeck(file.FullName, uniqueId));
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Failed to load decks from " + file.FullName + ":\n" + e);
                        }
                    Singleton<DeckXmlList>.Instance.AddDeckByMod(list2);
                }
            }
        }

        public static List<DeckXmlInfo> LoadDeck(string path, string modid)
        {
            List<DeckXmlInfo> deckXmlInfoList = new List<DeckXmlInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                        deckXmlInfoList = (new XmlSerializer(typeof(DeckXmlRoot)).Deserialize((TextReader)streamReader) as DeckXmlRoot).deckXmlList;
                }
                foreach (DeckXmlInfo deckXmlInfo in deckXmlInfoList)
                {
                    deckXmlInfo.workshopId = modid;
                    LorId.InitializeLorIds<LorIdXml>(deckXmlInfo._cardIdList, deckXmlInfo.cardIdList, modid);
                }
            }
            catch (Exception ex)
            {
                Debug.Log((ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return deckXmlInfoList;
        }

        public static void LoadDropBooks()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/DropBook");
            List<DropBookXmlInfo> list1 = new List<DropBookXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                try {
                List<DropBookXmlInfo> collection = LogLikeMod.LoadDropBook(file.FullName, LogLikeMod.ModId);
                list1.AddRange(collection);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load dropbooks from " + file.FullName + ":\n" + e);
                }
            }
            Singleton<DropBookXmlList>.Instance.SetDropTableByMod(list1);
            Singleton<DropBookXmlList>.Instance.AddBookByMod(LogLikeMod.ModId, list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<DropBookXmlInfo> list2 = new List<DropBookXmlInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/DropBook");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles())
                    {
                        try {
                            File.ReadAllText(file.FullName);
                            list2.AddRange(LogLikeMod.LoadDropBook(file.FullName, uniqueId));
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Failed to load dropbooks from " + file.FullName + ":\n" + e);
                        }
                    }
                    Singleton<DropBookXmlList>.Instance.SetDropTableByMod(list2);
                    Singleton<DropBookXmlList>.Instance.AddBookByMod(uniqueId, list2);
                }
            }
        }

        public static List<DropBookXmlInfo> LoadDropBook(string path, string modid)
        {
            List<DropBookXmlInfo> dropBookXmlInfoList = new List<DropBookXmlInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                    {
                        dropBookXmlInfoList = (new XmlSerializer(typeof(BookUseXmlRoot)).Deserialize((TextReader)streamReader) as BookUseXmlRoot).bookXmlList;
                        foreach (DropBookXmlInfo dropBookXmlInfo in dropBookXmlInfoList)
                        {
                            dropBookXmlInfo.workshopID = modid;
                            dropBookXmlInfo.InitializeDropItemList(modid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log((ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return dropBookXmlInfoList;
        }

        public static void LoadCardDropTables()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/CardDropTable");
            List<CardDropTableXmlInfo> list1 = new List<CardDropTableXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                try {
                List<CardDropTableXmlInfo> collection = LogLikeMod.LoadCardDropTable(file.FullName, LogLikeMod.ModId);
                list1.AddRange(collection);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load card drop tables from " + file.FullName + ":\n" + e);
                }
            }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<CardDropTableXmlInfo> list2 = new List<CardDropTableXmlInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/CardDropTable");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles())
                    {
                        try {
                            List<CardDropTableXmlInfo> collection = LogLikeMod.LoadCardDropTable(file.FullName, uniqueId);
                            foreach (CardDropTableXmlInfo dropTableXmlInfo1 in new List<CardDropTableXmlInfo>((IEnumerable<CardDropTableXmlInfo>)collection))
                            {
                                CardDropTableXmlInfo tempinfo = dropTableXmlInfo1;
                                CardDropTableXmlInfo dropTableXmlInfo2 = list1.Find((Predicate<CardDropTableXmlInfo>)(x => x.id == tempinfo.id));
                                if (dropTableXmlInfo2 != null)
                                {
                                    dropTableXmlInfo2.cardIdList.AddRange(tempinfo.cardIdList);
                                    dropTableXmlInfo2._cardIdList.AddRange(tempinfo._cardIdList);
                                    collection.Remove(tempinfo);
                                }
                            }
                            list2.AddRange(collection);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Failed to load card drop tables from " + file.FullName + ":\n" + e);
                        }
                    }
                    Singleton<CardDropTableXmlList>.Instance.AddCardDropTableByMod(uniqueId, list2);
                }
            }
            Singleton<CardDropTableXmlList>.Instance.AddCardDropTableByMod(LogLikeMod.ModId, list1);
        }

        public static List<CardDropTableXmlInfo> LoadCardDropTable(string path, string modid)
        {
            List<CardDropTableXmlInfo> dropTableXmlInfoList = new List<CardDropTableXmlInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                        dropTableXmlInfoList = (new XmlSerializer(typeof(abcdcode_LOGLIKE_MOD_Extension.CardDropTableXmlRoot)).Deserialize((TextReader)streamReader) as abcdcode_LOGLIKE_MOD_Extension.CardDropTableXmlRoot).Convert().dropTableXmlList;
                }
                foreach (CardDropTableXmlInfo dropTableXmlInfo in dropTableXmlInfoList)
                {
                    if (dropTableXmlInfo.workshopId == string.Empty)
                        dropTableXmlInfo.workshopId = modid;
                    dropTableXmlInfo.cardIdList.Clear();
                    LorId.InitializeLorIds<LorIdXml>(dropTableXmlInfo._cardIdList, dropTableXmlInfo.cardIdList, modid);
                }
            }
            catch (Exception ex)
            {
                Debug.Log((ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return dropTableXmlInfoList;
        }

        public static void LoadCardInfos()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/CardInfo");
            List<DiceCardXmlInfo> list1 = new List<DiceCardXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                try {
                    List<DiceCardXmlInfo> collection = LogLikeMod.LoadCardInfo(file.FullName, LogLikeMod.ModId);
                    list1.AddRange(collection);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load card infos from " + file.FullName + ":\n" + e);
                }
            }
            ItemXmlDataList.instance.AddCardInfoByMod(LogLikeMod.ModId, list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<DiceCardXmlInfo> list2 = new List<DiceCardXmlInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/CardInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles())
                        try
                        {
                            list2.AddRange(LogLikeMod.LoadCardInfo(file.FullName, uniqueId));
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Failed to load card infos from " + file.FullName + ":\n" + e);
                        }
                    ItemXmlDataList.instance.AddCardInfoByMod(uniqueId, list2);
                }
            }
        }

        public static List<DiceCardXmlInfo> LoadCardInfo(string path, string modid)
        {
            List<DiceCardXmlInfo> diceCardXmlInfoList = new List<DiceCardXmlInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                    {
                        diceCardXmlInfoList = (new XmlSerializer(typeof(DiceCardXmlRoot)).Deserialize((TextReader)streamReader) as DiceCardXmlRoot).cardXmlList;
                        foreach (DiceCardXmlInfo diceCardXmlInfo in diceCardXmlInfoList)
                            diceCardXmlInfo.workshopID = modid;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log((ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return diceCardXmlInfoList;
        }

        public static void LoadStageInfos()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/StageInfo");
            List<StageClassInfo> list1 = new List<StageClassInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                try {
                    List<StageClassInfo> collection = LogLikeMod.LoadStage(file.FullName, LogLikeMod.ModId);
                    list1.AddRange(collection);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load stage info from " + file.FullName + ":\n" + e);
                }
            }
            Singleton<StageClassInfoList>.Instance.AddStageByMod(LogLikeMod.ModId, list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<StageClassInfo> list2 = new List<StageClassInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/StageInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles())
                    {
                        try {
                            list2.AddRange(LogLikeMod.LoadStage(file.FullName, uniqueId));
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Failed to load stage info from " + file.FullName + ":\n" + e);
                        }
                    }
                    Singleton<StageClassInfoList>.Instance.AddStageByMod(uniqueId, list2);
                }
            }
        }

        public static List<StageClassInfo> LoadStage(string path, string modid)
        {
            List<StageClassInfo> stageClassInfoList = new List<StageClassInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                    {
                        stageClassInfoList = (new XmlSerializer(typeof(StageXmlRoot)).Deserialize((TextReader)streamReader) as StageXmlRoot).list;
                        foreach (StageClassInfo stageClassInfo in stageClassInfoList)
                        {
                            stageClassInfo.workshopID = modid;
                            stageClassInfo.InitializeIds(modid);
                            RestoreVanillaEnemyIdsForImpurityStage(stageClassInfo);
                            foreach (StageStoryInfo story in stageClassInfo.storyList)
                            {
                                story.packageId = modid;
                                story.valid = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log((ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return stageClassInfoList;
        }

        private static void RestoreVanillaEnemyIdsForImpurityStage(StageClassInfo stageClassInfo)
        {
            if (!IsImpurityRouteStage(stageClassInfo))
                return;
            if (stageClassInfo.waveList == null)
                return;

            foreach (StageWaveInfo wave in stageClassInfo.waveList)
                RestoreVanillaLorIdsInObject(wave);
        }

        private static bool IsImpurityRouteStage(StageClassInfo stageClassInfo)
        {
            return stageClassInfo?.id != null
                && stageClassInfo.id.packageId == LogLikeMod.ModId
                && IsVanillaImpurityBattleStage(stageClassInfo.id.id);
        }

        private static bool IsVanillaImpurityBattleStage(int stageId)
        {
            return (stageId >= 70001 && stageId <= 70010)
                || stageId == 70020
                || stageId == 70021;
        }

        private static string DescribeWaveEnemyIds(StageWaveInfo wave)
        {
            if (wave?.enemyUnitIdList == null || wave.enemyUnitIdList.Count == 0)
                return "(none)";
            return string.Join(", ", wave.enemyUnitIdList.Select(id => id?.ToString() ?? "NULL").ToArray());
        }

        private static void RestoreVanillaLorIdsInObject(object target)
        {
            if (target == null)
                return;
            foreach (FieldInfo field in target.GetType().GetFields(AccessTools.all))
            {
                object value = field.GetValue(target);
                if (value is LorId lorId)
                {
                    if (lorId.id > 0)
                        field.SetValue(target, new LorId(lorId.id));
                    continue;
                }
                if (value is System.Collections.IList list)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        object item = list[i];
                        if (item is LorId listLorId)
                        {
                            if (listLorId.id > 0)
                                list[i] = new LorId(listLorId.id);
                        }
                        else
                        {
                            TrySetLorIdXmlPackageId(item, string.Empty);
                        }
                    }
                }
            }
        }

        private static void TrySetLorIdXmlPackageId(object item, string packageId)
        {
            if (item == null)
                return;
            Type type = item.GetType();
            FieldInfo pidField = type.GetField("pid", AccessTools.all) ?? type.GetField("packageId", AccessTools.all);
            FieldInfo xmlIdField = type.GetField("xmlId", AccessTools.all) ?? type.GetField("id", AccessTools.all);
            PropertyInfo pidProperty = type.GetProperty("pid", AccessTools.all) ?? type.GetProperty("packageId", AccessTools.all);
            PropertyInfo xmlIdProperty = type.GetProperty("xmlId", AccessTools.all) ?? type.GetProperty("id", AccessTools.all);
            object idValue = xmlIdField != null
                ? xmlIdField.GetValue(item)
                : xmlIdProperty?.GetValue(item, null);
            if (idValue is int id && id > 0)
            {
                if (pidField != null)
                    pidField.SetValue(item, packageId);
                else if (pidProperty != null && pidProperty.CanWrite)
                    pidProperty.SetValue(item, packageId, null);
            }
        }

        public static void LoadEnemyUnitInfos()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/EnemyUnitInfo");
            List<EnemyUnitClassInfo> list1 = new List<EnemyUnitClassInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                try {
                    List<EnemyUnitClassInfo> collection = LogLikeMod.LoadEnemyUnit(file.FullName, LogLikeMod.ModId);
                    list1.AddRange(collection);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load enemy units infos from " + file.FullName + ":\n" + e);
                }
            }
            Singleton<EnemyUnitClassInfoList>.Instance.AddEnemyUnitByMod(LogLikeMod.ModId, list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<EnemyUnitClassInfo> list2 = new List<EnemyUnitClassInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/EnemyUnitInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles())
                    {
                        try {
                            list2.AddRange(LogLikeMod.LoadEnemyUnit(file.FullName, uniqueId));
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Failed to load enemy unit infos from " + file.FullName + ":\n" + e);
                        }
                    }
                    Singleton<EnemyUnitClassInfoList>.Instance.AddEnemyUnitByMod(uniqueId, list2);
                }
            }
        }

        public static List<EnemyUnitClassInfo> LoadEnemyUnit(string path, string modid)
        {
            List<EnemyUnitClassInfo> enemyUnitClassInfoList = new List<EnemyUnitClassInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                    {
                        enemyUnitClassInfoList = (new XmlSerializer(typeof(EnemyUnitClassRoot)).Deserialize((TextReader)streamReader) as EnemyUnitClassRoot).list;
                        foreach (EnemyUnitClassInfo enemyUnitClassInfo in enemyUnitClassInfoList)
                        {
                            enemyUnitClassInfo.workshopID = modid;
                            enemyUnitClassInfo.height = RandomUtil.SystemRange(enemyUnitClassInfo.maxHeight - enemyUnitClassInfo.minHeight) + enemyUnitClassInfo.minHeight;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log((ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return enemyUnitClassInfoList;
        }

        public static bool CheckIsEquipReward(RewardPassiveInfo x)
        {
            PickUpModelBase pickUp = LogLikeMod.FindPickUp(x.script);
            return pickUp is ShopPickUpModel && (pickUp as ShopPickUpModel).IsEquipReward();
        }

        /// <summary>
        /// Method that creates all the passive key pages automatically
        /// </summary>
        public static void CreateShopEquipPages()
        {
            if (LogLikeMod.CreatedShopEquipPages)
                return;
            List<RewardPassivesInfo> all = Singleton<RewardPassivesList>.Instance.infos.FindAll(x => x.rewardtype == PassiveRewardListType.Shop);
            List<RewardPassiveInfo> rewardPassiveInfoList = new List<RewardPassiveInfo>();
            Dictionary<string, List<BookXmlInfo>> dictionary = new Dictionary<string, List<BookXmlInfo>>();
            foreach (RewardPassivesInfo rewardPassivesInfo in all)
                rewardPassiveInfoList.AddRange(rewardPassivesInfo.RewardPassiveList.FindAll(x => LogLikeMod.CheckIsEquipReward(x)));
            string path = LogLikeMod.path + "/AddData/EquipPage/EquipPage_ShopDefault.xml";
            foreach (RewardPassiveInfo rewardPassiveInfo in rewardPassiveInfoList)
            {
                ShopPickUpModel pickUp = LogLikeMod.FindPickUp(rewardPassiveInfo.script) as ShopPickUpModel;
                BookXmlInfo bookXmlInfo = LogLikeMod.LoadEquipPage(path, LogLikeMod.ModId)[0];
                bookXmlInfo._bookIcon = "<LogLike>" + rewardPassiveInfo.artwork;
                bookXmlInfo.EquipEffect._PassiveList.Add(new LorIdXml()
                {
                    pid = pickUp.basepassive.workshopID,
                    xmlId = pickUp.basepassive._id
                });
                bookXmlInfo.EquipEffect.PassiveList.Add(pickUp.basepassive.id);
                bookXmlInfo.workshopID = rewardPassiveInfo.workshopID;
                bookXmlInfo._id = rewardPassiveInfo.passiveid;
                bookXmlInfo.skinType = "CUSTOM";
                bookXmlInfo.CharacterSkin[0] = bookXmlInfo._bookIcon;
                bookXmlInfo.InnerName = pickUp.Name;
                if (!dictionary.ContainsKey(rewardPassiveInfo.workshopID))
                    dictionary.Add(rewardPassiveInfo.workshopID, new List<BookXmlInfo>());
                dictionary[rewardPassiveInfo.workshopID].Add(bookXmlInfo);
            }
            foreach (KeyValuePair<string, List<BookXmlInfo>> keyValuePair in dictionary)
            {
                Singleton<BookXmlList>.Instance.AddEquipPageByMod(keyValuePair.Key, keyValuePair.Value);
                Dictionary<string, List<BookXmlInfo>> fieldValue = LogLikeMod.GetFieldValue<Dictionary<string, List<BookXmlInfo>>>(Singleton<BookXmlList>.Instance, "_workshopBookDict");
                if (!fieldValue.ContainsKey(keyValuePair.Key))
                    fieldValue.Add(keyValuePair.Key, new List<BookXmlInfo>());
                fieldValue[keyValuePair.Key].AddRange(keyValuePair.Value);
            }
            LogLikeMod.CreatedShopEquipPages = true;
        }

        public static void LoadEquipPages()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/EquipPage");
            List<BookXmlInfo> list1 = new List<BookXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles("*.xml"))
            {
                try {
                    List<BookXmlInfo> collection = LogLikeMod.LoadEquipPage(file.FullName, LogLikeMod.ModId);
                    list1.AddRange(collection);
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to load equip pages from " + file.FullName + ":\n" + e);
                }
            }
            Singleton<BookXmlList>.Instance.AddEquipPageByMod(LogLikeMod.ModId, list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<BookXmlInfo> list2 = new List<BookXmlInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/EquipPage");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles("*.xml"))
                    {
                        try {
                            list2.AddRange(LogLikeMod.LoadEquipPage(file.FullName, uniqueId));
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Failed to load equip pages from " + file.FullName + ":\n" + e);
                        }
                    }
                    Singleton<BookXmlList>.Instance.AddEquipPageByMod(uniqueId, list2);
                }
            }
        }

        public static List<BookXmlInfo> LoadEquipPage(string path, string modid)
        {
            List<BookXmlInfo> bookXmlInfoList = new List<BookXmlInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                    {
                        bookXmlInfoList = (new XmlSerializer(typeof(BookXmlRoot)).Deserialize((TextReader)streamReader) as BookXmlRoot).bookXmlList;
                        foreach (BookXmlInfo bookXmlInfo in bookXmlInfoList)
                        {
                            bookXmlInfo.workshopID = modid;
                            LorId.InitializeLorIds<LorIdXml>(bookXmlInfo.EquipEffect._PassiveList, bookXmlInfo.EquipEffect.PassiveList, modid);
                            if (!string.IsNullOrEmpty(bookXmlInfo.skinType))
                            {
                                if (bookXmlInfo.skinType == "UNKNOWN")
                                    bookXmlInfo.skinType = "Lor";
                                else if (bookXmlInfo.skinType == "CUSTOM")
                                    bookXmlInfo.skinType = "Custom";
                                else if (bookXmlInfo.skinType == "LOR")
                                    bookXmlInfo.skinType = "Lor";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log((ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return bookXmlInfoList;
        }

        public override void OnInitializeMod()
        {
            try
            {
                LogLikeMod.path = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
                LogLikeMod.logLikeHooks = new LogLikeHooks();
                base.OnInitializeMod();

                Harmony.CreateAndPatchAll(typeof(LogLikePatches), "abcdcode.LogLikeMOD");

                /*
                try
                {
                    HarmonyMethod postfix1 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.StageController_ClearBattle), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(StageController).GetMethod("ClearBattle", AccessTools.all), postfix: postfix1);
                    HarmonyMethod postfix2 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIController_CallUIPhase), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UI.UIController).GetMethod("CallUIPhase", AccessTools.all, (System.Reflection.Binder)null, new System.Type[1]
                    {
                    typeof (UIPhase)
                    }, (ParameterModifier[])null), postfix: postfix2);
                    HarmonyMethod prefix1 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleAllyCardDetail_ReturnCardToHand), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleAllyCardDetail).GetMethod("ReturnCardToHand", AccessTools.all), prefix1);
                    HarmonyMethod prefix2 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleEmotionCoinUI_Init), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleEmotionCoinUI).GetMethod("Init", AccessTools.all), prefix2);
                    HarmonyMethod prefix3 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleEmotionInfo_CenterBtn_OnPointerUp), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleEmotionInfo_CenterBtn).GetMethod("OnPointerUp", AccessTools.all), prefix3);
                    HarmonyMethod prefix4 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleEmotionRewardInfoUI_SetData), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleEmotionRewardInfoUI).GetMethod("SetData", AccessTools.all), prefix4);
                    HarmonyMethod prefix5 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattlePlayingCardDataInUnitModel_OnUseCard), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattlePlayingCardDataInUnitModel).GetMethod("OnUseCard", AccessTools.all), prefix5);
                    HarmonyMethod prefix6 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattlePlayingCardSlotDetail_OnApplyCard), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattlePlayingCardSlotDetail).GetMethod("OnApplyCard", AccessTools.all), prefix6);
                    HarmonyMethod prefix7 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattlePersonalEgoCardDetail_ReturnCardToHand), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattlePersonalEgoCardDetail).GetMethod("ReturnCardToHand", AccessTools.all), prefix7);

                    HarmonyMethod postfix4 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleDiceCardUI_GetClickableState), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleDiceCardUI).GetMethod("GetClickableState", AccessTools.all), postfix: postfix4);
                    HarmonyMethod prefix9 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleUnitInfoManagerUI_Initialize), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleUnitInfoManagerUI).GetMethod("Initialize", AccessTools.all), prefix9);
                    HarmonyMethod postfix5 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleUnitModel_OnDie), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleUnitModel).GetMethod("OnDie", AccessTools.all), postfix: postfix5);
                    HarmonyMethod postfix6 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleUnitPassiveDetail_DmgFactor), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleUnitPassiveDetail).GetMethod("DmgFactor", AccessTools.all), postfix: postfix6);
                    HarmonyMethod prefix10 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleUnitModel_BeforeRollDice), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleUnitModel).GetMethod("BeforeRollDice", AccessTools.all), prefix10);
                    HarmonyMethod postfix7 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleUnitBuf_Destroy), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleUnitBuf).GetMethod("Destroy", AccessTools.all), postfix: postfix7);
                    HarmonyMethod prefix11 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleUnitBuf_burn_OnRoundEnd), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleUnitBuf_burn).GetMethod("OnRoundEnd", AccessTools.all), prefix11);
                    HarmonyMethod postfix8 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleUnitBufListDetail_CheckGift), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleUnitBufListDetail).GetMethod("CheckGift", AccessTools.all), postfix: postfix8);
                    HarmonyMethod postfix9 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleUnitPassiveDetail_OnDie), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleUnitPassiveDetail).GetMethod("OnDie", AccessTools.all), postfix: postfix9);
                    HarmonyMethod postfix10 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BattleUnitPassiveDetail_OnKill), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BattleUnitPassiveDetail).GetMethod("OnKill", AccessTools.all), postfix: postfix10);
                    HarmonyMethod postfix11 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BookInventoryModel_GetBookList_PassiveEquip), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BookInventoryModel).GetMethod("GetBookList_PassiveEquip", AccessTools.all), postfix: postfix11);
                    HarmonyMethod postfix12 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BookInventoryModel_GetBookListAll), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BookInventoryModel).GetMethod("GetBookListAll", AccessTools.all), postfix: postfix12);
                    HarmonyMethod postfix13 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BookInventoryModel_GetBookByInstanceId), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BookInventoryModel).GetMethod("GetBookByInstanceId", AccessTools.all), postfix: postfix13);
                    HarmonyMethod postfix14 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BookInventoryModel_GetAllBookByInstanceId), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BookInventoryModel).GetMethod("GetAllBookByInstanceId", AccessTools.all), postfix: postfix14);
                    HarmonyMethod postfix15 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BookPassiveInfo_get_desc_postfix), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BookPassiveInfo).GetMethod("get_desc", AccessTools.all), postfix: postfix15);
                    HarmonyMethod prefix12 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BookPassiveInfo_get_desc), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BookPassiveInfo).GetMethod("get_desc", AccessTools.all), prefix12);
                    HarmonyMethod prefix13 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BookPassiveInfo_get_name), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BookPassiveInfo).GetMethod("get_name", AccessTools.all), prefix13);
                    HarmonyMethod prefix14 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.CharacterAppearance_ChangeMotion_prefix), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(CharacterAppearance).GetMethod("ChangeMotion", AccessTools.all), prefix14);
                    HarmonyMethod postfix16 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.CharacterAppearance_ChangeMotion), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(CharacterAppearance).GetMethod("ChangeMotion", AccessTools.all), postfix: postfix16);
                    HarmonyMethod postfix17 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.CustomizingCardArtworkLoader_GetSpecificArtworkSprite), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(CustomizingCardArtworkLoader).GetMethod("GetSpecificArtworkSprite", AccessTools.all), postfix: postfix17);
                    HarmonyMethod prefix15 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.EmotionEgoXmlInfo_get_CardId), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(EmotionEgoXmlInfo).GetMethod("get_CardId", AccessTools.all), prefix15);
                    HarmonyMethod postfix18 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.EmotionPassiveCardUI_SetSprites), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(EmotionPassiveCardUI).GetMethod("SetSprites", AccessTools.all), postfix: postfix18);
                    HarmonyMethod postfix19 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.EmotionPassiveCardUI_Init), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(EmotionPassiveCardUI).GetMethod("Init", AccessTools.all), postfix: postfix19);
                    HarmonyMethod prefix16 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.ItemXmlDataList_GetCardItem), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(ItemXmlDataList).GetMethod("GetCardItem", AccessTools.all, (System.Reflection.Binder)null, new System.Type[2]
                    {
                    typeof (LorId),
                    typeof (bool)
                    }, (ParameterModifier[])null), prefix16);
                    HarmonyMethod postfix20 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.LevelUpUI_OnClickTargetUnit), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(LevelUpUI).GetMethod("OnClickTargetUnit", AccessTools.all), postfix: postfix20);
                    HarmonyMethod postfix21 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.LocalizedTextLoader_LoadOthers), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(LocalizedTextLoader).GetMethod("LoadOthers", AccessTools.all), postfix: postfix21);
                    HarmonyMethod prefix17 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.SpecialCardListModel_ReturnCardToHand), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(SpecialCardListModel).GetMethod("ReturnCardToHand", AccessTools.all), prefix17);
                    
                    HarmonyMethod postfix23 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.StageController_OnFixedUpdateLate), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(StageController).GetMethod("OnFixedUpdateLate", AccessTools.all), postfix: postfix23);
                    HarmonyMethod postfix24 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.StageController_ActivateStartBattleEffectPhase), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(StageController).GetMethod("ActivateStartBattleEffectPhase", AccessTools.all), postfix: postfix24);

                    HarmonyMethod prefix18 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.StageController_RoundStartPhase_System), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(StageController).GetMethod("RoundStartPhase_System", AccessTools.all), prefix18);
                    HarmonyMethod prefix19 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.StageController_CheckStoryAfterBattle), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(StageController).GetMethod("CheckStoryAfterBattle", AccessTools.all), prefix19);
                    HarmonyMethod postfix26 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.StageModel_GetFrontAvailableFloor), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(StageModel).GetMethod("GetFrontAvailableFloor", AccessTools.all), postfix: postfix26);
                    HarmonyMethod postfix27 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.StageWaveModel_Init), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(StageWaveModel).GetMethod("Init", AccessTools.all), postfix: postfix27);
                    HarmonyMethod postfix28 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.StageWaveModel_GetUnitBattleDataListByFormation), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(StageWaveModel).GetMethod("GetUnitBattleDataListByFormation", AccessTools.all), postfix: postfix28);
                    HarmonyMethod prefix20 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.TextDataModel_GetText), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(TextDataModel).GetMethod("GetText", AccessTools.all), prefix20);

                    HarmonyMethod postfix30 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIBattleSettingEditPanel_Close), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIBattleSettingEditPanel).GetMethod("Close", AccessTools.all), postfix: postfix30);
                    HarmonyMethod prefix21 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIBattleSettingEditPanel_SetBUttonState), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIBattleSettingEditPanel).GetMethod("SetBUttonState", AccessTools.all), prefix21);
                    HarmonyMethod prefix22 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIBattleSettingPanel_OnClickBackButton), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIBattleSettingPanel).GetMethod("OnClickBackButton", AccessTools.all), prefix22);
                    HarmonyMethod prefix23 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIBattleSettingWaveList_SetData), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIBattleSettingWaveList).GetMethod("SetData", AccessTools.all), prefix23);
                    HarmonyMethod postfix31 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIColorManager_GetSephirahColor), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIColorManager).GetMethod("GetSephirahColor", AccessTools.all), postfix: postfix31);
                    HarmonyMethod postfix32 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIColorManager_GetSephirahGlowColor), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIColorManager).GetMethod("GetSephirahGlowColor", AccessTools.all), postfix: postfix32);
                    HarmonyMethod postfix33 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIController_Awake), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UI.UIController).GetMethod("Awake", AccessTools.all), postfix: postfix33);
                    HarmonyMethod postfix34 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UICharacterStatInfoPanel_SetData), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UICharacterStatInfoPanel).GetMethod("SetData", AccessTools.all, (System.Reflection.Binder)null, new System.Type[1]
                    {
                     typeof (UnitDataModel)
                    }, (ParameterModifier[])null), postfix: postfix34);
                    HarmonyMethod postfix35 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIEmotionPassiveCardInven_SetSprites), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIEmotionPassiveCardInven).GetMethod("SetSprites", AccessTools.all), postfix: postfix35);
                    HarmonyMethod postfix36 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIInvitationRightMainPanel_OpenInit), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIInvitationRightMainPanel).GetMethod("OpenInit", AccessTools.all), postfix: postfix36);
                    HarmonyMethod postfix37 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UILibrarianEquipInfoSlot_SetData), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UILibrarianEquipInfoSlot).GetMethod("SetData", AccessTools.all), postfix: postfix37);
                    HarmonyMethod postfix38 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIManualContentPanel_SetData), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIManualContentPanel).GetMethod("SetData", AccessTools.all), postfix: postfix38);
                    HarmonyMethod postfix39 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIManualScreenPage_LoadContent), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIManualScreenPage).GetMethod("LoadContent", AccessTools.all), postfix: postfix39);
                    HarmonyMethod postfix40 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIOptionWindow_Open), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIOptionWindow).GetMethod("Open", AccessTools.all), postfix: postfix40);
                    HarmonyMethod postfix41 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIPassiveSuccessionPopup_InitReservedData), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIPassiveSuccessionPopup).GetMethod("InitReservedData", AccessTools.all), postfix: postfix41);
                    HarmonyMethod prefix24 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIPassiveSuccessionSlot_SetDataModel), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIPassiveSuccessionSlot).GetMethod("SetDataModel", AccessTools.all), prefix24);
                    HarmonyMethod prefix25 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIPopupWindowManager_Update), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIPopupWindowManager).GetMethod("Update", AccessTools.all), prefix25);
                    HarmonyMethod prefix26 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UISpriteDataManager_GetStoryIcon), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UISpriteDataManager).GetMethod("GetStoryIcon", AccessTools.all), prefix26);
                    HarmonyMethod postfix42 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UIBattleSettingLibrarianInfoPanel_SetData), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UIBattleSettingLibrarianInfoPanel).GetMethod("SetData", AccessTools.all), postfix: postfix42);
                    HarmonyMethod prefix27 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UnitDataModel_EquipBookForUI), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UnitDataModel).GetMethod("EquipBookForUI", AccessTools.all), prefix27);
                    HarmonyMethod prefix28 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.UICharacterListPanel_RefreshBattleUnitDataModel), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(UICharacterListPanel).GetMethod("RefreshBattleUnitDataModel", AccessTools.all), prefix28);
                    HarmonyMethod postfix43 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BookModel_GetThumbSprite), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BookModel).GetMethod("GetThumbSprite", AccessTools.all), postfix: postfix43);
                    HarmonyMethod postfix44 = new HarmonyMethod(typeof(LogLikePatches).GetMethod(nameof(LogLikePatches.BookModel_GetMaxPassiveCost), AccessTools.all));
                    this.Patching(harmony, (MethodBase)typeof(BookModel).GetMethod("GetMaxPassiveCost", AccessTools.all), postfix: postfix44);
                } catch (Exception e)
                {
                    this.Log(e.Message + "\n" + e.StackTrace);
                }
                */

                // do monomod hooks
                HookHelper.CreateHook(typeof(BattleSceneRoot), nameof(BattleSceneRoot.InitInvitationMap), LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.CryingChildMapManager_InitInvitationMap));
                HookHelper.CreateHook(typeof(StageController), "RoundEndPhase_ChoiceEmotionCard", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.StageController_RoundEndPhase_ChoiceEmotionCard));
                // UNUSED // HookHelper.CreateHook(typeof(StageController), "InitStageByInvitation", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.StageController_InitStageByInvitation));
                // UNUSED // HookHelper.CreateHook(typeof(StageController), "RoundEndPhase_ReturnUnit", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.RoundEndPhase_ReturnUnit));
                HookHelper.CreateHook(typeof(StageController), (MethodBase)typeof(StageController).GetMethod("CreateLibrarianUnit", AccessTools.all, (System.Reflection.Binder)null, new System.Type[1]
                {
                    typeof (SephirahType)
                }, (ParameterModifier[])null), LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.StageController_CreateLibrarianUnit));
                HookHelper.CreateHook(typeof(StageController), "StartBattle", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.StageController_StartBattle));
                HookHelper.CreateHook(typeof(StageController), "OnEnemyDropBookForAdded", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.StageController_OnEnemyDropBookForAdded));
                HookHelper.CreateHook(typeof(StageController), "EndBattlePhase", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.StageController_EndBattlePhase));
                HookHelper.CreateHook(typeof(StageController), "EndBattle", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.StageController_EndBattle));

                HookHelper.CreateHook(typeof(BookModel), "ChangePassive", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.BookModel_ChangePassive));
                HookHelper.CreateHook(typeof(StageLibraryFloorModel), "OnPickPassiveCard", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.StageLibraryFloorModel_OnPickPassiveCard));
                HookHelper.CreateHook(typeof(StageLibraryFloorModel), "OnPickEgoCard", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.StageLibraryFloorModel_OnPickEgoCard));
                HookHelper.CreateHook(typeof(LevelUpUI), "InitBase", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.LevelUpUI_InitBase));
                HookHelper.CreateHook(typeof(LevelUpUI), "SetEmotionPerDataUI", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.LevelUpUI_SetEmotionPerDataUI));
                HookHelper.CreateHook(typeof(LevelUpUI), "OnSelectRoutine", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.LevelUpUI_OnSelectRoutine));
                // DEPRECATED // HookHelper.CreateHook(typeof(DropBookInventoryModel), "GetBookList_invitationBookList", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.DropBookInventoryModel_GetBookList_invitationBookList));
                HookHelper.CreateHook(typeof(BookInventoryModel), "GetBookList_equip", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.BookInventoryModel_GetBookList_equip));
                HookHelper.CreateHook(typeof(UIBattleSettingPanel), "SetCurrentSephirahButton", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UIBattleSettingPanel_SetCurrentSephirahButton));
                HookHelper.CreateHook(typeof(UILibrarianCharacterListPanel), "InitSephirahSelectionButtonsInBattle", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UILibrarianCharacterListPanel_InitSephirahSelectionButtonsInBattle));
                HookHelper.CreateHook(typeof(UILibrarianCharacterListPanel), "SetLibrarianCharacterListPanel_Battle", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UILibrarianCharacterListPanel_SetLibrarianCharacterListPanel_Battle));
                HookHelper.CreateHook(typeof(UIInvitationRightMainPanel), "ConfirmSendInvitation", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UIInvitationRightMainPanel_ConfirmSendInvitation));
                HookHelper.CreateHook(typeof(UIInvenCardSlot), "SetSlotState", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UIInvenCardSlot_SetSlotState));
                HookHelper.CreateHook(typeof(UIInvenCardSlot), "OnClickCardEquipInfoButton", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UIInvenCardSlot_OnClickCardEquipInfoButton));
                HookHelper.CreateHook(typeof(UnitDataModel), "AddCardFromInventory", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UnitDataModel_AddCardFromInventory));
                HookHelper.CreateHook(typeof(UIInvenCardListScroll), "ApplyFilterAll", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UIInvenCardScrollList_ApplyFilterAll));
                HookHelper.CreateHook(typeof(BookModel), "AddCardFromInventoryToCurrentDeck", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.BookModel_AddCardFromInventory));
                HookHelper.CreateHook(typeof(UIInvenCardListScroll), "SetData", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UIInvenCardListScroll_SetData));
                HookHelper.CreateHook(typeof(DeckModel), "AddCardFromInventory", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.DeckModel_AddCardFromInventory));
                HookHelper.CreateHook(typeof(DeckModel), "MoveCardToInventory", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.DeckModel_MoveCardToInventory));
                HookHelper.CreateHook(typeof(BattleUnitCardsInHandUI), "SetCardsObject", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.BattleUnitCardsInHandUI_SetCardsObject));
                HookHelper.CreateHook(typeof(BattleDiceCardUI), "SetEgoCardForPopup", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.BattleDiceCardUI_SetEgoCardForPopup));
                HookHelper.CreateHook(typeof(UIBattleSettingLibrarianInfoPanel), "SetBattleCardSlotState", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UIBattleSettingLibrarianInfoPanel_SetBattleCardSlotState));
                HookHelper.CreateHook(typeof(UIBattleSettingLibrarianInfoPanel), "SetEquipPageSlotState", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UIBattleSettingLibrarianInfoPanel_SetEquipPageSlotState));
                HookHelper.CreateHook(typeof(UIBattleSettingEditPanel), "Open", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UIBattleSettingEditPanel_Open));
                HookHelper.CreateHook(typeof(BattleUnitEmotionDetail), "CreateEmotionCoin", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.BattleUnitEmotionDetail_CreateEmotionCoin));
                // TURNED INTO FINALIZER // HookHelper.CreateHook(typeof(BattleUnitEmotionDetail), "Reset", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.BattleUnitEmotionDetail_Reset));
                // UNUSED // HookHelper.CreateHook(typeof(WorkshopSkinDataSetter), "LateInit", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.WorkshopSkinDataSetter_LateInit));
                HookHelper.CreateHook(typeof(BookXmlInfo), "get_DeckId", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.BookXmlInfo_get_DeckId));
                HookHelper.CreateHook(typeof(UnitDataModel), "GetDeckForBattle", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UnitDataModel_GetDeckForBattle));
                HookHelper.CreateHook(typeof(BookModel), "IsFixedDeck", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.BookModel_IsFixedDeck));
                HookHelper.CreateHook(typeof(BookModel), "GetCardListFromCurrentDeck", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.BookModel_GetCardListFromCurrentDeck));

                HookHelper.CreateHook(typeof(BattleSceneRoot), "Update", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.BattleSceneRoot_Update));
                HookHelper.CreateHook(typeof(UIGetAbnormalityPanel), "PointerClickButton", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.UIGetAbnormalityPanel_PointerClickButton));
                HookHelper.CreateHook(typeof(AbnormalityCardDescXmlList), "GetAbnormalityCard", LogLikeMod.logLikeHooks, nameof(LogLikeMod.logLikeHooks.AbnormalityCardDescXmlList_GetAbnormalityCard));

                LogLikeMod.LogModAssemblys = new List<Assembly>();
                LogLikeMod.LoadSpineAssets();
                LogLikeMod.ArtWorks = new LogLikeMod.CacheDic<string, Sprite>(new LogLikeMod.CacheDic<string, Sprite>.getdele(LogLikeMod.GetArtWorks));
                LogLikeMod.LogUIObjs = new LogLikeMod.CacheDic<int, GameObject>(new LogLikeMod.CacheDic<int, GameObject>.getdele(LogLikeMod.GetLogUIObj));
                this.LoadStages();
                this.LoadMysteryInfos();
                this.LoadRewardPassiveInfos();
                this.LoadDropValues();
                this.LoadStoryPath();
                LogLikeMod.LoadStageInfos();
                LogLikeMod.LoadEnemyUnitInfos();
                LogLikeMod.LoadEquipPages();
                LogLikeMod.LoadCardDropTables();
                LogLikeMod.LoadDropBooks();
                LogLikeMod.LoadCardInfos();
                LogLikeMod.LoadDecks();
                LogLikeMod.LoadPassives();
                LogLikeMod.LoadTextData(ResolveInitialTextLanguage());
                LogLikeMod.spinemotions = new Dictionary<string, Dictionary<ActionDetail, Dictionary<GameObject, SkeletonAnimation>>>();
                FormationXmlRoot formationXmlRoot;
                using (StringReader stringReader = new StringReader(File.ReadAllText(LogLikeMod.path + "/AddData/FormationInfo.txt")))
                    formationXmlRoot = (FormationXmlRoot)new XmlSerializer(typeof(FormationXmlRoot)).Deserialize((TextReader)stringReader);
                ((List<FormationXmlInfo>)typeof(FormationXmlList).GetField("_list", AccessTools.all).GetValue(Singleton<FormationXmlList>.Instance)).AddRange((IEnumerable<FormationXmlInfo>)formationXmlRoot.list);
                LogLikeMod.CheckExceptionModList = new List<string>()
                {
                    "kr.heukhyeon.sayotoeveryone"
                };
                LogLikeMod.PreLoader preLoader = GlobalGameManager.Instance.gameObject.AddComponent<LogLikeMod.PreLoader>();
                preLoader.StartArtWorkPreLoad();
                preLoader.StartAssetBundlePreload();
                preLoader.StartUpgradeInfoPreload();
                preLoader.StartCreatureTabPreload();
                preLoader.StartSoundPreload();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message + Environment.NewLine + ex.StackTrace);
                Singleton<ModContentManager>.Instance.AddErrorLog($"LogLikeMod Init error : {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        public static PickUpModelBase FindPickUp(string script)
        {
            if (LogLikeMod.FindPickUpCache == null)
                LogLikeMod.FindPickUpCache = new Dictionary<string, System.Type>();
            if (LogLikeMod.FindPickUpCache.ContainsKey(script))
                return Activator.CreateInstance(LogLikeMod.FindPickUpCache[script]) as PickUpModelBase;
            foreach (Assembly assem in LogLikeMod.GetAssemList())
            {
                foreach (System.Type type in assem.GetTypes())
                {
                    if (type.Name == "PickUpModel_" + script.Trim())
                    {
                        LogLikeMod.FindPickUpCache[script] = type;
                        return Activator.CreateInstance(type) as PickUpModelBase;
                    }
                }
            }
            PickUpModelBase vanillaEmotion = PickUpModel_RMRVanillaEmotion.TryCreate(script);
            if (vanillaEmotion != null)
                return vanillaEmotion;
            return (PickUpModelBase)null;
        }

        public class PreLoader : MonoBehaviour
        {
            public bool LoadingArtWork;
            public Stack<System.Type> UpgradeInfos;
            public Stack<AssetBundle> AssetBundles;
            public Stack<KeyValuePair<string, string>> NameAndPathDic;

            public void StartSoundPreload()
            {
                this.StartCoroutine(Singleton<LogSoundEffectManager>.Instance.PreloadAudioClip());
            }

            public void StartCreatureTabPreload()
            {
                this.StartCoroutine(Singleton<LogCreatureTabPanel>.Instance.PreloadImages());
            }

            public void StartUpgradeInfoPreload()
            {
            }

            public void GetArtWorks(DirectoryInfo dir)
            {
                if (dir.GetDirectories().Length != 0)
                {
                    foreach (DirectoryInfo directory in dir.GetDirectories())
                        this.GetArtWorks(directory);
                }
                foreach (System.IO.FileInfo file in dir.GetFiles())
                    this.NameAndPathDic.Push(new KeyValuePair<string, string>(file.FullName, Path.GetFileNameWithoutExtension(file.FullName)));
            }

            public void StartArtWorkPreLoad()
            {
                DirectoryInfo dir = new DirectoryInfo(LogLikeMod.path + "/ArtWork");
                this.NameAndPathDic = new Stack<KeyValuePair<string, string>>();
                this.GetArtWorks(dir);
                this.Log($"Detect {this.NameAndPathDic.Count.ToString()} ArtWorks");
                this.Log("Start PreLoad ArtWork : " + DateTime.Now.ToString());
                this.StartCoroutine(this.ArtWorkPreLoading());
                DirectoryInfo directoryInfo = new DirectoryInfo(LogLikeMod.path + "/AssetBundle");
            }

            public void GetAssetBundles(DirectoryInfo dir)
            {
                if (dir.GetDirectories().Length != 0)
                {
                    foreach (DirectoryInfo directory in dir.GetDirectories())
                        this.GetAssetBundles(directory);
                }
                foreach (System.IO.FileInfo file in dir.GetFiles())
                {
                    AssetBundle assetBundle = AssetBundle.LoadFromFile(file.FullName);
                    assetBundle.name = Path.GetFileNameWithoutExtension(file.FullName);
                    this.AssetBundles.Push(assetBundle);
                }
            }

            public void StartAssetBundlePreload()
            {
                this.AssetBundles = new Stack<AssetBundle>();
                DirectoryInfo dir = new DirectoryInfo(LogLikeMod.path + "/AssetBundle");
                this.Log("Start Load AssetBundle : " + DateTime.Now.ToString());
                this.GetAssetBundles(dir);
                Singleton<LogAssetBundleManager>.Instance.SetBundles(this.AssetBundles.ToList<AssetBundle>());
                this.Log("End Load AssetBundle : " + DateTime.Now.ToString());
                this.StartCoroutine(this.AssetBundlePreLoading());
            }

            public IEnumerator AssetBundlePreLoading()
            {
                DateTime now = DateTime.Now;
                this.Log("Start PreLoad AssetBundle : " + now.ToString());
                while (this.AssetBundles.Count > 0)
                {
                    AssetBundle bundle = this.AssetBundles.Pop();
                    this.Log("LOADING BUNDLE -- " + bundle.name);
                    string[] strings = bundle.GetAllAssetNames();
                    string[] strArray = strings;
                    for (int index = 0; index < strArray.Length; ++index)
                    {
                        string name = strArray[index];
                        GameObject Gobj = bundle.LoadAsset<GameObject>(name);
                        if (Gobj != null)
                        {
                            this.Log("LOADING ASSET -- " + name + " // " + Gobj.name);
                            LogAssetBundleManager.GameObjectBundleCache cache = new LogAssetBundleManager.GameObjectBundleCache()
                            {
                                BundleName = bundle.name,
                                objname = Gobj.name,
                                obj = Gobj
                            };
                            Singleton<LogAssetBundleManager>.Instance.GObjList.Add(cache);
                            cache = (LogAssetBundleManager.GameObjectBundleCache)null;
                        }
                        yield return new WaitForEndOfFrame();
                        Gobj = (GameObject)null;
                        name = (string)null;
                    }
                    strArray = (string[])null;
                    bundle = (AssetBundle)null;
                    strings = (string[])null;
                }
                now = DateTime.Now;
                this.Log("End PreLoad AssetBundle : " + now.ToString());
            }

            public IEnumerator ArtWorkPreLoading()
            {
                while (this.NameAndPathDic.Count > 0)
                {
                    KeyValuePair<string, string> nam = this.NameAndPathDic.Pop();
                    Texture2D texture2D = new Texture2D(2, 2);
                    byte[] bytes = File.ReadAllBytes(nam.Key);
                    texture2D.LoadImage(bytes);
                    Sprite value = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.0f, 0.0f), 100f, 0U, SpriteMeshType.FullRect);
                    LogLikeMod.ArtWorks.dic[nam.Key] = value;
                    yield return new WaitForEndOfFrame();
                    nam = new KeyValuePair<string, string>();
                    texture2D = (Texture2D)null;
                    bytes = (byte[])null;
                    value = (Sprite)null;
                }
                this.Log("End PreLoad ArtWork : " + DateTime.Now.ToString());
            }
        }

        public class UIActiveChecker : MonoBehaviour
        {
            public void Update()
            {
                int childCount = this.gameObject.transform.childCount;
                this.gameObject.GetComponent<GraphicRaycaster>();
                for (int index = 0; index < childCount; ++index)
                {
                    if (this.gameObject.transform.GetChild(index).gameObject.activeSelf)
                    {
                        this.gameObject.SetActive(true);
                        return;
                    }
                }
                this.gameObject.SetActive(false);
            }
        }

        public class LoadList
        {
            [XmlElement("Dll")]
            public List<string> ReadDll;
        }

        public class ModLoader
        {
            public static void OnInitializeMod(string path, string modid)
            {
                LogLikeMod.LoadList loadList = (LogLikeMod.LoadList)null;
                if (!File.Exists(path + "/AssemList.xml"))
                    return;
                using (StreamReader streamReader = new StreamReader(path + "/AssemList.xml"))
                    loadList = new XmlSerializer(typeof(LogLikeMod.LoadList)).Deserialize((TextReader)streamReader) as LogLikeMod.LoadList;
                List<string> stringList = new List<string>();
                foreach (string str in loadList.ReadDll)
                {
                    if (File.Exists($"{path}/Roguedlls/{str}.dll"))
                        stringList.Add($"{path}/Roguedlls/{str}.dll");
                }
                Dictionary<string, BattleCardAbilityDesc> abilityText;
                LogLikeMod.ModLoader.LoadAllAssembly(modid + "<Loader>", stringList.ToArray(), out abilityText);
                Singleton<BattleCardAbilityDescXmlList>.Instance.AddByMode(modid, abilityText);
            }

            public static void LoadTypesFromAssembly(Assembly assembly)
            {
                foreach (System.Type type in assembly.GetTypes())
                {
                    string name = type.Name;
                    if (type.IsSubclassOf(typeof(DiceCardSelfAbilityBase)) && name.StartsWith("DiceCardSelfAbility_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>(Singleton<AssemblyManager>.Instance, "_diceCardSelfAbilityDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
             name.Substring("DiceCardSelfAbility_".Length),
             type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(DiceCardAbilityBase)) && name.StartsWith("DiceCardAbility_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>(Singleton<AssemblyManager>.Instance, "_diceCardAbilityDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
             name.Substring("DiceCardAbility_".Length),
             type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(BehaviourActionBase)) && name.StartsWith("BehaviourAction_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>(Singleton<AssemblyManager>.Instance, "_behaviourActionDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
             name.Substring("BehaviourAction_".Length),
             type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(PassiveAbilityBase)) && name.StartsWith("PassiveAbility_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>(Singleton<AssemblyManager>.Instance, "_passiveAbilityDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
             name.Substring("PassiveAbility_".Length),
             type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(DiceCardPriorityBase)) && name.StartsWith("DiceCardPriority_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>(Singleton<AssemblyManager>.Instance, "_diceCardPriorityDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
             name.Substring("DiceCardPriority_".Length),
             type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(EnemyUnitAggroSetter)) && name.StartsWith("EnemyUnitAggroSetter_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>(Singleton<AssemblyManager>.Instance, "_enemyUnitAggroSetterDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
             name.Substring("EnemyUnitAggroSetter_".Length),
             type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(EnemyTeamStageManager)) && name.StartsWith("EnemyTeamStageManager_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>(Singleton<AssemblyManager>.Instance, "_enemyTeamStageManagerDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
             name.Substring("EnemyTeamStageManager_".Length),
             type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(EnemyUnitTargetSetter)) && name.StartsWith("EnemyUnitTargetSetter_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>(Singleton<AssemblyManager>.Instance, "_enemyUnitTargetSetterDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
             name.Substring("EnemyUnitTargetSetter_".Length),
             type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(ModInitializer)))
                        (Activator.CreateInstance(type) as ModInitializer).OnInitializeMod();
                }
            }

            public static void LoadAllAssembly(
              string uid,
              string[] filenames,
              out Dictionary<string, BattleCardAbilityDesc> abilityText)
            {
                Dictionary<string, List<Assembly>> fieldValue = LogLikeMod.GetFieldValue<Dictionary<string, List<Assembly>>>(Singleton<AssemblyManager>.Instance, "_assemblyDict");
                abilityText = new Dictionary<string, BattleCardAbilityDesc>();
                List<Assembly> collection = new List<Assembly>();
                for (int index = 0; index < filenames.Length; ++index)
                {
                    string filename = filenames[index];
                    try
                    {
                        List<Assembly> assemblyList = new List<Assembly>((IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies());
                        Assembly assembly = Assembly.LoadFile(filename);
                        if (assembly != (Assembly)null)
                        {
                            if (!assemblyList.Contains(assembly))
                            {
                                foreach (System.Type type in assembly.GetTypes())
                                {
                                    string key = type.Name;
                                    bool flag = false;
                                    if (type.IsSubclassOf(typeof(DiceCardAbilityBase)))
                                    {
                                        if (key.StartsWith("DiceCardAbility_"))
                                            key = key.Substring("DiceCardAbility_".Length);
                                        flag = true;
                                    }
                                    if (type.IsSubclassOf(typeof(DiceCardSelfAbilityBase)))
                                    {
                                        if (key.StartsWith("DiceCardSelfAbility_"))
                                            key = key.Substring("DiceCardSelfAbility_".Length);
                                        flag = true;
                                    }
                                    if (flag)
                                    {
                                        FieldInfo field = type.GetField("Desc");
                                        if (field != (FieldInfo)null)
                                        {
                                            string str = field.GetValue(null) as string;
                                            if (!string.IsNullOrEmpty(str) && !abilityText.ContainsKey(key))
                                                abilityText.Add(key, new BattleCardAbilityDesc()
                                                {
                                                    id = key,
                                                    desc = new List<string>() { str }
                                                });
                                        }
                                    }
                                }
                                collection.Add(assembly);
                                LogLikeMod.LogModAssemblys.Add(assembly);
                            }
                        }
                        else
                            typeof(LogLikeMod.ModLoader).Log("load fail : " + filename);
                    }
                    catch (Exception ex)
                    {
                        typeof(LogLikeMod.ModLoader).LogError(ex);
                    }
                }
                foreach (Assembly assembly in collection)
                    LogLikeMod.ModLoader.LoadTypesFromAssembly(assembly);
                if (collection.Count <= 0)
                    return;
                if (!fieldValue.ContainsKey(uid))
                    fieldValue.Add(uid, collection);
                else
                    fieldValue[uid].AddRange((IEnumerable<Assembly>)collection);
            }
        }

        public class CacheDic<Tkey, TValue>
        {
            public LogLikeMod.CacheDic<Tkey, TValue>.getdele del;
            public Dictionary<Tkey, TValue> dic;

            public bool ContainsKey(Tkey key)
            {
                if (this.dic.ContainsKey(key))
                    return true;
                TValue obj = this.del(key);
                if (obj == null)
                    return false;
                this.dic[key] = obj;
                return true;
            }

            public CacheDic(LogLikeMod.CacheDic<Tkey, TValue>.getdele del)
            {
                this.dic = new Dictionary<Tkey, TValue>();
                this.del = del;
            }

            public TValue this[Tkey key]
            {
                get
                {
                    if (this.dic.ContainsKey(key))
                    {
                        if (this.dic[key] is GameObject)
                            (this.dic[key] as GameObject).SetActive(true);
                        return this.dic[key];
                    }
                    this.dic[key] = this.del(key);
                    return this.dic[key];
                }
                set => this.dic[key] = value;
            }

            public void PreLoading(Tkey key) => this.dic[key] = this.del(key);

            public delegate TValue getdele(Tkey key);
        }

        public class LogUIBookSlot : MonoBehaviour
        {
            public CanvasGroup cg;
            public UICustomSelectable selectable;
            public Image Frame;
            public Image FrameGlow;
            public Image Icon;
            public Image IconGlow;
            public TextMeshProUGUI BookName;
            public int originSiblingIdx = -1;
            public int stringAscendidx = -1;
            public LorId _bookId = LorId.None;
            public DropBookXmlInfo bookInfo;
            public bool isDisabled;
            public GameObject bookNumRoot;
            public Image bookNumBg;
            public TextMeshProUGUI txt_bookNum;
            public GameObject ob_tutorialhighlight;

            public static LogLikeMod.LogUIBookSlot SlotCopying()
            {
                UIInvitationDropBookSlot bookSlot = (UI.UIController.Instance.GetUIPanel(UIPanelType.Invitation) as UIInvitationPanel).InvCenterBookListPanel.BookSlotList[0] as UIInvitationDropBookSlot;
                LogLikeMod.LogUIBookSlot original = bookSlot.gameObject.AddComponent<LogLikeMod.LogUIBookSlot>();
                original.cg = (CanvasGroup)typeof(UIBookSlot).GetField("cg", AccessTools.all).GetValue(bookSlot);
                original.selectable = (UICustomSelectable)typeof(UIBookSlot).GetField("selectable", AccessTools.all).GetValue(bookSlot);
                original.Frame = (Image)typeof(UIBookSlot).GetField("Frame", AccessTools.all).GetValue(bookSlot);
                original.FrameGlow = (Image)typeof(UIBookSlot).GetField("FrameGlow", AccessTools.all).GetValue(bookSlot);
                original.Icon = (Image)typeof(UIBookSlot).GetField("Icon", AccessTools.all).GetValue(bookSlot);
                original.IconGlow = (Image)typeof(UIBookSlot).GetField("IconGlow", AccessTools.all).GetValue(bookSlot);
                original.BookName = (TextMeshProUGUI)typeof(UIBookSlot).GetField("BookName", AccessTools.all).GetValue(bookSlot);
                original.bookNumRoot = (GameObject)typeof(UIInvitationDropBookSlot).GetField("bookNumRoot", AccessTools.all).GetValue(bookSlot);
                original.bookNumBg = (Image)typeof(UIInvitationDropBookSlot).GetField("bookNumBg", AccessTools.all).GetValue(bookSlot);
                original.txt_bookNum = (TextMeshProUGUI)typeof(UIInvitationDropBookSlot).GetField("txt_bookNum", AccessTools.all).GetValue(bookSlot);
                original.ob_tutorialhighlight = (GameObject)typeof(UIInvitationDropBookSlot).GetField("ob_tutorialhighlight", AccessTools.all).GetValue(bookSlot);
                LogLikeMod.LogUIBookSlot logUiBookSlot = UnityEngine.Object.Instantiate<LogLikeMod.LogUIBookSlot>(original);
                UnityEngine.Object.Destroy(logUiBookSlot.gameObject.GetComponent<UIInvitationDropBookSlot>());
                UnityEngine.Object.Destroy(original);
                logUiBookSlot.selectable.SubmitEvent.RemoveAllListeners();
                logUiBookSlot.selectable.SubmitEvent.AddListener(new UnityAction<BaseEventData>(logUiBookSlot.OnPointerClick));
                logUiBookSlot.selectable.SelectEvent.RemoveAllListeners();
                logUiBookSlot.selectable.SelectEvent.AddListener(new UnityAction<BaseEventData>(logUiBookSlot.OnPointerEnter));
                logUiBookSlot.selectable.DeselectEvent.RemoveAllListeners();
                logUiBookSlot.selectable.DeselectEvent.AddListener(new UnityAction<BaseEventData>(logUiBookSlot.OnPointerExit));
                logUiBookSlot.selectable.XEvent.RemoveAllListeners();
                return logUiBookSlot;
            }

            public void Initialized()
            {
                this._bookId = LorId.None;
                this.bookInfo = (DropBookXmlInfo)null;
                this.isDisabled = false;
            }

            public void SetData_DropBook(LorId bookId, int value)
            {
                if (!this.gameObject.activeSelf)
                    this.gameObject.SetActive(true);
                this.isDisabled = false;
                this._bookId = bookId;
                this.bookInfo = Singleton<DropBookXmlList>.Instance.GetData(this._bookId);
                if (this.bookInfo == null)
                {
                    this.SetActivatedSlot(false);
                    Debug.LogError("dropbook Info null Error");
                }
                else
                {
                    this.SetActivatedSlot(true);
                    if (this.BookName != null)
                        this.BookName.text = RewardingModel.SanitizeDisplayText(this.bookInfo.Name);
                    if (this.Icon != null)
                        this.Icon.sprite = this.bookInfo.bookIcon;
                    if (this.IconGlow != null)
                        this.IconGlow.sprite = this.bookInfo.bookIconGlow;
                    this.SetHighlighted(false);
                    if (!(this.bookNumRoot != null))
                        return;
                    if (!this.bookNumRoot.activeSelf)
                        this.bookNumRoot.SetActive(true);
                    this.txt_bookNum.text = "x" + value.ToString();
                }
            }

            public void SetEmptyViewSlot()
            {
                if (this.BookName != null)
                    this.BookName.text = TextDataModel.GetText("ui_book_bookname_emptybook");
                if (this.Icon != null)
                {
                    Image icon = this.Icon;
                    UISpriteDataManager instance = UISpriteDataManager.instance;
                    icon.sprite = instance != null ? instance.GetStoryIcon("None").icon : (Sprite)null;
                }
                if (this.IconGlow != null)
                {
                    Image iconGlow = this.IconGlow;
                    UISpriteDataManager instance = UISpriteDataManager.instance;
                    iconGlow.sprite = instance != null ? instance.GetStoryIcon("None").iconGlow : (Sprite)null;
                }
                this.SetDisabled(true);
                GameObject bookNumRoot = this.bookNumRoot;
                if (bookNumRoot != null)
                    bookNumRoot.SetActive(false);
                if (!this.ob_tutorialhighlight.activeSelf)
                    return;
                this.ob_tutorialhighlight.SetActive(false);
            }

            public void SetAleadyHighlighted()
            {
                this.SetGlowColor(UIColorManager.Manager.GetUIColor(UIColor.Highlighted));
                this.SetColor(UIColorManager.Manager.GetUIColor(UIColor.Default));
                if (!this.ob_tutorialhighlight.activeSelf)
                    return;
                this.ob_tutorialhighlight.SetActive(false);
            }

            public void SetHighlighted(bool on)
            {
                Color c = on ? (this.isDisabled ? UIColorManager.Manager.GetUIColor(UIColor.Disabled) : UIColorManager.Manager.GetUIColor(UIColor.Highlighted)) : UIColorManager.Manager.GetUIColor(UIColor.Default);
                this.SetColor(c);
                this.SetGlowColor(c);
                if (!on || !this.ob_tutorialhighlight.activeSelf)
                    return;
                this.ob_tutorialhighlight.SetActive(false);
            }

            public void SetColor(Color c)
            {
                this.Frame.color = c;
                this.BookName.color = c;
                this.Icon.color = Color.white;
                this.bookNumBg.color = c;
                this.txt_bookNum.color = c;
            }

            public void OnPointerEnter(BaseEventData eventData)
            {
                if (this.isDisabled)
                    return;
                UISoundManager.instance.PlayEffectSound(UISoundType.Ui_BookOver);
                this.SetHighlighted(true);
            }

            public void OnPointerExit(BaseEventData eventData)
            {
                if (this.isDisabled)
                    return;
                this.SetHighlighted(false);
            }

            public void OnPointerClick(BaseEventData eventData)
            {
                if (this.isDisabled)
                    UISoundManager.instance.PlayEffectSound(UISoundType.Card_Lock);
                else
                    UISoundManager.instance.PlayEffectSound(UISoundType.Ui_EnemyButton);
            }

            public void OnScroll(BaseEventData eventData)
            {
            }

            public void OnXEvent()
            {
            }

            public void OnCancel()
            {
            }

            public DropBookXmlInfo DropBookInfo => this.bookInfo;

            public LorId BookId => this._bookId;

            public void SetActivatedSlot(bool on)
            {
                if (on)
                {
                    this.cg.alpha = 1f;
                    this.cg.interactable = true;
                    this.cg.blocksRaycasts = true;
                    if (!(this.selectable != null))
                        return;
                    this.selectable.interactable = true;
                }
                else
                {
                    this.cg.alpha = 0.0f;
                    this.cg.interactable = false;
                    this.cg.blocksRaycasts = false;
                    if (this.selectable != null)
                        this.selectable.interactable = false;
                    this._bookId = LorId.None;
                    this.bookInfo = (DropBookXmlInfo)null;
                }
            }

            public bool GetActiveState() => (double)this.cg.alpha == 1.0;

            public void SetGlowColor(Color c)
            {
                this.FrameGlow.color = c;
                if (this.IconGlow != null)
                    this.IconGlow.color = c;
                TextMeshProMaterialSetter component = this.BookName.GetComponent<TextMeshProMaterialSetter>();
                if (component != null)
                    component.underlayColor = c;
                component.enabled = false;
                component.enabled = true;
            }

            public void SetDisabled(bool on)
            {
                this.isDisabled = on;
                if (this.isDisabled)
                {
                    this.SetColor(UIColorManager.Manager.GetUIColor(UIColor.Disabled));
                    this.SetGlowColor(UIColorManager.Manager.GetUIColor(UIColor.Disabled));
                    this.Icon.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                }
                else
                {
                    this.SetColor(UIColorManager.Manager.GetUIColor(UIColor.Default));
                    this.SetGlowColor(UIColorManager.Manager.GetUIColor(UIColor.Default));
                }
            }
        }

        public class LogUISettingInvenEquipPageSlot : UIOriginEquipPageSlot
        {
            [Header("---Child---")]
            [SerializeField]
            public UISettingEquipPageScrollList listRoot;
            [Header("Equip Character Sprite")]
            [SerializeField]
            public Image img_equipFrame;
            [SerializeField]
            public GameObject ob_equipRoot;
            [SerializeField]
            public CanvasGroup cg_equiproot;
            [SerializeField]
            public FaceEditor faceEditor;
            [Header("Operating Panel")]
            [SerializeField]
            public GameObject ob_OperatingPanel;
            [SerializeField]
            public CanvasGroup cg_operatingPanel;
            [Header("BookMark Button")]
            [SerializeField]
            public UICustomGraphicObject button_BookMark;
            [SerializeField]
            public TextMeshProUGUI txt_bookmarkButton;
            [SerializeField]
            public Image img_bookmarkbuttonIcon;
            [Header("Equip Button")]
            [SerializeField]
            public UICustomGraphicObject button_Equip;
            [SerializeField]
            public TextMeshProUGUI txt_equipButton;
            [SerializeField]
            public Image img_equipbuttonIcon;
            [SerializeField]
            public UICustomGraphicObject button_EmptyDeck;
            [Header("Block Frame")]
            [SerializeField]
            public GameObject ob_blockFrame;
            [SerializeField]
            public Image img_SepIcon;
            public bool isBlock;

            public override void Initialized()
            {
                base.Initialized();
                this.SetActiveOperatinPanel(false);
                if (!(this.selectable == null))
                    return;
                this.selectable = this.GetComponent<UICustomSelectable>();
            }

            public override void SetActiveSlot(bool on)
            {
                base.SetActiveSlot(on);
                if (on)
                    return;
                this.SetActiveOperatinPanel(false);
            }

            public override void SetData(BookModel book)
            {
                base.SetData(book);
                this.SetActiveSlot(true);
                this.cg_equiproot.alpha = 0.0f;
                if (this._bookDataModel.owner != null)
                {
                    if (this._bookDataModel != UI.UIController.Instance.CurrentUnit.bookItem)
                    {
                        if (this.img_equipFrame != null && this.faceEditor != null)
                        {
                            if (book.owner.isSephirah)
                                this.faceEditor.InitBySephirah(book.owner.defaultBook.GetBookClassInfoId());
                            else
                                this.faceEditor.Init(book.owner.customizeData);
                            this.cg_equiproot.alpha = 1f;
                            this.SetColorFrame(UIEquipPageSlotState.OtherEquiped);
                        }
                    }
                    else
                        this.SetColorFrame(UIEquipPageSlotState.None);
                }
                else if (!this._bookDataModel.CanEquipBookByGivePassive())
                    this.SetColorFrame(UIEquipPageSlotState.SuccessionMatter);
                this.SetActiveOperatinPanel(false);
                this.SetOperatingPanel();
                if (this.ob_blockFrame != null)
                {
                    if (this.ob_blockFrame.activeSelf)
                        this.ob_blockFrame.gameObject.SetActive(false);
                    this.isBlock = false;
                }
                if (LibraryModel.Instance.PlayHistory.Start_TheBlueReverberationPrimaryBattle != 1 || !(this.ob_blockFrame != null))
                    return;
                SephirahType index = this._bookDataModel.IsCanUsingEquipPageWhenBlueReverberation();
                if (index == SephirahType.None)
                {
                    if (this.ob_blockFrame.activeSelf)
                        this.ob_blockFrame.gameObject.SetActive(false);
                }
                else
                {
                    if (!this.ob_blockFrame.activeSelf)
                        this.ob_blockFrame.gameObject.SetActive(true);
                    this.img_SepIcon.sprite = UISpriteDataManager.instance._floorIconSet[(int)index].icon;
                    this.isBlock = true;
                }
            }

            public override void SetActiveOperatinPanel(bool on)
            {
                if (!(this.ob_OperatingPanel != null))
                    return;
                if (!this.ob_OperatingPanel.gameObject.activeSelf)
                    this.ob_OperatingPanel.gameObject.SetActive(true);
                if (this.cg_operatingPanel == null)
                    return;
                this.cg_operatingPanel.alpha = on ? 1f : 0.0f;
                this.cg_operatingPanel.blocksRaycasts = on;
                this.cg_operatingPanel.interactable = on;
            }

            public void SetOperatingPanel()
            {
                if (this.ob_OperatingPanel == null)
                    return;
                if (this._bookDataModel == null)
                {
                    this.SetActiveOperatinPanel(false);
                }
                else
                {
                    this.SetActiveOperatinPanel(true);
                    this.button_Equip.interactable = this._bookDataModel.CanEquipBookByGivePassive();
                    string id = this._bookDataModel.owner == null ? "ui_bookinventory_equipbook" : "ui_book_bookname_unequip";
                    this.img_equipbuttonIcon.sprite = this._bookDataModel.owner == null ? UISpriteDataManager.instance.EquipIcon[0] : UISpriteDataManager.instance.EquipIcon[1];
                    this.img_equipbuttonIcon.rectTransform.anchoredPosition = this._bookDataModel.owner == null ? new Vector2(0.0f, -5f) : Vector2.zero;
                    this.SetActiveOperatinPanel(false);
                    if (!this.button_EmptyDeck.gameObject.activeSelf)
                        this.button_EmptyDeck.gameObject.SetActive(true);
                    this.button_EmptyDeck.interactable = !this._bookDataModel.IsEmptyDeckAll();
                    if (this._bookDataModel.owner != null)
                    {
                        if (Singleton<StageController>.Instance.GetStageModel().IsUsedSephirah(this._bookDataModel.owner.OwnerSephirah))
                        {
                            id = "ui_cannotbemodifiedequippage";
                            this.button_Equip.interactable = false;
                            this.button_EmptyDeck.interactable = false;
                        }
                    }
                    else
                    {
                        UnitDataModel currentUnit = UI.UIController.Instance.CurrentUnit;
                        if (currentUnit != null)
                        {
                            if (this._bookDataModel.ClassInfo.id == 250022)
                            {
                                id = currentUnit.IsGebura() ? "ui_bookinventory_equipbook" : "ui_equippage_notequip";
                                this.button_Equip.interactable = currentUnit.IsGebura();
                            }
                            else if (currentUnit.IsChangeItemLock())
                            {
                                id = "ui_equippage_notequip";
                                this.button_Equip.interactable = false;
                            }
                        }
                    }
                    if (LibraryModel.Instance.PlayHistory.Start_TheBlueReverberationPrimaryBattle == 1 && LibraryModel.Instance.IsClearTheBlueReverberationPrimary(UI.UIController.Instance.CurrentUnit.OwnerSephirah))
                    {
                        id = "ui_equippage_notequip";
                        this.button_Equip.interactable = false;
                    }
                    this.img_equipbuttonIcon.color = this.button_Equip.interactable ? Color.white : UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                    this.txt_equipButton.text = TextDataModel.GetText(id);
                    if (!this.button_BookMark.gameObject.activeSelf)
                        this.button_BookMark.gameObject.SetActive(true);
                    this.txt_bookmarkButton.text = TextDataModel.GetText(this._bookDataModel.GetBookMarkState() ? "ui_equippageinventory_releasebookmark" : "ui_equippageinventory_addbookmark");
                    this.img_bookmarkbuttonIcon.color = this._bookDataModel.GetBookMarkState() ? UIColorManager.Manager.GetUIColor(UIColor.Highlighted) : UIColorManager.Manager.GetUIColor(UIColor.Default);
                }
            }

            public override void SetEmptySlot()
            {
                this.SetActiveOperatinPanel(false);
                this.cg_equiproot.alpha = 0.0f;
                base.SetEmptySlot();
                if (!(this.ob_blockFrame != null))
                    return;
                if (this.ob_blockFrame.activeSelf)
                    this.ob_blockFrame.gameObject.SetActive(false);
                this.selectable.interactable = true;
            }

            public void OnClickEquipButton()
            {
                if (!this._bookDataModel.CanEquipBookByGivePassive())
                    Debug.LogError("귀속된 책장입니다.");
                else if (this._bookDataModel.owner != null && Singleton<StageController>.Instance.GetStageModel().IsUsedSephirah(this._bookDataModel.owner.OwnerSephirah))
                {
                    Debug.LogError("중고층에서 장착중인 책장입니다.");
                }
                else
                {
                    if (this._bookDataModel.owner == null)
                    {
                        UnitDataModel currentUnit = UI.UIController.Instance.CurrentUnit;
                        BookModel bookItem = currentUnit.bookItem;
                        BookModel bookDataModel = this._bookDataModel;
                        if (bookDataModel.ClassInfo.canNotEquip || !bookDataModel.CanEquipBookByGivePassive())
                        {
                            Debug.LogError("장착 불가 책장");
                            return;
                        }
                        bool flag;
                        if (bookDataModel == bookItem)
                        {
                            flag = UI.UIController.Instance.CurrentUnit.EquipBookForUI((BookModel)null);
                        }
                        else
                        {
                            flag = UI.UIController.Instance.CurrentUnit.EquipBookForUI(bookDataModel);
                            this.GetEquipInvenPanel().isSaveCheck = false;
                            currentUnit.appearanceType = Gender.N;
                        }
                        if (flag)
                            UIAlarmPopup.instance.SetAlarmText_unused(UIAlarmType.DeckSizeReduced);
                        UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click);
                        currentUnit.appearanceType = Gender.N;
                        this.SetOperatingPanel();
                        SingletonBehavior<UICharacterRenderer>.Instance.ReloadCharacter(UI.UIController.Instance.CurrentUnit);
                        this.GetEquipInvenPanel().ChangeEquipBook(UI.UIController.Instance.CurrentUnit);
                    }
                    else
                    {
                        UnitDataModel owner = this._bookDataModel.owner;
                        owner.EquipBookForUI((BookModel)null);
                        UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click);
                        this.GetEquipInvenPanel().EquipLeftPanel.EquipPageList.ReleaseSelectedSlot(this._bookDataModel);
                        owner.appearanceType = Gender.N;
                        this.SetOperatingPanel();
                        this.GetEquipInvenPanel().isSaveCheck = false;
                        SingletonBehavior<UICharacterRenderer>.Instance.ReloadCharacter(owner);
                        this.GetEquipInvenPanel().ChangeEquipBook(owner);
                    }
                    if (this.selectable.interactable)
                    {
                        UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                    }
                    else
                    {
                        UICustomSelectable component = this.selectable.FindSelectableOnLeft().GetComponent<UICustomSelectable>();
                        if (component == null)
                            UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                        else
                            UIControlManager.Instance.SelectSelectableForcely(component);
                    }
                }
            }

            public void OnClickEmptyDeckButton()
            {
                if (this._bookDataModel == null)
                    return;
                if (this._bookDataModel.owner != null && Singleton<StageController>.Instance.GetStageModel().IsUsedSephirah(this._bookDataModel.owner.OwnerSephirah))
                {
                    Debug.LogError("중고층에서 장착중인 책장입니다.");
                }
                else
                {
                    this._bookDataModel.EmptyDeckToInventoryAll();
                    this.GetEquipInvenPanel().isSaveCheck = false;
                    this.GetEquipInvenPanel().UpdateRightPanel();
                    this.GetEquipInvenPanel().ReleaseSelectedSlot();
                    this.SetOperatingPanel();
                    if (this.selectable.interactable)
                    {
                        UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                    }
                    else
                    {
                        UICustomSelectable component = this.selectable.FindSelectableOnLeft().GetComponent<UICustomSelectable>();
                        if (component == null)
                            UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                        else
                            UIControlManager.Instance.SelectSelectableForcely(component);
                    }
                }
            }

            public void OnClickBookMarkButton()
            {
                if (this._bookDataModel == null)
                    return;
                this._bookDataModel.ResisterBookMark(!this._bookDataModel.GetBookMarkState());
                this.GetEquipInvenPanel().EquipLeftPanel.EquipPageList.ReleaseSelectedSlot(this._bookDataModel);
                this.GetEquipInvenPanel().isSaveCheck = false;
                this.GetEquipInvenPanel().UpdateLeftPanel();
                this.GetEquipInvenPanel().UpdateCenterPanel();
                this.GetEquipInvenPanel().ReleaseSelectedSlot();
                if (this.selectable.interactable)
                {
                    UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                }
                else
                {
                    UICustomSelectable component = this.selectable.FindSelectableOnLeft().GetComponent<UICustomSelectable>();
                    if (component == null)
                        UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                    else
                        UIControlManager.Instance.SelectSelectableForcely(component);
                }
            }

            public void OnPointerEnter(BaseEventData eventData)
            {
                if (this.isEmptyBook)
                    return;
                UISoundManager.instance.PlayEffectSound(UISoundType.Card_Over);
                if (UIControlManager.GetInpuTypeOf(eventData) == UI.InputType.ControllerInput)
                {
                    this.listRoot.SetSaveFirstChild((UIOriginEquipPageSlot)this);
                    this.listRoot.CheckSelectSlotMove((UIOriginEquipPageSlot)this);
                    this.SetHighlighted(true, this.listRoot.CurrentSelectedBook == this._bookDataModel, true);
                }
                else
                    this.SetHighlighted(true, this.listRoot.CurrentSelectedBook == this._bookDataModel);
                if (!(this.GetEquipInvenPanel().CurrentSelectedSlot == null))
                    return;
                this.GetEquipInvenPanel().currentOverSlot = (UIOriginEquipPageSlot)this;
                this.GetEquipInvenPanel().ShowPreviewPanel((UIOriginEquipPageSlot)this);
            }

            public void OnPointerExit(BaseEventData eventData)
            {
                if (this.GetEquipInvenPanel().CurrentSelectedSlot == null)
                {
                    this.GetEquipInvenPanel().currentOverSlot = (UIOriginEquipPageSlot)null;
                    this.GetEquipInvenPanel().HidePreviewPanel();
                }
                if (this.isEmptyBook)
                    return;
                if (this.listRoot.CurrentSelectedBook == this._bookDataModel)
                    this.SetOffPadSelectHighlighted();
                else
                    this.SetHighlighted(false);
            }

            public void OnPointerClick(BaseEventData data)
            {
                if (this.isEmptyBook || this.isBlock || UIControlManager.GetInpuTypeOf(data) == UI.InputType.RightClick)
                    return;
                this.GetEquipInvenPanel().OnClickSlot((UIOriginEquipPageSlot)this);
                UIControlManager.Instance.SelectSelectableForcely(this.button_Equip.interactable ? this.button_Equip.selectable : this.button_BookMark.selectable);
            }

            public void OnCancelOperating()
            {
                this.SetActiveOperatinPanel(false);
                UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                this.GetEquipInvenPanel().ReleaseSelectedSlotNotHidePreview();
            }

            public void OnScroll(BaseEventData eventData)
            {
                this.listRoot.OnScroll(eventData as PointerEventData);
            }

            public void OnXEvent() => this.listRoot.ChangeSelectableToFilter();

            public void OnYEvent()
            {
                this.GetEquipInvenPanel().SwitchPreviewVisible((UIOriginEquipPageSlot)this);
            }

            public UISettingEquipPageInvenPanel GetEquipInvenPanel()
            {
                return (UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel).EditPanel.EquipPagePanel;
            }
        }

        public class LogBattleEmotionRewardSlotUI : MonoBehaviour
        {
            public BattleEmotionRewardInfoUI panel;
            public RectTransform rect;
            public RectTransform rect_frame;
            public RectTransform rect_bg;
            public Image img_emotionlevel;
            public TextMeshProUGUI txt_Name;
            public List<TextMeshProUGUI> rewardtexts;

            public static BattleEmotionRewardSlotUI BattleEmotionRewardSlotUI_Copying(
              BattleEmotionRewardSlotUI baseobj)
            {
                LogLikeMod.LogBattleEmotionRewardSlotUI original = baseobj.gameObject.AddComponent<LogLikeMod.LogBattleEmotionRewardSlotUI>();
                original.panel = LogLikeMod.GetFieldValue<BattleEmotionRewardInfoUI>(baseobj, "panel");
                original.rect = LogLikeMod.GetFieldValue<RectTransform>(baseobj, "rect");
                original.rect_frame = LogLikeMod.GetFieldValue<RectTransform>(baseobj, "rect_frame");
                original.rect_bg = LogLikeMod.GetFieldValue<RectTransform>(baseobj, "rect_bg");
                original.img_emotionlevel = LogLikeMod.GetFieldValue<Image>(baseobj, "img_emotionlevel");
                original.txt_Name = LogLikeMod.GetFieldValue<TextMeshProUGUI>(baseobj, "txt_Name");
                original.rewardtexts = LogLikeMod.GetFieldValue<List<TextMeshProUGUI>>(baseobj, "rewardtexts");
                LogLikeMod.LogBattleEmotionRewardSlotUI emotionRewardSlotUi1 = UnityEngine.Object.Instantiate<LogLikeMod.LogBattleEmotionRewardSlotUI>(original, original.transform.parent);
                BattleEmotionRewardSlotUI emotionRewardSlotUi2 = emotionRewardSlotUi1.GetComponent<BattleEmotionRewardSlotUI>() == null ? emotionRewardSlotUi1.gameObject.AddComponent<BattleEmotionRewardSlotUI>() : emotionRewardSlotUi1.GetComponent<BattleEmotionRewardSlotUI>();
                LogLikeMod.SetFieldValue(emotionRewardSlotUi2, "panel", emotionRewardSlotUi1.panel);
                LogLikeMod.SetFieldValue(emotionRewardSlotUi2, "rect", emotionRewardSlotUi1.rect);
                LogLikeMod.SetFieldValue(emotionRewardSlotUi2, "rect_frame", emotionRewardSlotUi1.rect_frame);
                LogLikeMod.SetFieldValue(emotionRewardSlotUi2, "rect_bg", emotionRewardSlotUi1.rect_bg);
                LogLikeMod.SetFieldValue(emotionRewardSlotUi2, "img_emotionlevel", emotionRewardSlotUi1.img_emotionlevel);
                LogLikeMod.SetFieldValue(emotionRewardSlotUi2, "txt_Name", emotionRewardSlotUi1.txt_Name);
                LogLikeMod.SetFieldValue(emotionRewardSlotUi2, "rewardtexts", emotionRewardSlotUi1.rewardtexts);
                UnityEngine.Object.Destroy(original);
                UnityEngine.Object.Destroy(emotionRewardSlotUi1);
                return emotionRewardSlotUi2;
            }
        }

        public class LogBattleCharacterProfileUI : MonoBehaviour
        {
            public Transform uiRoot;
            public Image img_bg;
            public BattleUnitProfileInfoUI_EmotionLvTooltip _emotionLvTooltip;
            public RawImage rawImg_portrait;
            public TextMeshProUGUI txt_unitName;
            public GameObject go_hpValueLayout;
            public GameObject go_emotionLVPivot;
            public BattleCharacterProfileUI.HpBar hpBar;
            public BattleCharacterProfileUI.HpBar img_damagedHp;
            public BattleCharacterProfileUI.HpBar img_healedHp;
            public Text txt_hp;
            public BattleCharacterProfileUI_CoinManager coinUI;
            public BattleCharacterProfileEmotionUI emotionUI;
            public Color _colorDialogBG_New;
            public CanvasGroup _battleDialogCanvasGroup;
            public TextMeshProUGUI _battleDialog;
            public Image _battleDialogBG;
            public Image _battleDialogNewBG;
            public Image _battleDialogChildImg;
            public Image _battleDialogLinearDodge;
            public bool isLeft = true;

            public static BattleCharacterProfileUI BattleUnitInfoManagerUI_Copying(
              BattleCharacterProfileUI baseobj)
            {
                LogLikeMod.LogBattleCharacterProfileUI original = baseobj.gameObject.AddComponent<LogLikeMod.LogBattleCharacterProfileUI>();
                original.uiRoot = LogLikeMod.GetFieldValue<Transform>(baseobj, "uiRoot");
                original.img_bg = LogLikeMod.GetFieldValue<Image>(baseobj, "img_bg");
                original._emotionLvTooltip = LogLikeMod.GetFieldValue<BattleUnitProfileInfoUI_EmotionLvTooltip>(baseobj, "_emotionLvTooltip");
                original.rawImg_portrait = LogLikeMod.GetFieldValue<RawImage>(baseobj, "rawImg_portrait");
                original.txt_unitName = LogLikeMod.GetFieldValue<TextMeshProUGUI>(baseobj, "txt_unitName");
                original.go_hpValueLayout = LogLikeMod.GetFieldValue<GameObject>(baseobj, "go_hpValueLayout");
                original.go_emotionLVPivot = LogLikeMod.GetFieldValue<GameObject>(baseobj, "go_emotionLVPivot");
                original.hpBar = LogLikeMod.GetFieldValue<BattleCharacterProfileUI.HpBar>(baseobj, "hpBar");
                original.img_damagedHp = LogLikeMod.GetFieldValue<BattleCharacterProfileUI.HpBar>(baseobj, "img_damagedHp");
                original.img_healedHp = LogLikeMod.GetFieldValue<BattleCharacterProfileUI.HpBar>(baseobj, "img_healedHp");
                original.txt_hp = LogLikeMod.GetFieldValue<Text>(baseobj, "txt_hp");
                original.coinUI = LogLikeMod.GetFieldValue<BattleCharacterProfileUI_CoinManager>(baseobj, "coinUI");
                original.emotionUI = LogLikeMod.GetFieldValue<BattleCharacterProfileEmotionUI>(baseobj, "emotionUI");
                original._colorDialogBG_New = LogLikeMod.GetFieldValue<Color>(baseobj, "_colorDialogBG_New");
                original._battleDialogCanvasGroup = LogLikeMod.GetFieldValue<CanvasGroup>(baseobj, "_battleDialogCanvasGroup");
                original._battleDialog = LogLikeMod.GetFieldValue<TextMeshProUGUI>(baseobj, "_battleDialog");
                original._battleDialogBG = LogLikeMod.GetFieldValue<Image>(baseobj, "_battleDialogBG");
                original._battleDialogNewBG = LogLikeMod.GetFieldValue<Image>(baseobj, "_battleDialogNewBG");
                original._battleDialogChildImg = LogLikeMod.GetFieldValue<Image>(baseobj, "_battleDialogChildImg");
                original._battleDialogLinearDodge = LogLikeMod.GetFieldValue<Image>(baseobj, "_battleDialogLinearDodge");
                original.isLeft = true;
                LogLikeMod.LogBattleCharacterProfileUI characterProfileUi1 = UnityEngine.Object.Instantiate<LogLikeMod.LogBattleCharacterProfileUI>(original, original.transform.parent);
                BattleCharacterProfileUI characterProfileUi2 = characterProfileUi1.GetComponent<BattleCharacterProfileUI>() == null ? characterProfileUi1.gameObject.AddComponent<BattleCharacterProfileUI>() : characterProfileUi1.GetComponent<BattleCharacterProfileUI>();
                LogLikeMod.SetFieldValue(characterProfileUi2, "uiRoot", characterProfileUi1.uiRoot);
                LogLikeMod.SetFieldValue(characterProfileUi2, "img_bg", characterProfileUi1.img_bg);
                LogLikeMod.SetFieldValue(characterProfileUi2, "_emotionLvTooltip", characterProfileUi1._emotionLvTooltip);
                LogLikeMod.SetFieldValue(characterProfileUi2, "rawImg_portrait", characterProfileUi1.rawImg_portrait);
                LogLikeMod.SetFieldValue(characterProfileUi2, "txt_unitName", characterProfileUi1.txt_unitName);
                LogLikeMod.SetFieldValue(characterProfileUi2, "go_hpValueLayout", characterProfileUi1.go_hpValueLayout);
                LogLikeMod.SetFieldValue(characterProfileUi2, "go_emotionLVPivot", characterProfileUi1.go_emotionLVPivot);
                LogLikeMod.SetFieldValue(characterProfileUi2, "hpBar", characterProfileUi1.hpBar);
                LogLikeMod.SetFieldValue(characterProfileUi2, "img_damagedHp", characterProfileUi1.img_damagedHp);
                LogLikeMod.SetFieldValue(characterProfileUi2, "img_healedHp", characterProfileUi1.img_healedHp);
                LogLikeMod.SetFieldValue(characterProfileUi2, "txt_hp", characterProfileUi1.txt_hp);
                LogLikeMod.SetFieldValue(characterProfileUi2, "coinUI", characterProfileUi1.coinUI);
                LogLikeMod.SetFieldValue(characterProfileUi2, "emotionUI", characterProfileUi1.emotionUI);
                LogLikeMod.SetFieldValue(characterProfileUi2, "_colorDialogBG_New", characterProfileUi1._colorDialogBG_New);
                LogLikeMod.SetFieldValue(characterProfileUi2, "_battleDialogCanvasGroup", characterProfileUi1._battleDialogCanvasGroup);
                LogLikeMod.SetFieldValue(characterProfileUi2, "_battleDialog", characterProfileUi1._battleDialog);
                LogLikeMod.SetFieldValue(characterProfileUi2, "_battleDialogBG", characterProfileUi1._battleDialogBG);
                LogLikeMod.SetFieldValue(characterProfileUi2, "_battleDialogNewBG", characterProfileUi1._battleDialogNewBG);
                LogLikeMod.SetFieldValue(characterProfileUi2, "_battleDialogChildImg", characterProfileUi1._battleDialogChildImg);
                LogLikeMod.SetFieldValue(characterProfileUi2, "_battleDialogLinearDodge", characterProfileUi1._battleDialogLinearDodge);
                LogLikeMod.SetFieldValue(characterProfileUi2, "isLeft", characterProfileUi1.isLeft);
                UnityEngine.Object.Destroy(original);
                UnityEngine.Object.Destroy(characterProfileUi1);
                return characterProfileUi2;
            }
        }

        public class UILogBattleDiceCardUI : MonoBehaviour
        {
            public static LogLikeMod.UILogBattleDiceCardUI _instance;
            public RectTransform vibeRect;
            public Sprite[] costNumberSprite;
            public Sprite costNumberSprite_1;
            public Sprite costNumberSprite_2;
            public Sprite costNumberSprite_3;
            public Sprite costNumberSprite_4;
            public Sprite costNumberSprite_5;
            public Sprite costNumberSprite_6;
            public Sprite costNumberSprite_7;
            public Sprite costNumberSprite_8;
            public Sprite costNumberSprite_9;
            public Sprite costNumberSprite_10;
            public Color[] refColors_Cost;
            public Color refColors_Cost_1;
            public Color refColors_Cost_2;
            public Color refColors_Cost_3;
            public TextMeshProUGUI txt_cardName;
            public Image[] img_Frames;
            public Image img_Frames_1;
            public Image img_Frames_2;
            public Image img_Frames_3;
            public Image img_Frames_4;
            public Image img_Frames_5;
            public Image[] img_linearDodges;
            public Image img_linearDodges_1;
            public Image img_linearDodges_2;
            public Image img_linearDodges_3;
            public Image img_linearDodges_4;
            public Image img_linearDodges_5;
            public Image img_linearDodges_6;
            public GameObject selfAbilityArea;
            public TextMeshProUGUI txt_selfAbility;
            public List<BattleDiceCard_BehaviourDescUI> ui_behaviourDescList;
            public BattleDiceCard_BehaviourDescUI ui_behaviourDescList_1;
            public BattleDiceCard_BehaviourDescUI ui_behaviourDescList_2;
            public BattleDiceCard_BehaviourDescUI ui_behaviourDescList_3;
            public BattleDiceCard_BehaviourDescUI ui_behaviourDescList_4;
            public BattleDiceCard_BehaviourDescUI ui_behaviourDescList_5;
            public List<Image> img_behaviourDetatilList;
            public Image img_behaviourDetatilList_1;
            public Image img_behaviourDetatilList_2;
            public Image img_behaviourDetatilList_3;
            public Image img_behaviourDetatilList_4;
            public Image img_behaviourDetatilList_5;
            public Animator anim;
            public Image img_artwork;
            public Image img_icon;
            public RefineHsv hsv_rangeIcon;
            public RefineHsv hsv_Cost;
            public float _glowElapsedTime;
            public BattleDiceCardBufUI[] bufIconListUI;
            public BattleDiceCardBufUI bufIconListUI_1;
            public BattleDiceCardBufUI bufIconListUI_2;
            public BattleDiceCardBufUI bufIconListUI_3;
            public KeywordListUI KeywordListUI;
            public List<RefineHsv> hsv_behaviourIcons;
            public RefineHsv hsv_behaviourIcons_1;
            public RefineHsv hsv_behaviourIcons_2;
            public RefineHsv hsv_behaviourIcons_3;
            public RefineHsv hsv_behaviourIcons_4;
            public RefineHsv hsv_behaviourIcons_5;
            public List<TextMeshProUGUI> txt_Resist;
            public TextMeshProUGUI txt_Resist_1;
            public TextMeshProUGUI txt_Resist_2;
            public TextMeshProUGUI txt_Resist_3;
            public TextMeshProUGUI txt_Resist_4;
            public TextMeshProUGUI txt_Resist_5;
            public List<TextMeshProUGUI> txt_bpResist;
            public TextMeshProUGUI txt_bpResist_1;
            public TextMeshProUGUI txt_bpResist_2;
            public TextMeshProUGUI txt_bpResist_3;
            public TextMeshProUGUI txt_bpResist_4;
            public TextMeshProUGUI txt_bpResist_5;
            public GameObject[] ob_NormalFrames;
            public GameObject ob_NormalFrames_1;
            public GameObject ob_NormalFrames_2;
            public GameObject[] ob_EgoFrames;
            public GameObject ob_EgoFrames_1;
            public GameObject ob_EgoFrames_2;
            public UICustomSelectable selectable;
            public bool isProfileCard;
            public bool isEmotionSelectedPopup;
            public Canvas _parentCanvas;
            public Transform mouseTransform;
            public int _defaultIdx;
            public BattleDiceCardModel _cardModel;
            public bool _bClicked;
            public bool _bAvailable;
            public bool _bFirstClicked;
            public bool _bEntered;
            public BattleUnitTargetArrowUI arrow;
            public Color colorFrame;
            public Color colorLineardodge;
            public Color colorLineardodge_deactive;
            public Vector3 scaleOrigin;
            public NumbersData costNumbers;
            public int _cost;
            public int _originCost;
            public bool _editor;
            public bool isRunningVibeCard;
            public float vibeCounter;
            public Vector2 rangeIconOriginPos = new Vector2(305f, 189f);
            public Vector2 rangeIconEgoPos = new Vector2(335f, 189f);
            public EmotionEgoXmlInfo egoxmldata;
            public GameObject ob_EgoCoolTime;
            public RectTransform rect_Gauge;
            public Image img_Bg;
            public Image img_BgGlow;
            public RefineHsv hsv_bgGlow;
            public Animator anim_gaugebgglow;
            public Graphic[] graphics_EgoLockFrames;
            public Graphic graphics_EgoLockFrames_1;
            public RefineHsv[] hsv_EgoLockFrames;
            public RefineHsv hsv_EgoLockFrames_1;
            public bool isEgoCoolTimeLock;
            public float gaugeLength = 950f;

            public static LogLikeMod.UILogBattleDiceCardUI Instance
            {
                get
                {
                    if (LogLikeMod.UILogBattleDiceCardUI._instance == null)
                        LogLikeMod.UILogBattleDiceCardUI._instance = LogLikeMod.UILogBattleDiceCardUI.SlotCopying();
                    return LogLikeMod.UILogBattleDiceCardUI._instance;
                }
                set => LogLikeMod.UILogBattleDiceCardUI._instance = value;
            }

            public static LogLikeMod.UILogBattleDiceCardUI SlotCopying()
            {
                BattleDiceCardUI battleDiceCardUi1 = (BattleDiceCardUI)typeof(BattleUnitInformationUI).GetField("previewCardUI", AccessTools.all).GetValue(SingletonBehavior<BattleManagerUI>.Instance.ui_unitInformationPlayer);
                LogLikeMod.UILogBattleDiceCardUI original = battleDiceCardUi1.gameObject.AddComponent<LogLikeMod.UILogBattleDiceCardUI>();
                original.vibeRect = (RectTransform)typeof(BattleDiceCardUI).GetField("vibeRect", AccessTools.all).GetValue(battleDiceCardUi1);
                original.costNumberSprite = (Sprite[])typeof(BattleDiceCardUI).GetField("costNumberSprite", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 10; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("costNumberSprite_" + (index + 1).ToString()).SetValue(original, original.costNumberSprite[index]);
                original.refColors_Cost = (Color[])typeof(BattleDiceCardUI).GetField("refColors_Cost", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 3; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("refColors_Cost_" + (index + 1).ToString()).SetValue(original, original.refColors_Cost[index]);
                original.txt_cardName = (TextMeshProUGUI)typeof(BattleDiceCardUI).GetField("txt_cardName", AccessTools.all).GetValue(battleDiceCardUi1);
                original.img_Frames = (Image[])typeof(BattleDiceCardUI).GetField("img_Frames", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 5; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("img_Frames_" + (index + 1).ToString()).SetValue(original, original.img_Frames[index]);
                original.img_linearDodges = (Image[])typeof(BattleDiceCardUI).GetField("img_linearDodges", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 6; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("img_linearDodges_" + (index + 1).ToString()).SetValue(original, original.img_linearDodges[index]);
                original.selfAbilityArea = (GameObject)typeof(BattleDiceCardUI).GetField("selfAbilityArea", AccessTools.all).GetValue(battleDiceCardUi1);
                original.txt_selfAbility = (TextMeshProUGUI)typeof(BattleDiceCardUI).GetField("txt_selfAbility", AccessTools.all).GetValue(battleDiceCardUi1);
                original.ui_behaviourDescList = (List<BattleDiceCard_BehaviourDescUI>)typeof(BattleDiceCardUI).GetField("ui_behaviourDescList", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 5; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("ui_behaviourDescList_" + (index + 1).ToString()).SetValue(original, original.ui_behaviourDescList[index]);
                original.img_behaviourDetatilList = (List<Image>)typeof(BattleDiceCardUI).GetField("img_behaviourDetatilList", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 5; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("img_behaviourDetatilList_" + (index + 1).ToString()).SetValue(original, original.img_behaviourDetatilList[index]);
                original.anim = (Animator)typeof(BattleDiceCardUI).GetField("anim", AccessTools.all).GetValue(battleDiceCardUi1);
                original.img_artwork = (Image)typeof(BattleDiceCardUI).GetField("img_artwork", AccessTools.all).GetValue(battleDiceCardUi1);
                original.img_icon = (Image)typeof(BattleDiceCardUI).GetField("img_icon", AccessTools.all).GetValue(battleDiceCardUi1);
                original.hsv_rangeIcon = (RefineHsv)typeof(BattleDiceCardUI).GetField("hsv_rangeIcon", AccessTools.all).GetValue(battleDiceCardUi1);
                original.hsv_Cost = (RefineHsv)typeof(BattleDiceCardUI).GetField("hsv_Cost", AccessTools.all).GetValue(battleDiceCardUi1);
                original._glowElapsedTime = (float)typeof(BattleDiceCardUI).GetField("_glowElapsedTime", AccessTools.all).GetValue(battleDiceCardUi1);
                original.bufIconListUI = (BattleDiceCardBufUI[])typeof(BattleDiceCardUI).GetField("bufIconListUI", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 3; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("bufIconListUI_" + (index + 1).ToString()).SetValue(original, original.bufIconListUI[index]);
                original.KeywordListUI = (KeywordListUI)typeof(BattleDiceCardUI).GetField("KeywordListUI", AccessTools.all).GetValue(battleDiceCardUi1);
                original.hsv_behaviourIcons = (List<RefineHsv>)typeof(BattleDiceCardUI).GetField("hsv_behaviourIcons", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 5; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("hsv_behaviourIcons_" + (index + 1).ToString()).SetValue(original, original.hsv_behaviourIcons[index]);
                original.txt_Resist = (List<TextMeshProUGUI>)typeof(BattleDiceCardUI).GetField("txt_Resist", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 5; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("txt_Resist_" + (index + 1).ToString()).SetValue(original, original.txt_Resist[index]);
                original.txt_bpResist = (List<TextMeshProUGUI>)typeof(BattleDiceCardUI).GetField("txt_bpResist", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 5; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("txt_bpResist_" + (index + 1).ToString()).SetValue(original, original.txt_bpResist[index]);
                original.ob_NormalFrames = (GameObject[])typeof(BattleDiceCardUI).GetField("ob_NormalFrames", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 2; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("ob_NormalFrames_" + (index + 1).ToString()).SetValue(original, original.ob_NormalFrames[index]);
                original.ob_EgoFrames = (GameObject[])typeof(BattleDiceCardUI).GetField("ob_EgoFrames", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 2; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("ob_EgoFrames_" + (index + 1).ToString()).SetValue(original, original.ob_EgoFrames[index]);
                original.selectable = (UICustomSelectable)typeof(BattleDiceCardUI).GetField("selectable", AccessTools.all).GetValue(battleDiceCardUi1);
                original.isProfileCard = (bool)typeof(BattleDiceCardUI).GetField("isProfileCard", AccessTools.all).GetValue(battleDiceCardUi1);
                original.isEmotionSelectedPopup = (bool)typeof(BattleDiceCardUI).GetField("isEmotionSelectedPopup", AccessTools.all).GetValue(battleDiceCardUi1);
                original.mouseTransform = (Transform)typeof(BattleDiceCardUI).GetField("mouseTransform", AccessTools.all).GetValue(battleDiceCardUi1);
                original._defaultIdx = (int)typeof(BattleDiceCardUI).GetField("_defaultIdx", AccessTools.all).GetValue(battleDiceCardUi1);
                original._cardModel = (BattleDiceCardModel)typeof(BattleDiceCardUI).GetField("_cardModel", AccessTools.all).GetValue(battleDiceCardUi1);
                original._bClicked = (bool)typeof(BattleDiceCardUI).GetField("_bClicked", AccessTools.all).GetValue(battleDiceCardUi1);
                original._bAvailable = (bool)typeof(BattleDiceCardUI).GetField("_bAvailable", AccessTools.all).GetValue(battleDiceCardUi1);
                original._bFirstClicked = (bool)typeof(BattleDiceCardUI).GetField("_bFirstClicked", AccessTools.all).GetValue(battleDiceCardUi1);
                original._bEntered = (bool)typeof(BattleDiceCardUI).GetField("_bEntered", AccessTools.all).GetValue(battleDiceCardUi1);
                original.arrow = (BattleUnitTargetArrowUI)null;
                original.colorFrame = (Color)typeof(BattleDiceCardUI).GetField("colorFrame", AccessTools.all).GetValue(battleDiceCardUi1);
                original.colorLineardodge = (Color)typeof(BattleDiceCardUI).GetField("colorLineardodge", AccessTools.all).GetValue(battleDiceCardUi1);
                original.colorLineardodge_deactive = (Color)typeof(BattleDiceCardUI).GetField("colorLineardodge_deactive", AccessTools.all).GetValue(battleDiceCardUi1);
                original.scaleOrigin = (Vector3)typeof(BattleDiceCardUI).GetField("scaleOrigin", AccessTools.all).GetValue(battleDiceCardUi1);
                original.costNumbers = (NumbersData)typeof(BattleDiceCardUI).GetField("costNumbers", AccessTools.all).GetValue(battleDiceCardUi1);
                original._cost = (int)typeof(BattleDiceCardUI).GetField("_cost", AccessTools.all).GetValue(battleDiceCardUi1);
                original._originCost = (int)typeof(BattleDiceCardUI).GetField("_originCost", AccessTools.all).GetValue(battleDiceCardUi1);
                original._editor = (bool)typeof(BattleDiceCardUI).GetField("_editor", AccessTools.all).GetValue(battleDiceCardUi1);
                original.isRunningVibeCard = (bool)typeof(BattleDiceCardUI).GetField("isRunningVibeCard", AccessTools.all).GetValue(battleDiceCardUi1);
                original.vibeCounter = (float)typeof(BattleDiceCardUI).GetField("vibeCounter", AccessTools.all).GetValue(battleDiceCardUi1);
                original.rangeIconOriginPos = (Vector2)typeof(BattleDiceCardUI).GetField("rangeIconOriginPos", AccessTools.all).GetValue(battleDiceCardUi1);
                original.rangeIconEgoPos = (Vector2)typeof(BattleDiceCardUI).GetField("rangeIconEgoPos", AccessTools.all).GetValue(battleDiceCardUi1);
                original.egoxmldata = (EmotionEgoXmlInfo)typeof(BattleDiceCardUI).GetField("egoxmldata", AccessTools.all).GetValue(battleDiceCardUi1);
                original.ob_EgoCoolTime = (GameObject)typeof(BattleDiceCardUI).GetField("ob_EgoCoolTime", AccessTools.all).GetValue(battleDiceCardUi1);
                original.rect_Gauge = (RectTransform)typeof(BattleDiceCardUI).GetField("rect_Gauge", AccessTools.all).GetValue(battleDiceCardUi1);
                original.img_Bg = (Image)typeof(BattleDiceCardUI).GetField("img_Bg", AccessTools.all).GetValue(battleDiceCardUi1);
                original.img_BgGlow = (Image)typeof(BattleDiceCardUI).GetField("img_BgGlow", AccessTools.all).GetValue(battleDiceCardUi1);
                original.hsv_bgGlow = (RefineHsv)typeof(BattleDiceCardUI).GetField("hsv_bgGlow", AccessTools.all).GetValue(battleDiceCardUi1);
                original.anim_gaugebgglow = (Animator)typeof(BattleDiceCardUI).GetField("anim_gaugebgglow", AccessTools.all).GetValue(battleDiceCardUi1);
                original.graphics_EgoLockFrames = (Graphic[])typeof(BattleDiceCardUI).GetField("graphics_EgoLockFrames", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 1; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("graphics_EgoLockFrames_" + (index + 1).ToString()).SetValue(original, original.graphics_EgoLockFrames[index]);
                original.hsv_EgoLockFrames = (RefineHsv[])typeof(BattleDiceCardUI).GetField("hsv_EgoLockFrames", AccessTools.all).GetValue(battleDiceCardUi1);
                for (int index = 0; index < 1; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("hsv_EgoLockFrames_" + (index + 1).ToString()).SetValue(original, original.hsv_EgoLockFrames[index]);
                original.isEgoCoolTimeLock = (bool)typeof(BattleDiceCardUI).GetField("isEgoCoolTimeLock", AccessTools.all).GetValue(battleDiceCardUi1);
                original.gaugeLength = (float)typeof(BattleDiceCardUI).GetField("gaugeLength", AccessTools.all).GetValue(battleDiceCardUi1);
                LogLikeMod.UILogBattleDiceCardUI battleDiceCardUi2 = UnityEngine.Object.Instantiate<LogLikeMod.UILogBattleDiceCardUI>(original, SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.transform);
                int num;
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    Sprite[] costNumberSprite = battleDiceCardUi2.costNumberSprite;
                    int index2 = index1;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index1 + 1;
                    string name = "costNumberSprite_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    Sprite sprite = (Sprite)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    costNumberSprite[index2] = sprite;
                }
                for (int index3 = 0; index3 < 3; ++index3)
                {
                    Color[] refColorsCost = battleDiceCardUi2.refColors_Cost;
                    int index4 = index3;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index3 + 1;
                    string name = "refColors_Cost_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    Color color = (Color)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    refColorsCost[index4] = color;
                }
                for (int index5 = 0; index5 < 5; ++index5)
                {
                    Image[] imgFrames = battleDiceCardUi2.img_Frames;
                    int index6 = index5;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index5 + 1;
                    string name = "img_Frames_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    Image image = (Image)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    imgFrames[index6] = image;
                }
                for (int index7 = 0; index7 < 6; ++index7)
                {
                    Image[] imgLinearDodges = battleDiceCardUi2.img_linearDodges;
                    int index8 = index7;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index7 + 1;
                    string name = "img_linearDodges_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    Image image = (Image)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    imgLinearDodges[index8] = image;
                }
                for (int index9 = 0; index9 < 5; ++index9)
                {
                    List<BattleDiceCard_BehaviourDescUI> behaviourDescList = battleDiceCardUi2.ui_behaviourDescList;
                    int index10 = index9;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index9 + 1;
                    string name = "ui_behaviourDescList_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    BattleDiceCard_BehaviourDescUI cardBehaviourDescUi = (BattleDiceCard_BehaviourDescUI)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    behaviourDescList[index10] = cardBehaviourDescUi;
                }
                for (int index11 = 0; index11 < 5; ++index11)
                {
                    List<Image> behaviourDetatilList = battleDiceCardUi2.img_behaviourDetatilList;
                    int index12 = index11;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index11 + 1;
                    string name = "img_behaviourDetatilList_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    Image image = (Image)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    behaviourDetatilList[index12] = image;
                }
                for (int index13 = 0; index13 < 3; ++index13)
                {
                    BattleDiceCardBufUI[] bufIconListUi = battleDiceCardUi2.bufIconListUI;
                    int index14 = index13;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index13 + 1;
                    string name = "bufIconListUI_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    BattleDiceCardBufUI battleDiceCardBufUi = (BattleDiceCardBufUI)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    bufIconListUi[index14] = battleDiceCardBufUi;
                }
                for (int index15 = 0; index15 < 5; ++index15)
                {
                    List<RefineHsv> hsvBehaviourIcons = battleDiceCardUi2.hsv_behaviourIcons;
                    int index16 = index15;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index15 + 1;
                    string name = "hsv_behaviourIcons_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    RefineHsv refineHsv = (RefineHsv)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    hsvBehaviourIcons[index16] = refineHsv;
                }
                for (int index17 = 0; index17 < 5; ++index17)
                {
                    List<TextMeshProUGUI> txtResist = battleDiceCardUi2.txt_Resist;
                    int index18 = index17;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index17 + 1;
                    string name = "txt_Resist_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    TextMeshProUGUI textMeshProUgui = (TextMeshProUGUI)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    txtResist[index18] = textMeshProUgui;
                }
                for (int index19 = 0; index19 < 5; ++index19)
                {
                    List<TextMeshProUGUI> txtBpResist = battleDiceCardUi2.txt_bpResist;
                    int index20 = index19;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index19 + 1;
                    string name = "txt_bpResist_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    TextMeshProUGUI textMeshProUgui = (TextMeshProUGUI)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    txtBpResist[index20] = textMeshProUgui;
                }
                for (int index21 = 0; index21 < 2; ++index21)
                {
                    GameObject[] obNormalFrames = battleDiceCardUi2.ob_NormalFrames;
                    int index22 = index21;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index21 + 1;
                    string name = "ob_NormalFrames_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    GameObject gameObject = (GameObject)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    obNormalFrames[index22] = gameObject;
                }
                for (int index23 = 0; index23 < 2; ++index23)
                {
                    GameObject[] obEgoFrames = battleDiceCardUi2.ob_EgoFrames;
                    int index24 = index23;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index23 + 1;
                    string name = "ob_EgoFrames_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    GameObject gameObject = (GameObject)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    obEgoFrames[index24] = gameObject;
                }
                for (int index25 = 0; index25 < 1; ++index25)
                {
                    Graphic[] graphicsEgoLockFrames = battleDiceCardUi2.graphics_EgoLockFrames;
                    int index26 = index25;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index25 + 1;
                    string name = "graphics_EgoLockFrames_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    Graphic graphic = (Graphic)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    graphicsEgoLockFrames[index26] = graphic;
                }
                for (int index27 = 0; index27 < 1; ++index27)
                {
                    RefineHsv[] hsvEgoLockFrames = battleDiceCardUi2.hsv_EgoLockFrames;
                    int index28 = index27;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index27 + 1;
                    string name = "hsv_EgoLockFrames_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    RefineHsv refineHsv = (RefineHsv)(type.GetField(name)).GetValue(battleDiceCardUi2);
                    hsvEgoLockFrames[index28] = refineHsv;
                }
                UnityEngine.Object.Destroy(original);
                UnityEngine.Object.Destroy(battleDiceCardUi2.gameObject.GetComponent<BattleDiceCardUI>());
                return battleDiceCardUi2;
            }

            public void EnableAddedIcons()
            {
                List<BattleDiceCardBuf> bufList = this._cardModel.GetBufList();
                int index1 = 0;
                foreach (BattleDiceCardBuf cardBuf in bufList)
                {
                    if (index1 < this.bufIconListUI.Length)
                    {
                        if (!(cardBuf.GetBufIcon() == null))
                        {
                            this.bufIconListUI[index1].SetBufIcon(cardBuf);
                            this.bufIconListUI[index1].SetEnable(true);
                            ++index1;
                        }
                    }
                    else
                        break;
                }
                for (int index2 = index1; index2 < this.bufIconListUI.Length; ++index2)
                    this.bufIconListUI[index2].SetEnable(false);
            }

            public void SetEgoFrameLockColor(bool on)
            {
                Color color = on ? UIColorManager.Manager.GetUIColor(UIColor.Disabled) : Color.white;
                foreach (Graphic graphicsEgoLockFrame in this.graphics_EgoLockFrames)
                {
                    if (!(graphicsEgoLockFrame == null))
                        graphicsEgoLockFrame.color = color;
                }
                float num = on ? 0.0f : 1f;
                foreach (RefineHsv hsvEgoLockFrame in this.hsv_EgoLockFrames)
                {
                    hsvEgoLockFrame._Saturation = num;
                    hsvEgoLockFrame.CallUpdate();
                }
                foreach (RefineHsv hsvBehaviourIcon in this.hsv_behaviourIcons)
                {
                    hsvBehaviourIcon._Saturation = num;
                    hsvBehaviourIcon.CallUpdate();
                }
            }

            public void SetEgoLock()
            {
                this.isEgoCoolTimeLock = true;
                Color uiColor = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                this.SetFrameColor(uiColor);
                this.SetLinearDodgeColor(uiColor);
                this.SetEgoFrameLockColor(true);
                this.SetRangeIconHsv(new Vector3(0.0f, 0.0f, 1f));
            }

            public void SetEgoCoolTimeGauge()
            {
                if (this.ob_EgoCoolTime == null)
                    return;
                if (!this._cardModel.XmlData.IsEgo())
                {
                    this.ob_EgoCoolTime.SetActive(false);
                    this.SetEgoFrameLockColor(false);
                }
                else if ((this.isProfileCard || this.isEmotionSelectedPopup) && this.ob_EgoCoolTime != null && this.ob_EgoCoolTime.activeSelf)
                {
                    this.ob_EgoCoolTime.gameObject.SetActive(false);
                }
                else
                {
                    this.ob_EgoCoolTime.gameObject.SetActive(true);
                    float num = 0.0f;
                    if ((double)this._cardModel.MaxCooltimeValue != 0.0)
                        num = this._cardModel.CurrentCooltimeValue / this._cardModel.MaxCooltimeValue;
                    if ((double)num >= 1.0)
                    {
                        this.ob_EgoCoolTime.gameObject.SetActive(false);
                        this.SetEgoFrameLockColor(false);
                    }
                    else
                    {
                        this.rect_Gauge.anchoredPosition = new Vector2(this.gaugeLength * num, 0.0f);
                        this.hsv_bgGlow.CallUpdate();
                        if ((double)num < 0.699999988079071)
                        {
                            Color color = this.img_BgGlow.color;
                            color.a = 1f;
                            this.img_BgGlow.color = color;
                            this.anim_gaugebgglow.enabled = false;
                            this.hsv_bgGlow._ValueBrightness = 0.3f;
                            Color color2 = Color.white;
                            color2.a = 0.3f;
                            this.img_Bg.color = color2;
                        }
                        else
                        {
                            this.hsv_bgGlow._ValueBrightness = num * 1.2f;
                            this.anim_gaugebgglow.enabled = true;
                            Color color3 = Color.white;
                            color3.a = 0.7f;
                            this.img_Bg.color = color3;
                        }
                        this.SetEgoLock();
                    }
                }
            }

            public void SetRangeIconHsv(Vector3 hsvvalue)
            {
                this.img_icon.color = Color.white;
                if (this.hsv_rangeIcon == null)
                    this.hsv_rangeIcon = this.img_icon.GetComponent<RefineHsv>();
                if (this.hsv_rangeIcon == null)
                {
                    Debug.LogError("Hsv Not Reference");
                }
                else
                {
                    this.hsv_rangeIcon._HueShift = hsvvalue.x;
                    this.hsv_rangeIcon._Saturation = hsvvalue.y;
                    this.hsv_rangeIcon._ValueBrightness = hsvvalue.z;
                    this.hsv_rangeIcon.CallUpdate();
                }
            }

            public void SetLinearDodgeColor(Color c)
            {
                for (int index = 0; index < this.img_linearDodges.Length; ++index)
                    this.img_linearDodges[index].color = c;
            }

            public void SetFrameColor(Color c)
            {
                for (int index = 0; index < this.img_Frames.Length; ++index)
                    this.img_Frames[index].color = c;
                if (this._cost == this._originCost)
                {
                    this.costNumbers.SetContentColor(c);
                    this.hsv_Cost._HueShift = 0.0f;
                    this.hsv_Cost.CallUpdate();
                }
                else if (this._cost < this._originCost)
                {
                    this.costNumbers.SetContentColor(Color.white);
                    this.hsv_Cost._HueShift = 0.0f;
                    this.hsv_Cost.CallUpdate();
                }
                else
                {
                    if (this._cost <= this._originCost)
                        return;
                    this.costNumbers.SetContentColor(Color.white);
                    this.hsv_Cost._HueShift = 150f;
                    this.hsv_Cost.CallUpdate();
                }
            }

            public void SetDefaultPreviewResistText()
            {
                foreach (TMP_Text tmpText in this.txt_Resist)
                    tmpText.text = "";
                foreach (TMP_Text tmpText in this.txt_bpResist)
                    tmpText.text = "";
                foreach (RefineHsv hsvBehaviourIcon in this.hsv_behaviourIcons)
                {
                    hsvBehaviourIcon._Saturation = 1f;
                    hsvBehaviourIcon.CallUpdate();
                }
            }

            public void SetCard(BattleDiceCardModel cardModel)
            {
                bool __state = false;
                if (cardModel != this._cardModel)
                {
                    __state = true;
                }
                BCEVLoglikeExtensions.ConfigureBattleCard(this, cardModel.GetBehaviourList().Count, cardModel != this._cardModel);
                this.egoxmldata = (EmotionEgoXmlInfo)null;
                this._cardModel = cardModel;
                bool flag = this._cardModel.XmlData.IsEgo();
                foreach (GameObject obNormalFrame in this.ob_NormalFrames)
                    obNormalFrame.SetActive(!flag);
                foreach (GameObject obEgoFrame in this.ob_EgoFrames)
                    obEgoFrame.SetActive(flag);
                if (LorId.IsModId(cardModel.XmlData.workshopID))
                    this.txt_cardName.text = RewardingModel.SanitizeDisplayText(cardModel.XmlData.workshopName);
                else
                    this.txt_cardName.text = RewardingModel.SanitizeDisplayText(Singleton<BattleCardDescXmlList>.Instance.GetCardName(cardModel.GetTextId()));
                this.SetDefaultPreviewResistText();
                this._cost = this._cardModel.GetCost();
                this._originCost = this._cardModel.GetOriginCost();
                Sprite[] sp = this.costNumberSprite;
                if ((this._cost < this._originCost ? 1 : (this._cost > this._originCost ? 1 : 0)) != 0)
                    sp = UISpriteDataManager.instance.CardCostAddGlow;
                this.costNumbers.SetOneValue(this._cardModel.GetCost(), sp);
                this.img_icon.sprite = UISpriteDataManager.instance.GetRangeIconSprite(cardModel.GetSpec().Ranged);
                this.img_icon.rectTransform.anchoredPosition = flag ? this.rangeIconEgoPos : this.rangeIconOriginPos;
                List<DiceBehaviour> behaviourList = cardModel.GetBehaviourList();
                int b = 4 - behaviourList.Count;
                string text = Singleton<BattleCardDescXmlList>.Instance.GetAbilityDesc(cardModel.GetID());
                if (string.IsNullOrEmpty(text))
                {
                    List<string> abilityDesc = Singleton<BattleCardAbilityDescXmlList>.Instance.GetAbilityDesc(cardModel.XmlData);
                    if (abilityDesc.Count > 0)
                        text = string.Join("\n", (IEnumerable<string>)abilityDesc);
                }
                else
                {
                    string abilityDescString = Singleton<BattleCardAbilityDescXmlList>.Instance.GetDefaultAbilityDescString(cardModel.XmlData);
                    if (!string.IsNullOrEmpty(abilityDescString))
                        text = $"{abilityDescString}\n{text}";
                }
                if (!string.IsNullOrEmpty(text))
                {
                    this.selfAbilityArea.SetActive(true);
                    this.txt_selfAbility.text = RewardingModel.SanitizeDisplayText(TextUtil.TransformConditionKeyword(text));
                    float preferredHeight = this.txt_selfAbility.preferredHeight;
                    int num = Mathf.Min((double)preferredHeight >= 260.0 ? ((double)preferredHeight >= 480.0 ? ((double)preferredHeight >= 700.0 ? 3 : 2) : 1) : 0, b);
                    RectTransform component = this.selfAbilityArea.GetComponent<RectTransform>();
                    if (component != null)
                    {
                        switch (num)
                        {
                            case 1:
                                component.sizeDelta = new Vector2(component.sizeDelta.x, 440f);
                                break;
                            case 2:
                                component.sizeDelta = new Vector2(component.sizeDelta.x, 660f);
                                break;
                            case 3:
                                component.sizeDelta = new Vector2(component.sizeDelta.x, 880f);
                                break;
                            default:
                                component.sizeDelta = new Vector2(component.sizeDelta.x, 220f);
                                break;
                        }
                    }
                }
                else
                    this.selfAbilityArea.SetActive(false);
                for (int index = 0; index < behaviourList.Count; ++index)
                {
                    this.ui_behaviourDescList[index].SetBehaviourInfo(behaviourList[index], cardModel.GetID(), cardModel.GetBehaviourList());
                    this.ui_behaviourDescList[index].gameObject.SetActive(true);
                    Sprite sprite = behaviourList[index].Type == BehaviourType.Standby ? UISpriteDataManager.instance.CardStandbyBehaviourDetailIcons[(int)behaviourList[index].Detail] : UISpriteDataManager.instance._cardBehaviourDetailIcons[(int)behaviourList[index].Detail];
                    this.img_behaviourDetatilList[index].sprite = sprite;
                    this.img_behaviourDetatilList[index].gameObject.SetActive(true);
                }
                for (int count = behaviourList.Count; count < this.ui_behaviourDescList.Count; ++count)
                {
                    this.ui_behaviourDescList[count].gameObject.SetActive(false);
                    this.img_behaviourDetatilList[count].gameObject.SetActive(false);
                }
                this.colorFrame = flag ? UIColorManager.Manager.CardEgoCostColor : UIColorManager.Manager.GetCardRarityColor(cardModel.GetRarity());
                this.colorLineardodge = flag ? UIColorManager.Manager.CardEgoLinearColor : UIColorManager.Manager.GetCardRarityLinearColor(cardModel.GetRarity());
                this.colorLineardodge_deactive = this.colorLineardodge;
                this.colorLineardodge_deactive.a = 1f;
                this.hsv_Cost._ValueBrightness = flag ? 1.5f : 1f;
                this.hsv_Cost.CallUpdate();
                this.SetFrameColor(this.colorFrame);
                this.SetLinearDodgeColor(this.colorLineardodge);
                this.SetRangeIconHsv(flag ? UIColorManager.Manager.CardRangeHsvValue[5] : UIColorManager.Manager.CardRangeHsvValue[(int)this._cardModel.GetRarity()]);
                this.KeywordListUI.Activate();
                this.KeywordListUI.Init(this._cardModel.XmlData, this._cardModel.GetBehaviourList());
                if (this._editor)
                    return;
                Sprite sprite1 = !LorId.IsModId(cardModel.XmlData.workshopID) ? Singleton<AssetBundleManagerRemake>.Instance.LoadCardSprite(cardModel.GetArtworkSrc()) : Singleton<CustomizingCardArtworkLoader>.Instance.GetSpecificArtworkSprite(cardModel.XmlData.workshopID, cardModel.GetArtworkSrc());
                if (sprite1 != null)
                    this.img_artwork.sprite = sprite1;
                else
                    Debug.Log("Can't find sprite");
                this.isEgoCoolTimeLock = false;
                this.SetEgoCoolTimeGauge();
                this.EnableAddedIcons();
                BCEVLoglikeExtensions.ForceUpdateLeftBehaviourMaterial(this);
                if (cardModel != null && this.isProfileCard)
                {
                    Vector3 localPosition = this.KeywordListUI.transform.localPosition;
                    if (this.transform.position.x < 0f)
                    {
                        localPosition.x = -2200f;
                    }
                    else
                    {
                        localPosition.x = 900f;
                    }
                    this.KeywordListUI.transform.localPosition = localPosition;
                    this.KeywordListUI.Activate();
                    this.KeywordListUI.Init(cardModel.XmlData, cardModel.XmlData.DiceBehaviourList);
                }
                if (__state)
                {
                    LayoutRebuilder.MarkLayoutForRebuild(this.transform as RectTransform);
                }
                if (!this.name.Contains("PreviewCard"))
                {
                    BCEVLoglikeExtensions.SetBattleCardPreview(this, cardModel, new BattleDiceCardUI.Option[] { });
                }
            }

        }

        public class LogLikeBattleDiceCardPreviewUI : MonoBehaviour
        {
            public static LogLikeBattleDiceCardPreviewUI GetOrCreateUI(UILogBattleDiceCardUI mainUI, bool createIfNull)
            {
                LogLikeBattleDiceCardPreviewUI battleDiceCardPreviewUI = mainUI.GetComponent<LogLikeBattleDiceCardPreviewUI>();
                if (battleDiceCardPreviewUI)
                {
                    return battleDiceCardPreviewUI;
                }
                if (!createIfNull)
                {
                    return null;
                }
                battleDiceCardPreviewUI = mainUI.gameObject.AddComponent<LogLikeBattleDiceCardPreviewUI>();
                GameObject gameObject = new GameObject("[Rect]PreviewCardRoot");
                battleDiceCardPreviewUI.previewRoot = gameObject;
                RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
                rectTransform.SetParent(mainUI.transform);
                rectTransform.localScale = Vector3.one * 0.95f;
                rectTransform.localRotation = Quaternion.identity;
                rectTransform.localPosition = new Vector3(0f, 1500f, 0f);
                UILogBattleDiceCardUI battleDiceCardUI = UnityEngine.Object.Instantiate<UILogBattleDiceCardUI>(mainUI, rectTransform);
                battleDiceCardPreviewUI.cardObject = battleDiceCardUI;
                RectTransform rectTransform2 = battleDiceCardUI.transform as RectTransform;
                rectTransform2.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform2.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform2.localPosition = new Vector2(0f, -50f);
                rectTransform2.localScale = (mainUI.isProfileCard ? Vector3.one : (Vector3.one * 0.8f));
                battleDiceCardUI.scaleOrigin = rectTransform2.localScale;
                rectTransform2.localRotation = Quaternion.identity;
                battleDiceCardUI.name = "[Rect]PreviewCardBattle";
                GameObject gameObject2 = new GameObject("[Rect]PreviewSelectArrows");
                battleDiceCardPreviewUI.arrowsRoot = gameObject2;
                RectTransform rectTransform3 = gameObject2.AddComponent<RectTransform>();
                rectTransform3.SetParent(rectTransform);
                rectTransform3.pivot = new Vector2(0.5f, 0.65f);
                rectTransform3.localRotation = Quaternion.identity;
                rectTransform3.localPosition = new Vector3(0f, 750f, 0f);
                rectTransform3.localScale = new Vector3(5f, 5f, 1f);
                rectTransform3.sizeDelta = new Vector2(50f, 40f);
                gameObject2.AddComponent<Image>().sprite = CardViewInternals.GetArrowsBgSprite();
                GameObject gameObject3 = new GameObject("[Image]Left");
                RectTransform rectTransform4 = gameObject3.AddComponent<RectTransform>();
                rectTransform4.SetParent(rectTransform3);
                rectTransform4.localScale = Vector3.one;
                rectTransform4.localRotation = Quaternion.Euler(0f, 0f, -90f);
                rectTransform4.sizeDelta = new Vector2(40f, 30f);
                rectTransform4.anchoredPosition3D = new Vector3(-10f, 0f, 0f);
                Image image = gameObject3.AddComponent<Image>();
                image.sprite = CardViewInternals.GetArrowSprite();
                battleDiceCardPreviewUI.prevArrow = image;
                GameObject gameObject4 = new GameObject("[Image]Right");
                RectTransform rectTransform5 = gameObject4.AddComponent<RectTransform>();
                rectTransform5.SetParent(rectTransform3);
                rectTransform5.localScale = Vector3.one;
                rectTransform5.localRotation = Quaternion.Euler(0f, 0f, 90f);
                rectTransform5.sizeDelta = new Vector2(40f, 30f);
                rectTransform5.anchoredPosition3D = new Vector3(10f, 0f, 0f);
                Image image2 = gameObject4.AddComponent<Image>();
                image2.sprite = CardViewInternals.GetArrowSprite();
                battleDiceCardPreviewUI.nextArrow = image2;
                Graphic[] componentsInChildren = gameObject.GetComponentsInChildren<Graphic>(true);
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].raycastTarget = false;
                }
                battleDiceCardPreviewUI.Awake();
                return battleDiceCardPreviewUI;
            }

            public void SetCardList(List<BattleDiceCardModel> previewList, BattleDiceCardUI.Option[] options)
            {
                if (previewList == null || previewList.Count == 0)
                {
                    this.previewRoot.SetActive(false);
                    this.cardList = null;
                    return;
                }
                this.cardList = previewList;
                this.curIndex = 0;
                this.curOptions = options;
                if (this.cardList.Count == 1)
                {
                    this.arrowsRoot.SetActive(false);
                }
                else
                {
                    this.arrowsRoot.SetActive(true);
                    this.prevArrow.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                    this.nextArrow.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
                }
                if (this.cardObject.isProfileCard)
                {
                    this.SetCard(previewList[0], options);
                    return;
                }
                this.previewRoot.SetActive(false);
            }
            public void NextCard()
            {
                if (this.cardList == null)
                {
                    return;
                }
                if (this.curIndex >= this.cardList.Count - 1)
                {
                    return;
                }
                this.curIndex++;
                this.SetCard(this.cardList[this.curIndex], this.curOptions);
                this.prevArrow.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
                if (this.curIndex == this.cardList.Count - 1)
                {
                    this.nextArrow.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                }
            }
            public void PrevCard()
            {
                if (this.cardList == null)
                {
                    return;
                }
                if (this.curIndex <= 0)
                {
                    return;
                }
                this.curIndex--;
                this.SetCard(this.cardList[this.curIndex], this.curOptions);
                this.nextArrow.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
                if (this.curIndex == 0)
                {
                    this.prevArrow.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                }
            }

            public void ShowCard()
            {
                if (this.cardList == null || this.curIndex < 0 || this.curIndex >= this.cardList.Count)
                {
                    return;
                }
                this.SetCard(this.cardList[this.curIndex], this.curOptions);
            }

            public void HideCard()
            {
                this.SetCard(null, null);
            }
            public void SetCard(BattleDiceCardModel card, BattleDiceCardUI.Option[] options)
            {
                if (card == null)
                {
                    this.previewRoot.SetActive(false);
                    return;
                }
                this.previewRoot.SetActive(true);
                this.cardObject.SetCard(card);
            }
            public void Awake()
            {
                if (this.cardObject)
                {
                    foreach (object obj in this.cardObject.transform)
                    {
                        Transform transform = (Transform)obj;
                        if (transform.name.Contains("PreviewCard"))
                        {
                            UnityEngine.Object.Destroy(transform.gameObject);
                        }
                    }
                    BattleDiceCardPreviewUI component = this.cardObject.GetComponent<BattleDiceCardPreviewUI>();
                    if (component)
                    {
                        UnityEngine.Object.Destroy(component);
                    }
                }
            }

            public void Update()
            {
                if (this.previewRoot.activeSelf)
                {
                    if (this.cardObject.isProfileCard)
                    {
                        if (this.previewRoot.transform.localPosition.y > 0f)
                        {
                            Vector3 localPosition = this.previewRoot.transform.localPosition;
                            localPosition.y = -localPosition.y;
                            this.previewRoot.transform.localPosition = localPosition;
                        }
                    }
                    else if (this.previewRoot.transform.localPosition.y < 0f)
                    {
                        Vector3 localPosition2 = this.previewRoot.transform.localPosition;
                        localPosition2.y = -localPosition2.y;
                        this.previewRoot.transform.localPosition = localPosition2;
                    }
                    if (Input.GetKeyDown(CardViewInternals.NextCardKey))
                    {
                        this.NextCard();
                        this.cooldown = CardViewInternals.CardChangeHoldDelay;
                        return;
                    }
                    if (Input.GetKeyDown(CardViewInternals.PrevCardKey))
                    {
                        this.PrevCard();
                        this.cooldown = CardViewInternals.CardChangeHoldDelay;
                        return;
                    }
                    bool key = Input.GetKey(CardViewInternals.NextCardKey);
                    bool key2 = Input.GetKey(CardViewInternals.PrevCardKey);
                    if (!key && !key2)
                    {
                        this.cooldown = 0f;
                        return;
                    }
                    this.cooldown -= Time.deltaTime;
                    if (this.cooldown > 0f)
                    {
                        return;
                    }
                    if (key && key2)
                    {
                        return;
                    }
                    this.cooldown = CardViewInternals.CardChangeRepeatHoldDelay;
                    if (key)
                    {
                        this.NextCard();
                        return;
                    }
                    this.PrevCard();
                }
            }

            [SerializeField]
            public UILogBattleDiceCardUI cardObject;

            [SerializeField]
            public Image nextArrow;

            [SerializeField]
            public Image prevArrow;

            [SerializeField]
            public GameObject arrowsRoot;

            [SerializeField]
            public GameObject previewRoot;

            public List<BattleDiceCardModel> cardList;

            public int curIndex;

            public BattleDiceCardUI.Option[] curOptions;

            public float cooldown;
        }



        /// <summary>
        /// This is quite literally just a bunch of code adapted from BattleCardEnhancedView<br></br> 
        /// made to work with abcdcode's custom card UI class.
        /// </summary>
        public static class BCEVLoglikeExtensions
        {
            public static void ConfigureBattleCard(UILogBattleDiceCardUI __instance, int cardCount, bool resetScrolls)
            {
                UICardSlotScroller uicardSlotScroller = __instance.GetComponent<UICardSlotScroller>();
                CardViewPatches.ConfigureKeywordList(__instance.KeywordListUI, __instance.gameObject, ref uicardSlotScroller, true);
                RectTransform rectTransform = __instance.img_behaviourDetatilList[0].transform.parent as RectTransform;
                RectTransform rectTransform2 = rectTransform.parent as RectTransform;
                if (!rectTransform2.GetComponent<ScrollRect>())
                {
                    Vector2 anchoredPosition = rectTransform.anchoredPosition;
                    anchoredPosition.x = 0f;
                    if (__instance.isProfileCard)
                    {
                        if (anchoredPosition.y < -400f)
                        {
                            anchoredPosition.y = -400f;
                        }
                        foreach (TextMeshProUGUI textMeshProUGUI in __instance.txt_Resist)
                        {
                            textMeshProUGUI.transform.localPosition = new Vector2(-10f, -130f);
                        }
                        foreach (TextMeshProUGUI textMeshProUGUI2 in __instance.txt_bpResist)
                        {
                            textMeshProUGUI2.transform.localPosition = new Vector2(-20f, -220f);
                        }
                    }
                    rectTransform.anchoredPosition = anchoredPosition;
                    GameObject gameObject = new GameObject("[RectMask]BehaviourDetailList");
                    RectTransform rectTransform3 = gameObject.AddComponent<RectTransform>();
                    rectTransform3.SetParent(rectTransform2);
                    rectTransform3.SetSiblingIndex(rectTransform.GetSiblingIndex());
                    rectTransform3.localScale = Vector3.one;
                    rectTransform3.localRotation = Quaternion.identity;
                    rectTransform3.anchoredPosition3D = new Vector3(0f, -500f, 0f);
                    rectTransform3.sizeDelta = new Vector2(870f, 1000f);
                    gameObject.AddComponent<Image>().raycastTarget = false;
                    gameObject.AddComponent<Mask>().showMaskGraphic = false;
                    rectTransform.SetParent(rectTransform3, true);
                    rectTransform2.localScale = Vector3.one;
                    GameObject gameObject2 = new GameObject("[ScrollRect]BehaviourDetailList");
                    RectTransform rectTransform4 = gameObject2.AddComponent<RectTransform>();
                    rectTransform4.SetParent(rectTransform3);
                    rectTransform4.localRotation = rectTransform.localRotation;
                    rectTransform4.localScale = Vector3.one;
                    rectTransform4.pivot = rectTransform.pivot;
                    rectTransform4.anchorMin = rectTransform.anchorMin;
                    rectTransform4.anchorMax = rectTransform.anchorMax;
                    rectTransform4.anchoredPosition3D = rectTransform.anchoredPosition3D;
                    rectTransform4.sizeDelta = new Vector2(925f, rectTransform.sizeDelta.y);
                    rectTransform.SetParent(rectTransform4, true);
                    rectTransform2.localScale = Vector3.one;
                    rectTransform.pivot = new Vector2(0f, 0.5f);
                    if (!rectTransform.GetComponent<ContentSizeFitter>())
                    {
                        ContentSizeFitter contentSizeFitter = rectTransform.gameObject.AddComponent<ContentSizeFitter>();
                        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                    }
                    ScrollRect scrollRect = gameObject2.AddComponent<ScrollRect>();
                    scrollRect.vertical = false;
                    scrollRect.horizontal = true;
                    scrollRect.content = rectTransform;
                    scrollRect.scrollSensitivity = 250f;
                    scrollRect.movementType = ScrollRect.MovementType.Clamped;
                    if (!uicardSlotScroller)
                    {
                        uicardSlotScroller = __instance.gameObject.AddComponent<UICardSlotScroller>();
                    }
                    ScrollRectHandler scrollRectHandler = gameObject2.AddComponent<ScrollRectHandler>();
                    scrollRectHandler.scrollRect = scrollRect;
                    scrollRectHandler.axis = RectTransform.Axis.Horizontal;
                    uicardSlotScroller.scrollHandlers.Add(scrollRectHandler);
                }
                RectTransform rectTransform5 = __instance.ui_behaviourDescList[0].transform.parent as RectTransform;
                RectTransform rectTransform6 = rectTransform5.parent as RectTransform;
                if (!rectTransform6.GetComponent<ScrollRect>())
                {
                    float num = CardViewInternals.Config.CardAbilityMaxFontSize * 10f;
                    float num2 = CardViewInternals.Config.CardAbilityMinFontSize * 10f;
                    float num3 = num2 * (float)CardViewInternals.Config.CardAbilityMaxBaseLines;
                    List<AbilityDescSizeController> list = null;
                    if (!CardViewInternals.Config.DisableDiceLayoutChange)
                    {
                        list = new List<AbilityDescSizeController>();
                        foreach (BattleDiceCard_BehaviourDescUI battleDiceCard_BehaviourDescUI in __instance.ui_behaviourDescList)
                        {
                            RectTransform rectTransform7 = battleDiceCard_BehaviourDescUI.transform as RectTransform;
                            rectTransform7.pivot = new Vector2(0.5f, 1f);
                            TextMeshProUGUI txt_range = battleDiceCard_BehaviourDescUI.txt_range;
                            RectTransform rectTransform8 = txt_range.transform as RectTransform;
                            if (rectTransform8.parent != rectTransform7)
                            {
                                rectTransform8.SetParent(rectTransform7, true);
                            }
                            if (rectTransform8.anchorMin.y == 0.5f && rectTransform8.anchorMax.y == 0.5f)
                            {
                                Vector2 anchoredPosition2 = rectTransform8.anchoredPosition;
                                anchoredPosition2.y = 0f;
                                rectTransform8.anchoredPosition = anchoredPosition2;
                            }
                            RectTransform rectTransform9 = battleDiceCard_BehaviourDescUI.img_detail.transform as RectTransform;
                            while (rectTransform9 && rectTransform9 != rectTransform7)
                            {
                                if (rectTransform9.anchorMin.y == 0.5f && rectTransform9.anchorMax.y == 0.5f)
                                {
                                    Vector2 anchoredPosition3 = rectTransform9.anchoredPosition;
                                    anchoredPosition3.y = 0f;
                                    rectTransform9.anchoredPosition = anchoredPosition3;
                                }
                                rectTransform9 = (rectTransform9.parent as RectTransform);
                            }
                            rectTransform7.sizeDelta = new Vector2(rectTransform7.sizeDelta.x, num3);
                            foreach (object obj in rectTransform7)
                            {
                                RectTransform rectTransform10 = ((Transform)obj) as RectTransform;
                                if (rectTransform10 != null && rectTransform10.anchorMin.y == 0.5f && rectTransform10.anchorMax.y == 0.5f)
                                {
                                    Vector3 localPosition = rectTransform10.localPosition;
                                    rectTransform10.anchorMin = new Vector2(rectTransform10.anchorMin.x, 1f);
                                    rectTransform10.anchorMax = new Vector2(rectTransform10.anchorMax.x, 1f);
                                    rectTransform10.localPosition = localPosition;
                                }
                            }
                            txt_range.alignment = TextAlignmentOptions.Left;
                            txt_range.fontSizeMax = 100f;
                            txt_range.enableWordWrapping = true;
                            rectTransform8.anchorMin = new Vector2(0f, 1f);
                            rectTransform8.offsetMin = new Vector2(150f, -num3);
                            rectTransform8.anchorMax = new Vector2(1f, 1f);
                            rectTransform8.offsetMax = new Vector2(0f, 0f);
                            TextMeshProUGUI txt_ability = battleDiceCard_BehaviourDescUI.txt_ability;
                            RectTransform rectTransform11 = txt_ability.transform as RectTransform;
                            txt_ability.fontSizeMax = num;
                            txt_ability.fontSizeMin = num2;
                            txt_ability.alignment = TextAlignmentOptions.Left;
                            rectTransform11.transform.localScale = Vector3.one;
                            rectTransform11.pivot = new Vector2(0.5f, 1f);
                            rectTransform11.anchorMin = new Vector2(0f, 1f);
                            rectTransform11.offsetMin = new Vector2(300f, -(num3 + num2 * 0.1f));
                            rectTransform11.anchorMax = new Vector2(1f, 1f);
                            rectTransform11.offsetMax = new Vector2(0f, num2 * 0.1f);
                            GameObject gameObject3 = new GameObject("[Text]Behaviour_Ability_Overflow");
                            RectTransform rectTransform12 = gameObject3.AddComponent<RectTransform>();
                            rectTransform12.SetParent(rectTransform8);
                            rectTransform12.localScale = Vector3.one;
                            rectTransform12.localRotation = Quaternion.identity;
                            rectTransform12.pivot = new Vector2(0.5f, 1f);
                            rectTransform12.anchorMin = new Vector2(0f, 0f);
                            rectTransform12.offsetMin = new Vector2(-50f, -50f);
                            rectTransform12.anchorMax = new Vector2(1f, 0f);
                            rectTransform12.offsetMax = new Vector2(0f, 0f);
                            TextMeshProUGUI textMeshProUGUI3 = gameObject3.AddComponent<TextMeshProUGUI>();
                            textMeshProUGUI3.fontSize = txt_ability.fontSizeMin;
                            if (txt_ability.font)
                            {
                                textMeshProUGUI3.font = txt_ability.font;
                            }
                            textMeshProUGUI3.color = txt_ability.color;
                            textMeshProUGUI3.overflowMode = TextOverflowModes.Overflow;
                            textMeshProUGUI3.alignment = TextAlignmentOptions.Left;
                            textMeshProUGUI3.ignoreRectMaskCulling = true;
                            txt_ability.overflowMode = TextOverflowModes.Linked;
                            txt_ability.linkedTextComponent = textMeshProUGUI3;
                            AbilityDescSizeController abilityDescSizeController = battleDiceCard_BehaviourDescUI.gameObject.AddComponent<AbilityDescSizeController>();
                            abilityDescSizeController.rect = rectTransform7;
                            abilityDescSizeController.baselineText = txt_range;
                            abilityDescSizeController.abilityTextFixed = txt_ability;
                            abilityDescSizeController.abilityTextFlex = textMeshProUGUI3;
                            abilityDescSizeController.abilityTextMinHeight = 75f;
                            abilityDescSizeController.fixedAbilityTextHSpacing = 20f;
                            abilityDescSizeController.flexCheck = new Vector2(-50f, -50f);
                            list.Add(abilityDescSizeController);
                        }
                        RectTransform rectTransform13 = __instance.selfAbilityArea.transform as RectTransform;
                        RectTransform rectTransform14 = __instance.txt_selfAbility.transform as RectTransform;
                        rectTransform13.pivot = new Vector2(0.5f, 1f);
                        rectTransform14.anchorMin = new Vector2(rectTransform14.anchorMin.x, 1f);
                        rectTransform14.anchorMax = new Vector2(rectTransform14.anchorMax.x, 1f);
                        rectTransform14.pivot = new Vector2(rectTransform14.pivot.x, 1f);
                        rectTransform14.anchoredPosition = new Vector2(rectTransform14.anchoredPosition.x, 0f);
                        rectTransform14.offsetMin = new Vector2(rectTransform14.offsetMin.x - 100f, rectTransform14.offsetMin.y);
                        __instance.txt_selfAbility.fontSizeMax = num;
                        __instance.txt_selfAbility.fontSizeMin = num2;
                        __instance.txt_selfAbility.overflowMode = TextOverflowModes.Overflow;
                        AbilityDescSizeController abilityDescSizeController2 = __instance.selfAbilityArea.gameObject.AddComponent<AbilityDescSizeController>();
                        abilityDescSizeController2.rect = rectTransform13;
                        abilityDescSizeController2.abilityTextFlex = __instance.txt_selfAbility;
                        abilityDescSizeController2.abilityTextMinHeight = num;
                        list.Add(abilityDescSizeController2);
                    }
                    GameObject gameObject4 = new GameObject("[ScrollRect]BehaviourList");
                    RectTransform rectTransform15 = gameObject4.AddComponent<RectTransform>();
                    rectTransform15.SetParent(rectTransform6);
                    rectTransform15.localScale = rectTransform5.localScale;
                    rectTransform15.localRotation = rectTransform5.localRotation;
                    rectTransform15.anchorMin = rectTransform5.anchorMin;
                    rectTransform15.anchorMax = rectTransform5.anchorMax;
                    VerticalLayoutGroup component = rectTransform5.GetComponent<VerticalLayoutGroup>();
                    int top = component.padding.top;
                    component.padding.top = 30;
                    component.padding.bottom = 50;
                    if (component.spacing < 15f)
                    {
                        component.spacing = 15f;
                    }
                    Vector2 offsetMin = rectTransform5.offsetMin;
                    Vector2 offsetMax = rectTransform5.offsetMax;
                    offsetMax.y -= (float)top;
                    RectTransform rectTransform16 = rectTransform15;
                    RectTransform rectTransform17 = rectTransform5;
                    Vector2 vector = new Vector2(0.5f, 1f);
                    rectTransform17.pivot = vector;
                    rectTransform16.pivot = vector;
                    RectTransform rectTransform18 = rectTransform15;
                    vector = (rectTransform5.offsetMin = offsetMin);
                    rectTransform18.offsetMin = vector;
                    RectTransform rectTransform19 = rectTransform15;
                    vector = (rectTransform5.offsetMax = offsetMax);
                    rectTransform19.offsetMax = vector;
                    rectTransform5.sizeDelta = new Vector2(rectTransform5.sizeDelta.x, 1340f);
                    rectTransform15.sizeDelta = new Vector2(900f, 1340f);
                    rectTransform5.gameObject.AddComponent<LayoutElement>().minHeight = 1340f;
                    rectTransform5.SetParent(rectTransform15, true);
                    rectTransform5.localScale = Vector3.one;
                    rectTransform5.anchoredPosition3D = Vector3.zero;
                    if (list != null)
                    {
                        AbilityDescListSizeFitter abilityDescListSizeFitter = rectTransform5.gameObject.AddComponent<AbilityDescListSizeFitter>();
                        abilityDescListSizeFitter.subControllers = list;
                        using (List<AbilityDescSizeController>.Enumerator enumerator4 = list.GetEnumerator())
                        {
                            while (enumerator4.MoveNext())
                            {
                                AbilityDescSizeController abilityDescSizeController3 = enumerator4.Current;
                                abilityDescSizeController3.mainController = abilityDescListSizeFitter;
                            }
                            goto IL_B4B;
                        }
                    }
                    ContentSizeFitter contentSizeFitter2 = rectTransform5.gameObject.AddComponent<ContentSizeFitter>();
                    contentSizeFitter2.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                    contentSizeFitter2.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                IL_B4B:
                    ScrollRect scrollRect2 = gameObject4.AddComponent<ScrollRect>();
                    scrollRect2.horizontal = false;
                    scrollRect2.vertical = true;
                    scrollRect2.content = rectTransform5;
                    scrollRect2.scrollSensitivity = 250f;
                    scrollRect2.movementType = ScrollRect.MovementType.Clamped;
                    gameObject4.AddComponent<Image>().raycastTarget = false;
                    gameObject4.AddComponent<Mask>().showMaskGraphic = false;
                    if (!uicardSlotScroller)
                    {
                        uicardSlotScroller = __instance.gameObject.AddComponent<UICardSlotScroller>();
                    }
                    ScrollRectHandler scrollRectHandler2 = gameObject4.AddComponent<ScrollRectHandler>();
                    scrollRectHandler2.scrollRect = scrollRect2;
                    uicardSlotScroller.scrollHandlers.Add(scrollRectHandler2);
                }
                int count = __instance.img_behaviourDetatilList.Count;
                if (count < cardCount)
                {
                    Image image = __instance.img_behaviourDetatilList[0];
                    for (int i = count; i < cardCount; i++)
                    {
                        Image image2 = UnityEngine.Object.Instantiate<Image>(image, image.transform.parent);
                        image2.name = string.Format("[Image]Behaviour_Detail{0} (1)", i + 1);
                        __instance.img_behaviourDetatilList.Add(image2);
                        __instance.hsv_behaviourIcons.Add(image2.GetComponent<RefineHsv>());
                        TextMeshProUGUI[] componentsInChildren = image2.GetComponentsInChildren<TextMeshProUGUI>(true);
                        if (__instance.isProfileCard)
                        {
                            componentsInChildren[0].transform.localPosition = new Vector2(-10f, -130f);
                            componentsInChildren[1].transform.localPosition = new Vector2(-20f, -220f);
                        }
                        __instance.txt_Resist.Add(componentsInChildren[0]);
                        __instance.txt_bpResist.Add(componentsInChildren[1]);
                    }
                }
                int count2 = __instance.ui_behaviourDescList.Count;
                if (count2 < cardCount)
                {
                    BattleDiceCard_BehaviourDescUI battleDiceCard_BehaviourDescUI2 = __instance.ui_behaviourDescList[0];
                    for (int j = count2; j < cardCount; j++)
                    {
                        BattleDiceCard_BehaviourDescUI battleDiceCard_BehaviourDescUI3 = UnityEngine.Object.Instantiate<BattleDiceCard_BehaviourDescUI>(battleDiceCard_BehaviourDescUI2, battleDiceCard_BehaviourDescUI2.transform.parent);
                        battleDiceCard_BehaviourDescUI3.name = string.Format("[Image]Behaviour_Baseline ({0})", j);
                        __instance.ui_behaviourDescList.Add(battleDiceCard_BehaviourDescUI3);
                    }
                }
                if (uicardSlotScroller)
                {
                    if (__instance.isProfileCard)
                    {
                        uicardSlotScroller.isFocused = true;
                    }
                    if (resetScrolls)
                    {
                        if (uicardSlotScroller.scrollHandlers.Exists((ScrollRectHandler h) => h.axis == RectTransform.Axis.Horizontal))
                        {
                            if (cardCount <= 5)
                            {
                                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                            }
                            else
                            {
                                rectTransform.pivot = new Vector2(0f, 0.5f);
                            }
                            rectTransform.anchoredPosition = Vector2.zero;
                        }
                        uicardSlotScroller.ResetScrolls();
                    }
                }
            }

            public static void SetBattleCardPreview(UILogBattleDiceCardUI mainUI, BattleDiceCardModel card, BattleDiceCardUI.Option[] options)
            {
                LogLikeBattleDiceCardPreviewUI orCreateUI;
                if (card != null)
                {
                    List<BattleDiceCardModel> battleCardPreviewList = CardViewInternals.GetBattleCardPreviewList(card, options);
                    if (battleCardPreviewList.Count > 0)
                    {
                        orCreateUI = LogLikeBattleDiceCardPreviewUI.GetOrCreateUI(mainUI, true);
                        orCreateUI.SetCardList(battleCardPreviewList, options);
                        return;
                    }
                }
                orCreateUI = LogLikeBattleDiceCardPreviewUI.GetOrCreateUI(mainUI, false);
                if (orCreateUI)
                {
                    orCreateUI.SetCardList(null, null);
                }
            }
            public static void ForceUpdateLeftBehaviourMaterial(UILogBattleDiceCardUI cardUI)
            {
                GameObject gameObject = cardUI.hsv_behaviourIcons[0].transform.parent.gameObject;
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            }
        }

        public class UILogDetailCardSlot : MonoBehaviour
        {
            public static LogLikeMod.UILogDetailCardSlot Instance;
            public RectTransform Pivot;
            public CanvasGroup cg;
            public GameObject ob_NormalFrame;
            public Image[] img_Frames;
            public Image img_Frames_1;
            public Image img_Frames_2;
            public Image[] img_linearDodge;
            public Image img_linearDodge_1;
            public Image img_linearDodge_2;
            public Image[] img_BehaviourIcons;
            public Image img_BehaviourIcons_1;
            public Image img_BehaviourIcons_2;
            public Image img_BehaviourIcons_3;
            public Image img_BehaviourIcons_4;
            public Image img_BehaviourIcons_5;
            public _2dxFX_GrayScale[] gs_BehaviourIcons;
            public _2dxFX_GrayScale gs_BehaviourIcons_1;
            public _2dxFX_GrayScale gs_BehaviourIcons_2;
            public _2dxFX_GrayScale gs_BehaviourIcons_3;
            public _2dxFX_GrayScale gs_BehaviourIcons_4;
            public _2dxFX_GrayScale gs_BehaviourIcons_5;
            public Image img_RangeIcon;
            public NumbersData costNumbers;
            public TextMeshProUGUI txt_cardName;
            public Image img_Artwork;
            public RefineHsv hsv_rangeIcon;
            public UICustomSelectable selectable;
            public DiceCardItemModel _cardModel;
            public Color colorFrame;
            public Color colorLineardodge;
            public int originSiblingIdx = -1;
            public GameObject ob_selfAbility;
            public TextMeshProUGUI txt_selfAbility;
            public List<UIDetailCardDescSlot> rightDescSlotList;
            public UIDetailCardDescSlot rightDescSlotList_1;
            public UIDetailCardDescSlot rightDescSlotList_2;
            public UIDetailCardDescSlot rightDescSlotList_3;
            public UIDetailCardDescSlot rightDescSlotList_4;
            public UIDetailCardDescSlot rightDescSlotList_5;
            public KeywordListUI keywordListUI_R;
            public KeywordListUI keywordListUI_L;
            public bool OnKeyword = true;

            public static LogLikeMod.UILogDetailCardSlot SlotCopying()
            {
                UIInvenCardListScroll invenCardList = (UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel).EditPanel.BattleCardPanel.InvenCardList;
                UIDetailCardSlot uiDetailCardSlot = (UIDetailCardSlot)typeof(UIInvenCardListScroll).GetField("detailSlot", AccessTools.all).GetValue(invenCardList);
                LogLikeMod.UILogDetailCardSlot original = uiDetailCardSlot.gameObject.AddComponent<LogLikeMod.UILogDetailCardSlot>();
                original.Pivot = (RectTransform)typeof(UIOriginCardSlot).GetField("Pivot", AccessTools.all).GetValue(uiDetailCardSlot);
                original.cg = (CanvasGroup)typeof(UIOriginCardSlot).GetField("cg", AccessTools.all).GetValue(uiDetailCardSlot);
                original.ob_NormalFrame = (GameObject)typeof(UIOriginCardSlot).GetField("ob_NormalFrame", AccessTools.all).GetValue(uiDetailCardSlot);
                original.img_Frames = (Image[])typeof(UIOriginCardSlot).GetField("img_Frames", AccessTools.all).GetValue(uiDetailCardSlot);
                original.img_Frames_1 = original.img_Frames[0];
                original.img_Frames_2 = original.img_Frames[1];
                original.img_linearDodge = (Image[])typeof(UIOriginCardSlot).GetField("img_linearDodge", AccessTools.all).GetValue(uiDetailCardSlot);
                original.img_linearDodge_1 = original.img_linearDodge[0];
                original.img_linearDodge_2 = original.img_linearDodge[1];
                original.img_BehaviourIcons = (Image[])typeof(UIOriginCardSlot).GetField("img_BehaviourIcons", AccessTools.all).GetValue(uiDetailCardSlot);
                original.img_BehaviourIcons_1 = original.img_BehaviourIcons[0];
                original.img_BehaviourIcons_2 = original.img_BehaviourIcons[1];
                original.img_BehaviourIcons_3 = original.img_BehaviourIcons[2];
                original.img_BehaviourIcons_4 = original.img_BehaviourIcons[3];
                original.img_BehaviourIcons_5 = original.img_BehaviourIcons[4];
                original.gs_BehaviourIcons = (_2dxFX_GrayScale[])typeof(UIOriginCardSlot).GetField("gs_BehaviourIcons", AccessTools.all).GetValue(uiDetailCardSlot);
                original.gs_BehaviourIcons_1 = original.gs_BehaviourIcons[0];
                original.gs_BehaviourIcons_2 = original.gs_BehaviourIcons[1];
                original.gs_BehaviourIcons_3 = original.gs_BehaviourIcons[2];
                original.gs_BehaviourIcons_4 = original.gs_BehaviourIcons[3];
                original.gs_BehaviourIcons_5 = original.gs_BehaviourIcons[4];
                original.img_RangeIcon = (Image)typeof(UIOriginCardSlot).GetField("img_RangeIcon", AccessTools.all).GetValue(uiDetailCardSlot);
                original.costNumbers = (NumbersData)typeof(UIOriginCardSlot).GetField("costNumbers", AccessTools.all).GetValue(uiDetailCardSlot);
                original.txt_cardName = (TextMeshProUGUI)typeof(UIOriginCardSlot).GetField("txt_cardName", AccessTools.all).GetValue(uiDetailCardSlot);
                original.img_Artwork = (Image)typeof(UIOriginCardSlot).GetField("img_Artwork", AccessTools.all).GetValue(uiDetailCardSlot);
                original.hsv_rangeIcon = (RefineHsv)typeof(UIOriginCardSlot).GetField("hsv_rangeIcon", AccessTools.all).GetValue(uiDetailCardSlot);
                original.selectable = (UICustomSelectable)typeof(UIOriginCardSlot).GetField("selectable", AccessTools.all).GetValue(uiDetailCardSlot);
                original._cardModel = (DiceCardItemModel)typeof(UIOriginCardSlot).GetField("_cardModel", AccessTools.all).GetValue(uiDetailCardSlot);
                original.colorFrame = (Color)typeof(UIOriginCardSlot).GetField("colorFrame", AccessTools.all).GetValue(uiDetailCardSlot);
                original.colorLineardodge = (Color)typeof(UIOriginCardSlot).GetField("colorLineardodge", AccessTools.all).GetValue(uiDetailCardSlot);
                original.originSiblingIdx = (int)typeof(UIOriginCardSlot).GetField("originSiblingIdx", AccessTools.all).GetValue(uiDetailCardSlot);
                original.ob_selfAbility = (GameObject)typeof(UIDetailCardSlot).GetField("ob_selfAbility", AccessTools.all).GetValue(uiDetailCardSlot);
                original.txt_selfAbility = (TextMeshProUGUI)typeof(UIDetailCardSlot).GetField("txt_selfAbility", AccessTools.all).GetValue(uiDetailCardSlot);
                original.rightDescSlotList = (List<UIDetailCardDescSlot>)typeof(UIDetailCardSlot).GetField("rightDescSlotList", AccessTools.all).GetValue(uiDetailCardSlot);
                original.rightDescSlotList_1 = original.rightDescSlotList[0];
                original.rightDescSlotList_2 = original.rightDescSlotList[1];
                original.rightDescSlotList_3 = original.rightDescSlotList[2];
                original.rightDescSlotList_4 = original.rightDescSlotList[3];
                original.rightDescSlotList_5 = original.rightDescSlotList[4];
                original.keywordListUI_R = (KeywordListUI)typeof(UIDetailCardSlot).GetField("keywordListUI_R", AccessTools.all).GetValue(uiDetailCardSlot);
                original.keywordListUI_L = (KeywordListUI)typeof(UIDetailCardSlot).GetField("keywordListUI_L", AccessTools.all).GetValue(uiDetailCardSlot);
                original.OnKeyword = (bool)typeof(UIDetailCardSlot).GetField("OnKeyword", AccessTools.all).GetValue(uiDetailCardSlot);
                LogLikeMod.UILogDetailCardSlot logDetailCardSlot = UnityEngine.Object.Instantiate<LogLikeMod.UILogDetailCardSlot>(original, SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.transform);
                logDetailCardSlot.img_Frames[0] = logDetailCardSlot.img_Frames_1;
                logDetailCardSlot.img_Frames[1] = logDetailCardSlot.img_Frames_2;
                logDetailCardSlot.img_linearDodge[0] = logDetailCardSlot.img_linearDodge_1;
                logDetailCardSlot.img_linearDodge[1] = logDetailCardSlot.img_linearDodge_2;
                logDetailCardSlot.img_BehaviourIcons[0] = logDetailCardSlot.img_BehaviourIcons_1;
                logDetailCardSlot.img_BehaviourIcons[1] = logDetailCardSlot.img_BehaviourIcons_2;
                logDetailCardSlot.img_BehaviourIcons[2] = logDetailCardSlot.img_BehaviourIcons_3;
                logDetailCardSlot.img_BehaviourIcons[3] = logDetailCardSlot.img_BehaviourIcons_4;
                logDetailCardSlot.img_BehaviourIcons[4] = logDetailCardSlot.img_BehaviourIcons_5;
                logDetailCardSlot.gs_BehaviourIcons[0] = logDetailCardSlot.gs_BehaviourIcons_1;
                logDetailCardSlot.gs_BehaviourIcons[1] = logDetailCardSlot.gs_BehaviourIcons_2;
                logDetailCardSlot.gs_BehaviourIcons[2] = logDetailCardSlot.gs_BehaviourIcons_3;
                logDetailCardSlot.gs_BehaviourIcons[3] = logDetailCardSlot.gs_BehaviourIcons_4;
                logDetailCardSlot.gs_BehaviourIcons[4] = logDetailCardSlot.gs_BehaviourIcons_5;
                logDetailCardSlot.rightDescSlotList[0] = logDetailCardSlot.rightDescSlotList_1;
                logDetailCardSlot.rightDescSlotList[1] = logDetailCardSlot.rightDescSlotList_2;
                logDetailCardSlot.rightDescSlotList[2] = logDetailCardSlot.rightDescSlotList_3;
                logDetailCardSlot.rightDescSlotList[3] = logDetailCardSlot.rightDescSlotList_4;
                logDetailCardSlot.rightDescSlotList[4] = logDetailCardSlot.rightDescSlotList_5;
                UnityEngine.Object.Destroy(original);
                logDetailCardSlot.transform.GetChild(0).transform.localPosition = new Vector3(0.0f, 0.0f);
                return logDetailCardSlot;
            }

            public void SetLinearDodgeColor(Color c)
            {
                for (int index = 0; index < this.img_linearDodge.Length; ++index)
                {
                    this.img_linearDodge[index].color = c;
                    if (!(this.img_linearDodge[index] == null))
                        this.img_linearDodge[index].color = c;
                }
            }

            public void SetFrameColor(Color c)
            {
                for (int index = 0; index < this.img_Frames.Length; ++index)
                {
                    this.img_Frames[index].color = c;
                    if (!(this.img_Frames[index] == null))
                        this.img_Frames[index].color = c;
                }
            }

            public void SetRangeIconHsv(Vector3 hsvvalue)
            {
                this.img_RangeIcon.color = Color.white;
                if (this.hsv_rangeIcon == null)
                    this.hsv_rangeIcon = this.img_RangeIcon.GetComponent<RefineHsv>();
                if (this.hsv_rangeIcon == null)
                {
                    Debug.LogError("Hsv Not Reference");
                }
                else
                {
                    this.hsv_rangeIcon._HueShift = hsvvalue.x;
                    this.hsv_rangeIcon._Saturation = hsvvalue.y;
                    this.hsv_rangeIcon._ValueBrightness = hsvvalue.z;
                    this.hsv_rangeIcon.CallUpdate();
                    this.hsv_rangeIcon.enabled = false;
                    this.hsv_rangeIcon.enabled = true;
                }
            }

            public void BSetData(DiceCardItemModel cardmodel)
            {
                this._cardModel = cardmodel;
                if (this._cardModel == null)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    if (!this.ob_NormalFrame.activeSelf)
                        this.ob_NormalFrame.gameObject.SetActive(true);
                    if (LorId.IsModId(this._cardModel.ClassInfo.workshopID))
                        this.txt_cardName.text = RewardingModel.SanitizeDisplayText(this._cardModel.GetName());
                    else
                        this.txt_cardName.text = RewardingModel.SanitizeDisplayText(Singleton<BattleCardDescXmlList>.Instance.GetCardName(this._cardModel.GetTextId()));
                    this.costNumbers.SetOneValue(this._cardModel.GetSpec().Cost, UISpriteDataManager.instance._cardCostNumberSprites);
                    this.img_RangeIcon.sprite = UISpriteDataManager.instance.GetRangeIconSprite(this._cardModel.GetSpec().Ranged);
                    this.SetRangeIconHsv(UIColorManager.Manager.CardRangeHsvValue[(int)this._cardModel.GetRarity()]);
                    this.colorFrame = UIColorManager.Manager.GetCardRarityColor(this._cardModel.GetRarity());
                    this.colorLineardodge = UIColorManager.Manager.GetCardRarityLinearColor(this._cardModel.GetRarity());
                    this.SetFrameColor(this.colorFrame);
                    this.SetLinearDodgeColor(this.colorLineardodge);
                    this.costNumbers.SetContentColor(this.colorFrame);
                    List<DiceBehaviour> behaviourList = this._cardModel.GetBehaviourList();
                    for (int index = 0; index < this.img_BehaviourIcons.Length; ++index)
                    {
                        if (index < behaviourList.Count)
                        {
                            Sprite sprite = behaviourList[index].Type == BehaviourType.Standby ? UISpriteDataManager.instance.CardStandbyBehaviourDetailIcons[(int)behaviourList[index].Detail] : UISpriteDataManager.instance._cardBehaviourDetailIcons[(int)behaviourList[index].Detail];
                            this.img_BehaviourIcons[index].sprite = sprite;
                            this.img_BehaviourIcons[index].gameObject.SetActive(true);
                        }
                        else
                            this.img_BehaviourIcons[index].gameObject.SetActive(false);
                    }
                    Sprite sprite1;
                    if (LorId.IsModId(this._cardModel.ClassInfo.workshopID))
                    {
                        sprite1 = Singleton<CustomizingCardArtworkLoader>.Instance.GetSpecificArtworkSprite(this._cardModel.ClassInfo.workshopID, this._cardModel.GetArtworkSrc());
                        if (sprite1 == null)
                            sprite1 = Singleton<AssetBundleManagerRemake>.Instance.LoadCardSprite(this._cardModel.GetArtworkSrc());
                    }
                    else
                        sprite1 = Singleton<AssetBundleManagerRemake>.Instance.LoadCardSprite(this._cardModel.GetArtworkSrc());
                    if (sprite1 != null)
                        this.img_Artwork.sprite = sprite1;
                    else
                        Debug.Log("Can't find sprite");
                    this.gameObject.SetActive(true);
                }
            }

            public void SetData(DiceCardItemModel cardmodel)
            {
                this.BSetData(cardmodel);
                if (this._cardModel == null)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    List<DiceBehaviour> behaviourList = this._cardModel.GetBehaviourList();
                    int b = 4 - behaviourList.Count;
                    if (this.ob_selfAbility != null)
                    {
                        string text = Singleton<BattleCardDescXmlList>.Instance.GetAbilityDesc(this._cardModel.GetID());
                        if (string.IsNullOrEmpty(text))
                        {
                            List<string> abilityDesc = Singleton<BattleCardAbilityDescXmlList>.Instance.GetAbilityDesc(this._cardModel.ClassInfo);
                            if (abilityDesc.Count > 0)
                                text = string.Join("\n", (IEnumerable<string>)abilityDesc);
                        }
                        else
                        {
                            string abilityDescString = Singleton<BattleCardAbilityDescXmlList>.Instance.GetDefaultAbilityDescString(this._cardModel.ClassInfo);
                            if (!string.IsNullOrEmpty(abilityDescString))
                                text = $"{abilityDescString}\n{text}";
                        }
                        if (!string.IsNullOrEmpty(text))
                        {
                            this.ob_selfAbility.SetActive(true);
                            this.txt_selfAbility.text = RewardingModel.SanitizeDisplayText(TextUtil.TransformConditionKeyword(text));
                            float preferredHeight = this.txt_selfAbility.preferredHeight;
                            int num = Mathf.Min((double)preferredHeight >= 26.0 ? ((double)preferredHeight >= 48.0 ? ((double)preferredHeight >= 70.0 ? 3 : 2) : 1) : 0, b);
                            RectTransform component = this.ob_selfAbility.GetComponent<RectTransform>();
                            if (component != null)
                            {
                                switch (num)
                                {
                                    case 1:
                                        component.sizeDelta = new Vector2(component.sizeDelta.x, 44f);
                                        break;
                                    case 2:
                                        component.sizeDelta = new Vector2(component.sizeDelta.x, 66f);
                                        break;
                                    case 3:
                                        component.sizeDelta = new Vector2(component.sizeDelta.x, 88f);
                                        break;
                                    default:
                                        component.sizeDelta = new Vector2(component.sizeDelta.x, 22f);
                                        break;
                                }
                            }
                        }
                        else
                            this.ob_selfAbility.gameObject.SetActive(false);
                    }
                    for (int index = 0; index < this.rightDescSlotList.Count; ++index)
                    {
                        if (index >= behaviourList.Count)
                        {
                            this.rightDescSlotList[index].gameObject.SetActive(false);
                        }
                        else
                        {
                            this.rightDescSlotList[index].SetBehaviourInfo(behaviourList[index], this._cardModel.GetID(), this._cardModel.GetBehaviourList());
                            this.rightDescSlotList[index].gameObject.SetActive(true);
                        }
                    }
                    if (this.OnKeyword)
                    {
                        if ((double)this.keywordListUI_R.GetComponent<RectTransform>().position.x / (double)Screen.width > 0.75)
                        {
                            if (this.keywordListUI_L != null)
                            {
                                this.keywordListUI_L.Init(this._cardModel.ClassInfo, this._cardModel.GetBehaviourList());
                                this.keywordListUI_L.Activate();
                            }
                            if (!(this.keywordListUI_R != null))
                                return;
                            this.keywordListUI_R.Deactivate();
                        }
                        else
                        {
                            this.keywordListUI_R.Init(this._cardModel.ClassInfo, this._cardModel.GetBehaviourList());
                            this.keywordListUI_R.Activate();
                            if (!(this.keywordListUI_L != null))
                                return;
                            this.keywordListUI_L.Deactivate();
                        }
                    }
                    else
                    {
                        this.keywordListUI_R.Deactivate();
                        this.keywordListUI_L.Deactivate();
                    }
                }
            }
        }

        public class UILogCardSlot : MonoBehaviour
        {
            public static LogLikeMod.UILogCardSlot Original;
            public RectTransform Pivot;
            public CanvasGroup cg;
            public GameObject ob_NormalFrame;
            public Image[] img_Frames;
            public Image img_Frames_1;
            public Image img_Frames_2;
            public Image[] img_linearDodge;
            public Image img_linearDodge_1;
            public Image img_linearDodge_2;
            public Image[] img_BehaviourIcons;
            public Image img_BehaviourIcons_1;
            public Image img_BehaviourIcons_2;
            public Image img_BehaviourIcons_3;
            public Image img_BehaviourIcons_4;
            public Image img_BehaviourIcons_5;
            public _2dxFX_GrayScale[] gs_BehaviourIcons;
            public _2dxFX_GrayScale gs_BehaviourIcons_1;
            public _2dxFX_GrayScale gs_BehaviourIcons_2;
            public _2dxFX_GrayScale gs_BehaviourIcons_3;
            public _2dxFX_GrayScale gs_BehaviourIcons_4;
            public _2dxFX_GrayScale gs_BehaviourIcons_5;
            public Image img_RangeIcon;
            public NumbersData costNumbers;
            public TextMeshProUGUI txt_cardName;
            public Image img_Artwork;
            public RefineHsv hsv_rangeIcon;
            public UICustomSelectable selectable;
            public DiceCardItemModel _cardModel;
            public Color colorFrame;
            public Color colorLineardodge;
            public int originSiblingIdx = -1;
            public UIInvenCardListScroll listPanel;
            public Image img_cardNumberBg;
            public TextMeshProUGUI txt_cardNumbers;
            public GameObject deckLimitRoot;
            public TextMeshProUGUI txt_deckLimit;
            public UICustomGraphicObject EquipInfoButton;
            public Animator EquipInfoButtonAnim;
            public CanvasGroup cg_LeftPanel;
            public CanvasGroup cg_EmptyFrameRoot;
            public UIINVENCARD_STATE slotState;
            public bool isEmpty;

            public static LogLikeMod.UILogCardSlot SlotCopyingByOrig()
            {
                LogLikeMod.UILogCardSlot uiLogCardSlot = UnityEngine.Object.Instantiate<LogLikeMod.UILogCardSlot>(LogLikeMod.UILogCardSlot.Original);
                uiLogCardSlot.img_Frames[0] = uiLogCardSlot.img_Frames_1;
                uiLogCardSlot.img_Frames[1] = uiLogCardSlot.img_Frames_2;
                uiLogCardSlot.img_linearDodge[0] = uiLogCardSlot.img_linearDodge_1;
                uiLogCardSlot.img_linearDodge[1] = uiLogCardSlot.img_linearDodge_2;
                uiLogCardSlot.img_BehaviourIcons[0] = uiLogCardSlot.img_BehaviourIcons_1;
                uiLogCardSlot.img_BehaviourIcons[1] = uiLogCardSlot.img_BehaviourIcons_2;
                uiLogCardSlot.img_BehaviourIcons[2] = uiLogCardSlot.img_BehaviourIcons_3;
                uiLogCardSlot.img_BehaviourIcons[3] = uiLogCardSlot.img_BehaviourIcons_4;
                uiLogCardSlot.img_BehaviourIcons[4] = uiLogCardSlot.img_BehaviourIcons_5;
                uiLogCardSlot.gs_BehaviourIcons[0] = uiLogCardSlot.gs_BehaviourIcons_1;
                uiLogCardSlot.gs_BehaviourIcons[1] = uiLogCardSlot.gs_BehaviourIcons_2;
                uiLogCardSlot.gs_BehaviourIcons[2] = uiLogCardSlot.gs_BehaviourIcons_3;
                uiLogCardSlot.gs_BehaviourIcons[3] = uiLogCardSlot.gs_BehaviourIcons_4;
                uiLogCardSlot.gs_BehaviourIcons[4] = uiLogCardSlot.gs_BehaviourIcons_5;
                uiLogCardSlot.selectable.SubmitEvent.RemoveAllListeners();
                uiLogCardSlot.selectable.SubmitEvent.AddListener(new UnityAction<BaseEventData>(uiLogCardSlot.OnPointerClick));
                uiLogCardSlot.selectable.SelectEvent.RemoveAllListeners();
                uiLogCardSlot.selectable.SelectEvent.AddListener(new UnityAction<BaseEventData>(uiLogCardSlot.OnPointerEnter));
                uiLogCardSlot.selectable.DeselectEvent.RemoveAllListeners();
                uiLogCardSlot.selectable.DeselectEvent.AddListener(new UnityAction<BaseEventData>(uiLogCardSlot.OnPointerExit));
                uiLogCardSlot.selectable.XEvent.RemoveAllListeners();
                return uiLogCardSlot;
            }

            public static LogLikeMod.UILogCardSlot SlotCopying()
            {
                UIInvenCardListScroll invenCardList = (UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel).EditPanel.BattleCardPanel.InvenCardList;
                UIOriginCardSlot componentsInChild = ((Component)typeof(UIOriginCardList).GetField("transform_CardListRoot", AccessTools.all).GetValue(invenCardList)).GetComponentsInChildren<UIOriginCardSlot>(true)[0];
                LogLikeMod.UILogCardSlot original = componentsInChild.gameObject.AddComponent<LogLikeMod.UILogCardSlot>();
                original.Pivot = (RectTransform)typeof(UIOriginCardSlot).GetField("Pivot", AccessTools.all).GetValue(componentsInChild);
                original.cg = (CanvasGroup)typeof(UIOriginCardSlot).GetField("cg", AccessTools.all).GetValue(componentsInChild);
                original.ob_NormalFrame = (GameObject)typeof(UIOriginCardSlot).GetField("ob_NormalFrame", AccessTools.all).GetValue(componentsInChild);
                original.img_Frames = (Image[])typeof(UIOriginCardSlot).GetField("img_Frames", AccessTools.all).GetValue(componentsInChild);
                original.img_Frames_1 = original.img_Frames[0];
                original.img_Frames_2 = original.img_Frames[1];
                original.img_linearDodge = (Image[])typeof(UIOriginCardSlot).GetField("img_linearDodge", AccessTools.all).GetValue(componentsInChild);
                original.img_linearDodge_1 = original.img_linearDodge[0];
                original.img_linearDodge_2 = original.img_linearDodge[1];
                original.img_BehaviourIcons = (Image[])typeof(UIOriginCardSlot).GetField("img_BehaviourIcons", AccessTools.all).GetValue(componentsInChild);
                original.img_BehaviourIcons_1 = original.img_BehaviourIcons[0];
                original.img_BehaviourIcons_2 = original.img_BehaviourIcons[1];
                original.img_BehaviourIcons_3 = original.img_BehaviourIcons[2];
                original.img_BehaviourIcons_4 = original.img_BehaviourIcons[3];
                original.img_BehaviourIcons_5 = original.img_BehaviourIcons[4];
                original.gs_BehaviourIcons = (_2dxFX_GrayScale[])typeof(UIOriginCardSlot).GetField("gs_BehaviourIcons", AccessTools.all).GetValue(componentsInChild);
                original.gs_BehaviourIcons_1 = original.gs_BehaviourIcons[0];
                original.gs_BehaviourIcons_2 = original.gs_BehaviourIcons[1];
                original.gs_BehaviourIcons_3 = original.gs_BehaviourIcons[2];
                original.gs_BehaviourIcons_4 = original.gs_BehaviourIcons[3];
                original.gs_BehaviourIcons_5 = original.gs_BehaviourIcons[4];
                original.img_RangeIcon = (Image)typeof(UIOriginCardSlot).GetField("img_RangeIcon", AccessTools.all).GetValue(componentsInChild);
                original.costNumbers = (NumbersData)typeof(UIOriginCardSlot).GetField("costNumbers", AccessTools.all).GetValue(componentsInChild);
                original.txt_cardName = (TextMeshProUGUI)typeof(UIOriginCardSlot).GetField("txt_cardName", AccessTools.all).GetValue(componentsInChild);
                original.img_Artwork = (Image)typeof(UIOriginCardSlot).GetField("img_Artwork", AccessTools.all).GetValue(componentsInChild);
                original.hsv_rangeIcon = (RefineHsv)typeof(UIOriginCardSlot).GetField("hsv_rangeIcon", AccessTools.all).GetValue(componentsInChild);
                original.selectable = (UICustomSelectable)typeof(UIOriginCardSlot).GetField("selectable", AccessTools.all).GetValue(componentsInChild);
                original._cardModel = (DiceCardItemModel)typeof(UIOriginCardSlot).GetField("_cardModel", AccessTools.all).GetValue(componentsInChild);
                original.colorFrame = (Color)typeof(UIOriginCardSlot).GetField("colorFrame", AccessTools.all).GetValue(componentsInChild);
                original.colorLineardodge = (Color)typeof(UIOriginCardSlot).GetField("colorLineardodge", AccessTools.all).GetValue(componentsInChild);
                original.originSiblingIdx = (int)typeof(UIOriginCardSlot).GetField("originSiblingIdx", AccessTools.all).GetValue(componentsInChild);
                original.listPanel = (UIInvenCardListScroll)typeof(UIInvenCardSlot).GetField("listPanel", AccessTools.all).GetValue(componentsInChild);
                original.img_cardNumberBg = (Image)typeof(UIInvenCardSlot).GetField("img_cardNumberBg", AccessTools.all).GetValue(componentsInChild);
                original.txt_cardNumbers = (TextMeshProUGUI)typeof(UIInvenCardSlot).GetField("txt_cardNumbers", AccessTools.all).GetValue(componentsInChild);
                original.deckLimitRoot = (GameObject)typeof(UIInvenCardSlot).GetField("deckLimitRoot", AccessTools.all).GetValue(componentsInChild);
                original.txt_deckLimit = (TextMeshProUGUI)typeof(UIInvenCardSlot).GetField("txt_deckLimit", AccessTools.all).GetValue(componentsInChild);
                original.EquipInfoButton = (UICustomGraphicObject)typeof(UIInvenCardSlot).GetField("EquipInfoButton", AccessTools.all).GetValue(componentsInChild);
                original.EquipInfoButtonAnim = (Animator)typeof(UIInvenCardSlot).GetField("EquipInfoButtonAnim", AccessTools.all).GetValue(componentsInChild);
                original.cg_LeftPanel = (CanvasGroup)typeof(UIInvenCardSlot).GetField("cg_LeftPanel", AccessTools.all).GetValue(componentsInChild);
                original.cg_EmptyFrameRoot = (CanvasGroup)typeof(UIInvenCardSlot).GetField("cg_EmptyFrameRoot", AccessTools.all).GetValue(componentsInChild);
                original.slotState = (UIINVENCARD_STATE)typeof(UIInvenCardSlot).GetField("slotState", AccessTools.all).GetValue(componentsInChild);
                original.isEmpty = (bool)typeof(UIInvenCardSlot).GetField("isEmpty", AccessTools.all).GetValue(componentsInChild);
                LogLikeMod.UILogCardSlot uiLogCardSlot = UnityEngine.Object.Instantiate<LogLikeMod.UILogCardSlot>(original);
                uiLogCardSlot.img_Frames[0] = uiLogCardSlot.img_Frames_1;
                uiLogCardSlot.img_Frames[1] = uiLogCardSlot.img_Frames_2;
                uiLogCardSlot.img_linearDodge[0] = uiLogCardSlot.img_linearDodge_1;
                uiLogCardSlot.img_linearDodge[1] = uiLogCardSlot.img_linearDodge_2;
                uiLogCardSlot.img_BehaviourIcons[0] = uiLogCardSlot.img_BehaviourIcons_1;
                uiLogCardSlot.img_BehaviourIcons[1] = uiLogCardSlot.img_BehaviourIcons_2;
                uiLogCardSlot.img_BehaviourIcons[2] = uiLogCardSlot.img_BehaviourIcons_3;
                uiLogCardSlot.img_BehaviourIcons[3] = uiLogCardSlot.img_BehaviourIcons_4;
                uiLogCardSlot.img_BehaviourIcons[4] = uiLogCardSlot.img_BehaviourIcons_5;
                uiLogCardSlot.gs_BehaviourIcons[0] = uiLogCardSlot.gs_BehaviourIcons_1;
                uiLogCardSlot.gs_BehaviourIcons[1] = uiLogCardSlot.gs_BehaviourIcons_2;
                uiLogCardSlot.gs_BehaviourIcons[2] = uiLogCardSlot.gs_BehaviourIcons_3;
                uiLogCardSlot.gs_BehaviourIcons[3] = uiLogCardSlot.gs_BehaviourIcons_4;
                uiLogCardSlot.gs_BehaviourIcons[4] = uiLogCardSlot.gs_BehaviourIcons_5;
                UnityEngine.Object.Destroy(original);
                typeof(UIInvenCardSlot).GetField("EquipInfoButton", AccessTools.all).SetValue(uiLogCardSlot.gameObject.GetComponent<UIInvenCardSlot>(), null);
                typeof(UIInvenCardSlot).GetField("EquipInfoButtonAnim", AccessTools.all).SetValue(uiLogCardSlot.gameObject.GetComponent<UIInvenCardSlot>(), null);
                UnityEngine.Object.Destroy(uiLogCardSlot.gameObject.GetComponent<UIInvenCardSlot>());
                uiLogCardSlot.selectable.SubmitEvent.RemoveAllListeners();
                uiLogCardSlot.selectable.SubmitEvent.AddListener(new UnityAction<BaseEventData>(uiLogCardSlot.OnPointerClick));
                uiLogCardSlot.selectable.SelectEvent.RemoveAllListeners();
                uiLogCardSlot.selectable.SelectEvent.AddListener(new UnityAction<BaseEventData>(uiLogCardSlot.OnPointerEnter));
                uiLogCardSlot.selectable.DeselectEvent.RemoveAllListeners();
                uiLogCardSlot.selectable.DeselectEvent.AddListener(new UnityAction<BaseEventData>(uiLogCardSlot.OnPointerExit));
                uiLogCardSlot.selectable.XEvent.RemoveAllListeners();
                UnityEngine.Object.Destroy(uiLogCardSlot.EquipInfoButton.gameObject);
                return uiLogCardSlot;
            }

            public void SetCgLeftPanel(bool on)
            {
                if (this.cg_LeftPanel == null)
                    return;
                this.cg_LeftPanel.alpha = on ? 1f : 0.0f;
                this.cg_LeftPanel.blocksRaycasts = on;
                this.cg_LeftPanel.interactable = on;
            }

            public void OnPointerEnter(BaseEventData eventData)
            {
                LogLikeMod.UILogBattleDiceCardUI.Instance.transform.SetParent(this.transform);
                LogLikeMod.UILogBattleDiceCardUI.Instance.gameObject.SetActive(true);
                LogLikeMod.UILogBattleDiceCardUI.Instance.SetCard(BattleDiceCardModel.CreatePlayingCard(this._cardModel.ClassInfo));
                LogLikeMod.UILogBattleDiceCardUI.Instance.transform.localPosition = new Vector3(250f, -100f, -1f);
                LogLikeMod.UILogBattleDiceCardUI.Instance.transform.localScale = new Vector3(0.2f, 0.2f);
            }

            public void OnPointerExit(BaseEventData eventData)
            {
                if (LogLikeMod.UILogBattleDiceCardUI.Instance == null)
                    return;
                LogLikeMod.UILogBattleDiceCardUI.Instance.gameObject.SetActive(false);
            }

            public void OnPointerClick(BaseEventData eventData)
            {
            }

            public void SetLinearDodgeColor(Color c)
            {
                for (int index = 0; index < this.img_linearDodge.Length; ++index)
                {
                    if (!(this.img_linearDodge[index] == null))
                        this.img_linearDodge[index].color = c;
                }
            }

            public void SetFrameColor(Color c)
            {
                for (int index = 0; index < this.img_Frames.Length; ++index)
                {
                    if (!(this.img_Frames[index] == null))
                        this.img_Frames[index].color = c;
                }
            }

            public void SetRangeIconHsv(Vector3 hsvvalue)
            {
                this.img_RangeIcon.color = Color.white;
                if (this.hsv_rangeIcon == null)
                    this.hsv_rangeIcon = this.img_RangeIcon.GetComponent<RefineHsv>();
                if (this.hsv_rangeIcon == null)
                {
                    Debug.LogError("Hsv Not Reference");
                }
                else
                {
                    this.hsv_rangeIcon._HueShift = hsvvalue.x;
                    this.hsv_rangeIcon._Saturation = hsvvalue.y;
                    this.hsv_rangeIcon._ValueBrightness = hsvvalue.z;
                    this.hsv_rangeIcon.CallUpdate();
                    this.hsv_rangeIcon.enabled = false;
                    this.hsv_rangeIcon.enabled = true;
                }
            }

            public void RefreshNumbersData() => this.txt_cardNumbers.text = "";

            public void SetSlotState()
            {
                this.slotState = UIINVENCARD_STATE.None;
                this.deckLimitRoot.gameObject.SetActive(false);
                this.RefreshNumbersData();
            }

            public void SetData(DiceCardItemModel cardmodel)
            {
                this._cardModel = cardmodel;
                if (this._cardModel == null)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    if (!this.ob_NormalFrame.activeSelf)
                        this.ob_NormalFrame.gameObject.SetActive(true);
                    if (LorId.IsModId(this._cardModel.ClassInfo.workshopID))
                        this.txt_cardName.text = RewardingModel.SanitizeDisplayText(this._cardModel.GetName());
                    else
                        this.txt_cardName.text = RewardingModel.SanitizeDisplayText(Singleton<BattleCardDescXmlList>.Instance.GetCardName(this._cardModel.GetTextId()));
                    this.costNumbers.SetOneValue(this._cardModel.GetSpec().Cost, UISpriteDataManager.instance._cardCostNumberSprites);
                    this.img_RangeIcon.sprite = UISpriteDataManager.instance.GetRangeIconSprite(this._cardModel.GetSpec().Ranged);
                    this.SetRangeIconHsv(UIColorManager.Manager.CardRangeHsvValue[(int)this._cardModel.GetRarity()]);
                    this.colorFrame = UIColorManager.Manager.GetCardRarityColor(this._cardModel.GetRarity());
                    this.colorLineardodge = UIColorManager.Manager.GetCardRarityLinearColor(this._cardModel.GetRarity());
                    this.SetFrameColor(this.colorFrame);
                    this.SetLinearDodgeColor(this.colorLineardodge);
                    this.costNumbers.SetContentColor(this.colorFrame);
                    List<DiceBehaviour> behaviourList = this._cardModel.GetBehaviourList();
                    for (int index = 0; index < this.img_BehaviourIcons.Length; ++index)
                    {
                        if (index < behaviourList.Count)
                        {
                            Sprite sprite = behaviourList[index].Type == BehaviourType.Standby ? UISpriteDataManager.instance.CardStandbyBehaviourDetailIcons[(int)behaviourList[index].Detail] : UISpriteDataManager.instance._cardBehaviourDetailIcons[(int)behaviourList[index].Detail];
                            this.img_BehaviourIcons[index].sprite = sprite;
                            this.img_BehaviourIcons[index].gameObject.SetActive(true);
                        }
                        else
                            this.img_BehaviourIcons[index].gameObject.SetActive(false);
                    }
                    Sprite sprite1;
                    if (LorId.IsModId(this._cardModel.ClassInfo.workshopID))
                    {
                        sprite1 = Singleton<CustomizingCardArtworkLoader>.Instance.GetSpecificArtworkSprite(this._cardModel.ClassInfo.workshopID, this._cardModel.GetArtworkSrc());
                        if (sprite1 == null)
                            sprite1 = Singleton<AssetBundleManagerRemake>.Instance.LoadCardSprite(this._cardModel.GetArtworkSrc());
                    }
                    else
                        sprite1 = Singleton<AssetBundleManagerRemake>.Instance.LoadCardSprite(this._cardModel.GetArtworkSrc());
                    if (sprite1 != null)
                        this.img_Artwork.sprite = sprite1;
                    else
                        Debug.Log("Can't find sprite");
                    this.gameObject.SetActive(true);
                    if (cardmodel == null)
                        return;
                    this.SetSlotState();
                    this.isEmpty = false;
                    if (this.cg_EmptyFrameRoot != null)
                        this.cg_EmptyFrameRoot.alpha = 0.0f;
                    this.SetCgLeftPanel(true);
                }
            }
        }

        public class BattleMoneyUI
        {
            public static Dictionary<string, GameObject> obj_dic;

            public static void Create()
            {
                if (LogLikeMod.DefFont == null)
                {
                    LogLikeMod.DefFont = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
                    LogLikeMod.DefFontColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    LogLikeMod.DefFont_TMP = (SingletonBehavior<UIPopupWindowManager>.Instance.popupPanels[1] as UIOptionWindow).displayDropdown.itemText.font;
                }
                LogLikeMod.BattleMoneyUI.obj_dic = new Dictionary<string, GameObject>();
                Image image = ModdingUtils.CreateImage(LogLikeMod.LogUIObjs[100].transform, "MoneyIcon", new Vector2(1f, 1f), new Vector2(610f, 510f));
                TextMeshProUGUI textTmp = ModdingUtils.CreateText_TMP(image.transform, new Vector2(40f, 0.0f), 30, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.MidlineRight, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
                textTmp.rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
                textTmp.rectTransform.sizeDelta = new Vector2(200f, 50f);
                textTmp.text = PassiveAbility_MoneyCheck.GetMoney().ToString();
                textTmp.overflowMode = TextOverflowModes.Overflow;
                textTmp.autoSizeTextContainer = true;
                LogLikeMod.BattleMoneyUI.obj_dic.Add("MoneyIcon", image.gameObject);
                LogLikeMod.BattleMoneyUI.obj_dic.Add("Money", textTmp.gameObject);
            }

            public static void Active()
            {
                if (LogLikeMod.BattleMoneyUI.obj_dic == null)
                    LogLikeMod.BattleMoneyUI.Create();
                foreach (GameObject gameObject in LogLikeMod.BattleMoneyUI.obj_dic.Values)
                    gameObject.SetActive(true);
            }

            public static void DeActive()
            {
                if (LogLikeMod.BattleMoneyUI.obj_dic == null)
                    return;
                foreach (GameObject gameObject in LogLikeMod.BattleMoneyUI.obj_dic.Values)
                    gameObject.SetActive(false);
            }

            public static void UpdateMoney()
            {
                if (LogLikeMod.BattleMoneyUI.obj_dic == null)
                    return;
                LogLikeMod.BattleMoneyUI.obj_dic["Money"].GetComponent<TextMeshProUGUI>().text = PassiveAbility_MoneyCheck.GetMoney().ToString();
            }
        }
    }
}
