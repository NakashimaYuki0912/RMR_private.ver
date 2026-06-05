// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_250009Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_250009Log : PassiveAbilityBase
{
  public int _count;

  public override void OnExhaustBullet()
  {
    ++this._count;
    if (this._count < 3)
      return;
    this._count = 0;
    this.owner.bufListDetail.AddBuf((BattleUnitBuf) new PassiveAbility_250009Log.BattleUnitBuf_addBullet());
  }

  public class BattleUnitBuf_addBullet : BattleUnitBuf
  {
    public override void OnRoundEnd()
    {
      this._owner.allyCardDetail.AddNewCard(ThumbBulletClassLog.GetRandomBulletId());
      this.Destroy();
    }
  }
}
}
