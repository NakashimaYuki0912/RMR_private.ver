// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardAbility_downgradeNext2pwLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardAbility_downgradeNext2pwLog : DiceCardAbilityBase
{
  public override void OnWinParrying()
  {
    BattleUnitModel target = this.card.target;
    if (target == null)
      return;
    BattlePlayingCardDataInUnitModel currentDiceAction = target.currentDiceAction;
    if (currentDiceAction == null)
      return;
    currentDiceAction.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus()
    {
      max = -4
    });
  }
}
}
