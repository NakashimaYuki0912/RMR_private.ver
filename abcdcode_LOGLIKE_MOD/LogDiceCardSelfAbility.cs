// -----------------------------------------------------------------------------
// Library of Ruina mod script: LogDiceCardSelfAbility
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\LogDiceCardSelfAbility.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{
    /// <summary>
    /// A special DiceCardSelfAbilityBase class for manipulating
    /// </summary>
    public class LogDiceCardSelfAbility : DiceCardSelfAbilityBase
    {

        public virtual bool CanUpgrade() => true;


        public virtual bool CanAddDeck(DeckModel self, out CardEquipState state)
        {
            state = CardEquipState.Equippable;
            return true;
        }


        public void DeleteCard(BattleUnitModel target, LorId id, int num = 1)
        {
            BookModel bookItem = target.UnitData.unitData.bookItem;
            for (int index = 0; index < num; ++index)
            {
                if (bookItem.GetDeckAll_nocopy()[bookItem.GetCurrentDeckIndex()].MoveCardToInventory(id))
                    LogueBookModels.DeleteCard(id);
            }
        }
    }
}
