// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_MeatPieQuestComplete
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_MeatPieQuestComplete.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_MeatPieQuestComplete : PickUpModelBase
    {
        public PickUpModel_MeatPieQuestComplete()
        {
            this.Name = TextDataModel.GetText("Stage_MeatPieQuest");
            this.Desc = TextDataModel.GetText("Stage_MeatPieQuest_Desc");
            this.FlaverText = "";
            this.ArtWork = "Stage_ch2_MeatPieQuestComplete";
        }
    }
}
