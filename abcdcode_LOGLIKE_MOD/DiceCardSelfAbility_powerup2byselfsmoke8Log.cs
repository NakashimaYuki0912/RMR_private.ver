// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_powerup2byselfsmoke8Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_powerup2byselfsmoke8Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_powerup2byselfsmoke8Log</summary>

public class DiceCardSelfAbility_powerup2byselfsmoke8Log : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Smoke_Keyword" };
  }

  public override void OnUseCard()
  {
    BattleUnitBuf activatedBuf = this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.Smoke);
    if (activatedBuf == null || activatedBuf.stack < 8)
      return;
    BattlePlayingCardDataInUnitModel currentDiceAction = this.owner.currentDiceAction;
    if (currentDiceAction == null)
      return;
    currentDiceAction.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
    {
      power = 2
    });
  }
}
}
