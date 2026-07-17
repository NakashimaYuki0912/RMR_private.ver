// -----------------------------------------------------------------------------
// Battle buffer / status effect: CircusDawn1Buf
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\CircusDawn1Buf.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using HarmonyLib;
using System;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>CircusDawn1Buf</summary>

public class CircusDawn1Buf : BattleUnitBuf
{
  public CircusDawn1_1Buf checkbuf;
  public BattleUnitModel targetunit;

  public override string keywordId => "LogueLikeMod_CricusDawn1Buf";

  public override void Init(BattleUnitModel owner)
  {
    base.Init(owner);
    typeof (BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue( this,  LogLikeMod.ArtWorks["buff_CricusDawn1"]);
    typeof (BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue( this,  true);
    this.checkbuf = new CircusDawn1_1Buf();
    this.checkbuf.stack = -1;
    this.checkbuf.Init(this.targetunit);
    this.targetunit.bufListDetail.AddBuf((BattleUnitBuf) this.checkbuf);
  }

  public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
  {
    base.OnTakeDamageByAttack(atkDice, dmg);
    this.targetunit.TakeDamage(dmg, DamageType.Buf);
  }

  public override void Destroy()
  {
    base.Destroy();
    if (this.checkbuf == null)
      return;
    this.checkbuf.Destroy();
  }

  public override void OnRoundEnd()
  {
    base.OnRoundEnd();
    this.Destroy();
    if (this._owner.breakDetail.IsBreakLifeZero() || this._owner.IsDead())
      return;
    CircusDawn2Buf.GiveBuf(this.targetunit);
  }

  public static CircusDawn1Buf IshaveBuf(BattleUnitModel target, bool findready = false)
  {
    foreach (BattleUnitBuf activatedBuf in target.bufListDetail.GetActivatedBufList())
    {
      if (activatedBuf is CircusDawn1Buf)
        return activatedBuf as CircusDawn1Buf;
    }
    if (findready)
    {
      foreach (BattleUnitBuf readyBuf in target.bufListDetail.GetReadyBufList())
      {
        if (readyBuf is CircusDawn1Buf)
          return readyBuf as CircusDawn1Buf;
      }
    }
    return (CircusDawn1Buf) null;
  }

  public static void GiveBuf(BattleUnitModel unit, BattleUnitModel targetunit)
  {
    if (CircusDawn1Buf.IshaveBuf(unit) != null || targetunit.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is CircusDawn1_1Buf)) != null)
      return;
    CircusDawn1Buf buf = new CircusDawn1Buf();
    buf.stack = -1;
    buf.Init(unit);
    buf.targetunit = targetunit;
    unit.bufListDetail.AddReadyBuf((BattleUnitBuf) buf);
  }
}
}
