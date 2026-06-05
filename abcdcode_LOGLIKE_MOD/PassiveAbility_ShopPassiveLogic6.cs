// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveLogic6
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

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
