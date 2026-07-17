// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveMook5
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveMook5.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassiveMook5</summary>

    public class PassiveAbility_ShopPassiveMook5 : PassiveAbilityBase
    {
        public override string debugDesc => "책장의 첫 공격 주사위로 적 처치 시 해당 주사위와 같은 반격 주사위 획득";

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            curCard.ApplyDiceAbility(DiceMatch.NextAttackDice, (DiceCardAbilityBase)new PassiveAbility_ShopPassiveMook5.Mook5Ability());
        }

        /// <summary>Mook5Ability</summary>

        public class Mook5Ability : DiceCardAbilityBase
        {
            public override void AfterAction()
            {
                base.AfterAction();
                if (!this.card.target.IsDead())
                    return;
                this.owner.cardSlotDetail.keepCard.AddBehaviour(this.card.card, this.behavior);
            }
        }
    }
}
