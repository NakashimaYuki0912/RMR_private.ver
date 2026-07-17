// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_quickness2atkLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_quickness2atkLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_quickness2atkLog</summary>

public class DiceCardAbility_quickness2atkLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Quickness_Keyword" };
  }

  public override void OnSucceedAttack()
  {
    this.owner?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 2, this.owner);
  }
}
}
