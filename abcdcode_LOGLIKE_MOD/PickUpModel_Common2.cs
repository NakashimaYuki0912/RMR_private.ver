// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_Common2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_Common2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_Common2</summary>

    public class PickUpModel_Common2 : PickUpModelBase
    {
        public override bool IsCanPickUp(UnitDataModel target) => true;

        public override void OnPickUp(BattleUnitModel model)
        {
            model.UnitData.unitData.bookItem.equipeffect.Hp += 6;
            model.UnitData.unitData.bookItem.equipeffect.Break += 3;
        }
    }
}
