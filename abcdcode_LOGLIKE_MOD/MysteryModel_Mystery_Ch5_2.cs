// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch5_2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch5_2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Mystery node model: MysteryModel_Mystery_Ch5_2</summary>

    public class MysteryModel_Mystery_Ch5_2 : MysteryBase
    {
        public override void SwapFrame(int id) => base.SwapFrame(id);

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 1)
            {
                LogueBookModels.AddMoney(40);
                LogueStageInfo stageInfo = Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 250002));
                stageInfo.type = StageType.Mystery;
                MysteryBase.AddStageList(stageInfo, ChapterGrade.Grade5);
            }
            base.OnClickChoice(choiceid);
        }
    }
}
