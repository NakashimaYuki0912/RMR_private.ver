// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_MachineDawnCard2_3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_MachineDawnCard2_3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_MachineDawnCard2_3</summary>

public class DiceCardAbility_MachineDawnCard2_3 : DiceCardAbilityBase
{
  public int _repeatCount;

  public override void AfterAction()
  {
    BattleUnitBuf activatedBuf = this.card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable);
    if (activatedBuf == null)
      return;
    int num = activatedBuf.stack > 3 ? 3 : activatedBuf.stack;
    if (!this.owner.IsBreakLifeZero() && this._repeatCount < num)
    {
      ++this._repeatCount;
      this.ActivateBonusAttackDice();
    }
  }
}
}
