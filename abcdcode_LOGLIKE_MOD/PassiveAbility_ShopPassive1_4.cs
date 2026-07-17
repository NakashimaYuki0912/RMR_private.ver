// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassive1_4
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassive1_4.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassive1_4</summary>

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
