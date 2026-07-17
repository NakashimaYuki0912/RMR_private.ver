// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_liuBurnDiceLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_liuBurnDiceLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_liuBurnDiceLog</summary>

public class DiceCardAbility_liuBurnDiceLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Burn_Keyword" };
  }

  public override void OnSucceedAttack()
  {
    this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 2 + this.owner.emotionDetail.EmotionLevel, this.owner);
  }
}
}
