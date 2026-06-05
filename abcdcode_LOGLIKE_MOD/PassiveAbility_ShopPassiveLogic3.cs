// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveLogic3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

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
