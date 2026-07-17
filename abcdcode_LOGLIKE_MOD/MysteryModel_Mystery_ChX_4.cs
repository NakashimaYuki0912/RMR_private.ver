// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_ChX_4
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_ChX_4.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Mystery node model: MysteryModel_Mystery_ChX_4</summary>

    public class MysteryModel_Mystery_ChX_4 : MysteryBase
    {
        public int index;
        public List<LorId> droplist;

        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (this.droplist == null)
                this.droplist = new List<LorId>();
            if (PassiveAbility_MoneyCheck.GetMoney() >= 10 || id != 0)
                return;
            this.FrameObj["choice_btn0"].GetComponent<Image>().sprite = LogLikeMod.ArtWorks["disabledButton"];
            this.FrameObj["Desc0"].GetComponent<TextMeshProUGUI>().text = TextDataModel.GetText("Mystery_RequireCondition") + this.FrameObj["Desc0"].GetComponent<TextMeshProUGUI>().text;
        }

        public override void OnClickChoice(int choiceid)
        {
            if (choiceid == 0 && PassiveAbility_MoneyCheck.GetMoney() >= 10)
            {
                if (this.droplist == null)
                    this.droplist = new List<LorId>();
                List<RewardPassiveInfo> all = Singleton<RewardPassivesList>.Instance.GetChapterData(ChapterGrade.GradeAll, PassiveRewardListType.Shop, new LorId(-1))?.FindAll((Predicate<RewardPassiveInfo>)(x => x != null && x.shoptype == ShopRewardType.Once)) ?? new List<RewardPassiveInfo>();
                all.RemoveAll(x => x.id != null && this.droplist.Contains(x.id));
                if (all.Count == 0)
                {
                    UnityEngine.Debug.LogWarning("[RMR Mystery_ChX_4] No one-time shop rewards available.");
                    base.OnClickChoice(choiceid);
                    return;
                }
                LogueBookModels.SubMoney(10);
                ModdingUtils.SuffleList<RewardPassiveInfo>(all);
                MysteryModel_ShopItemReward.PopupShopReward(new List<RewardPassiveInfo>()
      {
        all[0]
      }, new MysteryModel_ShopItemReward.ChoiceResult(this.Checking));
                this.droplist.Add(all[0].id);
            }
            base.OnClickChoice(choiceid);
        }

        public void Checking(
          MysteryModel_ShopItemReward mystery,
          MysteryModel_ShopItemReward.RewardGood good)
        {
            LoguePlayDataSaver.SavePlayData_Menu();
            Singleton<MysteryManager>.Instance.EndMystery((MysteryBase)mystery);
        }
    }
}
