// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MaxDonwMinUpBuf
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;

 
namespace abcdcode_LOGLIKE_MOD {

public class MaxDonwMinUpBuf : BattleUnitBuf
{
  public override string keywordId => "LogueLikeMod_MaxDonwMinUpBuf";

  public override void Init(BattleUnitModel owner)
  {
    base.Init(owner);
    typeof (BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue( this,  LogLikeMod.ArtWorks["buff_MaxDonwMinUp"]);
    typeof (BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue( this,  true);
  }

  public override void OnRoundEnd()
  {
    base.OnRoundEnd();
    --this.stack;
    if (this.stack > 0)
      return;
    this.Destroy();
  }

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    base.BeforeRollDice(behavior);
    behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      min = 2,
      max = -1
    });
  }

  public static MaxDonwMinUpBuf IshaveBuf(BattleUnitModel target, bool findready = false)
  {
    foreach (BattleUnitBuf activatedBuf in target.bufListDetail.GetActivatedBufList())
    {
      if (activatedBuf is MaxDonwMinUpBuf)
        return activatedBuf as MaxDonwMinUpBuf;
    }
    if (findready)
    {
      foreach (BattleUnitBuf readyBuf in target.bufListDetail.GetReadyBufList())
      {
        if (readyBuf is MaxDonwMinUpBuf)
          return readyBuf as MaxDonwMinUpBuf;
      }
    }
    return (MaxDonwMinUpBuf) null;
  }

  public static void GiveBufThisRound(BattleUnitModel target, int stack)
  {
    MaxDonwMinUpBuf maxDonwMinUpBuf = MaxDonwMinUpBuf.IshaveBuf(target);
    if (maxDonwMinUpBuf != null)
    {
      maxDonwMinUpBuf.stack += stack;
    }
    else
    {
      MaxDonwMinUpBuf buf = new MaxDonwMinUpBuf();
      buf.stack = stack;
      buf.Init(target);
      target.bufListDetail.AddBuf((BattleUnitBuf) buf);
    }
  }
}
}
