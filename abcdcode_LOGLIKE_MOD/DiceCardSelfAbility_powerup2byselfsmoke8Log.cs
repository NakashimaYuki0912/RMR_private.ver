// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_powerup2byselfsmoke8Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_powerup2byselfsmoke8Log : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Smoke_Keyword" };
  }

  public override void OnUseCard()
  {
    BattleUnitBuf activatedBuf = this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.Smoke);
    if (activatedBuf == null || activatedBuf.stack < 8)
      return;
    BattlePlayingCardDataInUnitModel currentDiceAction = this.owner.currentDiceAction;
    if (currentDiceAction == null)
      return;
    currentDiceAction.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
    {
      power = 2
    });
  }
}
}
