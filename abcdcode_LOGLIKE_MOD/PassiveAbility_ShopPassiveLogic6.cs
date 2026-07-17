// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveLogic6
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveLogic6.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;
using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveLogic6</summary>

public class PassiveAbility_ShopPassiveLogic6 : PassiveAbilityBase
{
  public override string debugDesc => "무대 시작 시 덱의 가장 첫번째 원거리 책장을 복사해 손으로 가져옴";

  public override void OnWaveStart()
  {
    base.OnWaveStart();
    List<DiceCardXmlInfo> all = this.owner.UnitData.unitData.bookItem.GetCardListFromCurrentDeck().FindAll((Predicate<DiceCardXmlInfo>) (x => x.Spec.Ranged == CardRange.Far));
    if (all.Count > 0)
      all.Sort(new Comparison<DiceCardXmlInfo>(SortUtil.CardInfoCompByCost));
    this.owner.allyCardDetail.AddNewCard(all[0].id, true);
  }
}
}
