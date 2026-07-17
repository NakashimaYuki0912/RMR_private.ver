// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_Ch6RedMistChallenge
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_Ch6RedMistChallenge.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_Ch6RedMistChallenge : PickUpModelBase
    {
        public PickUpModel_Ch6RedMistChallenge()
        {
            Name = TextDataModel.GetText("Stage_RedMistChallenge");
            Desc = TextDataModel.GetText("Stage_RedMistChallenge_Desc");
            FlaverText = "";
            ArtWork = "Stage_ch6_RedMistChallenge";
        }

        public override void LoadFromSaveData(LogueStageInfo stage)
        {
            stage.type = StageType.Elite;
            stage.stageid = 60020;
        }
    }
}
