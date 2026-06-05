// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassive3_3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassive3_3 : PassiveAbilityBase
{
  public bool Can;

  public override string debugDesc => "매 전투마다 처음으로 버려지는 책장을 획득";

  public override void OnWaveStart()
  {
    base.OnWaveStart();
    this.Can = true;
  }

  public override void OnDiscardByAbility(List<BattleDiceCardModel> cards)
  {
    base.OnDiscardByAbility(cards);
    if (!this.Can)
      return;
    LogueBookModels.AddCard(cards[0].GetID());
    this.Can = false;
  }
}
}
