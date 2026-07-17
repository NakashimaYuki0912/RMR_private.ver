// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassive7
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassive7.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassive7</summary>

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
