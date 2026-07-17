// -----------------------------------------------------------------------------
// Library of Ruina mod script: EquipChangeOrder
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\EquipChangeOrder.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using GameSave;
using System;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>EquipChangeOrder</summary>

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
