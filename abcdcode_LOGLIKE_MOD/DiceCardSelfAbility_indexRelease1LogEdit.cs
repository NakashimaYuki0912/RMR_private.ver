// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_indexRelease1LogEdit
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_indexRelease1LogEdit.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_indexRelease1LogEdit</summary>

public class DiceCardSelfAbility_indexRelease1LogEdit : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[4]
      {
        "IndexReleaseCard2_Keyword",
        "IndexReleaseCard3_Keyword",
        "DrawCard_Keyword",
        "Energy_Keyword"
      };
    }
  }

  public override void OnUseCard()
  {
    this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 605009));
    this.card.card.exhaust = true;
  }
}
}
