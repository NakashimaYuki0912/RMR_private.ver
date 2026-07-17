// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassive45
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassive45.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassive45</summary>

    public class PassiveAbility_ShopPassive45 : PassiveAbilityBase
    {
        public override string debugDesc => "막 시작 시 50% 확률로 속도 주사위 슬롯 + 1(중복 불가)";

        private bool rolled;

        public override int SpeedDiceNumAdder()
        {
            rolled = Singleton<Random>.Instance.Next(0, 2) == 1;
            return rolled ? 1 : base.SpeedDiceNumAdder();
        }

        public override void OnAfterRollSpeedDice()
        {
            base.OnAfterRollSpeedDice();
            if (rolled)
            {
                owner.RollSpeedDice();
            }
        }
    }
}
