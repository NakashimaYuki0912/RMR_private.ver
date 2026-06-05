// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Ch4Mystery2Add
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_Ch4Mystery2Add : PickUpModelBase
    {
        public override void LoadFromSaveData(LogueStageInfo stage) => stage.type = StageType.Normal;

        public PickUpModel_Ch4Mystery2Add()
        {
            this.Name = TextDataModel.GetText("Stage_Normal");
            this.Desc = TextDataModel.GetText("Stage_Normal_Desc");
            this.FlaverText = "";
            this.ArtWork = "Stage_Normal";
        }
    }
}
