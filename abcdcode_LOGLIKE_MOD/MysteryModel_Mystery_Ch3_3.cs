// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch3_3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch3_3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Mystery node model: MysteryModel_Mystery_Ch3_3</summary>

    public class MysteryModel_Mystery_Ch3_3 : MysteryBase
    {
        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (PassiveAbility_MoneyCheck.GetMoney() >= 20)
                return;
            this.FrameObj["choice_btn0"].GetComponent<Image>().sprite = LogLikeMod.ArtWorks["disabledButton"];
            this.FrameObj["Desc0"].GetComponent<TextMeshProUGUI>().text = TextDataModel.GetText("Mystery_RequireCondition") + this.FrameObj["Desc0"].GetComponent<TextMeshProUGUI>().text;
        }

        public override void OnClickChoice(int choiceid)
        {
            if (choiceid == 0 && PassiveAbility_MoneyCheck.GetMoney() >= 20)
            {
                PassiveAbility_MoneyCheck.SubMoney(20);
                RewardInfo rewardInfo = new RewardInfo()
                {
                    grade = ChapterGrade.Grade3,
                    rewards = new List<RewardPassiveInfo>()
                };
                rewardInfo.rewards.Add(Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 1130003)));
                LogLikeMod.rewards_passive.Insert(0, rewardInfo);
            }
            base.OnClickChoice(choiceid);
        }
    }
}
