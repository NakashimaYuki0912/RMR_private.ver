// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_MachineOrdeal2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

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
