// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_burnAllLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_burnAllLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_burnAllLog</summary>

public class DiceCardAbility_burnAllLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Burn_Keyword" };
  }

  public override void OnSucceedAttack()
  {
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
      alive.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 3, this.owner);
  }
}
}
