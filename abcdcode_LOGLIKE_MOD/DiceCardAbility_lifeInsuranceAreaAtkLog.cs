// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardAbility_lifeInsuranceAreaAtkLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardAbility_lifeInsuranceAreaAtkLog : DiceCardAbilityBase
{
  public override void OnSucceedAttack(BattleUnitModel target)
  {
    if (target == null)
      return;
    target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Disarm, 2, this.owner);
    target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Binding, 2, this.owner);
    target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Weak, 2, this.owner);
    this.owner.RecoverHP(4);
  }
}
}
