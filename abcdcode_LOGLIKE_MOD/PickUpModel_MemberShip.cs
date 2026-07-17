// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_MemberShip
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_MemberShip.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_MemberShip : PickUpModelBase
    {
        public PickUpModel_MemberShip()
        {
            this.Name = TextDataModel.GetText("Stage_MemberShip");
            this.Desc = TextDataModel.GetText("Stage_MemberShip_Desc");
            this.FlaverText = "";
            this.ArtWork = "Stage_MemberShip";
        }
    }
}
