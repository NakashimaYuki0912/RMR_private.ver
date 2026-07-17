// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_awlofnight1Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_awlofnight1Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_awlofnight1Log</summary>

public class DiceCardSelfAbility_awlofnight1Log : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    base.OnUseCard();
    int num = 0;
    BattleUnitBuf activatedBuf1 = this.owner?.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness);
    if (activatedBuf1 != null)
      num += activatedBuf1.stack;
    BattleUnitBuf activatedBuf2 = this.card?.target?.bufListDetail.GetActivatedBuf(KeywordBuf.Binding);
    if (activatedBuf2 != null)
      num += activatedBuf2.stack;
    BattlePlayingCardDataInUnitModel card = this.card;
    if (card == null)
      return;
    card.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus()
    {
      power = num
    });
  }
}
}
