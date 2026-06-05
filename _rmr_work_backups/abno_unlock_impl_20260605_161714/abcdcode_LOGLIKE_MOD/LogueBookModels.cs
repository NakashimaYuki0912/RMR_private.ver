// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogueBookModels
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using HarmonyLib;
using LOR_DiceSystem;
using LOR_XML;
using RogueLike_Mod_Reborn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static CharacterSound;

namespace abcdcode_LOGLIKE_MOD
{

    public class LogueBookModels
    {
        public static BookXmlInfo BaseXmlInfo;
        public static BookEquipEffect BaseEquipStat;
        public static Dictionary<int, int> EmotionSelectDic;
        public static List<RewardPassiveInfo> selectedEmotion;
        public static List<RewardPassiveInfo> EmotionCardList;
        public static Dictionary<ChapterGrade, List<LogueStageInfo>> RemainStageList;
        public static List<BookModel> booklist;
        public static List<DiceCardItemModel> cardlist;
        public static List<UnitDataModel> playerModel;
        public static Dictionary<UnitDataModel, List<LorId>> playersPick;
        public static Dictionary<UnitDataModel, List<LorId>> playersperpassives;
        public static Dictionary<UnitDataModel, List<LogStatAdder>> playersstatadders;
        public static List<LorId> shopPick;
        public static List<UnitBattleDataModel> playerBattleModel;
        public static int nextinstanceid;

        public static BookModel LoadFromSaveData_BookModel(SaveData data)
        {
            BookModel bookModel = new BookModel(Singleton<BookXmlList>.Instance.GetData(ExtensionUtils.LogLoadFromSaveData(data.GetData("id"))));
            int index = data.GetInt("index");
            if (index != -1)
                bookModel.owner = LogueBookModels.playerModel[index];
            bookModel.instanceId = data.GetInt("instanceid");
            bookModel.originData.equipedPassiveBookInstanceId = data.GetInt("equippedbookid");
            bookModel.TryGainUniquePassive();
            return bookModel;
        }

        public static SaveData GetSaveData_BookModel(BookModel model)
        {
            SaveData data = new SaveData();
            data.AddData("id", model.ClassInfo.id.LogGetSaveData());
            if (model.owner != null)
            {
                int num = LogueBookModels.playerModel.IndexOf(model.owner);
                data.AddData("index", new SaveData(num));
            }
            else
                data.AddData("index", new SaveData(-1));
            data.AddData("instanceid", model.instanceId);
            data.AddData("equippedbookid", model.originData.equipedPassiveBookInstanceId);
            return data;
        }

        public static void LoadFromSaveData_UnitBattleDataModel(SaveData data, UnitBattleDataModel model)
        {
            BookXmlInfo classInfo = model.unitData.bookItem.ClassInfo;
            model.hp = (float)data.GetData("hp").GetIntSelf();
            model.isDead = data.GetData("isDead").GetIntSelf() == 1;
            classInfo._bookIcon = data.GetData("_bookIcon").GetStringSelf();
            classInfo.TextId = data.GetData("TextId").GetIntSelf();
            classInfo.Chapter = data.GetData("Chapter").GetIntSelf();
            classInfo.RangeType = (EquipRangeType)data.GetInt("RangeType");
            classInfo.CharacterSkin = new List<string>();
            foreach (SaveData saveData in data.GetData("CharacterSkin"))
                classInfo.CharacterSkin.Add(saveData.GetStringSelf());
            SaveData data1 = data.GetData("EquipEffect");
            classInfo.EquipEffect.AddedStartDraw = data1.GetInt("AddedStartDraw");
            classInfo.EquipEffect.Break = data1.GetInt("Break");
            classInfo.EquipEffect.HBResist = (AtkResist)data1.GetInt("HBResist");
            classInfo.EquipEffect.Hp = data1.GetInt("Hp");
            classInfo.EquipEffect.HpReduction = data1.GetInt("HpReduction");
            classInfo.EquipEffect.HResist = (AtkResist)data1.GetInt("HResist");
            classInfo.EquipEffect.MaxPlayPoint = data1.GetInt("MaxPlayPoint");
            foreach (SaveData data2 in data1.GetData("PassiveList"))
            {
                LorId lorId = ExtensionUtils.LogLoadFromSaveData(data2);
                if (lorId != new LorId(LogLikeMod.ModId, 1))
                    classInfo.EquipEffect.PassiveList.Add(lorId);
            }
            model.unitData.bookItem.instanceId = data.GetInt("instanceid");
            SaveData data3 = data.GetData("SucPassiveList");
            List<PassiveModel> fieldValue1 = LogLikeMod.GetFieldValue<List<PassiveModel>>(model.unitData.bookItem, "_activatedAllPassives");
            foreach (SaveData data4 in data3)
            {
                PassiveModel __instance = new PassiveModel(model.unitData.bookItem.instanceId);
                __instance.LoadFromSaveDataPassiveModel(data4);
                fieldValue1.Add(__instance);
            }
            classInfo.EquipEffect.PBResist = (AtkResist)data1.GetInt("PBResist");
            classInfo.EquipEffect.PResist = (AtkResist)data1.GetInt("PResist");
            classInfo.EquipEffect.SBResist = (AtkResist)data1.GetInt("SBResist");
            classInfo.EquipEffect.Speed = data1.GetInt("Speed");
            classInfo.EquipEffect.SpeedMin = data1.GetInt("SpeedMin");
            classInfo.EquipEffect.SResist = (AtkResist)data1.GetInt("SResist");
            classInfo.EquipEffect.StartPlayPoint = data1.GetInt("StartPlayPoint");
            model.unitData.bookItem.TryGainUniquePassive();
            model.unitData.bookItem.SetOriginalStat();
            model.unitData.bookItem.SetOriginalSpeed();
            model.unitData.bookItem.SetOriginalResists();
            model.unitData.bookItem.SetOriginalPlayPoint();
            model.unitData.bookItem.SetOriginalCharacterName();
            typeof(BookModel).GetField("_selectedOriginalSkin", AccessTools.all).SetValue(model.unitData.bookItem, classInfo.CharacterSkin[0]);
            typeof(BookModel).GetField("_characterSkin", AccessTools.all).SetValue(model.unitData.bookItem, classInfo.CharacterSkin[0]);
            SaveData data5 = data.GetData("_onlyCard");
            List<DiceCardXmlInfo> fieldValue2 = LogLikeMod.GetFieldValue<List<DiceCardXmlInfo>>(model.unitData.bookItem, "_onlyCards");
            if (data5.GetListCount() > 0)
            {
                foreach (SaveData data6 in data5)
                {
                    DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(ExtensionUtils.LogLoadFromSaveData(data6));
                    fieldValue2.Add(cardItem);
                }
            }
            SaveData data7 = data.GetData("deck");
            int num1 = data7.GetListCount() > 0 ? 1 : 0;
            model.Log("INITIALIZING DECKS");
            if (num1 != 0)
            {
                foreach (SaveData data8 in data7)
                {
                    LorId cardId = ExtensionUtils.LogLoadFromSaveData(data8);
                    model.Log($"INITIALIZING DECK FOR UNIT : {cardId.packageId}, {cardId.id.ToString()}");
                    LogueBookModels.AddCard(cardId, 1, false);
                    int num2 = (int)model.unitData.AddCardFromInventory(cardId);
                }
            }
            SaveData data9 = data.GetData("equipedbooklist");  
            model.unitData.bookItem.originData.equipedBookIdListInPassive = new List<int>();
            foreach (SaveData saveData in data9)
                model.unitData.bookItem.originData.equipedBookIdListInPassive.Add(saveData.GetIntSelf());
            
            var data10 = data.GetData("customizedAppearance");
            if (data10 != null)
                model.unitData.customizeData.LoadFromSaveData(data10);

            var data11 = data.GetData("nuggetName");
            if (data11 != null)
                model.unitData.SetCustomName(data11.GetStringSelf());
        }

