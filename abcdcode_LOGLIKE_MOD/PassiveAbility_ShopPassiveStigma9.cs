// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveStigma9
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveStigma9.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassiveStigma9</summary>

    public class PassiveAbility_ShopPassiveStigma9 : PassiveAbilityBase
    {
        public override string debugDesc => "자신에게 화상이 있다면 일방공격 대상이 되었을때 자신에게 있는 화상 수치만큼 적에게 화상 부여";

        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel attackerCard)
        {
            base.OnStartTargetedOneSide(attackerCard);
            if (!(this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.Burn) is BattleUnitBuf_burn activatedBuf))
                return;
            int stack = activatedBuf.stack;
            attackerCard.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Burn, stack);
        }
    }
}
