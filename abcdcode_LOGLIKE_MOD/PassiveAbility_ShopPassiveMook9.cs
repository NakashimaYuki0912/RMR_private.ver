// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveMook9
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_ShopPassiveMook9 : PassiveAbilityBase
    {
        public Dictionary<BattleDiceCardModel, int> cards;

        public override string debugDesc => "빛이 부족해도 대신 비용*3 만큼 흐트러짐 저항을 소모해 책장 사용 가능";

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (this.cards == null || !this.cards.ContainsKey(curCard.card))
                return;
            this.owner.TakeBreakDamage(this.cards[curCard.card] * 3);
        }

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            this.cards = new Dictionary<BattleDiceCardModel, int>();
        }

        public static PassiveAbility_ShopPassiveMook9 HasPassive(BattleUnitModel model)
        {
            return model == null || !model.passiveDetail.HasPassive<PassiveAbility_ShopPassiveMook9>() ? (PassiveAbility_ShopPassiveMook9)null : model.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_ShopPassiveMook9)) as PassiveAbility_ShopPassiveMook9;
        }
    }
}
