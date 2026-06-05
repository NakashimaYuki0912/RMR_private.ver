// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch4_3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using TMPro;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryModel_Mystery_Ch4_3 : MysteryBase
    {
        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (this.Condition() || id != 0)
                return;
            this.FrameObj["choice_btn1"].GetComponent<Image>().sprite = LogLikeMod.ArtWorks["disabledButton"];
            this.FrameObj["Desc1"].GetComponent<TextMeshProUGUI>().text = TextDataModel.GetText("Mystery_RequireCondition") + this.FrameObj["Desc1"].GetComponent<TextMeshProUGUI>().text;
        }

        public bool Condition()
        {
            return Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find(x => x is PickUpModel_ShopGood11.Hiding) != null;
        }

        public override void OnClickChoice(int choiceid)
        {
            if (choiceid == 0)
                MysteryBase.SetNextStageCustom(new LorId(LogLikeMod.ModId, 1400031));
            if (choiceid == 1)
            {
                if (!this.Condition())
                    return;
                (Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find(x => x is PickUpModel_ShopGood11.Hiding) as OnceEffect).Use();
            }
            base.OnClickChoice(choiceid);
        }
    }
}
