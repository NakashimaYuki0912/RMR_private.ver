// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveMook1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassiveMook1 : PassiveAbilityBase
{
  public override string debugDesc => "책장의 첫 공격 주사위 위력 + 2. 나머지 주사위 위력 - 1";

  public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
  {
    base.OnUseCard(curCard);
    curCard.ApplyDiceStatBonus(DiceMatch.NextAttackDice, new DiceStatBonus()
    {
      power = 2
    });
    curCard.ApplyDiceStatBonus(ExtensionUtils.NotFirstAttackDice(), new DiceStatBonus()
    {
      power = -1
    });
  }
}
}
