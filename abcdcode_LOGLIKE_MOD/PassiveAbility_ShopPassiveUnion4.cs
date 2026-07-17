// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveUnion4
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveUnion4.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using RogueLike_Mod_Reborn;

namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassiveUnion4</summary>

    public class PassiveAbility_ShopPassiveUnion4 : UnionWeaponPassiveAbilityBase
    {
        public override string debugDesc => "(다른 생체무기와 중복 불가) 막 시작 시 체력을 5% 잃음. 행운을 2 얻음";

        public override void OnRoundStartAfter()
        {
            base.OnRoundStartAfter();
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(RoguelikeBufs.RMRLuck, 2, owner);
        }
    }
}
