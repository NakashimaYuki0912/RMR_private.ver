// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveLogic8
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveLogic8.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassiveLogic8</summary>

    public class PassiveAbility_ShopPassiveLogic8 : PassiveAbilityBase
    {
        public override string debugDesc => "원거리 책장으로 일방 공격 시 피해량 + 3";

        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            if (behavior.IsParrying() || behavior.card.card.GetSpec().Ranged != LOR_DiceSystem.CardRange.Far)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = 3
            });
        }
    }
}
