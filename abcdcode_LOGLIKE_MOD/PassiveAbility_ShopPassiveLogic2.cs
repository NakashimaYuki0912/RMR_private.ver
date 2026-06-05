// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveLogic2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_ShopPassiveLogic2 : PassiveAbilityBase
    {
        public bool Used;

        public override string debugDesc => "원거리 책장으로 근접 책장과 합할때 내 책장의 주사위를 모두 소모했다면 합을 강제로 종료함(막당 1회)";

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            this.Used = false;
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (this.Used || curCard.card.GetSpec().Ranged != CardRange.Far || curCard.target.currentDiceAction == null || curCard.target.currentDiceAction.card.GetSpec().Ranged > CardRange.Near)
                return;
            List<BattleDiceBehavior> diceBehaviorList = curCard.GetDiceBehaviorList();
            diceBehaviorList.RemoveAll(x => x.Type != 0);
            if (diceBehaviorList.Count <= 0)
                return;
            diceBehaviorList[diceBehaviorList.Count - 1].AddAbility(new PassiveAbility_ShopPassiveLogic2.EndParryingAbility());
            this.Used = true;
        }

        public class EndParryingAbility : DiceCardAbilityBase
        {
            public override void AfterAction()
            {
                base.AfterAction();
                BattleUnitModel target = this.owner?.currentDiceAction?.target;
                if (target == null || target?.currentDiceAction == null)
                    return;
                Queue<BattleDiceBehavior> battleDiceBehaviorQueue = new Queue<BattleDiceBehavior>(target.currentDiceAction.cardBehaviorQueue);
                List<BattleDiceBehavior> behaviourList = new List<BattleDiceBehavior>();
                while (battleDiceBehaviorQueue.Count > 0)
                {
                    BattleDiceBehavior battleDiceBehavior = battleDiceBehaviorQueue.Dequeue();
                    behaviourList.Add(battleDiceBehavior);
                }
                target.cardSlotDetail.keepCard.AddBehaviours(target.currentDiceAction.card, behaviourList);
                target.currentDiceAction.cardBehaviorQueue.Clear();
            }
        }
    }
}
