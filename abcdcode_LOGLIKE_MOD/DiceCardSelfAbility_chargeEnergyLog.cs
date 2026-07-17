// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_chargeEnergyLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_chargeEnergyLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_chargeEnergyLog</summary>

public class DiceCardSelfAbility_chargeEnergyLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[2]{ "WarpCharge", "Energy_Keyword" };
  }

  public override void OnUseCard()
  {
    if (!(this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.WarpCharge) is BattleUnitBuf_warpCharge activatedBuf) || activatedBuf.stack < 3)
      return;
    activatedBuf.UseStack(3, true);
    this.owner.cardSlotDetail.RecoverPlayPointByCard(3);
  }
}
}
