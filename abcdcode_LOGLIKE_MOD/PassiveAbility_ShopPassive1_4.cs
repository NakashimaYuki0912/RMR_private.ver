// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassive1_4
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassive1_4 : PassiveAbilityBase
{
  public override string debugDesc => "뜬소문 등급 혹은 현재 챕터보다 이전 등급의 책장을 사용하면 이번막에 모든 주사위 최대값 + 2";

  public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
  {
    if (behavior.Detail != BehaviourDetail.Evasion)
      return;
    int diceMin = behavior.GetDiceMin();
    int diceMax = behavior.GetDiceMax();
    int num1 = diceResult;
    for (int index = 0; index < 2; ++index)
    {
      int num2 = DiceStatCalculator.MakeDiceResult(diceMin, diceMax, 0);
      if (num2 > num1)
        num1 = num2;
    }
    if (num1 <= diceResult)
      return;
    diceResult = num1;
  }
}
}
