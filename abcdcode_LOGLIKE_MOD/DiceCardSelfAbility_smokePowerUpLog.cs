// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_smokePowerUpLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_smokePowerUpLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_smokePowerUpLog</summary>

public class DiceCardSelfAbility_smokePowerUpLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Smoke_Keyword" };
  }

  public override void OnUseCard()
  {
    if (!(this.card.target?.bufListDetail.GetActivatedBuf(KeywordBuf.Smoke) is BattleUnitBuf_smoke activatedBuf) || activatedBuf.stack < 2)
      return;
    this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
    {
      power = 1
    });
  }
}
}
