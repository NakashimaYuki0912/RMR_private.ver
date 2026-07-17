// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_AddBullet1AndDrawLogEdit
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_AddBullet1AndDrawLogEdit.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_AddBullet1AndDrawLogEdit</summary>

public class DiceCardSelfAbility_AddBullet1AndDrawLogEdit : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[2]
      {
        "bullet_keyword",
        "DrawCard_Keyword"
      };
    }
  }

  public override void OnUseCard()
  {
    this.owner.allyCardDetail.AddNewCard(ThumbBulletClassLog.GetRandomBulletId());
    this.owner.allyCardDetail.DrawCards(1);
  }
}
}
