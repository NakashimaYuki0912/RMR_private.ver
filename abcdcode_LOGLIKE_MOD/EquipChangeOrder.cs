// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.EquipChangeOrder
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using System;


namespace abcdcode_LOGLIKE_MOD
{

    public class EquipChangeOrder : Savable
    {
        public UnitBattleDataModel model;
        public BookXmlInfo equip;

        public SaveData GetSaveData()
        {
            SaveData saveData = new SaveData();
            saveData.AddData("equip", this.equip.id.LogGetSaveData());
            saveData.AddData("model", this.model.unitData.bookItem.ClassInfo.id.LogGetSaveData());
            return saveData;
        }

        public void LoadFromSaveData(SaveData data)
        {
            LorId id = ExtensionUtils.LogLoadFromSaveData(data.GetData("equip"));
            this.equip = Singleton<BookXmlList>.Instance.GetData(id);
            id = ExtensionUtils.LogLoadFromSaveData(data.GetData("model"));
            this.model = LogueBookModels.playerBattleModel.Find((Predicate<UnitBattleDataModel>)(x => x.unitData.bookItem.ClassInfo.id == id));
        }
    }
}
