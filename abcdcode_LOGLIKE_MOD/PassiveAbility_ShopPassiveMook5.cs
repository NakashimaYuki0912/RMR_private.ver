// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveMook5
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_ShopPassiveMook5 : PassiveAbilityBase
    {
        public override string debugDesc => "책장의 첫 공격 주사위로 적 처치 시 해당 주사위와 같은 반격 주사위 획득";

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            curCard.ApplyDiceAbility(DiceMatch.NextAttackDice, (DiceCardAbilityBase)new PassiveAbility_ShopPassiveMook5.Mook5Ability());
        }

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
