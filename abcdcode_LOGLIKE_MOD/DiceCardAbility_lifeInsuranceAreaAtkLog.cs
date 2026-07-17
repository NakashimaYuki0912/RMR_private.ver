// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_lifeInsuranceAreaAtkLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_lifeInsuranceAreaAtkLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_lifeInsuranceAreaAtkLog</summary>

public class DiceCardAbility_lifeInsuranceAreaAtkLog : DiceCardAbilityBase
{
  public override void OnSucceedAttack(BattleUnitModel target)
  {
    if (target == null)
      return;
    target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Disarm, 2, this.owner);
    target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Binding, 2, this.owner);
    target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Weak, 2, this.owner);
    this.owner.RecoverHP(4);
  }
}
}
