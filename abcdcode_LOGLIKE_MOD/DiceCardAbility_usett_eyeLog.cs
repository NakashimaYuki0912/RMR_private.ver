// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_usett_eyeLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_usett_eyeLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_usett_eyeLog</summary>

public class DiceCardAbility_usett_eyeLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Protection_Keyword" };
  }

  public override void OnSucceedAttack()
  {
    base.OnSucceedAttack();
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction))
    {
      alive.bufListDetail.AddKeywordBufByCard(KeywordBuf.Protection, 2, this.owner);
      alive.bufListDetail.AddKeywordBufByCard(KeywordBuf.BreakProtection, 2, this.owner);
    }
  }
}
}
