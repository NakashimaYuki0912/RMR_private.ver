// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveUnion3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveUnion3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassiveUnion3</summary>

    public class PassiveAbility_ShopPassiveUnion3 : UnionWeaponPassiveAbilityBase
    {
        public override string debugDesc => "(다른 생체무기와 중복 불가) 막 시작 시 체력을 5% 잃음. 모든 주사위 최대값 + 3~5";

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            int num = Random.Range(3, 6);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                min = num
            });
        }
    }
}
