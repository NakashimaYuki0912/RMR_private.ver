// -----------------------------------------------------------------------------
// Library of Ruina mod script: ThumbBulletClassLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\ThumbBulletClassLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>ThumbBulletClassLog</summary>

public static class ThumbBulletClassLog
{
  public static LorId GetRandomBulletId()
  {
    return RandomUtil.SelectOne<LorId>(new LorId(LogLikeMod.ModId, 602020), new LorId(LogLikeMod.ModId, 602021), new LorId(LogLikeMod.ModId, 602022));
  }

  public static LorId GetRandomUpgradeBulletId()
  {
    return RandomUtil.SelectOne<LorId>(Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 602020)).id, Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 602021)).id, Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 602022)).id);
  }

  public static bool IsBulletId(LorId id)
  {
    LorId id1 = Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 602020)).id;
    LorId id2 = Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 602021)).id;
    LorId id3 = Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 602022)).id;
    return id == new LorId(LogLikeMod.ModId, 602020) || id == new LorId(LogLikeMod.ModId, 602021) || id == new LorId(LogLikeMod.ModId, 602022) || id == id1 || id == id2 || id == id3;
  }
}
}
