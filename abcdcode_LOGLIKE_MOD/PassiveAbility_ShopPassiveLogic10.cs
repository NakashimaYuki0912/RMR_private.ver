// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveLogic10
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System;


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_ShopPassiveLogic10 : PassiveAbilityBase
    {
        public override string debugDesc => "이전 막에 원거리 책장을 사용하지 않았다면 이번막에 원거리 주사위 위력 + 2";

        bool usedPage = false;
        bool doThing = false;
        public override void OnRoundEnd()
        {
            usedPage = doThing;
            doThing = false;
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (usedPage || behavior.card.card.GetSpec().Ranged != CardRange.Far)
                return;
            doThing = true;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = 2
            });
        }
    }
}