        public static SaveData GetSaveData_UnitBattleDataModel(UnitBattleDataModel model)
        {
            BookXmlInfo classInfo = model.unitData.bookItem.ClassInfo;
            SaveData data1 = new SaveData();
            data1.AddData("hp", new SaveData((int)model.hp));
            data1.AddData("isDead", model.isDead ? 1 : 0);
            data1.AddData("_bookIcon", new SaveData(classInfo._bookIcon));
            data1.AddData("TextId", new SaveData(classInfo.TextId));
            data1.AddData("Chapter", new SaveData(classInfo.Chapter));
            data1.AddData("RangeType", new SaveData((int)classInfo.RangeType));
            SaveData data2 = new SaveData();
            foreach (string str in classInfo.CharacterSkin)
                data2.AddToList(new SaveData(str));
            data1.AddData("CharacterSkin", data2);
            SaveData data3 = new SaveData();
            data3.AddData("AddedStartDraw", new SaveData(classInfo.EquipEffect.AddedStartDraw));
            data3.AddData("Break", new SaveData(classInfo.EquipEffect.Break));
            data3.AddData("HBResist", new SaveData((int)classInfo.EquipEffect.HBResist));
            data3.AddData("Hp", new SaveData(classInfo.EquipEffect.Hp));
            data3.AddData("HpReduction", new SaveData(classInfo.EquipEffect.HpReduction));
            data3.AddData("HResist", new SaveData((int)classInfo.EquipEffect.HResist));
            data3.AddData("MaxPlayPoint", new SaveData(classInfo.EquipEffect.MaxPlayPoint));
            SaveData data4 = new SaveData();
            foreach (LorId passive in classInfo.EquipEffect.PassiveList)
            {
                if (passive != new LorId(LogLikeMod.ModId, 1))
                    data4.AddToList(passive.LogGetSaveData());
            }
            data3.AddData("PassiveList", data4);
            data3.AddData("PBResist", new SaveData((int)classInfo.EquipEffect.PBResist));
            data3.AddData("PResist", new SaveData((int)classInfo.EquipEffect.PResist));
            data3.AddData("SBResist", new SaveData((int)classInfo.EquipEffect.SBResist));
            data3.AddData("Speed", new SaveData(classInfo.EquipEffect.Speed));
            data3.AddData("SpeedMin", new SaveData(classInfo.EquipEffect.SpeedMin));
            data3.AddData("SResist", new SaveData((int)classInfo.EquipEffect.SResist));
            data3.AddData("StartPlayPoint", new SaveData(classInfo.EquipEffect.StartPlayPoint));
            data1.AddData("EquipEffect", data3);
            SaveData data5 = new SaveData();
            List<DiceCardXmlInfo> listFromCurrentDeck = model.unitData.bookItem.GetCardListFromCurrentDeck();
            if (listFromCurrentDeck.Count > 0)
            {
                foreach (DiceCardXmlInfo diceCardXmlInfo in listFromCurrentDeck)
                    data5.AddToList(diceCardXmlInfo.id.LogGetSaveData());
            }
            data1.AddData("deck", data5);
            data1.AddData("instanceid", new SaveData(model.unitData.bookItem.instanceId));
            SaveData data6 = new SaveData();
            foreach (int num in model.unitData.bookItem.originData.equipedBookIdListInPassive)
                data6.AddToList(new SaveData(num));
            data1.AddData("equipedbooklist", data6);
            SaveData data7 = new SaveData();
            foreach (PassiveModel __instance in model.unitData.bookItem.GetPassiveModelList().FindAll((Predicate<PassiveModel>)(x => x.originData.receivepassivebookId != model.unitData.bookItem.instanceId)))
            {
                SaveData dataPassiveModel = __instance.GetSaveDataPassiveModel();
                data7.AddToList(dataPassiveModel);
            }
            data1.AddData("SucPassiveList", data7);
            SaveData data8 = new SaveData();
            List<DiceCardXmlInfo> fieldValue = LogLikeMod.GetFieldValue<List<DiceCardXmlInfo>>(model.unitData.bookItem, "_onlyCards");
            if (fieldValue.Count > 0)
            {
                foreach (DiceCardXmlInfo diceCardXmlInfo in fieldValue)
                    data8.AddToList(diceCardXmlInfo.id.LogGetSaveData());
            }
            data1.AddData("_onlyCard", data8);
            var data9 = model.unitData.customizeData.GetSaveData();
            data1.AddData("customizedAppearance", data9);
            var data10 = model.unitData.name;
            data1.AddData("nuggetName", data10);
            return data1;
        }

