// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch4_2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch4_2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Mystery node model: MysteryModel_Mystery_Ch4_2</summary>

    public class MysteryModel_Mystery_Ch4_2 : MysteryBase
    {
        int money = 0;

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0 && choiceid == 0)
            {
                money = Random.Range(30, 51);
                LogueBookModels.AddMoney(money);
            }
            if (this.curFrame.FrameID == 1)
            {
                MysteryBase.SetNextStageCustom(new LorId(LogLikeMod.ModId, 40012), StageType.Normal); 
            }
            base.OnClickChoice(choiceid);
        }

        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (this.curFrame.FrameID == 1)
            {
                this.ReformatButton(0, money);
            }
        }
    }
}
