// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch1_6
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch1_6.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Mystery node model: MysteryModel_Mystery_Ch1_6</summary>

    public class MysteryModel_Mystery_Ch1_6 : MysteryBase
    {
        public override void OnClickChoice(int choiceid)
        {
            if (choiceid == 0)
                MysteryBase.SetNextStageCustom(new LorId(LogLikeMod.ModId, 1100061), StageType.Normal);
            base.OnClickChoice(choiceid);
        }
    }
}
