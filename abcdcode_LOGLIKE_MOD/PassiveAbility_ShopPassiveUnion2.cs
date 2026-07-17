// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveUnion2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveUnion2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassiveUnion2</summary>

    public class PassiveAbility_ShopPassiveUnion2 : UnionWeaponPassiveAbilityBase
    {
        public override string debugDesc
        {
            get => "(다른 생체무기와 중복 불가) 막 시작 시 체력을 5% 잃음. 공격 주사위 적중 시 출혈 2 부여. 출혈을 부여할때마다 부여한 출혈만큼 피해를 입힘";
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (behavior.card.target == null)
                return;
            behavior.card.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 2);
        }
    }
}
