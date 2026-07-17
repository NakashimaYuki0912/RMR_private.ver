// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_AddBullet2LogEdit
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_AddBullet2LogEdit.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_AddBullet2LogEdit</summary>

public class DiceCardSelfAbility_AddBullet2LogEdit : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "bullet_keyword" };
  }

  public override void OnUseCard()
  {
    this.owner.allyCardDetail.AddNewCard(ThumbBulletClassLog.GetRandomBulletId());
    this.owner.allyCardDetail.AddNewCard(ThumbBulletClassLog.GetRandomBulletId());
  }
}
}
