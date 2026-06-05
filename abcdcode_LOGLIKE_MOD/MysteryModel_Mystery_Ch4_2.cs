// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch4_2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

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
