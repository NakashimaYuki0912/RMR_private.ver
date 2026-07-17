// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_dmgUp3AllLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_dmgUp3AllLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_dmgUp3AllLog</summary>

public class DiceCardSelfAbility_dmgUp3AllLog : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    List<BattleUnitModel> battleUnitModelList = new List<BattleUnitModel>();
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction))
      alive.bufListDetail.AddKeywordBufByCard(KeywordBuf.DmgUp, 3, this.owner);
  }
}
}
