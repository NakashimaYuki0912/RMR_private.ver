// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_Elite2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_Elite2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_Elite2</summary>

    public class PickUpModel_Elite2 : PickUpModelBase
    {
        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            this.GivePassive(new LorId(LogLikeMod.ModId, 15220002), model);
        }
    }
}
