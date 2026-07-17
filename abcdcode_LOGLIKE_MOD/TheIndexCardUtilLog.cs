// -----------------------------------------------------------------------------
// Library of Ruina mod script: TheIndexCardUtilLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\TheIndexCardUtilLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>TheIndexCardUtilLog</summary>

public static class TheIndexCardUtilLog
{
  public static LorId GetCardIdByBuf(BattleUnitModel unit)
  {
    if (unit.bufListDetail.GetKewordBufStack(KeywordBuf.SlashPowerUp) > 0)
      return new LorId(LogLikeMod.ModId, 505003);
    if (unit.bufListDetail.GetKewordBufStack(KeywordBuf.PenetratePowerUp) > 0)
      return new LorId(LogLikeMod.ModId, 605004);
    unit.bufListDetail.GetKewordBufStack(KeywordBuf.HitPowerUp);
    return new LorId(LogLikeMod.ModId, 505002);
  }

  public static LorId GetAddedCardIdByBuf(BattleUnitModel unit)
  {
    return unit.bufListDetail.GetKewordBufStack(KeywordBuf.SlashPowerUp) > 0 ? new LorId(LogLikeMod.ModId, 505001) : new LorId(LogLikeMod.ModId, 605005);
  }

  public static bool IsActivatedRelease(BattleUnitModel unit)
  {
    return unit.bufListDetail.GetActivatedBuf(KeywordBuf.IndexRelease) != null;
  }
}
}
