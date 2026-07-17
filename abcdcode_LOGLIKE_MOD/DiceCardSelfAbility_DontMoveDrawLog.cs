// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_DontMoveDrawLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_DontMoveDrawLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_DontMoveDrawLog</summary>

public class DiceCardSelfAbility_DontMoveDrawLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "DrawCard_Keyword" };
  }

  public override void OnUseCard()
  {
    if (this.owner.cardHistory.GetCurrentRoundCardList(Singleton<StageController>.Instance.RoundTurn).Count > 0)
      return;
    this.owner.cardSlotDetail.RecoverPlayPoint(1);
    this.owner.allyCardDetail.DrawCards(1);
  }
}
}
