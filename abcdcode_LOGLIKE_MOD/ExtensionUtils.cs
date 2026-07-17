// -----------------------------------------------------------------------------
// Library of Ruina mod script: ExtensionUtils
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\ExtensionUtils.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using GameSave;
using HarmonyLib;
using LOR_DiceSystem;
using Mod;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using RogueLike_Mod_Reborn;
using static UnityEngine.GraphicsBuffer;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>ExtensionUtils</summary>

    public static class ExtensionUtils
    {
        #region --- Getters / setters / checks ---

        public static LorId GetOriginalId(this LorId id)
        {
            return UpgradeMetadata.UnpackPid(id.packageId, out UpgradeMetadata metadata) ? new LorId(metadata.actualPid, id.id) : id;
        }
        #endregion

        #region --- Other helpers ---


        public static bool CheckCanUpgrade(this DiceCardXmlInfo info)
        {
            return UpgradeMetadata.UnpackPid(info.workshopID, out UpgradeMetadata metadata) ? metadata.canStack : true;
        }
        #endregion

        #region --- Battle hooks ---


        public static bool CheckUpgradeCard(this DiceCardXmlInfo info)
        {
            return info.id.packageId.Contains(LogCardUpgradeManager.UpgradeKeyword);
        }

        public static void LogAddModCard(this ItemXmlDataList datalist, DiceCardXmlInfo cardinfo)
        {
            if (datalist == null || cardinfo == null)
                return;

            // Private fields on ItemXmlDataList must use reflection under MonoMod
            // (FieldAccessException on _workshopDict / _cardInfoTable otherwise).
            string workshopId = cardinfo.id.packageId ?? string.Empty;
            try
            {
                var workshopDict = GetOrCreateWorkshopDict(datalist);
                if (workshopDict == null)
                    return;

                if (!workshopDict.ContainsKey(workshopId))
                    workshopDict[workshopId] = new List<DiceCardXmlInfo>();
                if (!workshopDict[workshopId].Exists(x => x != null && x.id == cardinfo.id))
                    workshopDict[workshopId].Add(cardinfo);

                var cardTable = AccessTools.Field(typeof(ItemXmlDataList), "_cardInfoTable")?.GetValue(datalist)
                    as Dictionary<LorId, DiceCardXmlInfo>;
                var cardList = AccessTools.Field(typeof(ItemXmlDataList), "_cardInfoList")?.GetValue(datalist)
                    as List<DiceCardXmlInfo>;
                if (cardTable != null && !cardTable.ContainsKey(cardinfo.id))
                {
                    cardList?.Add(cardinfo);
                    cardTable.Add(cardinfo.id, cardinfo);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR] LogAddModCard failed (non-fatal): " + ex.Message);
            }
        }

        private static Dictionary<string, List<DiceCardXmlInfo>> GetOrCreateWorkshopDict(ItemXmlDataList datalist)
        {
            var field = AccessTools.Field(typeof(ItemXmlDataList), "_workshopDict");
            if (field == null)
                return null;
            var dict = field.GetValue(datalist) as Dictionary<string, List<DiceCardXmlInfo>>;
            if (dict == null)
            {
                dict = new Dictionary<string, List<DiceCardXmlInfo>>();
                field.SetValue(datalist, dict);
            }
            return dict;
        }
        #endregion

        #region --- Getters / setters / checks ---


        public static void SetLayerAll(this GameObject obj, int layer)
        {
            if (obj.transform.childCount > 0)
            {
                for (int index = 0; index < obj.transform.childCount; ++index)
                    obj.transform.GetChild(index).gameObject.SetLayerAll(layer);
            }
            obj.layer = layer;
        }

        public static void SetLayerAll(this GameObject obj, string name)
        {
            if (obj.transform.childCount > 0)
            {
                for (int index = 0; index < obj.transform.childCount; ++index)
                    obj.transform.GetChild(index).gameObject.SetLayerAll(name);
            }
            obj.layer = LayerMask.NameToLayer(name);
        }
        #endregion

        #region --- Other helpers ---


        public static void LocalEachScalingAll(this GameObject obj, float x, float y, float z = 0.0f)
        {
            if (obj.transform.childCount > 0)
            {
                for (int index = 0; index < obj.transform.childCount; ++index)
                    obj.transform.GetChild(index).gameObject.LocalEachScalingAll(x, y, z);
            }
            Vector3 localScale = obj.transform.localScale;
            obj.transform.localScale = new Vector3(localScale.x * x, localScale.y * y, localScale.z * z);
        }

        public static void LocalScalingAll(this GameObject obj, float x, float y, float z = 0.0f)
        {
            if (obj.transform.childCount > 0)
            {
                for (int index = 0; index < obj.transform.childCount; ++index)
                    obj.transform.GetChild(index).gameObject.LocalScalingAll(x, y, z);
            }
            obj.transform.localScale = new Vector3(x, y, z);
        }
        #endregion

        #region --- Save / load ---


        public static SaveData GetSaveDataPassiveModel(this PassiveModel __instance)
        {
            SaveData data = new SaveData();
            if (__instance.originData != null && __instance.originData.currentpassive != null)
                data.AddData("passivecurrentid", __instance.originData.currentpassive.id.LogGetSaveData());
            if (__instance.originpassive != null)
            {
                data.AddData("passiveprevid", __instance.originpassive.id.LogGetSaveData());
                data.AddData("receivebookinstanceid", __instance.originData.receivepassivebookId);
                data.AddData("givebookinstanceid", __instance.originData.givePassiveBookId);
            }
            return data;
        }

        public static void LoadFromSaveDataPassiveModel(this PassiveModel __instance, SaveData data)
        {
            SaveData data1 = data.GetData("passivecurrentid");
            LorId id1 = LorId.None;
            if (data1 != null)
                id1 = ExtensionUtils.LogLoadFromSaveData(data1);
            SaveData data2 = data.GetData("passiveprevid");
            LorId id2 = LorId.None;
            if (data2 != null)
                id2 = ExtensionUtils.LogLoadFromSaveData(data2);
            PassiveXmlInfo data3 = Singleton<PassiveXmlList>.Instance.GetData(id1);
            __instance.originpassive = Singleton<PassiveXmlList>.Instance.GetData(id2);
            int receivepassivebookId = data.GetInt("receivebookinstanceid");
            int givePassiveBookId = data.GetInt("givebookinstanceid");
            __instance.originData = new PassiveModel.PassiveModelSavedData(data3, receivepassivebookId, givePassiveBookId);
        }
        #endregion

        #region --- Getters / setters / checks ---


        public static void SetData(this SaveData save, string key, SaveData data)
        {
            SaveDataType fieldValue1 = LogLikeMod.GetFieldValue<SaveDataType>(save, "_type");
            Dictionary<string, SaveData> fieldValue2 = LogLikeMod.GetFieldValue<Dictionary<string, SaveData>>(save, "_dic");
            if (fieldValue1 != SaveDataType.None && fieldValue1 != SaveDataType.Dictionary)
                Debug.LogError("SaveData cannot have multiple type");
            LogLikeMod.SetFieldValue(save, "_type", SaveDataType.Dictionary);
            fieldValue2[key] = data;
        }
        #endregion

        #region --- Other helpers ---


        public static void AddData(this SaveData data, string key, string value)
        {
            data.AddData(key, new SaveData(value));
        }

        public static void AddData(this SaveData data, string key, int value)
        {
            data.AddData(key, new SaveData(value));
        }
        #endregion

        #region --- UI show / hide / build ---


        public static void RefreshEquip(this UnitDataModel model)
        {
            LogueBookModels.EquipNewPage(model, LogueBookModels.CurPlayerEquipInfo(model));
        }

        public static void RefreshEquip(this UnitBattleDataModel model)
        {
            LogueBookModels.EquipNewPage(model, LogueBookModels.CurPlayerEquipInfo(model.unitData));
        }
        #endregion

        #region --- Other helpers ---


        public static void EquipNewPage(this UnitDataModel model, BookXmlInfo info)
        {
            LogueBookModels.EquipNewPage(model, info);
        }

        public static void EquipNewPage(this UnitBattleDataModel model, BookXmlInfo info)
        {
            LogueBookModels.EquipNewPage(model, info);
        }

        public static void LogId(this object obj) {
            string packageId = "";
            try
            {
                packageId = RMRCore.ClassIds[obj.GetType().Assembly.FullName];
            } catch
            {
                packageId = "NOT_FOUND";
            }
            Debug.Log(obj.GetType().Name + " -- IS OF ASSEMBLY PACK ID -- " + packageId);
        }
        #endregion

        #region --- Getters / setters / checks ---


        public static bool IsDead(this UnitDataModel model)
        {
            try
            {
                return LogueBookModels.playerBattleModel.Find(x => x.unitData == model).isDead;
            } catch
            {
                return false;
            }
        }
        #endregion

        #region --- Battle hooks ---


        public static CardDropTableXmlRoot Convert(this abcdcode_LOGLIKE_MOD_Extension.CardDropTableXmlRoot info)
        {
            CardDropTableXmlRoot dropTableXmlRoot = new CardDropTableXmlRoot();
            dropTableXmlRoot.dropTableXmlList = new List<CardDropTableXmlInfo>();
            foreach (abcdcode_LOGLIKE_MOD_Extension.CardDropTableXmlInfo dropTableXml in info.dropTableXmlList)
                dropTableXmlRoot.dropTableXmlList.Add(dropTableXml.Convert());
            return dropTableXmlRoot;
        }

        public static CardDropTableXmlInfo Convert(this abcdcode_LOGLIKE_MOD_Extension.CardDropTableXmlInfo info)
        {
            return new CardDropTableXmlInfo()
            {
                cardIdList = info.cardIdList,
                workshopId = info.pid,
                _cardIdList = info._cardIdList,
                _id = info._id,
                _validCardIdList = info._validCardIdList
            };
        }
        #endregion

        #region --- Other helpers ---


        /// <summary>
        /// Used for quickly adding combat pages to a drop pool.
        /// </summary>
        /// <param name="original">The original drop pool's ID. (ID OF THE CARD LIST, NOT DROP POOL ID!)</param>
        /// <param name="newId">The drop pool to add.</param>
        public static void MergeDropTables(this CardDropTableXmlList instance, LorId original, LorId newId)
        {
            var originalTable = instance.GetData(original);
            var newTable = instance.GetData(newId);
            if (originalTable == null || newTable == null)
                return;
            originalTable.cardIdList.AddRange(newTable.cardIdList);
            try
            {
                var field = AccessTools.Field(typeof(CardDropTableXmlList), "_workshopDict");
                var dict = field?.GetValue(instance) as Dictionary<string, List<CardDropTableXmlInfo>>;
                if (dict != null && dict.TryGetValue(original.packageId ?? string.Empty, out var list) && list != null)
                {
                    var entry = list.Find(x => x != null && x.id == original);
                    if (entry != null)
                        entry.cardIdList = originalTable.cardIdList;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR] MergeDropTables workshopDict update failed: " + ex.Message);
            }
        }
        #endregion

        #region --- Getters / setters / checks ---


        public static string GetLogArtWorkPath(this ModContentInfo info)
        {
            return Path.Combine(info.GetLogDllPath(), "ArtWork");
        }

        public static string GetAssemPath(this ModContentInfo info)
        {
            return Path.Combine(info.dirInfo.FullName, "Assemblies");
        }

        public static DirectoryInfo GetLogDllDir(this ModContentInfo info)
        {
            return Directory.Exists(info.GetLogDllPath()) ? new DirectoryInfo(info.GetLogDllPath()) : (DirectoryInfo)null;
        }

        public static string GetLogDllPath(this ModContentInfo info)
        {
            return Path.Combine(Path.Combine(info.dirInfo.FullName, "Assemblies"), "Roguedlls");
        }
        #endregion

        #region --- Other helpers ---


        public static void Log(this object obj, string str)
        {
            if (RMRCore.provideAdditionalLogging)
            {
                if (obj != null)
                    Debug.Log($"Log : {obj.GetType().Name} {str}");
                else
                    Debug.Log(("Log : " + str));
            }
        }

        public static void LogError(this object obj, Exception e)
        {
            if (obj != null)
                Debug.Log($"Log : {obj.GetType().Name}{e.Message}{Environment.NewLine}{e.StackTrace}");
            else
                Debug.Log($"Log : {e.Message}{Environment.NewLine}{e.StackTrace}");
        }
        #endregion

        #region --- Battle hooks ---


        public static Predicate<DiceMatch> NotFirstAttackDice()
        {
            int index = -1;
            return (Predicate<DiceMatch>)(match =>
            {
                if (index != -1 || match.abiliity.behaviourInCard.Type != BehaviourType.Atk)
                    return true;
                index = match.index;
                return false;
            });
        }

        public static Predicate<DiceMatch> NotFirstDice() => x => x.index != 0;

        public static List<T> RandomPickUp<T>(this List<T> list, int count)
        {
            List<T> objList = new List<T>(list);
            while (objList.Count > count)
                objList.RemoveAt(Singleton<System.Random>.Instance.Next(0, objList.Count));
            return objList;
        }
        #endregion

        #region --- Save / load ---


        public static SaveData LogGetSaveData(this LorId id)
        {
            SaveData saveData = new SaveData();
            saveData.AddData("_id", new SaveData(id.id));
            if (id.packageId != null)
                saveData.AddData("workshopId", new SaveData(id.packageId));
            else
                saveData.AddData("workshopId", new SaveData(""));
            return saveData;
        }

        public static LorId LogLoadFromSaveData(SaveData data)
        {
            int id = data.GetInt("_id");
            return new LorId(data.GetString("workshopId"), id);
        }
        #endregion

        #region --- Getters / setters / checks ---


        /// <summary>
        /// Yes/No confirm dialog with custom text. Never touches UIAlarmPopup fields directly —
        /// MonoMod throws FieldAccessException on Assembly-CSharp member access from mods.
        /// ButtonRoots is a List&lt;GameObject&gt; (not array). YesNo enum value is 1.
        /// Returns true only when dialog was opened with Yes/No + confirm callback wired.
        /// Callers must handle false (do not assume user can confirm).
        /// </summary>
        public static bool SetChoiceAlarmText(this UIAlarmPopup alarm, string text = "", ConfirmEvent confirmFunc = null)
        {
            if (alarm == null)
            {
                Debug.LogError("[RMR] SetChoiceAlarmText: alarm is null.");
                return false;
            }
            try
            {
                if (alarm.IsOpened())
                    alarm.Close();

                SetAlarmGoActive(alarm, "ob_blue", false);
                SetAlarmGoActive(alarm, "ob_normal", true);
                SetAlarmGoActive(alarm, "ob_Reward", false);
                SetAlarmGoActive(alarm, "ob_BlackBg", false);

                SetAlarmField(alarm, "currentAnimState", UIAlarmPopup.UIAlarmAnimState.Normal);
                SetAlarmField(alarm, "currentmode", AnimatorUpdateMode.UnscaledTime);
                SetAlarmField(alarm, "currentAlarmType", UIAlarmType.Default);

                // ButtonRoots is List<GameObject> in the real game assembly.
                var buttonRootsObj = AccessTools.Field(typeof(UIAlarmPopup), "ButtonRoots")?.GetValue(alarm);
                var buttonRoots = buttonRootsObj as System.Collections.IList;
                if (buttonRoots == null)
                {
                    Debug.LogError("[RMR] SetChoiceAlarmText: ButtonRoots missing/unreadable.");
                    return false;
                }

                for (int i = 0; i < buttonRoots.Count; i++)
                {
                    if (buttonRoots[i] is GameObject go && go != null)
                        go.SetActive(false);
                }

                object txtObj = AccessTools.Field(typeof(UIAlarmPopup), "txt_alarm")?.GetValue(alarm);
                if (txtObj == null)
                {
                    Debug.LogError("[RMR] SetChoiceAlarmText: txt_alarm missing.");
                    return false;
                }
                SetTmpOrUiText(txtObj, text);

                SetAlarmField(alarm, "_confirmEvent", confirmFunc);
                // Verify confirm was stored — OnYesButton reads this field.
                object storedConfirm = AccessTools.Field(typeof(UIAlarmPopup), "_confirmEvent")?.GetValue(alarm);
                if (confirmFunc != null && storedConfirm == null)
                {
                    Debug.LogError("[RMR] SetChoiceAlarmText: failed to store _confirmEvent.");
                    return false;
                }

                // Vanilla also tracks which button layout is active.
                UIAlarmButtonType index = UIAlarmButtonType.YesNo; // = 1
                SetAlarmField(alarm, "buttonNumberType", index);

                if ((int)index < 0 || (int)index >= buttonRoots.Count
                    || !(buttonRoots[(int)index] is GameObject yesNoRoot) || yesNoRoot == null)
                {
                    Debug.LogError($"[RMR] SetChoiceAlarmText: YesNo root missing (index={(int)index}, count={buttonRoots.Count}).");
                    return false;
                }
                yesNoRoot.SetActive(true);

                alarm.Open();

                try
                {
                    object yesObj = AccessTools.Property(typeof(UIAlarmPopup), "YesButton")?.GetValue(alarm, null);
                    if (yesObj == null)
                        yesObj = AccessTools.Field(typeof(UIAlarmPopup), "yesButton")?.GetValue(alarm);
                    if (yesObj != null && UIControlManager.Instance != null)
                    {
                        var method = AccessTools.Method(typeof(UIControlManager), "SelectSelectableForcely");
                        if (method != null)
                            method.Invoke(UIControlManager.Instance, new object[] { yesObj, false });
                    }
                }
                catch (Exception exSel)
                {
                    Debug.LogWarning("[RMR] SetChoiceAlarmText select-yes failed (non-fatal): " + exSel.Message);
                }

                Debug.Log("[RMR] SetChoiceAlarmText opened Yes/No confirm.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("[RMR] SetChoiceAlarmText failed: " + ex);
                // Do NOT fall back to SetAlarmText(string): that is OK-only and drops confirmFunc,
                // leaving the user with a dead dialog. Caller must handle false.
                return false;
            }
        }

        private static void SetAlarmField(UIAlarmPopup alarm, string fieldName, object value)
        {
            try
            {
                AccessTools.Field(typeof(UIAlarmPopup), fieldName)?.SetValue(alarm, value);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[RMR] SetAlarmField {fieldName} failed: {ex.Message}");
            }
        }

        private static void SetTmpOrUiText(object textObj, string text)
        {
            if (textObj == null)
                return;
            try
            {
                // Avoid `is TextMeshProUGUI` identity issues across TMP assemblies.
                var prop = textObj.GetType().GetProperty("text", AccessTools.all);
                if (prop != null && prop.CanWrite)
                    prop.SetValue(textObj, text, null);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR] SetTmpOrUiText failed: " + ex.Message);
            }
        }

        private static void SetAlarmGoActive(UIAlarmPopup alarm, string fieldName, bool active)
        {
            try
            {
                var go = AccessTools.Field(typeof(UIAlarmPopup), fieldName)?.GetValue(alarm) as GameObject;
                if (go == null)
                    return;
                if (go.activeSelf != active)
                    go.SetActive(active);
            }
            catch { }
        }
    
        #endregion

    }
}
