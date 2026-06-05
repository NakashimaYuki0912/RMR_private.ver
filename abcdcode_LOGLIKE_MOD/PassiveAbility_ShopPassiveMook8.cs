// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveMook8
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;
using LOR_DiceSystem;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_ShopPassiveMook8 : PassiveAbilityBase
    {
        public override string debugDesc => "책장의 첫 공격 주사위 합 승리 시 합 종료 후 해당 주사위를 일방공격으로 재사용(막당 1회)";

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            curCard.ApplyDiceAbility(DiceMatch.NextAttackDice, (DiceCardAbilityBase)new PassiveAbility_ShopPassiveMook8.Mook8Ability());
        }

        public class Mook8Ability : DiceCardAbilityBase
        {
            public override void OnWinParrying()
            {
                base.OnWinParrying();
                BattlePlayingCardDataInUnitModel currentDiceAction = this.owner.currentDiceAction;
                DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(currentDiceAction.card.GetID());
                BattlePlayingCardDataInUnitModel cardDataInUnitModel = new BattlePlayingCardDataInUnitModel();
                cardDataInUnitModel.owner = this.owner;
                BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(cardItem);
                playingCard.costSpended = true;
                cardDataInUnitModel.card = playingCard;
                cardDataInUnitModel.target = currentDiceAction.target;
                cardDataInUnitModel.speedDiceResultValue = 999;
                cardDataInUnitModel.cardBehaviorQueue.Clear();
                this.behavior.card = cardDataInUnitModel;
                cardDataInUnitModel.cardBehaviorQueue.Enqueue(this.behavior);
                this.behavior.abilityList.Remove((DiceCardAbilityBase)this);
                ((List<BattlePlayingCardDataInUnitModel>)typeof(StageController).GetField("_allCardList", AccessTools.all).GetValue(Singleton<StageController>.Instance)).Add(cardDataInUnitModel);
            }
        }
    }
}
