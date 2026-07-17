// -----------------------------------------------------------------------------
// Shop system component: ShopModel_Stigma
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\ShopModel_Stigma.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Shop component: ShopModel_Stigma</summary>

public class ShopModel_Stigma : MemberShipShop
{
  public override void CreateShop_passive(int num = 5)
  {
    List<RewardPassiveInfo> chapterData = Singleton<RewardPassivesList>.Instance.GetChapterData(ChapterGrade.GradeAll, PassiveRewardListType.Shop, new LorId(LogLikeMod.ModId, 82000));
    List<RewardPassiveInfo> rewardPassiveInfoList = new List<RewardPassiveInfo>();
    rewardPassiveInfoList.AddRange((IEnumerable<RewardPassiveInfo>) ShopBase.GetPassiveInList(chapterData, num, ShopRewardType.Eternal));
    for (int index = 0; index < rewardPassiveInfoList.Count; ++index)
      this.Shop_PassiveCreating(rewardPassiveInfoList[index], this.GetShopShape_Passive(rewardPassiveInfoList.Count, index), index);
  }
}
}
