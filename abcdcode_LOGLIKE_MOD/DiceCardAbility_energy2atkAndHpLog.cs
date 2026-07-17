// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_energy2atkAndHpLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_energy2atkAndHpLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_energy2atkAndHpLog</summary>

public class DiceCardAbility_energy2atkAndHpLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Energy_Keyword" };
  }

  public override void OnSucceedAttack()
  {
    if ((double) this.owner.hp > (double) (this.owner.MaxHp / 4))
      return;
    this.owner.cardSlotDetail.RecoverPlayPointByCard(2);
  }
}
}
