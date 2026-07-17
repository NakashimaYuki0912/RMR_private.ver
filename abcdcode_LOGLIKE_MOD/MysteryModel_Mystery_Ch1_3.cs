// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch1_3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch1_3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using TMPro;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Mystery node model: MysteryModel_Mystery_Ch1_3</summary>

    public class MysteryModel_Mystery_Ch1_3 : MysteryBase
    {
        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (PassiveAbility_MoneyCheck.GetMoney() >= 5 || id != 0)
                return;
            this.FrameObj["choice_btn0"].GetComponent<Image>().sprite = LogLikeMod.ArtWorks["disabledButton"];
            this.FrameObj["Desc0"].GetComponent<TextMeshProUGUI>().text = TextDataModel.GetText("Mystery_RequireCondition") + this.FrameObj["Desc0"].GetComponent<TextMeshProUGUI>().text;
        }

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0 && choiceid == 0 && PassiveAbility_MoneyCheck.GetMoney() < 5)
                return;
            if (this.curFrame.FrameID == 1)
            {
                PassiveAbility_MoneyCheck.SubMoney(5);
                MysteryBase.SetNextStageCustom(new LorId(LogLikeMod.ModId, 1100031));
            }
            base.OnClickChoice(choiceid);
        }
    }
}
