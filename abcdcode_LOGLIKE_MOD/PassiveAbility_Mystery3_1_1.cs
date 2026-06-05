// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_Mystery3_1_1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_Mystery3_1_1 : PassiveAbilityBase
{
  public override string debugDesc => "자신과 소속이 다른 모두를 공격 대상으로 삼음";

  public override void OnWaveStart()
  {
    base.OnWaveStart();
    this.owner.bufListDetail.AddBuf((BattleUnitBuf) new PassiveAbility_Mystery3_1_1.TargetingBuf1());
  }

  public class TargetingBuf1 : BattleUnitBuf
  {
    public override List<BattleUnitModel> GetFixedTarget()
    {
      List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList();
      aliveList.RemoveAll((Predicate<BattleUnitModel>) (x => x.passiveDetail.HasPassive<PassiveAbility_Mystery3_1_1>()));
      return aliveList;
    }
  }
}
}
