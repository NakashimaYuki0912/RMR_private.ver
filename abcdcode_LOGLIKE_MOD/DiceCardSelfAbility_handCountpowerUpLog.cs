// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_handCountpowerUpLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_handCountpowerUpLog : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    int count = this.owner.allyCardDetail.GetHand().Count;
    if (count <= 0)
      return;
    this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
    {
      power = count
    });
    this.owner.allyCardDetail.DiscardACardByAbility(this.owner.allyCardDetail.GetHand());
  }
}
}
