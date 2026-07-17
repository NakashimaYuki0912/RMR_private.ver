// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassive3_3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassive3_3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassive3_3</summary>

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
