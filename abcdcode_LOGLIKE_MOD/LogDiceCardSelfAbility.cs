// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogDiceCardSelfAbility
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


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
