// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_MachineOrdeal2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_MachineOrdeal2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_MachineOrdeal2</summary>

public class PassiveAbility_MachineOrdeal2 : PassiveAbilityBase
{
  public override void OnEndBattle(BattlePlayingCardDataInUnitModel curCard)
  {
    base.OnEndBattle(curCard);
    if (curCard.target == null || !curCard.target.IsDead())
      return;
    this.owner.battleCardResultLog.CurbehaviourResult.behaviourRawData = this.owner.battleCardResultLog.CurbehaviourResult.behaviourRawData.Copy();
    this.owner.battleCardResultLog.CurbehaviourResult.behaviourRawData.ActionScript = "MachineDawnCard";
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction == Faction.Enemy ? Faction.Player : Faction.Enemy))
    {
      int damage = Random.Range(4, 9);
      alive.TakeBreakDamage(damage, DamageType.Passive);
      alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1);
    }
  }
}
}
