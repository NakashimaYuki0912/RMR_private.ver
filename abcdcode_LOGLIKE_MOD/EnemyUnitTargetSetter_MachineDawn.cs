// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.EnemyUnitTargetSetter_MachineDawn
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

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
