// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.CircusDawn1_1Buf
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class CircusDawn1_1Buf : BattleUnitBuf
{
  public override void Init(BattleUnitModel owner) => base.Init(owner);

  public override void OnRoundEnd()
  {
    base.OnRoundEnd();
    this.Destroy();
  }
}
}
