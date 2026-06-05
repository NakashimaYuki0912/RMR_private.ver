// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_Chapter2Rat
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_Chapter2Rat : PassiveAbilityBase
{
  public override string debugDesc => "일방공격 시 피해량 + 1";

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    if (behavior.IsParrying())
      return;
    behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      dmg = 1
    });
  }
}
}
