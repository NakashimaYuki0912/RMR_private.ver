// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassive45
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;


namespace abcdcode_LOGLIKE_MOD
{

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
