// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_Common3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_Common3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_Common3</summary>

    public class PickUpModel_Common3 : PickUpModelBase
    {
        public override bool IsCanPickUp(UnitDataModel target)
        {
            int num = 0;
            for (int index = 0; index < LogueBookModels.playersPick[target].Count; ++index)
            {
                if (LogueBookModels.playersPick[target][index] == 15210003)
                    ++num;
                if (num == 3)
                    return false;
            }
            return true;
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            ++model.UnitData.unitData.bookItem.equipeffect.SpeedMin;
            ++model.UnitData.unitData.bookItem.equipeffect.Speed;
        }
    }
}
