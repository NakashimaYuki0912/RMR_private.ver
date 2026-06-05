// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Ch5_2EventResult
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_Ch5_2EventResult : PickUpModelBase
    {
        public override void LoadFromSaveData(LogueStageInfo stage) => stage.type = StageType.Mystery;

        public PickUpModel_Ch5_2EventResult()
        {
            this.Name = TextDataModel.GetText("Stage_Mystery");
            this.Desc = TextDataModel.GetText("Stage_Mystery_Desc");
            this.FlaverText = "";
            this.ArtWork = "Stage_Mystery";
        }
    }
}
