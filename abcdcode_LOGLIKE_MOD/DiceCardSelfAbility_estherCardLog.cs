// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_estherCardLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_estherCardLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_estherCardLog</summary>

public class DiceCardSelfAbility_estherCardLog : DiceCardSelfAbilityBase
{
  public override bool OnChooseCard(BattleUnitModel owner)
  {
    return owner.bufListDetail.GetActivatedBuf(KeywordBuf.IndexRelease) != null;
  }

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    if (behavior.TargetDice == null)
      return;
    behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus()
    {
      power = -15
    });
  }
}
}
