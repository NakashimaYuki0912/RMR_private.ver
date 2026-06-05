// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.CircusDawn2Buf
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;

 
namespace abcdcode_LOGLIKE_MOD {

public class CircusDawn2Buf : BattleUnitBuf
{
  public override string keywordId => "LogueLikeMod_CricusDawn2Buf";

  public override void Init(BattleUnitModel owner)
  {
    base.Init(owner);
    typeof (BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue( this,  LogLikeMod.ArtWorks["buff_CricusDawn2"]);
    typeof (BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue( this,  true);
  }

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    base.BeforeRollDice(behavior);
    behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      power = -1
    });
  }

  public static CircusDawn2Buf IshaveBuf(BattleUnitModel target, bool findready = false)
  {
    foreach (BattleUnitBuf activatedBuf in target.bufListDetail.GetActivatedBufList())
    {
      if (activatedBuf is CircusDawn2Buf)
        return activatedBuf as CircusDawn2Buf;
    }
    if (findready)
    {
      foreach (BattleUnitBuf readyBuf in target.bufListDetail.GetReadyBufList())
      {
        if (readyBuf is CircusDawn2Buf)
          return readyBuf as CircusDawn2Buf;
      }
    }
    return (CircusDawn2Buf) null;
  }

  public static void GiveBuf(BattleUnitModel unit)
  {
    if (CircusDawn2Buf.IshaveBuf(unit) != null)
      return;
    CircusDawn2Buf buf = new CircusDawn2Buf();
    buf.stack = -1;
    buf.Init(unit);
    unit.bufListDetail.AddReadyBuf((BattleUnitBuf) buf);
  }
}
}
