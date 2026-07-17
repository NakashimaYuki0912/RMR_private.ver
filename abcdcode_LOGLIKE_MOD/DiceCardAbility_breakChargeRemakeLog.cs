// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_breakChargeRemakeLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_breakChargeRemakeLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_breakChargeRemakeLog</summary>

public class DiceCardAbility_breakChargeRemakeLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "WarpCharge" };
  }

  public override void OnSucceedAttack()
  {
    for (int index = 0; index < 4; ++index)
    {
      if (this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.WarpCharge) is BattleUnitBuf_warpCharge activatedBuf && activatedBuf.stack >= 2)
      {
        activatedBuf.UseStack(2, true);
        if (this.card?.target != null)
          this.card.target.TakeBreakDamage(6, DamageType.Card_Ability, this.owner);
      }
    }
  }
}
}
