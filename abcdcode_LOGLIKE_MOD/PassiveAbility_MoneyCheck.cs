// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_MoneyCheck
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_MoneyCheck.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Passive ability: PassiveAbility_MoneyCheck</summary>

    public class PassiveAbility_MoneyCheck : PassiveAbilityBase
    {
        public static int money;
        public static PassiveAbility_MoneyCheck instance;

        public static void SetMoney(int amount)
        {
            PassiveAbility_MoneyCheck.money = amount;
            PassiveAbility_MoneyCheck.Update();
        }

        public static void AddMoney(int amount)
        {
            PassiveAbility_MoneyCheck.money += amount;
            PassiveAbility_MoneyCheck.Update();
        }

        public static void SubMoney(int amount)
        {
            PassiveAbility_MoneyCheck.money -= amount;
            if (PassiveAbility_MoneyCheck.money < 0)
                PassiveAbility_MoneyCheck.money = 0;
            PassiveAbility_MoneyCheck.Update();
        }

        public static void Update()
        {
            if (PassiveAbility_MoneyCheck.instance != null)
                PassiveAbility_MoneyCheck.instance.UpdateMoney();
            LogLikeMod.BattleMoneyUI.UpdateMoney();
        }

        public void UpdateMoney() => this.desc = PassiveAbility_MoneyCheck.money.ToString();

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            PassiveAbility_MoneyCheck.instance = this;
            PassiveAbility_MoneyCheck.Update();
        }

        public static int GetMoney() => PassiveAbility_MoneyCheck.money;
    }
}
