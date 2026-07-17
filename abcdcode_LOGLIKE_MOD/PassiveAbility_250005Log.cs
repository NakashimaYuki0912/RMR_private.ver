// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_250005Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_250005Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_250005Log</summary>

public class PassiveAbility_250005Log : PassiveAbilityBase
{
  public int _count;

  public override void OnSucceedAttack(BattleDiceBehavior behavior)
  {
    if (behavior.card.card.GetSpec().Ranged != CardRange.Near)
      return;
    ++this._count;
    if (this._count >= 3)
    {
      this.owner.allyCardDetail.AddNewCard(ThumbBulletClassLog.GetRandomBulletId());
      this._count = 0;
    }
  }
}
}
