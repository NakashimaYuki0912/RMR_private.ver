// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_lowelonlyLogEdit
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_lowelonlyLogEdit.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Card self-ability: DiceCardSelfAbility_lowelonlyLogEdit</summary>

    public class DiceCardSelfAbility_lowelonlyLogEdit : DiceCardSelfAbilityBase
    {
        public override string[] Keywords
        {
            get
            {
                return new string[2]
                {
        "AreaCard_Keyword",
        "onlypage_lowel_Keyword"
                };
            }
        }

        public override void OnUseCard()
        {
            this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 601014)).AddBuf(new DiceCardSelfAbility_lowelonly.BattleDiceCardBuf_lowel());
            this.card.card.exhaust = true;
            this.card.RemoveAllDice();
        }
    }
}
