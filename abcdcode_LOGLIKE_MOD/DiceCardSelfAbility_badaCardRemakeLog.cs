// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_badaCardRemakeLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_badaCardRemakeLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_badaCardRemakeLog</summary>

public class DiceCardSelfAbility_badaCardRemakeLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "WarpCharge" };
  }

  public override void OnUseCard()
  {
    if (!(this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.WarpCharge) is BattleUnitBuf_warpCharge activatedBuf) || activatedBuf.stack < 4)
      return;
    activatedBuf.UseStack(4, true);
    this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
    {
      power = 3
    });
  }
}
}
