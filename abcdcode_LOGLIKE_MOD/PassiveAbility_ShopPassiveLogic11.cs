// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveLogic11
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveLogic11.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveLogic11</summary>

public class PassiveAbility_ShopPassiveLogic11 : PassiveAbilityBase
{
  public bool give;

  public override string debugDesc => "원거리 책장을 사용하면 반격 주사위(방어,3~9)를 추가함.(막 당 1회)";

  public override void OnRoundStart()
  {
    base.OnRoundStart();
    this.give = false;
  }

  public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
  {
    base.OnUseCard(curCard);
    if (curCard.card.GetSpec().Ranged != CardRange.Far || this.give)
      return;
    BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(new LorId(LogLikeMod.ModId, 8580011)));
    if (playingCard != null)
    {
      foreach (BattleDiceBehavior diceCardBehavior in playingCard.CreateDiceCardBehaviorList())
        this.owner.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(playingCard, diceCardBehavior);
    }
    this.give = true;
  }
}
}
