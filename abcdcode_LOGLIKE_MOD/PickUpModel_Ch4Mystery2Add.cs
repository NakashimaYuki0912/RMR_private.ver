// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_Ch4Mystery2Add
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_Ch4Mystery2Add.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_Ch4Mystery2Add : PickUpModelBase
    {
        public override void LoadFromSaveData(LogueStageInfo stage) => stage.type = StageType.Normal;

        public PickUpModel_Ch4Mystery2Add()
        {
            this.Name = TextDataModel.GetText("Stage_Normal");
            this.Desc = TextDataModel.GetText("Stage_Normal_Desc");
            this.FlaverText = "";
            this.ArtWork = "Stage_Normal";
        }
    }
}
