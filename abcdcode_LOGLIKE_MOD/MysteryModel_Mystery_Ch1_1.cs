// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch1_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch1_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Mystery node model: MysteryModel_Mystery_Ch1_1</summary>

public class MysteryModel_Mystery_Ch1_1 : MysteryBase
{
  private static DropBookXmlInfo GetPlainDropBook(ChapterGrade chapter)
  {
    int chapterNumber = (int)chapter + 1;
    return Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, chapterNumber * 1000 + 1));
  }

  private static void QueuePlainDropBooks(ChapterGrade chapter, int count)
  {
    DropBookXmlInfo dropBook = GetPlainDropBook(chapter);
    if (dropBook == null)
    {
      UnityEngine.Debug.LogWarning($"[RMR Mystery_Ch1_1] Missing plain drop book for chapter {chapter}.");
      return;
    }
    for (int i = 0; i < count; i++)
      LogLikeMod.rewards.Add(dropBook);
  }

  public override void OnClickChoice(int choiceid)
  {
    if (this.curFrame.FrameID == 0)
    {
      if (choiceid == 0)
      {
        QueuePlainDropBooks(LogLikeMod.curchaptergrade, 2);
        MysteryModel_CardReward.PopupCardReward_AutoSave();
      }
      if (choiceid == 1)
      {
        ChapterGrade nextChapter = LogLikeMod.curchaptergrade < ChapterGrade.Grade7
          ? LogLikeMod.curchaptergrade + 1
          : ChapterGrade.Grade7;
        QueuePlainDropBooks(nextChapter, 1);
        MysteryModel_CardReward.PopupCardReward_AutoSave();
      }
    }
    base.OnClickChoice(choiceid);
  }
}
}
