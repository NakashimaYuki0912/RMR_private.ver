// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_AddBullet2Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_AddBullet2Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_AddBullet2Log</summary>

public class DiceCardSelfAbility_AddBullet2Log : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "bullet_keyword" };
  }

  public override void OnUseCard()
  {
    this.owner.allyCardDetail.AddNewCard(ThumbBulletClassLog.GetRandomUpgradeBulletId());
    this.owner.allyCardDetail.AddNewCard(ThumbBulletClassLog.GetRandomUpgradeBulletId());
  }
}
}
