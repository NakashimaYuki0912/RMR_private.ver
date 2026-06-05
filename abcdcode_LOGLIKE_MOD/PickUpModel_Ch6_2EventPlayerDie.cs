// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Ch6_2EventPlayerDie
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class PickUpModel_Ch6_2EventPlayerDie : PickUpModelBase
    {
        public PickUpModel_Ch6_2EventPlayerDie()
        {
            this.Name = TextDataModel.GetText("Ch6Event2Effect_Name");
            this.Desc = TextDataModel.GetText("Ch6Event2Effect_Desc");
            this.FlaverText = "";
            this.ArtWork = "Ch6Event2Effect";
        }

        public override bool IsCanPickUp(UnitDataModel target) => !target.IsDead();

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            model.Die();
        }
    }
}
