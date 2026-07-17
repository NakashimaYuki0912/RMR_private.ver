// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_indexRelease2Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_indexRelease2Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_indexRelease2Log</summary>

public class DiceCardSelfAbility_indexRelease2Log : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "IndexReleaseCard3_Keyword" };
  }

  public override void OnUseCard()
  {
    this.owner.allyCardDetail.AddNewCard(Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 605010)).id);
    this.card.card.exhaust = true;
  }
}
}
