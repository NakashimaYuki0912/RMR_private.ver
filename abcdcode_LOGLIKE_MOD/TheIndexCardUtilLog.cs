// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.TheIndexCardUtilLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

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
