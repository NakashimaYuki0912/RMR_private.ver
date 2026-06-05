// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveUnion2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

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
