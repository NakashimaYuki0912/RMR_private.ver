// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_254003Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

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
