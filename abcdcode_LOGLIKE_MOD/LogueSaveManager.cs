// -----------------------------------------------------------------------------
// LOGLIKE core UI/data: LogueSaveManager
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogueSaveManager.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using GameSave;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>LOGLIKE type: LogueSaveManager</summary>

    public class LogueSaveManager : Singleton<LogueSaveManager>
    {
        public void RemoveData(string savename)
        {
            if (!Directory.Exists(LogueSaveManager.Saveroot))
                Directory.CreateDirectory(LogueSaveManager.Saveroot);
            if (!File.Exists($"{LogueSaveManager.Saveroot}/{savename}"))
                return;
            File.Delete($"{LogueSaveManager.Saveroot}/{savename}");
        }

        public void SaveData(GameSave.SaveData data, string savename)
        {
            if (!Directory.Exists(LogueSaveManager.Saveroot))
                Directory.CreateDirectory(LogueSaveManager.Saveroot);
            object serializedData = data.GetSerializedData();
            using (FileStream serializationStream = File.Create($"{LogueSaveManager.Saveroot}/{savename}"))
                new BinaryFormatter().Serialize((Stream)serializationStream, serializedData);
        }

        public GameSave.SaveData LoadData(string savename)
        {
            if (!Directory.Exists(LogueSaveManager.Saveroot))
            {
                Directory.CreateDirectory(LogueSaveManager.Saveroot);
                return (GameSave.SaveData)null;
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if (File.Exists($"{LogueSaveManager.Saveroot}/{savename}"))
            {
                object serialized;
                using (FileStream serializationStream = File.Open($"{LogueSaveManager.Saveroot}/{savename}", FileMode.Open))
                    serialized = binaryFormatter.Deserialize((Stream)serializationStream);
                if (serialized != null)
                {
                    GameSave.SaveData saveData = new GameSave.SaveData();
                    saveData.LoadFromSerializedData(serialized);
                    return saveData;
                }
            }
            return (GameSave.SaveData)null;
        }

        public static string Saveroot => SaveManager.savePath + "/LogueSave";

        public void AddToObtainCount(object item, int count = 1)
        {
            GameSave.SaveData saveData = Singleton<LogueSaveManager>.Instance.LoadData("RMR_ItemCatalog");
            saveData.SetData("ObtainCount_" + item.GetType().Name, new GameSave.SaveData(saveData.GetInt("ObtainCount_" + item.GetType().Name) + count));
            Singleton<LogueSaveManager>.Instance.SaveData(saveData, "RMR_ItemCatalog");
        }
    }
}
