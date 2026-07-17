// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_burn8atkLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_burn8atkLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_burn8atkLog</summary>

public class DiceCardAbility_burn8atkLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Burn_Keyword" };
  }

  public override void OnSucceedAttack()
  {
    this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 8, this.owner);
  }
}
}
