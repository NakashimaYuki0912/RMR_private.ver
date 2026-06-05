// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveUnion4
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


using RogueLike_Mod_Reborn;

namespace abcdcode_LOGLIKE_MOD
{

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
