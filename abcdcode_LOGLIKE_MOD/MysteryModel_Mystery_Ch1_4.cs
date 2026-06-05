// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch1_4
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryModel_Mystery_Ch1_4 : MysteryBase
    {
        public static float[] values = new float[8]
        {
    0.0f,
    0.0f,
    0.0f,
    0.1f,
    0.15f,
    0.2f,
    0.25f,
    0.3f
        };

        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (id <= 2 || id >= 8 || PassiveAbility_MoneyCheck.GetMoney() >= 5)
                return;
            this.FrameObj["choice_btn1"].GetComponent<Image>().sprite = LogLikeMod.ArtWorks["disabledButton"];
            this.FrameObj["Desc1"].GetComponent<TextMeshProUGUI>().text = TextDataModel.GetText("Mystery_RequireCondition") + this.FrameObj["Desc1"].GetComponent<TextMeshProUGUI>().text;
        }

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 2)
                PassiveAbility_MoneyCheck.SubMoney(5);
            if (this.curFrame.FrameID > 2 && this.curFrame.FrameID < 8)
            {
                if (choiceid == 0 && (double)Random.value < (double)MysteryModel_Mystery_Ch1_4.values[this.curFrame.FrameID])
                {
                    this.SwapFrame(9);
                    return;
                }
                if (choiceid == 1 && PassiveAbility_MoneyCheck.GetMoney() < 5)
                    return;
            }
            if (this.curFrame.FrameID == 8)
                MysteryBase.SetNextStageCustom(new LorId(LogLikeMod.ModId, 1100041), StageType.Normal);
            if (this.curFrame.FrameID == 9)
                PassiveAbility_MoneyCheck.AddMoney(15);
            base.OnClickChoice(choiceid);
        }
    }
}
