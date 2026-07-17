// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_yanAreaDiceLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_yanAreaDiceLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_yanAreaDiceLog</summary>

public class DiceCardAbility_yanAreaDiceLog : DiceCardAbilityBase
{
  public override void OnSucceedAttack(BattleUnitModel target)
  {
    target?.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Decay, 3, this.owner);
    if (!(target.bufListDetail.GetActivatedBuf(KeywordBuf.Decay) is BattleUnitBuf_Decay activatedBuf))
      return;
    activatedBuf.ChangeToYanDecay();
  }
}
}
