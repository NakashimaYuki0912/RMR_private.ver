// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveStigma9
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

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
