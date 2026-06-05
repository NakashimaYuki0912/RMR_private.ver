// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveLogic7
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;
using LOR_DiceSystem;
using RogueLike_Mod_Reborn;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_ShopPassiveLogic7 : PassiveAbilityBase
    {
        public override string debugDesc
        {
            get => "원거리 책장으로 적 처치 시 책장에 남은 공격 주사위가 있다면 무작위 적에게 남은 주사위를 일방공격으로 재사용";
        }

        public override void OnKill(BattleUnitModel target)
        {
            base.OnKill(target);
            if (this.owner.currentDiceAction == null || this.owner.currentDiceAction.card.GetSpec().Ranged != CardRange.Far || this.owner.currentDiceAction.cardBehaviorQueue.Count <= 0 || BattleObjectManager.instance.GetAliveList_opponent(owner.faction).Count < 1)
                return;
            BattlePlayingCardDataInUnitModel currentDiceAction = this.owner.currentDiceAction;
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(currentDiceAction.card.GetID());
            BattlePlayingCardDataInUnitModel cardDataInUnitModel = new BattlePlayingCardDataInUnitModel();
            cardDataInUnitModel.owner = this.owner;
            BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(cardItem);
            playingCard.costSpended = true;
            cardDataInUnitModel.card = playingCard;
            cardDataInUnitModel.target = BattleObjectManager.instance.GetAliveList_opponent(owner.faction).SelectOneRandom();
            cardDataInUnitModel.speedDiceResultValue = 999;
            cardDataInUnitModel.cardBehaviorQueue.Clear();
            foreach (BattleDiceBehavior cardBehavior in this.owner.currentDiceAction.cardBehaviorQueue)
            {
                cardBehavior.card = cardDataInUnitModel;
                cardDataInUnitModel.cardBehaviorQueue.Enqueue(cardBehavior);
            }
          ((List<BattlePlayingCardDataInUnitModel>)typeof(StageController).GetField("_allCardList", AccessTools.all).GetValue(Singleton<StageController>.Instance)).Add(cardDataInUnitModel);
        }
    }
}
