// -----------------------------------------------------------------------------
// Library of Ruina mod script: MemberShipShop
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MemberShipShop.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using GameSave;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>MemberShipShop</summary>

public class MemberShipShop : ShopBase
{
  public override SaveData GetSaveData()
  {
    SaveData saveData1 = new SaveData();
    SaveData data1 = new SaveData();
    int num = 1000;
    while (true)
    {
      if (this.FrameObj.ContainsKey("Goods_Passive" + num.ToString()))
      {
        SaveData saveData2 = this.FrameObj["Goods_Passive" + num.ToString()].GetComponent<ShopGoods>().GetSaveData();
        saveData2.AddData("index", new SaveData(num));
        data1.AddToList(saveData2);
        ++num;
      }
      else
        break;
    }
    saveData1.AddData("Cards", data1);
    SaveData data2 = new SaveData();
    num = 0;
    while (true)
    {
      if (this.FrameObj.ContainsKey("Goods_Passive" + num.ToString()))
      {
        SaveData saveData3 = this.FrameObj["Goods_Passive" + num.ToString()].GetComponent<ShopGoods>().GetSaveData();
        saveData3.AddData("index", new SaveData(num));
        data2.AddToList(saveData3);
        ++num;
      }
      else
        break;
    }
    saveData1.AddData("Passives", data2);
    return saveData1;
  }

  public override void LoadFromSaveData(SaveData data)
  {
    foreach (SaveData data1 in data.GetData("Cards"))
      this.Shop_PassiveCreating(Singleton<RewardPassivesList>.Instance.GetPassiveInfo(ExtensionUtils.LogLoadFromSaveData(data1.GetData("Id"))), new Vector2(0.0f, 0.0f), data1.GetInt("index")).LoadFromSaveData(data1);
    foreach (SaveData data2 in data.GetData("Passives"))
      this.Shop_PassiveCreating(Singleton<RewardPassivesList>.Instance.GetPassiveInfo(ExtensionUtils.LogLoadFromSaveData(data2.GetData("Id"))), new Vector2(0.0f, 0.0f), data2.GetInt("index")).LoadFromSaveData(data2);
  }

  public override void CreateShop_Card(int num = 5)
  {
    List<RewardPassiveInfo> chapterData = Singleton<RewardPassivesList>.Instance.GetChapterData(ChapterGrade.GradeAll, PassiveRewardListType.Shop, new LorId(LogLikeMod.ModId, 90000));
    List<RewardPassiveInfo> rewardPassiveInfoList = new List<RewardPassiveInfo>();
    int count = this.OnceCount(num);
    rewardPassiveInfoList.AddRange((IEnumerable<RewardPassiveInfo>) ShopBase.GetPassiveInList(chapterData, count, ShopRewardType.Once));
    for (int index = 0; index < rewardPassiveInfoList.Count; ++index)
      this.Shop_PassiveCreating(rewardPassiveInfoList[index], this.GetShopShape_Card(rewardPassiveInfoList.Count, index), index + 1000);
  }
}
}
