// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveMook2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveMook2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassiveMook2</summary>

    public class PassiveAbility_ShopPassiveMook2 : PassiveAbilityBase
    {
        public override string debugDesc => "책장의 첫 공격 주사위는 공격 속성에 상관없이 적 내성을 '취약'으로 적용(면역이라면 미적용)";

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            curCard.ApplyDiceAbility(DiceMatch.NextAttackDice, (DiceCardAbilityBase)new PassiveAbility_ShopPassiveMook2.Mook2Ability());
        }

        /// <summary>Mook2Ability</summary>

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

            /// <summary>Mook2Debuf</summary>

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
