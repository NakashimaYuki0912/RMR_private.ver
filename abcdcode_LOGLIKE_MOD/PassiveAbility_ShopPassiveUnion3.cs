// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveUnion3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_ShopPassiveUnion3 : UnionWeaponPassiveAbilityBase
    {
        public override string debugDesc => "(다른 생체무기와 중복 불가) 막 시작 시 체력을 5% 잃음. 모든 주사위 최대값 + 3~5";

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            int num = Random.Range(3, 6);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                min = num
            });
        }
    }
}
