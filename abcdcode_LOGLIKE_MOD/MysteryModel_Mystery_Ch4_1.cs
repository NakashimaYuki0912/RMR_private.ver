// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch4_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch4_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using TMPro;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Mystery node model: MysteryModel_Mystery_Ch4_1</summary>

    public class MysteryModel_Mystery_Ch4_1 : MysteryBase
    {
        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (id != 2)
                return;
            int num = Random.Range(0, 4);
            this.FrameObj["Dia"].GetComponent<TextMeshProUGUI>().text = TextDataModel.GetText("MysteryCh4_1Frame1Dia" + num.ToString());
        }

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0)
            {
                if (choiceid == 0)
                {
                    LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 4001)));
                    LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 4001)));
                    LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 4001)));
                    LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 4001)));
                    LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 4001)));
                    MysteryModel_CardReward.PopupCardReward_AutoSave();
                } else if (choiceid == 1)
                    LogueBookModels.AddMoney(25);
            }
            base.OnClickChoice(choiceid);
        }
    }
}
