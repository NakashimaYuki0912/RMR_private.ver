// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveMook2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_ShopPassiveMook2 : PassiveAbilityBase
    {
        public override string debugDesc => "책장의 첫 공격 주사위는 공격 속성에 상관없이 적 내성을 '취약'으로 적용(면역이라면 미적용)";

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            curCard.ApplyDiceAbility(DiceMatch.NextAttackDice, (DiceCardAbilityBase)new PassiveAbility_ShopPassiveMook2.Mook2Ability());
        }

        public class Mook2Ability : DiceCardAbilityBase
        {
            public override void BeforeRollDice()
            {
                base.BeforeRollDice();
                this.card.target.bufListDetail.AddBuf((BattleUnitBuf)new PassiveAbility_ShopPassiveMook2.Mook2Ability.Mook2Debuf());
            }

            public override void AfterAction()
            {
                base.AfterAction();
                var buf = this.card.target?.bufListDetail?.GetActivatedBufList().Find(x => x is Mook2Ability.Mook2Debuf);
                if (buf != null)
                    buf.Destroy();
            }

            public class Mook2Debuf : BattleUnitBuf
            {
                public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
                {
                    return origin != AtkResist.Immune ? AtkResist.Weak : origin;
                }

                public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
                {
                    return origin != AtkResist.Immune ? AtkResist.Weak : origin;
                }

                public override void AfterDiceAction(BattleDiceBehavior behavior)
                {
                    base.AfterDiceAction(behavior);
                    this.Destroy();
                }
            }
        }
    }
}
