// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveLogic10
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveLogic10.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;
using System;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassiveLogic10</summary>

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
