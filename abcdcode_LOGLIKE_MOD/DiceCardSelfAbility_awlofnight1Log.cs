// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_awlofnight1Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_awlofnight1Log : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    base.OnUseCard();
    int num = 0;
    BattleUnitBuf activatedBuf1 = this.owner?.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness);
    if (activatedBuf1 != null)
      num += activatedBuf1.stack;
    BattleUnitBuf activatedBuf2 = this.card?.target?.bufListDetail.GetActivatedBuf(KeywordBuf.Binding);
    if (activatedBuf2 != null)
      num += activatedBuf2.stack;
    BattlePlayingCardDataInUnitModel card = this.card;
    if (card == null)
      return;
    card.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus()
    {
      power = num
    });
  }
}
}
