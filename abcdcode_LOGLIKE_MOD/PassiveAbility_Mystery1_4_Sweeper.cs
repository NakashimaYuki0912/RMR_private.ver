// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_Mystery1_4_Sweeper
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class PassiveAbility_Mystery1_4_Sweeper : PassiveAbilityBase
    {
        public int round;

        public override string debugDesc => "3막 후 뒷골목의 밤이 끝남";

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            ++this.round;
            if (this.round <= 3)
                return;
            Singleton<StageController>.Instance.GetStageModel().GetWave(Singleton<StageController>.Instance.CurrentWave).Defeat();
            Singleton<StageController>.Instance.EndBattle();
        }
    }
}
