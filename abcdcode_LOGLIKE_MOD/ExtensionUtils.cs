// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.ExtensionUtils
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

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

    public static class ExtensionUtils
    {
        public static LorId GetOriginalId(this LorId id)
        {
            return UpgradeMetadata.UnpackPid(id.packageId, out UpgradeMetadata metadata) ? new LorId(metadata.actualPid, id.id) : id;
        }

        public static bool CheckCanUpgrade(this DiceCardXmlInfo info)
        {
            return UpgradeMetadata.UnpackPid(info.workshopID, out UpgradeMetadata metadata) ? metadata.canStack : true;
        }

        public static bool CheckUpgradeCard(this DiceCardXmlInfo info)
        {
            return info.id.packageId.Contains(LogCardUpgradeManager.UpgradeKeyword);
        }

        public static void LogAddModCard(this ItemXmlDataList datalist, DiceCardXmlInfo cardinfo)
        {
            string workshopId = cardinfo.id.packageId;
            if (datalist._workshopDict == null)
            {
                datalist._workshopDict = new Dictionary<string, List<DiceCardXmlInfo>>();
            }

            if (!datalist._workshopDict.ContainsKey(workshopId))
            {
                datalist._workshopDict[workshopId] = new List<DiceCardXmlInfo>();
            }
            if (!datalist._workshopDict[workshopId].Exists(x => x.id == cardinfo.id))
            {
                datalist._workshopDict[workshopId].Add(cardinfo);
            }

            if (!datalist._cardInfoTable.ContainsKey(cardinfo.id))
            {
                datalist._cardInfoList.Add(cardinfo);
                datalist._cardInfoTable.Add(cardinfo.id, cardinfo);
            }

        }

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

        public static void SetData(this SaveData save, string key, SaveData data)
        {
            SaveDataType fieldValue1 = LogLikeMod.GetFieldValue<SaveDataType>(save, "_type");
            Dictionary<string, SaveData> fieldValue2 = LogLikeMod.GetFieldValue<Dictionary<string, SaveData>>(save, "_dic");
            if (fieldValue1 != SaveDataType.None && fieldValue1 != SaveDataType.Dictionary)
                Debug.LogError("SaveData cannot have multiple type");
            LogLikeMod.SetFieldValue(save, "_type", SaveDataType.Dictionary);
            fieldValue2[key] = data;
        }

        public static void AddData(this SaveData data, string key, string value)
        {
            data.AddData(key, new SaveData(value));
        }

        public static void AddData(this SaveData data, string key, int value)
        {
            data.AddData(key, new SaveData(value));
        }

        public static void RefreshEquip(this UnitDataModel model)
        {
            LogueBookModels.EquipNewPage(model, LogueBookModels.CurPlayerEquipInfo(model));
        }

        public static void RefreshEquip(this UnitBattleDataModel model)
        {
            LogueBookModels.EquipNewPage(model, LogueBookModels.CurPlayerEquipInfo(model.unitData));
        }

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

        /// <summary>
        /// Used for quickly adding combat pages to a drop pool.
        /// </summary>
        /// <param name="original">The original drop pool's ID. (ID OF THE CARD LIST, NOT DROP POOL ID!)</param>
        /// <param name="newId">The drop pool to add.</param>
        public static void MergeDropTables(this CardDropTableXmlList instance, LorId original, LorId newId)
        {
            var originalTable = instance.GetData(original);
            var newTable = instance.GetData(newId);
            originalTable.cardIdList.AddRange(newTable.cardIdList);
            instance._workshopDict[original.packageId].Find(x => x.id == original).cardIdList = originalTable.cardIdList;
        }

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

        public static void SetChoiceAlarmText(this UIAlarmPopup alarm, string text = "", ConfirmEvent confirmFunc = null)
        {
            if (alarm.IsOpened())
            {
                alarm.Close();
            }
            if (alarm.ob_blue.activeSelf)
            {
                alarm.ob_blue.gameObject.SetActive(false);
            }
            if (!alarm.ob_normal.activeSelf)
            {
                alarm.ob_normal.gameObject.SetActive(true);
            }
            if (alarm.ob_Reward.activeSelf)
            {
                alarm.ob_Reward.SetActive(false);
            }
            alarm.currentAnimState = UIAlarmPopup.UIAlarmAnimState.Normal;
            alarm.currentmode = AnimatorUpdateMode.UnscaledTime;
            foreach (GameObject gameObject in alarm.ButtonRoots)
            {
                gameObject.gameObject.SetActive(false);
            }
            if (alarm.ob_BlackBg.activeSelf)
            {
                alarm.ob_BlackBg.SetActive(false);
            }
            alarm.currentAlarmType = UIAlarmType.Default;
            UIAlarmButtonType index = UIAlarmButtonType.YesNo;
            alarm.txt_alarm.text = text;
            alarm._confirmEvent = confirmFunc;
            alarm.ButtonRoots[(int)index].gameObject.SetActive(true);
            alarm.Open();
            UIControlManager.Instance.SelectSelectableForcely(alarm.YesButton, false);
        }
    
    }
}
