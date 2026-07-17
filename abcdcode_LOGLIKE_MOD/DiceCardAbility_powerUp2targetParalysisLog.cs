// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_powerUp2targetParalysisLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_powerUp2targetParalysisLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_powerUp2targetParalysisLog</summary>

public class DiceCardAbility_powerUp2targetParalysisLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Paralysis_Keyword" };
  }

  public override void BeforeRollDice()
  {
    BattleUnitModel target = this.card.target;
    if (target == null || target.bufListDetail.GetKewordBufStack(KeywordBuf.Paralysis) <= 0)
      return;
    this.behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      power = 2
    });
  }
}
}
