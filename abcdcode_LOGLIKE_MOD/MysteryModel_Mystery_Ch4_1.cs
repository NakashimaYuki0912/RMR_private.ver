// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch4_1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using TMPro;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

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
