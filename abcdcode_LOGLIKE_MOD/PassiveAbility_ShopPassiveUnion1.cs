// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveUnion1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_ShopPassiveUnion1 : UnionWeaponPassiveAbilityBase
    {
        public override string debugDesc => "(다른 생체무기와 중복 불가) 막 시작 시 체력을 5% 잃음. 입힌 피해의 25%를 회복함";

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            int diceResultDamage = behavior.DiceResultDamage;
            if (diceResultDamage <= 3)
                return;
            this.owner.RecoverHP(diceResultDamage / 4);
        }
    }
}
