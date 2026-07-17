// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_Ch2BossMars
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_Ch2BossMars.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_Ch2BossMars : PickUpModelBase
    {
        public override void LoadFromSaveData(LogueStageInfo stage)
        {
            stage.type = StageType.Boss;
            stage.stageid = 20005;
        }

        public PickUpModel_Ch2BossMars()
        {
            this.Name = TextDataModel.GetText("Stage_BossMars");
            this.Desc = TextDataModel.GetText("Stage_BossMars_Desc");
            this.FlaverText = "";
            this.ArtWork = "Stage_ch2_BossMars";
        }
    }
}
