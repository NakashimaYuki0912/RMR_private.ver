// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveLogic9
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveLogic9.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveLogic9</summary>

public class PassiveAbility_ShopPassiveLogic9 : PassiveAbilityBase
{
  public override string debugDesc => "원거리 공격 주사위에 행운 1 적용";

  public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
  {
    base.ChangeDiceResult(behavior, ref diceResult);
    int diceMin = behavior.GetDiceMin();
    int diceMax = behavior.GetDiceMax();
    int num1 = diceResult;
    for (int index = 0; index < 1; ++index)
    {
      int num2 = DiceStatCalculator.MakeDiceResult(diceMin, diceMax, 0);
      if (num2 > num1)
        num1 = num2;
    }
    diceResult = num1;
  }
}
}
