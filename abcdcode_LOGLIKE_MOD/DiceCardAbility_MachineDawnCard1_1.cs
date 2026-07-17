// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_MachineDawnCard1_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_MachineDawnCard1_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_MachineDawnCard1_1</summary>

public class DiceCardAbility_MachineDawnCard1_1 : DiceCardAbilityBase
{
  public override void OnWinParrying()
  {
    this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Vulnerable, 1, this.owner);
  }
}
}
