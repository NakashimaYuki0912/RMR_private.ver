// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_Elite1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_Elite1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_Elite1</summary>

    public class PickUpModel_Elite1 : PickUpModelBase
    {
        public override void OnPickUp(BattleUnitModel model)
        {
            ++model.UnitData.unitData.bookItem.equipeffect.StartPlayPoint;
            ++model.UnitData.unitData.bookItem.equipeffect.MaxPlayPoint;
        }
    }
}
