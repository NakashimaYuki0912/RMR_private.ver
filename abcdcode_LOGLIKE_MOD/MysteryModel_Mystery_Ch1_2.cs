// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch1_2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class MysteryModel_Mystery_Ch1_2 : MysteryBase
{
  public override void OnEnterChoice(int choiceid)
  {
    if (choiceid != 0 || this.curFrame.FrameID != 0)
      return;
    this.ShowDetailCard(choiceid, new LorId(LogLikeMod.ModId, 80001));
  }

  public override void OnExitChoice(int choiceid) => this.HideDetailCard();

  public override void OnClickChoice(int choiceid)
  {
    if (this.curFrame.FrameID == 1)
      LogueBookModels.AddCard(new LorId(LogLikeMod.ModId, 80001));
    if (this.curFrame.FrameID == 2)
      LogueBookModels.AddCard(new LorId(LogLikeMod.ModId, 80001));
    if (this.curFrame.FrameID == 3)
      LogueBookModels.AddCard(new LorId(LogLikeMod.ModId, 80001));
    base.OnClickChoice(choiceid);
  }
}
}
