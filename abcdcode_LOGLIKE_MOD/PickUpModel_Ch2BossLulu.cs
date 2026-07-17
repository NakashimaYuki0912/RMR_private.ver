// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_Ch2BossLulu
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_Ch2BossLulu.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_Ch2BossLulu : PickUpModelBase
    {
        public override void LoadFromSaveData(LogueStageInfo stage)
        {
            stage.type = StageType.Boss;
            stage.stageid = 20006;
        }

        public PickUpModel_Ch2BossLulu()
        {
            this.Name = TextDataModel.GetText("Stage_BossLulu");
            this.Desc = TextDataModel.GetText("Stage_BossLulu_Desc");
            this.FlaverText = "";
            this.ArtWork = "Stage_ch2_BossLulu";
        }
    }
}
