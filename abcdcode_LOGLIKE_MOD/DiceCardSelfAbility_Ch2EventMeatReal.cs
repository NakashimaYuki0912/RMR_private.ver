// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_Ch2EventMeatReal
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_Ch2EventMeatReal.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Card self-ability: DiceCardSelfAbility_Ch2EventMeatReal</summary>

    public class DiceCardSelfAbility_Ch2EventMeatReal : LogDiceCardSelfAbility
    {
        public override void OnUseInstance(
          BattleUnitModel unit,
          BattleDiceCardModel self,
          BattleUnitModel targetUnit)
        {
            unit.RecoverHP(20);
            BookModel bookItem = unit.UnitData.unitData.bookItem;
            this.DeleteCard(unit, new LorId(LogLikeMod.ModId, 2000002));
            if (!bookItem.GetDeckAll_nocopy()[bookItem.GetCurrentDeckIndex()].MoveCardToInventory(new LorId(LogLikeMod.ModId, 2000002)))
                return;
            LogueBookModels.DeleteCard(new LorId(LogLikeMod.ModId, 2000002));
        }
    }
}
