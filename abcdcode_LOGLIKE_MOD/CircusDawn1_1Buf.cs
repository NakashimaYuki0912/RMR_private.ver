// -----------------------------------------------------------------------------
// Battle buffer / status effect: CircusDawn1_1Buf
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\CircusDawn1_1Buf.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>CircusDawn1_1Buf</summary>

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
