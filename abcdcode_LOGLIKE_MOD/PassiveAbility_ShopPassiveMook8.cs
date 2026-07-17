// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveMook8
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveMook8.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using HarmonyLib;
using LOR_DiceSystem;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassiveMook8</summary>

    public class PassiveAbility_ShopPassiveMook8 : PassiveAbilityBase
    {
        public override string debugDesc => "책장의 첫 공격 주사위 합 승리 시 합 종료 후 해당 주사위를 일방공격으로 재사용(막당 1회)";

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            curCard.ApplyDiceAbility(DiceMatch.NextAttackDice, (DiceCardAbilityBase)new PassiveAbility_ShopPassiveMook8.Mook8Ability());
        }

        /// <summary>Mook8Ability</summary>

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
