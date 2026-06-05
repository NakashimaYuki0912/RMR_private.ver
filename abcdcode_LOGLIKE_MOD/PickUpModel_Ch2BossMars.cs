// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Ch2BossMars
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_Ch2BossMars : PickUpModelBase
    {
        public override void LoadFromSaveData(LogueStageInfo stage)
        {
            stage.type = StageType.Boss;
            stage.stageid = 20005;
        }

        public PickUpModel_Ch2BossMars()
        {
            this.Name = TextDataModel.GetText("Stage_BossMars");
            this.Desc = TextDataModel.GetText("Stage_BossMars_Desc");
            this.FlaverText = "";
            this.ArtWork = "Stage_ch2_BossMars";
        }
    }
}
