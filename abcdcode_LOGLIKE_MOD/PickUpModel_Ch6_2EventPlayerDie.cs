// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_Ch6_2EventPlayerDie
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_Ch6_2EventPlayerDie.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_Ch6_2EventPlayerDie</summary>

    public class PickUpModel_Ch6_2EventPlayerDie : PickUpModelBase
    {
        public PickUpModel_Ch6_2EventPlayerDie()
        {
            this.Name = TextDataModel.GetText("Ch6Event2Effect_Name");
            this.Desc = TextDataModel.GetText("Ch6Event2Effect_Desc");
            this.FlaverText = "";
            this.ArtWork = "Ch6Event2Effect";
        }

        public override bool IsCanPickUp(UnitDataModel target) => !target.IsDead();

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            model.Die();
        }
    }
}
