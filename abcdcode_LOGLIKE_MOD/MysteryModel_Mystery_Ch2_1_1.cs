// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch2_1_1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryModel_Mystery_Ch2_1_1 : MysteryBase
    {
        int money = Singleton<System.Random>.Instance.Next(20, 31);
        public override void OnEnterChoice(int choiceid)
        {
            if (choiceid != 1)
                return;
            this.ShowDetailCard(choiceid, new LorId(LogLikeMod.ModId, 2000002));
        }

        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            this.ReformatButton(0, money);
        }

        public override void OnExitChoice(int choiceid) => this.HideDetailCard();

        public override void OnClickChoice(int choiceid)
        {
            if (choiceid == 0)
            {
                LogueBookModels.AddMoney(money);
            }
            if (choiceid == 1)
                LogueBookModels.AddCard(new LorId(LogLikeMod.ModId, 2000002), 5);
            LogueBookModels.DeleteCard(new LorId(LogLikeMod.ModId, 2000001), 5);
            base.OnClickChoice(choiceid);
        }
    }
}
