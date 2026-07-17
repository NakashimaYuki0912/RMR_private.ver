// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_250009Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_250009Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_250009Log</summary>

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

  /// <summary>BattleUnitBuf_addBullet</summary>

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
