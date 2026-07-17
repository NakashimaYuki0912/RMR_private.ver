// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveLogic3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveLogic3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_ShopPassiveLogic3</summary>

    public class PassiveAbility_ShopPassiveLogic3 : PassiveAbilityBase
    {
        public override string debugDesc => "첫 막에 속도 주사위 + 1";

        public override void OnUnitCreated() => base.OnUnitCreated();

        public override int SpeedDiceNumAdder()
        {
            return Singleton<StageController>.Instance.RoundTurn == 1 ? 1 : base.SpeedDiceNumAdder();
        }
    }
}
