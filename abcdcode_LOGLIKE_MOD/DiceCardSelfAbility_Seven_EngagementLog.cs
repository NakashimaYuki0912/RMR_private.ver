// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_Seven_EngagementLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_Seven_EngagementLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_Seven_EngagementLog</summary>

public class DiceCardSelfAbility_Seven_EngagementLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Engard_Keyword" };
  }

  public override void OnUseCard()
  {
    base.OnUseCard();
    this.owner.allyCardDetail.AddNewCard(Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 511005)).id);
  }
}
}
