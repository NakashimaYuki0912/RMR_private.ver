// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardAbility_powerUp2targetParalysisLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardAbility_powerUp2targetParalysisLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Paralysis_Keyword" };
  }

  public override void BeforeRollDice()
  {
    BattleUnitModel target = this.card.target;
    if (target == null || target.bufListDetail.GetKewordBufStack(KeywordBuf.Paralysis) <= 0)
      return;
    this.behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      power = 2
    });
  }
}
}
