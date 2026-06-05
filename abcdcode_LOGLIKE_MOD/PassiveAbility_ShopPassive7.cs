// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassive7
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_ShopPassive7 : PassiveAbilityBase
    {
        public override string debugDesc => "이전 챕터의 책장을 사용시 모든 주사위 위력 + 1. 이전 챕터 단계당 위력이 추가로 1 증가";

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.card.owner.faction == Faction.Enemy)
                return;
            int num = (int)LogLikeMod.curchaptergrade + 1 > behavior.card.card.XmlData.Chapter ? 1 : 0;
            if (num <= 0)
                return;
            this.owner.battleCardResultLog?.SetPassiveAbility(this);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = num
            });
        }
    }
}
