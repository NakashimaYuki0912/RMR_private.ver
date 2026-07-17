// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_deerDiceLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_deerDiceLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_deerDiceLog</summary>

public class DiceCardAbility_deerDiceLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "WarpCharge" };
  }

  public override void OnSucceedAttack(BattleUnitModel target)
  {
    if (target == null || !(this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.WarpCharge) is BattleUnitBuf_warpCharge activatedBuf) || activatedBuf.stack < 3)
      return;
    activatedBuf.UseStack(3, true);
    target.TakeBreakDamage(10, DamageType.Card_Ability, this.owner);
  }
}
}
