// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_yanOnly1DiceLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_yanOnly1DiceLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_yanOnly1DiceLog</summary>

public class DiceCardAbility_yanOnly1DiceLog : DiceCardAbilityBase
{
  public override void OnSucceedAttack(BattleUnitModel target)
  {
    if (target == null)
      return;
    target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Decay, 3, this.owner);
    if (target.bufListDetail.GetActivatedBuf(KeywordBuf.Decay) is BattleUnitBuf_Decay activatedBuf)
      activatedBuf.ChangeToYanDecay();
  }
}
}
