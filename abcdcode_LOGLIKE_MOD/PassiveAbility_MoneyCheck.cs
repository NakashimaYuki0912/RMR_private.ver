// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_MoneyCheck
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

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
