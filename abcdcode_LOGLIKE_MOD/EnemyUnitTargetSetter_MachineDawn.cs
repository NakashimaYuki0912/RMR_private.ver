// -----------------------------------------------------------------------------
// Library of Ruina mod script: EnemyUnitTargetSetter_MachineDawn
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\EnemyUnitTargetSetter_MachineDawn.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>EnemyUnitTargetSetter_MachineDawn</summary>

public class EnemyUnitTargetSetter_MachineDawn : EnemyUnitTargetSetter
{
  public override BattleUnitModel SelectTargetUnit(List<BattleUnitModel> candidates)
  {
    List<BattleUnitModel> battleUnitModelList = new List<BattleUnitModel>();
    foreach (BattleUnitModel candidate in candidates)
    {
      if (candidate.bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable) != null)
        battleUnitModelList.Add(candidate);
    }
    return battleUnitModelList.Count <= 0 ? base.SelectTargetUnit(candidates) : battleUnitModelList[Random.Range(0, battleUnitModelList.Count)];
  }
}
}
