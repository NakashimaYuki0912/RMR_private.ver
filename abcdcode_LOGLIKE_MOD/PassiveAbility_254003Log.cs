// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_254003Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_254003Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_254003Log</summary>

public class PassiveAbility_254003Log : PassiveAbilityBase
{
  public const float _MAX_HP_RATIO = 0.3f;
  public const int _SPECIAL_CARD_ID = 614006;
  public bool _bActivated;

  public override void OnWaveStart() => this._bActivated = false;

  public override void OnRoundEndTheLast()
  {
    if (this._bActivated || (double) this.owner.hp > (double) this.owner.MaxHp * 0.30000001192092896)
      return;
    if (this.owner.allyCardDetail.GetHand().Count >= this.owner.allyCardDetail.maxHandCount)
      this.owner.allyCardDetail.DiscardInHand(1);
    this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 614006));
    this._bActivated = true;
  }
}
}
