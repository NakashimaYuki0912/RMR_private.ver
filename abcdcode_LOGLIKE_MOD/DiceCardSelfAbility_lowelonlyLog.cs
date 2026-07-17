// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_lowelonlyLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_lowelonlyLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_lowelonlyLog</summary>

public class DiceCardSelfAbility_lowelonlyLog : DiceCardSelfAbilityBase
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
    this.owner.allyCardDetail.AddNewCard(Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 601014)).id).AddBuf((BattleDiceCardBuf) new DiceCardSelfAbility_lowelonly.BattleDiceCardBuf_lowel());
    this.card.card.exhaust = true;
  }
}
}
