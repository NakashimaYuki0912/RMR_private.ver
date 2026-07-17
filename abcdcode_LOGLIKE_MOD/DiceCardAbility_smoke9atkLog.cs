// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_smoke9atkLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_smoke9atkLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_smoke9atkLog</summary>

public class DiceCardAbility_smoke9atkLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Smoke_Keyword" };
  }

  public override void OnSucceedAttack()
  {
    this.card.target?.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Smoke, 9, this.owner);
  }
}
}
