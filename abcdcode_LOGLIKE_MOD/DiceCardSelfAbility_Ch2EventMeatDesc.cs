// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_Ch2EventMeatDesc
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_Ch2EventMeatDesc.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Card self-ability: DiceCardSelfAbility_Ch2EventMeatDesc</summary>

    public class DiceCardSelfAbility_Ch2EventMeatDesc : LogDiceCardSelfAbility
    {
        public override bool CanUpgrade() => false;

        public override bool CanAddDeck(DeckModel self, out CardEquipState state)
        {
            state = CardEquipState.Equippable;
            return false;
        }
    }
}
