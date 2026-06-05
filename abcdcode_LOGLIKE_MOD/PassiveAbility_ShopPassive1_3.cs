// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassive1_3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll
using RogueLike_Mod_Reborn;

namespace abcdcode_LOGLIKE_MOD
{

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
