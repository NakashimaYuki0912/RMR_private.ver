// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveStigma5
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_ShopPassiveStigma5 : PassiveAbilityBase
    {
        public int stack;

        public override string debugDesc => "내 화상이 사라질때 이번 전투에서 얻었던 최대 화상수치만큼 체력을 회복함";

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.stack = 0;
        }

        public void Recovering() => this.owner.RecoverHP(this.stack);
    }
}
