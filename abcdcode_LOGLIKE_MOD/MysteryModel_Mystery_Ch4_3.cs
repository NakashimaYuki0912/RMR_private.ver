// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch4_3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch4_3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using TMPro;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Mystery node model: MysteryModel_Mystery_Ch4_3</summary>

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
