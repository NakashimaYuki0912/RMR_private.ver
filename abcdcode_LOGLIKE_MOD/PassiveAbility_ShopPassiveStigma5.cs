// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveStigma5
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveStigma5.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassiveStigma5</summary>

    public class PassiveAbility_ShopPassiveStigma5 : PassiveAbilityBase
    {
        public int stack;

        public override string debugDesc => "내 화상이 사라질때 이번 전투에서 얻었던 최대 화상수치만큼 체력을 회복함";

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.stack = 0;
        }

        public void Recovering() => this.owner.RecoverHP(this.stack);
    }
}
