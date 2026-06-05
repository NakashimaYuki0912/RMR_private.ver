// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LoguePlayDataSaver
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using Mod;
using RogueLike_Mod_Reborn;
using System;


namespace abcdcode_LOGLIKE_MOD
{

    public class LoguePlayDataSaver
    {
        public static string version = "4.8";

        public static void LoadChDebugData(ChapterGrade grade)
        {
            SaveData data = new SaveData();
            data.AddData("version", new SaveData(LoguePlayDataSaver.version));
            data.AddData("LogueBookModel", LogueBookModels.CreateChSaveData(grade));
            data.AddData("LogLikeMod", LogLikeMod.CreateChSaveData(grade));
            data.AddData("GlobalEffect", Singleton<GlobalLogueEffectManager>.Instance.GetSaveData());
            LoguePlayDataSaver.LoadPlayData(data);
        }

        public static void RemoveFlashData()
        {
            LoguePlayDataSaver.RemoveShop();
            LoguePlayDataSaver.RemoveMystery();
        }

        public static void RemoveShop() => Singleton<LogueSaveManager>.Instance.RemoveData("LastShop");

        public static void SaveShop(ShopBase shop)
        {
            Singleton<LogueSaveManager>.Instance.SaveData(shop.GetSaveData(), "LastShop");
        }

        public static bool LoadShop(ShopBase shop)
        {
            SaveData data = Singleton<LogueSaveManager>.Instance.LoadData("LastShop");
            if (data == null)
                return false;
            shop.LoadFromSaveData(data);
            return true;
        }

        public static void RemoveMystery()
        {
            Singleton<LogueSaveManager>.Instance.RemoveData("LastMystery");
        }

        public static void SaveMystery(MysteryBase mystery)
        {
            SaveData saveData = mystery.GetSaveData();
            if (saveData == null)
                return;
            Singleton<LogueSaveManager>.Instance.SaveData(saveData, "LastMystery");
        }

        public static bool LoadMystery(MysteryBase mystery)
        {
            SaveData savedata = Singleton<LogueSaveManager>.Instance.LoadData("LastMystery");
            if (savedata == null)
                return false;
            mystery.LoadFromSaveData(savedata);
            return true;
        }

        public static void RemovePlayerData()
        {
            Singleton<LogueSaveManager>.Instance.RemoveData("Lastest");
        }

        public static void SavePlayData()
        {
            SaveData data1 = new SaveData();
            data1.AddData("version", new SaveData(LoguePlayDataSaver.version));
            data1.AddData("LogueBookModel", LogueBookModels.GetSaveData());
            data1.AddData("LogLikeMod", LogLikeMod.GetSaveData());
            data1.AddData("GlobalEffect", Singleton<GlobalLogueEffectManager>.Instance.GetSaveData());
            SaveData data2 = new SaveData();
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
                data2.AddToList(new SaveData(logMod.invInfo.workshopInfo.uniqueId));
            data1.AddData("modlist", data2);
            RoguelikeGamemodeController.Instance.SaveCurrentGamemodeName(data1);
            RoguelikeGamemodeController.Instance.SaveCurrentGamemodeData();
            Singleton<LogueSaveManager>.Instance.SaveData(data1, "Lastest");
        }

        public static void SavePlayData_Menu()
        {
            SaveData saveData = Singleton<LogueSaveManager>.Instance.LoadData("Lastest");
            if (saveData == null || saveData.GetString("version") != LoguePlayDataSaver.version)
                return;
            saveData.SetData("version", new SaveData(LoguePlayDataSaver.version));
            saveData.SetData("LogueBookModel", LogueBookModels.GetSaveData());
            saveData.SetData("GlobalEffect", Singleton<GlobalLogueEffectManager>.Instance.GetSaveData());
            saveData.GetData("LogLikeMod").SetData("Money", new SaveData(PassiveAbility_MoneyCheck.GetMoney()));
            Singleton<LogueSaveManager>.Instance.SaveData(saveData, "Lastest");
        }

        public static void LoadPlayData(SaveData data)
        {
            LogLikeMod.saveloading = true;
            LogueBookModels.LoadFromSaveData(data.GetData("LogueBookModel"));
            LogLikeMod.LoadFromSaveData(data.GetData("LogLikeMod"));
            Singleton<GlobalLogueEffectManager>.Instance.LoadFromSaveData(data.GetData("GlobalEffect"));
            LogLikeMod.saveloading = false;
        }

        public static void LoadPlayData()
        {
            LoguePlayDataSaver.LoadPlayData(Singleton<LogueSaveManager>.Instance.LoadData("Lastest"));
        }

        public static bool CheckPlayerData()
        {
            SaveData saveData1 = Singleton<LogueSaveManager>.Instance.LoadData("Lastest");
            if (saveData1 == null || saveData1.GetString("version") != LoguePlayDataSaver.version)
                return false;
            foreach (SaveData saveData2 in saveData1.GetData("modlist"))
            {
                SaveData modinfo = saveData2;
                if (LogLikeMod.GetLogMods().Find((Predicate<ModContentInfo>)(x => x.invInfo.workshopInfo.uniqueId == modinfo.GetStringSelf())) == null)
                    return false;
            }
            return true;
        }
    }
}
