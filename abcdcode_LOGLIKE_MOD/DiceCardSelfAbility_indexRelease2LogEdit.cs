// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_indexRelease2LogEdit
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_indexRelease2LogEdit.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_indexRelease2LogEdit</summary>

public class DiceCardSelfAbility_indexRelease2LogEdit : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "IndexReleaseCard3_Keyword" };
  }

  public override void OnUseCard()
  {
    this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 605010));
    this.card.card.exhaust = true;
  }
}
}
