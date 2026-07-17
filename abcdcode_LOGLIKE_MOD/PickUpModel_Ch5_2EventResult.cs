// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_Ch5_2EventResult
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_Ch5_2EventResult.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_Ch5_2EventResult : PickUpModelBase
    {
        public override void LoadFromSaveData(LogueStageInfo stage) => stage.type = StageType.Mystery;

        public PickUpModel_Ch5_2EventResult()
        {
            this.Name = TextDataModel.GetText("Stage_Mystery");
            this.Desc = TextDataModel.GetText("Stage_Mystery_Desc");
            this.FlaverText = "";
            this.ArtWork = "Stage_Mystery";
        }
    }
}
