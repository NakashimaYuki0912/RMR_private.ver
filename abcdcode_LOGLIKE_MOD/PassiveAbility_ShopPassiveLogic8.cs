// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveLogic8
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_ShopPassiveLogic8 : PassiveAbilityBase
    {
        public override string debugDesc => "원거리 책장으로 일방 공격 시 피해량 + 3";

        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            if (behavior.IsParrying() || behavior.card.card.GetSpec().Ranged != LOR_DiceSystem.CardRange.Far)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = 3
            });
        }
    }
}
