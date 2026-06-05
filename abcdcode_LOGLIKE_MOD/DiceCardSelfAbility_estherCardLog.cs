// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_estherCardLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_estherCardLog : DiceCardSelfAbilityBase
{
  public override bool OnChooseCard(BattleUnitModel owner)
  {
    return owner.bufListDetail.GetActivatedBuf(KeywordBuf.IndexRelease) != null;
  }

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    if (behavior.TargetDice == null)
      return;
    behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus()
    {
      power = -15
    });
  }
}
}
