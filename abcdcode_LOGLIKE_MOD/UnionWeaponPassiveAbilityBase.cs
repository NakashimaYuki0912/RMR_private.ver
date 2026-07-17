// -----------------------------------------------------------------------------
// Library of Ruina mod script: UnionWeaponPassiveAbilityBase
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\UnionWeaponPassiveAbilityBase.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>UnionWeaponPassiveAbilityBase</summary>

    public class UnionWeaponPassiveAbilityBase : PassiveAbilityBase
    {
        public override string debugDesc => "막 시작 시 체력 5% 잃음";

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            this.owner.TakeDamage(this.owner.MaxHp / 20 > 12 ? 12 : this.owner.MaxHp / 20, DamageType.Passive);
        }
    }
}
