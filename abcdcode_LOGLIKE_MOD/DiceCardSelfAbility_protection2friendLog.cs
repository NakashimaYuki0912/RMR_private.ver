// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_protection2friendLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_protection2friendLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_protection2friendLog</summary>

public class DiceCardSelfAbility_protection2friendLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Protection_Keyword" };
  }

  public override void OnUseCard()
  {
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.card.owner.faction))
      alive.bufListDetail.AddKeywordBufByCard(KeywordBuf.Protection, 2, this.owner);
  }
}
}
