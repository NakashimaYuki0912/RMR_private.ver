// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassive1_3
// Shop passive: gain Luck 1 each round; lose 1 HP per card used.
// Namespace/file: abcdcode_LOGLIKE_MOD/PassiveAbility_ShopPassive1_3.cs
// -----------------------------------------------------------------------------
using RogueLike_Mod_Reborn;

namespace abcdcode_LOGLIKE_MOD
{
    /// <summary>Shop passive: round-start Luck; HP cost when using combat pages.</summary>
    public class PassiveAbility_ShopPassive1_3 : PassiveAbilityBase
    {
        public override string debugDesc => "막 시작 시 행운 1을 얻음. 책장을 사용할때마다 체력을 1 잃음";

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(RoguelikeBufs.RMRLuck, 1, owner);
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            this.owner.LoseHp(1);
        }
    }
}
