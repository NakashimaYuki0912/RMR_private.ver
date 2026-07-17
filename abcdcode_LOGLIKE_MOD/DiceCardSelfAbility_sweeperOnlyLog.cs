// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_sweeperOnlyLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_sweeperOnlyLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Card self-ability: DiceCardSelfAbility_sweeperOnlyLog</summary>

    public class DiceCardSelfAbility_sweeperOnlyLog : DiceCardSelfAbilityBase
    {
        public override string[] Keywords
        {
            get => new string[1] { "onlypage_sweeper_Keyword" };
        }

        public override void OnUseCard()
        {
            this.card.ApplyDiceAbility(DiceMatch.AllDice, (DiceCardAbilityBase)new DiceCardAbility_recoverHp3atk());
        }
    }
}
