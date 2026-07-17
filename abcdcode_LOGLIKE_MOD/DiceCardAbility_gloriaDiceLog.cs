// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_gloriaDiceLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_gloriaDiceLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_gloriaDiceLog</summary>

public class DiceCardAbility_gloriaDiceLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[2]
      {
        "AreaDiceAll_Keyword",
        "Weak_Keyword"
      };
    }
  }

  public override void OnSucceedAttack(BattleUnitModel target)
  {
    target?.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Weak, 2, this.owner);
  }
}
}
