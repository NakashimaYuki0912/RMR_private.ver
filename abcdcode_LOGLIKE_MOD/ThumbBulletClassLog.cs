// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.ThumbBulletClassLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

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