        public static SaveData CreateChSaveData(ChapterGrade grade)
        {
            LogueBookModels.CreatePlayer();
            LogueBookModels.CreatePlayerBattle();
            int num = grade > ChapterGrade.Grade5 ? 4 : (int)grade;
            for (int index = 0; index < num; ++index)
                LogueBookModels.AddSubPlayer();
            SaveData data1 = new SaveData();
            LogueBookModels.AddingRemainStageList();
            SaveData data2 = new SaveData();
            foreach (KeyValuePair<ChapterGrade, List<LogueStageInfo>> remainStage in LogueBookModels.RemainStageList)
            {
                SaveData data3 = new SaveData();
                foreach (LogueStageInfo logueStageInfo in remainStage.Value)
                    data3.AddToList(logueStageInfo.Id.LogGetSaveData());
                data2.AddData(remainStage.Key.ToString(), data3);
            }
            data1.AddData("RemainStageList", data2);
            SaveData data4 = new SaveData();
            foreach (LorId cardId in Singleton<CardDropTableXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, -854001)).cardIdList)
                LogueBookModels.AddCard(cardId, 99, false);
            foreach (LorId cardId in Singleton<CardDropTableXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, -854101)).cardIdList)
                LogueBookModels.AddCard(cardId, 99, false);
            if (grade > ChapterGrade.Grade2)
            {
                foreach (LorId cardId in Singleton<CardDropTableXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, -854201)).cardIdList)
                    LogueBookModels.AddCard(cardId, 99, false);
            }
            if (grade > ChapterGrade.Grade3)
            {
                foreach (LorId cardId in Singleton<CardDropTableXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, -854301)).cardIdList)
                    LogueBookModels.AddCard(cardId, 99, false);
            }
            if (grade > ChapterGrade.Grade4)
            {
                foreach (LorId cardId in Singleton<CardDropTableXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, -854401)).cardIdList)
                    LogueBookModels.AddCard(cardId, 99, false);
            }
            foreach (DiceCardItemModel diceCardItemModel in LogueBookModels.cardlist)
            {
                SaveData data5 = new SaveData();
                data5.AddData("id", diceCardItemModel.ClassInfo.id.LogGetSaveData());
                data5.AddData("num", new SaveData(diceCardItemModel.num));
                data4.AddToList(data5);
            }
            data1.AddData("cardlist", data4);
            SaveData data6 = new SaveData();
            foreach (UnitBattleDataModel model in LogueBookModels.playerBattleModel)
                data6.AddToList(LogueBookModels.GetSaveData_UnitBattleDataModel(model));
            data1.AddData("playerBattleModel", data6);
            SaveData data7 = new SaveData();
            foreach (KeyValuePair<UnitDataModel, List<LorId>> keyValuePair in LogueBookModels.playersPick)
            {
                SaveData data8 = new SaveData();
                foreach (LorId id in keyValuePair.Value)
                    data8.AddToList(id.LogGetSaveData());
                data7.AddData(keyValuePair.Key.bookItem.BookId.id.ToString(), data8);
            }
            data1.AddData("playersPick", data7);
            SaveData data9 = new SaveData();
            foreach (KeyValuePair<UnitDataModel, List<LorId>> playersperpassive in LogueBookModels.playersperpassives)
            {
                SaveData data10 = new SaveData();
                foreach (LorId id in playersperpassive.Value)
                    data10.AddToList(id.LogGetSaveData());
                data9.AddData(playersperpassive.Key.bookItem.BookId.id.ToString(), data10);
            }
            data1.AddData("playersperpassives", data9);
            SaveData data11 = new SaveData();
            foreach (KeyValuePair<UnitDataModel, List<LogStatAdder>> playersstatadder in LogueBookModels.playersstatadders)
            {
                SaveData data12 = new SaveData();
                foreach (LogStatAdder logStatAdder in playersstatadder.Value)
                    data12.AddToList(logStatAdder.GetSaveData());
                data11.AddData(playersstatadder.Key.bookItem.BookId.id.ToString(), data12);
            }
            data1.AddData("playersstatadders", data11);
            SaveData data13 = new SaveData();
            foreach (LorId id in LogueBookModels.shopPick)
                data13.AddToList(id.LogGetSaveData());
            data1.AddData("shopPick", data13);
            SaveData data14 = new SaveData();
            foreach (RewardPassiveInfo rewardPassiveInfo in Singleton<RewardPassivesList>.Instance.GetChapterData((Predicate<RewardPassivesInfo>)(x => x.rewardtype == PassiveRewardListType.CommonReward)))
            {
                if ((ChapterGrade)Singleton<BookXmlList>.Instance.GetData(rewardPassiveInfo.id).Chapter <= grade + 1)
                {
                    for (int index = 0; index < PickUpModel_EquipDefault.EquipLimit(rewardPassiveInfo.passiverarity); ++index)
                    {
                        BookModel bookModel = new BookModel(Singleton<BookXmlList>.Instance.GetData(rewardPassiveInfo.id));
                        bookModel.instanceId = LogueBookModels.nextinstanceid++;
                        bookModel.TryGainUniquePassive();
                        LogueBookModels.booklist.Add(bookModel);
                    }
                }
            }
            foreach (BookModel model in LogueBookModels.booklist)
            {
                SaveData saveDataBookModel = LogueBookModels.GetSaveData_BookModel(model);
                data14.AddToList(saveDataBookModel);
            }
            data1.AddData("booklist", data14);
            data1.AddData("SubPlayerNum", new SaveData(LogueBookModels.playerModel.Count - 1));
            data1.AddData("nextinstanceid", LogueBookModels.nextinstanceid);
            return data1;
        }

        public static SaveData GetSaveData()
        {
            "".Log("LogueInven Save Start : " + DateTime.Now.ToString());
            SaveData data1 = new SaveData();
            SaveData data2 = new SaveData();
            foreach (KeyValuePair<int, int> keyValuePair in LogueBookModels.EmotionSelectDic)
                data2.AddData(keyValuePair.Key.ToString(), keyValuePair.Value);
            data1.AddData("emotionSelect", data2);
            SaveData data3 = new SaveData();
            foreach (KeyValuePair<ChapterGrade, List<LogueStageInfo>> remainStage in LogueBookModels.RemainStageList)
            {
                SaveData data4 = new SaveData();
                foreach (LogueStageInfo logueStageInfo in remainStage.Value)
                    data4.AddToList(logueStageInfo.Id.LogGetSaveData());
                data3.AddData(remainStage.Key.ToString(), data4);
            }
            data1.AddData("RemainStageList", data3);
            SaveData data5 = new SaveData();
            foreach (DiceCardItemModel diceCardItemModel in LogueBookModels.cardlist)
            {
                SaveData data6 = new SaveData();
                data6.AddData("id", diceCardItemModel.ClassInfo.id.LogGetSaveData());
                data6.AddData("num", new SaveData(diceCardItemModel.num));
                data5.AddToList(data6);
            }
            data1.AddData("cardlist", data5);
            SaveData data7 = new SaveData();
            foreach (UnitBattleDataModel model in LogueBookModels.playerBattleModel)
                data7.AddToList(LogueBookModels.GetSaveData_UnitBattleDataModel(model));
            data1.AddData("playerBattleModel", data7);
            SaveData data8 = new SaveData();
            foreach (KeyValuePair<UnitDataModel, List<LorId>> keyValuePair in LogueBookModels.playersPick)
            {
                SaveData data9 = new SaveData();
                foreach (LorId id in keyValuePair.Value)
                    data9.AddToList(id.LogGetSaveData());
                data8.AddData(keyValuePair.Key.bookItem.BookId.id.ToString(), data9);
            }
            data1.AddData("playersPick", data8);
            SaveData data10 = new SaveData();
            foreach (KeyValuePair<UnitDataModel, List<LorId>> playersperpassive in LogueBookModels.playersperpassives)
            {
                SaveData data11 = new SaveData();
                foreach (LorId id in playersperpassive.Value)
                    data11.AddToList(id.LogGetSaveData());
                data10.AddData(playersperpassive.Key.bookItem.BookId.id.ToString(), data11);
            }
            data1.AddData("playersperpassives", data10);
            SaveData data12 = new SaveData();
            foreach (KeyValuePair<UnitDataModel, List<LogStatAdder>> playersstatadder in LogueBookModels.playersstatadders)
            {
                SaveData data13 = new SaveData();
                foreach (LogStatAdder logStatAdder in playersstatadder.Value)
                    data13.AddToList(logStatAdder.GetSaveData());
                data12.AddData(playersstatadder.Key.bookItem.BookId.id.ToString(), data13);
            }
            data1.AddData("playersstatadders", data12);
            SaveData data14 = new SaveData();
            foreach (LorId id in LogueBookModels.shopPick)
                data14.AddToList(id.LogGetSaveData());
            data1.AddData("shopPick", data14);
            SaveData data15 = new SaveData();
            foreach (BookModel model in LogueBookModels.booklist)
            {
                SaveData saveDataBookModel = LogueBookModels.GetSaveData_BookModel(model);
                data15.AddToList(saveDataBookModel);
            }
            data1.AddData("booklist", data15);
            data1.AddData("SubPlayerNum", new SaveData(LogueBookModels.playerModel.Count - 1));
            data1.AddData("nextinstanceid", LogueBookModels.nextinstanceid);
            "".Log("LogueInven Save End : " + DateTime.Now.ToString());
            return data1;
        }

        public static void LoadFromSaveData(SaveData save)
        {
            LogueBookModels.CreatePlayer();
            LogueBookModels.CreatePlayerBattle();
            if (save.GetData("SubPlayerNum").GetIntSelf() > 0)
            {
                for (int index = 0; index < save.GetData("SubPlayerNum").GetIntSelf(); ++index)
                    LogueBookModels.AddSubPlayer();
            }
            int index1 = 0;
            foreach (SaveData data in save.GetData("playerBattleModel"))
            {
                data.Log("CURRENTLY INITIALIZING UNIT NUMBER: " + index1.ToString());
                LogueBookModels.LoadFromSaveData_UnitBattleDataModel(data, LogueBookModels.playerBattleModel[index1]);
                ++index1;
            }
            foreach (KeyValuePair<string, SaveData> keyValuePair in save.GetData("playersPick").GetDictionarySelf())
            {
                KeyValuePair<string, SaveData> dic = keyValuePair;
                UnitDataModel key = LogueBookModels.playerModel.Find((Predicate<UnitDataModel>)(x => x.bookItem.BookId.id.ToString() == dic.Key));
                foreach (SaveData data in dic.Value)
                    LogueBookModels.playersPick[key].Add(ExtensionUtils.LogLoadFromSaveData(data));
            }
            foreach (KeyValuePair<string, SaveData> keyValuePair in save.GetData("playersperpassives").GetDictionarySelf())
            {
                KeyValuePair<string, SaveData> dic = keyValuePair;
                UnitDataModel key = LogueBookModels.playerModel.Find((Predicate<UnitDataModel>)(x => x.bookItem.BookId.id.ToString() == dic.Key));
                foreach (SaveData data in dic.Value)
                    LogueBookModels.playersperpassives[key].Add(ExtensionUtils.LogLoadFromSaveData(data));
            }
            foreach (SaveData data in save.GetData("shopPick"))
                LogueBookModels.shopPick.Add(ExtensionUtils.LogLoadFromSaveData(data));
            foreach (KeyValuePair<string, SaveData> keyValuePair in save.GetData("playersstatadders").GetDictionarySelf())
            {
                KeyValuePair<string, SaveData> dic = keyValuePair;
                UnitDataModel key = LogueBookModels.playerModel.Find(x => x.bookItem.BookId.id.ToString() == dic.Key);
                foreach (SaveData saveData in dic.Value)
                {
                    LogStatAdder statAdderBySave = LogStatAdder.CreateStatAdderBySave(saveData);
                    if (statAdderBySave != null)
                    {
                        statAdderBySave.LoadFromSaveData(saveData);
                        LogueBookModels.playersstatadders[key].Add(statAdderBySave);
                    }
                }
            }
            LogueBookModels.EmotionCardList = new List<RewardPassiveInfo>();
            try
            {
                SaveData data = save.GetData("emotionSelect");
                LogueBookModels.EmotionSelectDic[0] = data.GetInt("0");
                LogueBookModels.EmotionSelectDic[1] = data.GetInt("1");
                LogueBookModels.EmotionSelectDic[2] = data.GetInt("2");
                LogueBookModels.EmotionSelectDic[3] = data.GetInt("3");
            }
            catch
            {
            }
            LogueBookModels.RemainStageList = new Dictionary<ChapterGrade, List<LogueStageInfo>>();
            SaveData data1 = save.GetData("RemainStageList");
            foreach (ChapterGrade key in new List<ChapterGrade>()
            {
                ChapterGrade.Grade1,
                ChapterGrade.Grade2,
                ChapterGrade.Grade3,
                ChapterGrade.Grade4,
                ChapterGrade.Grade5,
                ChapterGrade.Grade6,
                ChapterGrade.Grade7
            })
            {
                if (data1.GetData(key.ToString()) != null)
                {
                    List<LogueStageInfo> logueStageInfoList = new List<LogueStageInfo>();
                    foreach (SaveData data2 in data1.GetData(key.ToString()))
                    {
                        LogueStageInfo stageInfo = Singleton<StagesXmlList>.Instance.GetStageInfo(ExtensionUtils.LogLoadFromSaveData(data2));
                        if (stageInfo != null)
                        {
                            if (!string.IsNullOrEmpty(stageInfo.script))
                                LogLikeMod.FindPickUp(stageInfo.script).LoadFromSaveData(stageInfo);
                        }
                        logueStageInfoList.Add(stageInfo);
                    }
                    LogueBookModels.RemainStageList.Add(key, logueStageInfoList);
                }
            }
            foreach (SaveData saveData in save.GetData("cardlist"))
                LogueBookModels.AddCard(ExtensionUtils.LogLoadFromSaveData(saveData.GetData("id")), saveData.GetData("num").GetIntSelf(), false);
            foreach (SaveData data3 in save.GetData("booklist"))
                LogueBookModels.booklist.Add(LogueBookModels.LoadFromSaveData_BookModel(data3));
            LogueBookModels.nextinstanceid = save.GetInt("nextinstanceid");
        }

        public static void CreateStageDesc(LogueStageInfo info)
        {
            EmotionCardXmlInfo registeredPickUpXml = LogLikeMod.GetRegisteredPickUpXml(info);
            AbnormalityCard abnormalityCard = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(registeredPickUpXml.Name);
            abnormalityCard.flavorText = "";
            if (info.type == StageType.Normal)
            {
                registeredPickUpXml._artwork = "Stage_Normal";
                abnormalityCard.cardName = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Stage_Normal");
                abnormalityCard.abilityDesc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Stage_Normal_Desc");
            }
            if (info.type == StageType.Elite)
            {
                registeredPickUpXml._artwork = "Stage_Elite";
                abnormalityCard.cardName = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Stage_Elite");
                abnormalityCard.abilityDesc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Stage_Elite_Desc");
            }
            if (info.type == StageType.Boss)
            {
                registeredPickUpXml._artwork = "Stage_Boss";
                abnormalityCard.cardName = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Stage_Boss");
                abnormalityCard.abilityDesc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Stage_Boss_Desc");
            }
            if (info.type == StageType.Mystery)
            {
                registeredPickUpXml._artwork = "Stage_Mystery";
                abnormalityCard.cardName = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Stage_Mystery");
                abnormalityCard.abilityDesc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Stage_Mystery_Desc");
            }
            if (info.type == StageType.Shop)
            {
                registeredPickUpXml._artwork = "Stage_Shop";
                abnormalityCard.cardName = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Stage_Shop");
                abnormalityCard.abilityDesc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Stage_Shop_Desc");
            }
            if (info.type == StageType.Rest)
            {
                registeredPickUpXml._artwork = "Stage_Rest";
                abnormalityCard.cardName = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Stage_Rest");
                abnormalityCard.abilityDesc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Stage_Rest_Desc");
            }
            if (!(info.script != string.Empty))
                return;
            PickUpModelBase pickUp = LogLikeMod.FindPickUp(info.script);
            registeredPickUpXml._artwork = pickUp.ArtWork == string.Empty ? info.script : pickUp.ArtWork;
            abnormalityCard.cardName = pickUp.Name;
            abnormalityCard.abilityDesc = pickUp.Desc;
            abnormalityCard.flavorText = pickUp.FlaverText;
        }

        public static bool CheckIsCanAdd(RewardPassiveInfo passive)
        {
            if (passive.passivetype == RewardPassiveType.Nolimit)
                return true;
            PickUpModelBase pickUp = LogLikeMod.FindPickUp(LogLikeMod.GetRegisteredPickUpXml(passive).Script[0]);
            pickUp.id = passive.id;
            pickUp.rewardinfo = passive;
            if (pickUp.GetPickupTarget() != null)
            {
                foreach (BattleUnitModel battleUnitModel in pickUp.GetPickupTarget())
                {
                    if (pickUp.IsCanPickUp(battleUnitModel.UnitData.unitData))
                        return true;
                }
                return false;
            }
            foreach (UnitDataModel target in LogueBookModels.playerModel)
            {
                if (pickUp.IsCanPickUp(target))
                    return true;
            }
            return false;
        }

        public static List<DiceCardItemModel> GetCardListForInven()
        {
            var ids = new List<DiceCardXmlInfo>();
            if (RMRCore.CurrentGamemode.ReplaceBaseDeck)
            {
                foreach (var card in DeckXmlList.Instance.GetData(RMRCore.CurrentGamemode.BaseDeckReplacement).cardIdList)
                {
                    if (ids.Count(x => x.id == card.id) == 0)
                        ids.Add(ItemXmlDataList.instance.GetCardItem(card, false));
                }
            }
            else ids.AddRange(new List<DiceCardXmlInfo>
            {
                ItemXmlDataList.instance.GetCardItem(1, false),
                ItemXmlDataList.instance.GetCardItem(2, false),
                ItemXmlDataList.instance.GetCardItem(3, false),
                ItemXmlDataList.instance.GetCardItem(4, false),
                ItemXmlDataList.instance.GetCardItem(5, false)
            });
            List<DiceCardItemModel> list3 = new List<DiceCardItemModel>();
            for (int i = 0; i < ids.Count; i++)
            {
                list3.Add(new DiceCardItemModel(ids[i])
                {
                    num = 99
                });
            }
            list3.AddRange(LogueBookModels.cardlist);
            list3.Sort(new Comparison<DiceCardItemModel>(SortUtil.CardItemCompByCost));
            return list3;
        }

        public static List<DiceCardItemModel> GetCardList(bool RemoveBasic = false, bool Decks = false)
        {
            List<DiceCardItemModel> list = LogueBookModels.cardlist;
            List<DiceCardItemModel> list2 = new List<DiceCardItemModel>();
            var ids = new List<DiceCardXmlInfo>();
            if (!RemoveBasic)
            {
                if (RMRCore.CurrentGamemode.ReplaceBaseDeck)
                {
                    foreach (var card in DeckXmlList.Instance.GetData(RMRCore.CurrentGamemode.BaseDeckReplacement).cardIdList)
                    {
                        if (ids.Count(x => x.id == card.id) == 0)
                            ids.Add(ItemXmlDataList.instance.GetCardItem(card, false));
                    }
                }
                else ids.AddRange(new List<DiceCardXmlInfo>
                {
                    ItemXmlDataList.instance.GetCardItem(1, false),
                    ItemXmlDataList.instance.GetCardItem(2, false),
                    ItemXmlDataList.instance.GetCardItem(3, false),
                    ItemXmlDataList.instance.GetCardItem(4, false),
                    ItemXmlDataList.instance.GetCardItem(5, false)
                });
                foreach (DiceCardXmlInfo xmlData in ids)
                {
                    list2.Add(new DiceCardItemModel(xmlData)
                    {
                        num = 99
                    });
                }
            }
            foreach (DiceCardItemModel diceCardItemModel in list)
            {
                list2.Add(new DiceCardItemModel(diceCardItemModel.ClassInfo)
                {
                    num = diceCardItemModel.num
                });
            }
            if (Decks)
            {
                foreach (UnitDataModel unitDataModel in LogueBookModels.playerModel)
                {
                    if (unitDataModel.bookItem.GetCardListFromCurrentDeck() != null)
                    {
                        using (List<DiceCardXmlInfo>.Enumerator enumerator4 = unitDataModel.bookItem.GetCardListFromCurrentDeck().GetEnumerator())
                        {
                            while (enumerator4.MoveNext())
                            {
                                DiceCardXmlInfo deckcard = enumerator4.Current;
                                DiceCardItemModel diceCardItemModel2 = list2.Find((DiceCardItemModel x) => x.ClassInfo.id == deckcard.id);
                                if (diceCardItemModel2 != null)
                                {
                                    diceCardItemModel2.num++;
                                }
                            }
                        }
                    }
                }
            }
            list2.RemoveAll((DiceCardItemModel x) => x.num <= 0);
            list2.Sort(new Comparison<DiceCardItemModel>(SortUtil.CardItemCompByCost));
            return list2;
        }

        public static void AddBook(LorId id)
        {
            BookModel bookModel = new BookModel(Singleton<BookXmlList>.Instance.GetData(id));
            bookModel.instanceId = LogueBookModels.nextinstanceid++;
            bookModel.TryGainUniquePassive();
            LogueBookModels.booklist.Add(bookModel);
        }

        public static void AddUpgradeCard(LorId cardid, bool callInvenChangeEvent = true) =>
            LogueBookModels.AddCard(Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(cardid).id, 1, callInvenChangeEvent);

        public static void AddCard(LorId cardId, int num = 1, bool callInvenChangeEvent = true)
        {
            if (callInvenChangeEvent)
                cardId = Singleton<GlobalLogueEffectManager>.Instance.InvenAddCardChange(cardId);
            if (num < 0)
                return;
            DiceCardItemModel diceCardItemModel = LogueBookModels.cardlist.Find(x => x.GetID() == cardId);
            if (diceCardItemModel != null)
            {
                diceCardItemModel.num += num;
            }
            else
            {
                DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(cardId);
                if (cardItem == null || cardItem.optionList.Contains(CardOption.NoInventory) || cardItem.optionList.Contains(CardOption.Basic))
                    return;
                LogueBookModels.cardlist.Add(new DiceCardItemModel(cardItem)
                {
                    num = num
                });
                LogueBookModels.cardlist.Sort(new Comparison<DiceCardItemModel>(SortUtil.CardItemCompByCost));
            }
        }

        public static void DeleteCard(LorId cardId, int num = 1)
        {
            DiceCardItemModel diceCardItemModel = LogueBookModels.cardlist.Find((Predicate<DiceCardItemModel>)(x => x.GetID() == cardId));
            if (num <= 0)
            {
                Debug.LogError("num must not be zero");
            }
            else
            {
                DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(cardId);
                if (diceCardItemModel == null)
                {
                    Debug.LogError("There's no Card");
                }
                else
                {
                    diceCardItemModel.num -= num;
                    if (diceCardItemModel.num > 0)
                        return;
                    using (List<UnitDataModel>.Enumerator enumerator = LogueBookModels.playerModel.GetEnumerator())
                    {
                    label_11:
                        if (enumerator.MoveNext())
                        {
                            UnitDataModel current = enumerator.Current;
                            bool inventory;
                            do
                            {
                                inventory = current.bookItem.MoveCardFromCurrentDeckToInventory(cardId);
                                if (diceCardItemModel.num >= 0)
                                    goto label_14;
                            }
                            while (inventory);
                            goto label_11;
                        }
                    }
                label_14:
                    foreach (UnitDataModel unitDataModel in LogueBookModels.playerModel)
                    {
                        if (unitDataModel.GetDeckAllList().Find((Predicate<DiceCardXmlInfo>)(x => x.id == cardItem.id)) != null)
                            return;
                    }
                    if (diceCardItemModel.num <= 0)
                        LogueBookModels.cardlist.Remove(diceCardItemModel);
                }
            }
        }

        public static bool RemoveCard(LorId cardId, int num = 1)
        {
            DiceCardItemModel diceCardItemModel = LogueBookModels.cardlist.Find((Predicate<DiceCardItemModel>)(x => x.GetID() == cardId));
            if (num <= 0)
            {
                Debug.LogError("num must not be zero");
                return false;
            }
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(cardId);
            if (cardItem != null && cardItem.optionList.Contains(CardOption.Basic))
                return true;
            if (diceCardItemModel == null)
            {
                Debug.LogError("not enough card");
                return false;
            }
            if (diceCardItemModel.num < num)
            {
                Debug.LogError("not enough card");
                return false;
            }
            diceCardItemModel.num -= num;
            return true;
        }

        public static void RemoveEquip(UnitDataModel model)
        {
            LogueBookModels.booklist.Find((Predicate<BookModel>)(x => x.owner == model))?.SetOwner((UnitDataModel)null);
        }

        public static void ResetPlayerData(UnitDataModel player)
        {
            if (LogueBookModels.BaseEquipStat == null)
            {
                LogueBookModels.BaseEquipStat = LogueBookModels.CopyEquipEffect(player.bookItem.ClassInfo.EquipEffect);
                LogueBookModels.BaseEquipStat.PassiveList.Clear();
                LogueBookModels.BaseEquipStat.SResist = AtkResist.Vulnerable;
                LogueBookModels.BaseEquipStat.SBResist = AtkResist.Vulnerable;
                LogueBookModels.BaseEquipStat.PResist = AtkResist.Vulnerable;
                LogueBookModels.BaseEquipStat.PBResist = AtkResist.Vulnerable;
                LogueBookModels.BaseEquipStat.HResist = AtkResist.Vulnerable;
                LogueBookModels.BaseEquipStat.HBResist = AtkResist.Vulnerable;
                LogueBookModels.BaseEquipStat.Hp = 40;
                LogueBookModels.BaseEquipStat.Break = 20;
                LogueBookModels.BaseEquipStat.SpeedMin = 1;
                LogueBookModels.BaseEquipStat.Speed = 4;
            }
            if (LogueBookModels.BaseXmlInfo == null)
            {
                LogueBookModels.BaseXmlInfo = LogueBookModels.CopyBookXmlInfo(player.bookItem.ClassInfo);
                LogueBookModels.BaseXmlInfo.EquipEffect = LogueBookModels.BaseEquipStat;
            }
            player.bookItem.ClassInfo._bookIcon = "";
            player.bookItem.ClassInfo.Chapter = 1;
            player.bookItem.ClassInfo.RangeType = EquipRangeType.Melee;
            player.bookItem.ClassInfo.EquipEffect = LogueBookModels.CopyEquipEffect(LogueBookModels.BaseEquipStat);
            player.bookItem.ClassInfo.CharacterSkin = new List<string>()
            {
                "Roland"
            };
            player.bookItem.TryGainUniquePassive();
            player.bookItem.SetOriginalStat();
            player.bookItem.SetOriginalSpeed();
            player.bookItem.SetOriginalResists();
            player.bookItem.SetOriginalPlayPoint();
            player.bookItem.SetOriginalCharacterName();
            typeof(BookModel).GetField("_selectedOriginalSkin", AccessTools.all).SetValue(player.bookItem, player.bookItem.ClassInfo.CharacterSkin[0]);
            typeof(BookModel).GetField("_characterSkin", AccessTools.all).SetValue(player.bookItem, player.bookItem.ClassInfo.CharacterSkin[0]);
        }

        public static int GetMoney() => PassiveAbility_MoneyCheck.GetMoney();

        public static void AddMoney(int amount) => PassiveAbility_MoneyCheck.AddMoney(amount);

        public static void SubMoney(int amount) => PassiveAbility_MoneyCheck.SubMoney(amount);

        public static BookXmlInfo CurPlayerEquipInfo(UnitDataModel data)
        {
            BookModel bookModel = LogueBookModels.booklist.Find((Predicate<BookModel>)(x => x.owner == data));
            BookXmlInfo bookXmlInfo = LogueBookModels.BaseXmlInfo;
            if (bookModel != null)
                bookXmlInfo = bookModel.ClassInfo;
            return bookXmlInfo;
        }

        public static void CreatePlayer()
        {
            LogueBookModels.EmotionSelectDic = new Dictionary<int, int>();
            LogueBookModels.EmotionSelectDic[0] = 0;
            LogueBookModels.EmotionSelectDic[1] = 0;
            LogueBookModels.EmotionSelectDic[2] = 0;
            LogueBookModels.EmotionSelectDic[3] = 0;
            LogLikeMod.AddPlayer = false;
            LogLikeMod.RecoverPlayers = false;
            LogLikeMod.LoadTextData(TextDataModel.CurrentLanguage);
            LogLikeMod.CreateShopEquipPages();
            Singleton<LogCardUpgradeManager>.Instance.ReLoadCurAllUpgradeCard();
            LogueBookModels.nextinstanceid = 0;
            LogLikeMod.ResetUIs();
            LogLikeMod.curChStageStep = 0;
            LogueBookModels.EmotionCardList = new List<RewardPassiveInfo>();
            LogueBookModels.RemainStageList = new Dictionary<ChapterGrade, List<LogueStageInfo>>();
            LogueBookModels.AddingRemainStageList();
            LogueBookModels.cardlist = new List<DiceCardItemModel>();
            LogueBookModels.booklist = new List<BookModel>();
            LogueBookModels.playerModel = new List<UnitDataModel>();
            UnitDataModel player = new UnitDataModel(new LorId(LogLikeMod.ModId, -854));
            player.bookItem.instanceId = LogueBookModels.nextinstanceid++;
            LogueBookModels.ResetPlayerData(player);
            player.bookItem.ClassInfo.EquipEffect.PassiveList.Add(new LorId(LogLikeMod.ModId, 1));
            if (File.Exists(LogueSaveManager.Saveroot + "/dshka"))
            {
                SaveData data = Singleton<LogueSaveManager>.Instance.LoadData("dshka");
                player.SetCustomName(abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLike_PlayerNugget_Dshka"));
                player.customizeData.LoadFromSaveData(data);
                
            }
            else if (File.Exists(LogueSaveManager.Saveroot + "/Beta14"))
            {
                SaveData data = Singleton<LogueSaveManager>.Instance.LoadData("Beta14");
                player.SetCustomName(abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLike_PlayerNugget_Berbtha"));
                player.customizeData.LoadFromSaveData(data);
            }
            else
            {
                player.SetCustomName(abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLike_PlayerName"));
                var unit = LibraryModel.Instance.GetFloor(UI.UIController.Instance.CurrentSephirah).GetUnitDataList()[0].customizeData;
                player.customizeData.Copy(unit);
                player.customizeData.specialCustomID = unit.specialCustomID;
            }
            player.customizeData.SetCustomData(true);
            player.bookItem.TryGainUniquePassive();
            LogueBookModels.playerModel.Add(player);
            LogLikeMod.curstagetype = StageType.Start;
            LogLikeMod.curchaptergrade = ChapterGrade.Grade1;
            LogueBookModels.playersPick = new Dictionary<UnitDataModel, List<LorId>>();
            LogueBookModels.playersPick.Add(LogueBookModels.playerModel[0], new List<LorId>());
            LogueBookModels.playersperpassives = new Dictionary<UnitDataModel, List<LorId>>();
            LogueBookModels.playersperpassives.Add(LogueBookModels.playerModel[0], new List<LorId>());
            LogueBookModels.playersperpassives[LogueBookModels.playerModel[0]].Add(new LorId(LogLikeMod.ModId, 1));
            LogueBookModels.playersstatadders = new Dictionary<UnitDataModel, List<LogStatAdder>>();
            LogueBookModels.playersstatadders.Add(LogueBookModels.playerModel[0], new List<LogStatAdder>());
            LogueBookModels.shopPick = new List<LorId>();
            LogLikeMod.NormalRewardCool = 0;
            if (LogLikeMod.saveloading)
                return;
            PassiveAbility_MoneyCheck.SetMoney(5);
        }

        public static BookXmlInfo CopyBookXmlInfo(BookXmlInfo original)
        {
            return new BookXmlInfo()
            {
                _bookIcon = original._bookIcon,
                TextId = original.TextId,
                Chapter = original.Chapter,
                CharacterSkin = new List<string>((IEnumerable<string>)original.CharacterSkin),
                skinType = original.skinType,
                RangeType = original.RangeType,
                EquipEffect = LogueBookModels.CopyEquipEffect(original.EquipEffect)
            };
        }

        public static BookEquipEffect CopyEquipEffect(BookEquipEffect original)
        {
            return new BookEquipEffect()
            {
                AddedStartDraw = original.AddedStartDraw,
                Break = original.Break,
                CardList = new List<BookSoulCardInfo>((IEnumerable<BookSoulCardInfo>)original.CardList),
                DeadLine = original.DeadLine,
                HBResist = original.HBResist,
                Hp = original.Hp,
                HpReduction = original.HpReduction,
                HResist = original.HResist,
                MaxPlayPoint = original.MaxPlayPoint,
                OnlyCard = original.OnlyCard,
                PassiveCost = original.PassiveCost,
                PassiveList = new List<LorId>((IEnumerable<LorId>)original.PassiveList),
                PBResist = original.PBResist,
                PResist = original.PResist,
                SBResist = original.SBResist,
                Speed = original.Speed,
                SpeedDiceNum = original.SpeedDiceNum,
                SpeedMin = original.SpeedMin,
                SResist = original.SResist,
                StartPlayPoint = original.StartPlayPoint,
                _PassiveList = new List<LorIdXml>((IEnumerable<LorIdXml>)original._PassiveList)
            };
        }

        public static bool CanAddCardToCurrentDeck(LorId cardId, BookModel model)
        {
            if (model.IsFixedDeck() || model.IsLockByBluePrimary())
                return false;
            DiceCardXmlInfo cardXmlInfo = ItemXmlDataList.instance.GetCardItem(cardId);
            if (cardXmlInfo == null)
                return false;
            if (cardXmlInfo.optionList.Contains(CardOption.OnlyPage))
            {
                if (!model.GetOnlyCards().Exists(x => x.id.GetOriginalId() == cardXmlInfo.id.GetOriginalId()))
                    return false;
            }
            else if (model.ClassInfo.RangeType == EquipRangeType.Melee)
            {
                if (cardXmlInfo.Spec.Ranged == CardRange.Far)
                    return false;
            }
            else if (model.ClassInfo.RangeType == EquipRangeType.Range && cardXmlInfo.Spec.Ranged == CardRange.Near)
                return false;
            DiceCardSelfAbilityBase diceCardSelfAbility = Singleton<AssemblyManager>.Instance.CreateInstance_DiceCardSelfAbility(ItemXmlDataList.instance.GetCardItem(cardId).Script);
            return diceCardSelfAbility == null || !(diceCardSelfAbility is LogDiceCardSelfAbility) || (diceCardSelfAbility as LogDiceCardSelfAbility).CanAddDeck(model.CopyCurrentDeck(), out CardEquipState _);
        }

        public static void EquipNewPage(UnitDataModel model, BookXmlInfo page, bool keepSuc = false)
        {
            UnitBattleDataModel model1 = LogueBookModels.playerBattleModel.Find((Predicate<UnitBattleDataModel>)(x => x.unitData == model));
            if (model1 == null)
                return;
            LogueBookModels.EquipNewPage(model1, page, keepSuc);
        }

        public static void EquipNewPage(UnitBattleDataModel model, BookXmlInfo page, bool keepSuc = false)
        {
            UnitDataModel unitData = model.unitData;
            float num = model.hp / (float)model.MaxHp;
            double hpReductionMod = model.hp;
            foreach (PassiveAbilityBase passive in unitData.bookItem.CreatePassiveList())
            {
                hpReductionMod = (double)passive.GetStartHp((float)(int)model.hp);
            }
            if (hpReductionMod == model.hp) hpReductionMod = 1f;
            else hpReductionMod = model.hp / hpReductionMod;

            num.Log("cur hp percent : " + num.ToString());
            BookXmlInfo classInfo = unitData.bookItem.ClassInfo;
            classInfo._bookIcon = page._bookIcon;
            classInfo.Chapter = page.Chapter;
            classInfo.CharacterSkin = new List<string>(page.CharacterSkin);
            classInfo.skinType = page.skinType;
            classInfo.EquipEffect = LogueBookModels.CopyEquipEffect(page.EquipEffect);
            classInfo.EquipEffect.PassiveList = new List<LorId>(classInfo.EquipEffect.PassiveList);
            for (int index = 0; index < LogueBookModels.playersperpassives[unitData].Count; ++index)
                classInfo.EquipEffect.PassiveList.Insert(index, LogueBookModels.playersperpassives[unitData][index]);
            if (!keepSuc)
                unitData.bookItem.ReleaseAllEquipedPassiveBooks();
            model.unitData.bookItem.ClassInfo.RangeType = page.RangeType;
            model.unitData.bookItem.TryGainUniquePassive();
            model.unitData.bookItem.SetOriginalStat();
            model.unitData.bookItem.SetOriginalSpeed();
            model.unitData.bookItem.SetOriginalResists();
            model.unitData.bookItem.SetOriginalPlayPoint();
            model.unitData.bookItem.SetOriginalSpeedNum();
            if (LogueBookModels.playersstatadders[model.unitData].Count > 0)
                LogueBookModels.ApplyPlayerStat(model, LogueBookModels.playersstatadders[model.unitData]);
            model.unitData.bookItem.SetOriginalCharacterName();
            typeof(BookModel).GetField("_selectedOriginalSkin", AccessTools.all).SetValue(model.unitData.bookItem, classInfo.CharacterSkin[0]);
            typeof(BookModel).GetField("_characterSkin", AccessTools.all).SetValue(model.unitData.bookItem, classInfo.CharacterSkin[0]);
            model.hp = (float)Mathf.RoundToInt(num * (float)model.MaxHp);
            double startHp = model.hp;
            foreach (PassiveAbilityBase passive in model.unitData.bookItem.CreatePassiveList())
            {
                startHp = (double)passive.GetStartHp((float)(int)model.hp);
            }
            model.hp = (float)(startHp * hpReductionMod);
            if ((double)model.hp <= 0.0 && !model.isDead)
                model.hp = 1f;
            List<DiceCardXmlInfo> fieldValue = LogLikeMod.GetFieldValue<List<DiceCardXmlInfo>>(unitData.bookItem, "_onlyCards");
            fieldValue.Clear();
            foreach (int id in page.EquipEffect.OnlyCard)
            {
                DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(page.workshopID, id), true);
                if (cardItem == null)
                    "".Log("Error : No Limit Card Data");
                else
                    fieldValue.Add(cardItem);
            }
            foreach (DiceCardXmlInfo diceCardXmlInfo in model.unitData.bookItem.GetCardListFromCurrentDeck())
            {
                if (!LogueBookModels.CanAddCardToCurrentDeck(diceCardXmlInfo.id, model.unitData.bookItem))
                {
                    diceCardXmlInfo.Log("ATTEMPTED TO UNEQUIP CARD");
                    model.unitData.bookItem.MoveCardFromCurrentDeckToInventory(diceCardXmlInfo.id);
                }
            }
        }

        public static void ApplyPlayerStat(UnitBattleDataModel model, List<LogStatAdder> adder)
        {
            int hp = LogueBookModels.StatAdderManager.GetHp(adder);
            int hpPercent = LogueBookModels.StatAdderManager.GetHpPercent(adder);
            int maxHp = (model.unitData.bookItem.HP + hp) * hpPercent / 100;
            if (maxHp < 1)
                maxHp = 1;

            int bp = LogueBookModels.StatAdderManager.GetBp(adder);
            int bpPercent = LogueBookModels.StatAdderManager.GetBpPercent(adder);
            int maxBp = (model.unitData.bookItem.Break + bp) * bpPercent / 100;
            if (maxBp < 1)
                maxBp = 1;

            model.unitData.bookItem.ClassInfo.EquipEffect.Hp = maxHp;
            model.unitData.bookItem.SetHp(maxHp);
            model.unitData.bookItem.ClassInfo.EquipEffect.Break = maxBp;
            model.unitData.bookItem.SetBp(maxBp);

            int speedDiceNum = model.unitData.bookItem.SpeedDiceNum + StatAdderManager.GetSpeedDiceNum(adder);
            model.unitData.bookItem.ClassInfo.EquipEffect.SpeedDiceNum = speedDiceNum;
            model.unitData.bookItem.SetSpeedDiceNum(speedDiceNum);

            int speedMax = model.unitData.bookItem.SpeedMax + StatAdderManager.GetSpeedMax(adder);
            model.unitData.bookItem.ClassInfo.EquipEffect.Speed = speedMax;
            model.unitData.bookItem.SetSpeedDiceMax(speedMax);

            int speedMin = model.unitData.bookItem.SpeedMin + StatAdderManager.GetSpeedMin(adder);
            model.unitData.bookItem.ClassInfo.EquipEffect.SpeedMin = speedMin;
            model.unitData.bookItem.SetSpeedDiceMin(speedMin);

            int startPlayPoint = model.unitData.bookItem.GetStartPlayPoint() + StatAdderManager.GetStartPlayPoint(adder);
            model.unitData.bookItem.ClassInfo.EquipEffect.StartPlayPoint = startPlayPoint;
            model.unitData.bookItem.SetStartPlayPoint(startPlayPoint);

            int maxPlayPoint = model.unitData.bookItem.ClassInfo.EquipEffect.MaxPlayPoint + StatAdderManager.GetMaxPlayPoint(adder);
            model.unitData.bookItem.ClassInfo.EquipEffect.MaxPlayPoint = maxPlayPoint;
            model.unitData.bookItem.SetMaxPlayPoint(maxPlayPoint);


            foreach (LogStatAdder logStatAdder in adder)
                model.unitData.bookItem.ClassInfo.RangeType = logStatAdder.GetRangeType(model.unitData.bookItem.ClassInfo.RangeType);
            foreach (LogStatAdder logStatAdder in adder)
            {
                AtkResist sresist = model.unitData.bookItem.equipeffect.SResist;
                model.unitData.bookItem.equipeffect.SResist = logStatAdder.GetResist(AtkResistType.SResist, sresist);
                AtkResist presist = model.unitData.bookItem.equipeffect.PResist;
                model.unitData.bookItem.equipeffect.PResist = logStatAdder.GetResist(AtkResistType.PResist, presist);
                AtkResist hresist = model.unitData.bookItem.equipeffect.HResist;
                model.unitData.bookItem.equipeffect.HResist = logStatAdder.GetResist(AtkResistType.HResist, hresist);
                AtkResist sbResist = model.unitData.bookItem.equipeffect.SBResist;
                model.unitData.bookItem.equipeffect.SBResist = logStatAdder.GetResist(AtkResistType.SBResist, sbResist);
                AtkResist pbResist = model.unitData.bookItem.equipeffect.PBResist;
                model.unitData.bookItem.equipeffect.PBResist = logStatAdder.GetResist(AtkResistType.PBResist, pbResist);
                AtkResist hbResist = model.unitData.bookItem.equipeffect.HBResist;
                model.unitData.bookItem.equipeffect.HBResist = logStatAdder.GetResist(AtkResistType.HBResist, hbResist);
            }
        }

        public static void AddPlayerStat(UnitDataModel model, LogStatAdder adder)
        {
            LogueBookModels.playersstatadders[model].Add(adder);
            BookModel bookModel = LogueBookModels.booklist.Find(x => x.owner == model);
            if (bookModel != null)
            {
                LogueBookModels.EquipNewPage(model, bookModel.ClassInfo, true);
            }
            else
            {
                LogueBookModels.EquipNewPage(model, LogueBookModels.BaseXmlInfo, true);
                if (LogueBookModels.playerModel[0] != model)
                {
                    model.bookItem.ClassInfo.CharacterSkin = new List<string>();
                    model.bookItem.ClassInfo.CharacterSkin.Add("KetherLibrarian");
                    typeof(BookModel).GetField("_selectedOriginalSkin", AccessTools.all).SetValue(model.bookItem, model.bookItem.ClassInfo.CharacterSkin[0]);
                    typeof(BookModel).GetField("_characterSkin", AccessTools.all).SetValue(model.bookItem, model.bookItem.ClassInfo.CharacterSkin[0]);
                }
            }
        }

        public static void AddPlayerStat(UnitBattleDataModel model, LogStatAdder adder)
        {
            LogueBookModels.AddPlayerStat(model.unitData, adder);
        }

        public static void AddSubPlayer()
        {
            if (LogueBookModels.playerModel.Count >= 5)
                return;
            UnitDataModel unitDataModel = new UnitDataModel(new LorId(LogLikeMod.ModId, -854 - LogueBookModels.playerModel.Count));
            unitDataModel.bookItem.instanceId = LogueBookModels.nextinstanceid++;
            unitDataModel.bookItem.ClassInfo.EquipEffect.PassiveList.Clear();
            try
            {
                var date = System.DateTime.Now;
                if (date.DayOfYear == 91 && date.Year % 4 > 0 || date.DayOfYear == 92 && date.Year % 4 == 0)
                {
                    SaveData beta14 = SaveManager.Instance.LoadData(LogLikeMod.path + "/DevNuggets/Beta14");
                    unitDataModel.customizeData.LoadFromSaveData(beta14);
                    unitDataModel.SetCustomName("Beta14");
                }
                else
                {
                    var floormodel = StageController.Instance.GetStageModel().GetFrontAvailableFloor();
                    UnitDataModel nugget = floormodel == null ? LibraryModel.Instance.GetFloor(SephirahType.Keter).GetUnitDataList()[LogueBookModels.playerModel.Count] : floormodel._floorModel.GetUnitDataList()[LogueBookModels.playerModel.Count];
                    unitDataModel.SetCustomName(nugget.name);
                    unitDataModel.customizeData.Copy(nugget.customizeData);
                }
            } catch (ArgumentOutOfRangeException e)
            {
                "".Log($"Player nugget {LogueBookModels.playerModel.Count} at currently selected floor does not exist: {e}");
                SaveData beta14 = SaveManager.Instance.LoadData(LogLikeMod.path + "/DevNuggets/Beta14");
                unitDataModel.customizeData.LoadFromSaveData(beta14);
                unitDataModel.SetCustomName(abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLike_PlayerName") + " " + LogueBookModels.playerModel.Count.ToString());
            } catch (Exception e)
            {
                Debug.LogWarning("OKAY REAL SHIT IS GOING ON IN SUBPLAYER ACTUALLY: " + e);
            }
            unitDataModel.customizeData.SetCustomData(true);
            LogueBookModels.ResetPlayerData(unitDataModel);
            unitDataModel.bookItem.ClassInfo.CharacterSkin = new List<string>()
            {
                "KetherLibrarian"
            };
            unitDataModel.bookItem._selectedOriginalSkin = unitDataModel.bookItem.ClassInfo.CharacterSkin[0];
            unitDataModel.bookItem._characterSkin = unitDataModel.bookItem.ClassInfo.CharacterSkin[0];
            LogueBookModels.playerModel.Add(unitDataModel);
            UnitBattleDataModel unitBattleDataModel = new UnitBattleDataModel(Singleton<StageController>.Instance.GetStageModel(), unitDataModel);
            LogueBookModels.playerBattleModel.Add(unitBattleDataModel);
            LogueBookModels.playersPick.Add(unitDataModel, new List<LorId>());
            LogueBookModels.playersperpassives.Add(unitDataModel, new List<LorId>());
            LogueBookModels.playersstatadders.Add(unitDataModel, new List<LogStatAdder>());
            Singleton<GlobalLogueEffectManager>.Instance.OnAddSubPlayer(unitDataModel);
        }

        /// <summary>
        /// Picks a number of encounters based on their types and the given <see cref="StageLimits"/>.
        /// </summary>
        /// <param name="stage">An empty list to be fed the new list of unique encounters.</param>
        /// <param name="allstage">A list containing all the encounters to pick from.</param>
        /// <param name="stageLimits">An object that limit how many of each type of encounter should appear in a chapter.</param>
        /// <param name="guaranteeChapterEvent">As to whether or not guarantee at least one event from that chapter.</param>
        public static void HandleLimitPícking(
          List<LogueStageInfo> stage,
          List<LogueStageInfo> allstage,
          StageLimits stageLimits, bool guaranteeChapterEvent = true)
        {
            int numBoss = stageLimits.Boss;
            int numNormal = stageLimits.Normal;
            int numShop = stageLimits.Shop;
            int numMystery = stageLimits.Mystery;
            int numElite = stageLimits.Elite;
            int numRest = stageLimits.Rest;
            if (guaranteeChapterEvent)
            {
                List<LogueStageInfo> mstage = StagesXmlList.Instance.GetChapterData(stageLimits.Chapter, true);
                ModdingUtils.SuffleList(mstage);
                stage.Add(mstage[0]);
                allstage.Remove(mstage[0]);
            }
            ModdingUtils.SuffleList(allstage);
            foreach (LogueStageInfo logueStageInfo in allstage)
            {
                if (logueStageInfo.type == StageType.Boss && numBoss > 0)
                {
                    stage.Add(logueStageInfo);
                    --numBoss;
                }
                else if (logueStageInfo.type == StageType.Normal && numNormal > 0)
                {
                    stage.Add(logueStageInfo);
                    --numNormal;
                }
                else if (logueStageInfo.type == StageType.Mystery && numMystery > 0)
                {
                    stage.Add(logueStageInfo);
                    --numMystery;
                }
                else if (logueStageInfo.type == StageType.Shop && numShop > 0)
                {
                    stage.Add(logueStageInfo);
                    --numShop;
                }
                else if (logueStageInfo.type == StageType.Elite && numElite > 0)
                {
                    stage.Add(logueStageInfo);
                    --numElite;
                }
            }
            LogueStageInfo stageInfo = Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 855));
            for (int index = 0; index < numRest; ++index)
                stage.Add(stageInfo);
        }

        public static List<LogueStageInfo> VanillaGamemodeReceptionList(ChapterGrade chapter)
        {
            List<LogueStageInfo> choiceReceptions = new List<LogueStageInfo>();
            List<LogueStageInfo> allReceptions = Singleton<StagesXmlList>.Instance.GetChapterDataVanilla(chapter);
            StageLimits stageLimits = new StageLimits
            {
                Normal = 0,
                Elite = 0,
                Mystery = 0,
                Shop = 0,
                Boss = 0,
                Rest = 0,
                Chapter = ChapterGrade.DummyGrade
            };
            switch (chapter)
            {
                case ChapterGrade.Grade1:
                    stageLimits = new StageLimits
                    {
                        Normal = 3,
                        Elite = 0,
                        Mystery = 3,
                        Shop = 1,
                        Boss = 1,
                        Rest = 1,
                        Chapter = chapter
                    };
                    break;
                case ChapterGrade.Grade2:
                    stageLimits = new StageLimits
                    {
                        Normal = 3,
                        Elite = 0,
                        Mystery = 2,
                        Shop = 1,
                        Boss = 1,
                        Rest = 1,
                        Chapter = chapter
                    };
                    break;
                case ChapterGrade.Grade3:
                    stageLimits = new StageLimits
                    {
                        Normal = 4,
                        Elite = 0,
                        Mystery = 2,
                        Shop = 1,
                        Boss = 1,
                        Rest = 1,
                        Chapter = chapter
                    };
                    break;
                case ChapterGrade.Grade4:
                    stageLimits = new StageLimits
                    {
                        Normal = 4,
                        Elite = 0,
                        Mystery = 2,
                        Shop = 2,
                        Boss = 1,
                        Rest = 2,
                        Chapter = chapter
                    };
                    break;
                case ChapterGrade.Grade5:
                    stageLimits = new StageLimits
                    {
                        Normal = 5,
                        Elite = 0,
                        Mystery = 2,
                        Shop = 2,
                        Boss = 1,
                        Rest = 2,
                        Chapter = chapter
                    };
                    break;
                case ChapterGrade.Grade6:
                    stageLimits = new StageLimits
                    {
                        Normal = 5,
                        Elite = 0,
                        Mystery = 2,
                        Shop = 2,
                        Boss = 1,
                        Rest = 2,
                        Chapter = chapter
                    };
                    break;
            }
            LogueBookModels.HandleLimitPícking(choiceReceptions, allReceptions, stageLimits);
            if (chapter == ChapterGrade.Grade4)
                choiceReceptions.Add(Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 80000)));
            // WORKSHOP CONTRACT EVENT
            return choiceReceptions;
        }

        public static void AddingRemainStageList()
        {
            for (int i = 0; i < 7; i++)
            {
                try
                {
                    var list = RMRCore.CurrentGamemode.InitializeChapterStageList((ChapterGrade)i);
                    var list2 = new List<LogueStageInfo>();
                    list2.AddRange(list);
                    "".Log($"Chapter {(i + 1).ToString()} StageList");
                    foreach (LogueStageInfo logueStageInfo in list2)
                    {
                        if (StageClassInfoList.Instance.GetData(logueStageInfo.Id) is var stage && (stage == null || stage.waveList == null))
                        {
                            list.Remove(logueStageInfo);
                            "".Log($"!WARNING! -- INVALID STAGE REMOVED FROM STAGE LIST ON INITIALIZE");
                            "".Log($"{logueStageInfo.Id.packageId} _ {logueStageInfo.Id.id.ToString()}");
                        }
                        else
                            "".Log($"{logueStageInfo.Id.packageId} _ {logueStageInfo.Id.id.ToString()}");
                    }
                    if (list != null && list.Count > 0)
                    {
                        LogueBookModels.RemainStageList[(ChapterGrade)i] = new List<LogueStageInfo>();
                        LogueBookModels.RemainStageList[(ChapterGrade)i].AddRange(list);
                    }
                } catch (Exception e)
                {
                    Debug.Log("Error when initializing chapters: " + e);
                }
            }
        }

        public static List<EmotionCardXmlInfo> GetNextList(ChapterGrade grade, bool Stepone = false)
        {
            List<LogueStageInfo> collection = new List<LogueStageInfo>(LogueBookModels.RemainStageList[grade]);
            if (collection.Count == 0)
                return new List<EmotionCardXmlInfo>();
            List<EmotionCardXmlInfo> nextList = new List<EmotionCardXmlInfo>();
            List<LogueStageInfo> logueStageInfoList = new List<LogueStageInfo>(collection);
            if (logueStageInfoList.FindAll(x => x.type == StageType.Normal).Count > 1 || logueStageInfoList.Count > 5)
                logueStageInfoList.Remove(logueStageInfoList.Find(x => x.type == StageType.Boss));
            for (int index = 0; index < 3; )
            {
                LogueStageInfo info = logueStageInfoList[UnityEngine.Random.Range(0, logueStageInfoList.Count)];
                EmotionCardXmlInfo thing = LogLikeMod.GetRegisteredPickUpXml(info);
                logueStageInfoList.Remove(info);
                string pid = LogLikeMod.GetPickUpXmlWorkShopId_Stage(thing);
                if (thing != null)
                {
                    nextList.Add(thing);
                    LogueBookModels.CreateStageDesc(info);
                    if (logueStageInfoList.Count == 0)
                        break;
                    index++;
                } else
                {
                    Debug.Log("NULL STAGE!!! WATCH THE FUCK OUT!!: " + info.Id.packageId + " --- " + info.Id.id.ToString());
                    Debug.Log(pid == null ? "PID IS NULL" : "PID IS: " + pid);
                }
            }
            return nextList;
        }

        public static List<EmotionCardXmlInfo> GetPassiveRewards(List<RewardPassiveInfo> list)
        {
            List<RewardPassiveInfo> rewardPassiveInfoList = new List<RewardPassiveInfo>();
            List<RewardPassiveInfo> rewards = new List<RewardPassiveInfo>();
            if (list != null) rewards.AddRange(list);
            List<EmotionCardXmlInfo> passiveRewards = new List<EmotionCardXmlInfo>();
            do
            {
                RewardPassiveInfo reward = RewardingModel.GetReward(rewards);
                if (reward != null)
                {
                    if (LogueBookModels.CheckIsCanAdd(reward))
                        rewardPassiveInfoList.Add(reward);
                    rewards.Remove(reward);
                }
            }
            while (rewards.Count != 0 && rewardPassiveInfoList.Count != 3);
            for (int index = 0; index < rewardPassiveInfoList.Count; ++index)
                passiveRewards.Add(LogLikeMod.GetRegisteredPickUpXml(rewardPassiveInfoList[index]));
            return passiveRewards;
        }

        public static List<EmotionCardXmlInfo> GetPassiveRewards_Inlist(List<RewardPassiveInfo> list)
        {
            return LogueBookModels.GetPassiveRewards(list);
        }

        public static void RemoveStageInlist(LorId id)
        {
            foreach (KeyValuePair<ChapterGrade, List<LogueStageInfo>> remainStage in LogueBookModels.RemainStageList)
                remainStage.Value.RemoveAll((Predicate<LogueStageInfo>)(x => x.Id == id));
        }

        public static void RemoveStageInlist(LorId id, ChapterGrade chapter)
        {
            LogueStageInfo logueStageInfo = LogueBookModels.RemainStageList[chapter].Find((Predicate<LogueStageInfo>)(x => x.Id == id));
            if (logueStageInfo == null)
                return;
            LogueBookModels.RemainStageList[chapter].Remove(logueStageInfo);
        }

        public static void CreatePlayerBattle()
        {
            LogueBookModels.playerBattleModel = new List<UnitBattleDataModel>();
            foreach (UnitDataModel data in LogueBookModels.playerModel)
            {
                UnitBattleDataModel unitBattleDataModel = new UnitBattleDataModel(Singleton<StageController>.Instance.GetStageModel(), data);
                LogueBookModels.playerBattleModel.Add(unitBattleDataModel);
            }
        }

        public static void AddUpgradeCard(LorId cardid, int index = 0, int count = 1, int num = 1, bool callInvenChangeEvent = true)
        {
            LogueBookModels.AddCard(Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(cardid, index, count).id, num, callInvenChangeEvent);
        }

        public static int GetIndexOfUnit(BattleUnitModel model)
        {
            return playerModel.IndexOf(model.UnitData.unitData);
        }

        public static int GetIndexOfUnit(UnitBattleDataModel model)
        {
            return playerBattleModel.IndexOf(model);
        }

        public class StatAdderManager
        {
            public static int GetHp(List<LogStatAdder> adder)
            {
                int hp = 0;
                foreach (LogStatAdder logStatAdder in adder)
                    hp += logStatAdder.maxhp;
                return hp;
            }

            public static int GetHpPercent(List<LogStatAdder> adder)
            {
                int hpPercent = 100;
                foreach (LogStatAdder logStatAdder in adder)
                    hpPercent += logStatAdder.maxhppercent;
                return hpPercent;
            }

            public static int GetBp(List<LogStatAdder> adder)
            {
                int bp = 0;
                foreach (LogStatAdder logStatAdder in adder)
                    bp += logStatAdder.maxbreak;
                return bp;
            }

            public static int GetBpPercent(List<LogStatAdder> adder)
            {
                int bpPercent = 100;
                foreach (LogStatAdder logStatAdder in adder)
                    bpPercent += logStatAdder.maxbreakpercent;
                return bpPercent;
            }

            public static int GetStartPlayPoint(List<LogStatAdder> adder)
            {
                int startplaypoint = 0;
                foreach (LogStatAdder logStatAdder in adder)
                    startplaypoint += logStatAdder.startplaypoint;
                return startplaypoint;
            }

            public static int GetMaxPlayPoint(List<LogStatAdder> adder)
            {
                int maxplaypoint = 0;
                foreach (LogStatAdder logStatAdder in adder)
                    maxplaypoint += logStatAdder.maxplaypoint;
                return maxplaypoint;
            }

            public static int GetSpeedDiceNum(List<LogStatAdder> adder)
            {
                int speeddicenum = 0;
                foreach (LogStatAdder logStatAdder in adder)
                    speeddicenum += logStatAdder.speeddicenum;
                return speeddicenum;
            }

            public static int GetSpeedMax(List<LogStatAdder> adder)
            {
                int speedmax = 0;
                foreach (LogStatAdder logStatAdder in adder)
                    speedmax += logStatAdder.speedmax;
                return speedmax;
            }

            public static int GetSpeedMin(List<LogStatAdder> adder)
            {
                int speedmin = 0;
                foreach (LogStatAdder logStatAdder in adder)
                    speedmin += logStatAdder.speedmin;
                return speedmin;
            }

        }

        public enum StageFlag
        {
            None,
            OnlyNormal,
        }

        public struct StageLimits
        {
            public int Normal;
            public int Elite;
            public int Mystery;
            public int Shop;
            public int Boss;
            public int Rest;
            public ChapterGrade Chapter;
        }
    }
}
