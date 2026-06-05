// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassive19
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassive19 : PassiveAbilityBase
{
  public override string debugDesc => "막 시작 시 무작위 내성 하나가 '면역' 상태가 됨";

  public override void OnRoundStart()
  {
    BehaviourDetail behaviourDetail = (BehaviourDetail) Random.Range(0, 3);
    bool flag = (double) Random.value > 0.5;
    this.owner.bufListDetail.AddBuf((BattleUnitBuf) new PassiveAbility_ShopPassive19.BattleUnitBuf_Immune()
    {
      Target = behaviourDetail,
      IsHp = flag
    });
  }

  public class BattleUnitBuf_Immune : BattleUnitBuf
  {
    public bool IsHp;
    public BehaviourDetail Target = BehaviourDetail.None;

    public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
    {
      return this.IsHp && this.Target == detail ? AtkResist.Immune : base.GetResistHP(origin, detail);
    }

    public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
    {
      return !this.IsHp && this.Target == detail ? AtkResist.Immune : base.GetResistHP(origin, detail);
    }

    public override void OnRoundEnd() => this.Destroy();
  }
}
}
