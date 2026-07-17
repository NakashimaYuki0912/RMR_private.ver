// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch1_2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch1_2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Mystery node model: MysteryModel_Mystery_Ch1_2</summary>

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
