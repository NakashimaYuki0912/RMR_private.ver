// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveLogic1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveLogic1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;
using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveLogic1</summary>

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
