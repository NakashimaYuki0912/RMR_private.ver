// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_indexRelease2LogEdit
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_indexRelease2LogEdit : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "IndexReleaseCard3_Keyword" };
  }

  public override void OnUseCard()
  {
    this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 605010));
    this.card.card.exhaust = true;
  }
}
}
