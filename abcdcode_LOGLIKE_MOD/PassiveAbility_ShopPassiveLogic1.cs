// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveLogic1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassiveLogic1 : PassiveAbilityBase
{
  public override string debugDesc => "막 종료 시 손에 원거리 책장이 없다면 원거리 책장 2장을 뽑음";

  public override void OnRoundEnd()
  {
    base.OnRoundEnd();
    if (this.owner.allyCardDetail.GetHand().Find((Predicate<BattleDiceCardModel>) (x => x.XmlData.Spec.Ranged == CardRange.Far)) != null)
      return;
    List<BattleDiceCardModel> all = this.owner.allyCardDetail.GetDeck().FindAll((Predicate<BattleDiceCardModel>) (x => x.XmlData.Spec.Ranged == CardRange.Far));
    if (all.Count == 0)
      return;
    if (all.Count == 1)
    {
      this.owner.allyCardDetail.DrawCardsAllSpecific(all[0].GetID());
    }
    else
    {
      foreach (BattleDiceCardModel battleDiceCardModel in all.RandomPickUp<BattleDiceCardModel>(2))
        this.owner.allyCardDetail.DrawCardsAllSpecific(battleDiceCardModel.GetID());
    }
  }
}
}
