// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_energyAndChargeLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_energyAndChargeLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_energyAndChargeLog</summary>

public class DiceCardSelfAbility_energyAndChargeLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[2]{ "WarpCharge", "Energy_Keyword" };
  }

  public override void OnUseCard()
  {
    this.owner.cardSlotDetail.RecoverPlayPointByCard(1);
    this.owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.WarpCharge, 4, this.owner);
  }
}
}
