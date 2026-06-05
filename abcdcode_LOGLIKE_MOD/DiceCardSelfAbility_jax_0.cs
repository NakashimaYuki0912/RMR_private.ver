// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_jax_0
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_jax_0 : DiceCardSelfAbilityBase
{
  public static string Desc = "[장착 시 발동] 최대 체력의 10% 만큼 피해를 입고 이번막에 힘을 2 얻음";

  public override void OnUseInstance(
    BattleUnitModel unit,
    BattleDiceCardModel self,
    BattleUnitModel targetUnit)
  {
    unit.TakeDamage(unit.MaxHp / 10 > 0 ? unit.MaxHp / 10 : 1);
    unit.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 2);
  }
}
}
