// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_CircusDawnCard1_0
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_CircusDawnCard1_0 : DiceCardSelfAbilityBase
{
  public int count;

  public override void OnUseCard()
  {
    base.OnUseCard();
    this.count = 0;
  }

  public override void OnLoseParrying()
  {
    base.OnLoseParrying();
    ++this.count;
  }

  public override void OnEndBattle()
  {
    base.OnEndBattle();
    if (this.count >= 3 || this.card.target == null || this.card.target.IsDead())
      return;
    CircusDawn1Buf.GiveBuf(this.owner, this.card.target);
  }
}
}
