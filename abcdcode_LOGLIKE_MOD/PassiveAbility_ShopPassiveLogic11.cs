// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveLogic11
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

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
