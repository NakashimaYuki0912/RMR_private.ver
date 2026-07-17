// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch2_1_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch2_1_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Mystery node model: MysteryModel_Mystery_Ch2_1_1</summary>

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
