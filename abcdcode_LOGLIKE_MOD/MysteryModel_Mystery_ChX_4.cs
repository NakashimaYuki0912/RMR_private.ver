// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_ChX_4
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryModel_Mystery_ChX_4 : MysteryBase
    {
        public int index;
        public List<LorId> droplist;

        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (PassiveAbility_MoneyCheck.GetMoney() >= 10 || id != 0)
                return;
            this.FrameObj["choice_btn0"].GetComponent<Image>().sprite = LogLikeMod.ArtWorks["disabledButton"];
            this.FrameObj["Desc0"].GetComponent<TextMeshProUGUI>().text = TextDataModel.GetText("Mystery_RequireCondition") + this.FrameObj["Desc0"].GetComponent<TextMeshProUGUI>().text;
        }

        public override void OnClickChoice(int choiceid)
        {
            if (choiceid == 0 && PassiveAbility_MoneyCheck.GetMoney() >= 10)
            {
                LogueBookModels.SubMoney(10);
                List<RewardPassiveInfo> all = Singleton<RewardPassivesList>.Instance.GetChapterData(ChapterGrade.GradeAll, PassiveRewardListType.Shop, new LorId(-1)).FindAll((Predicate<RewardPassiveInfo>)(x => x.shoptype == ShopRewardType.Once));
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
