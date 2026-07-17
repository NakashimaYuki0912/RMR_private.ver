// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveUnion1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveUnion1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassiveUnion1</summary>

    public class PassiveAbility_ShopPassiveUnion1 : UnionWeaponPassiveAbilityBase
    {
        public override string debugDesc => "(다른 생체무기와 중복 불가) 막 시작 시 체력을 5% 잃음. 입힌 피해의 25%를 회복함";

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            int diceResultDamage = behavior.DiceResultDamage;
            if (diceResultDamage <= 3)
                return;
            this.owner.RecoverHP(diceResultDamage / 4);
        }
    }
}
