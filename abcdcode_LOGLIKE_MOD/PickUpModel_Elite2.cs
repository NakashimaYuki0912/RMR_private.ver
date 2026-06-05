// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Elite2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class PickUpModel_Elite2 : PickUpModelBase
    {
        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            this.GivePassive(new LorId(LogLikeMod.ModId, 15220002), model);
        }
    }
}
