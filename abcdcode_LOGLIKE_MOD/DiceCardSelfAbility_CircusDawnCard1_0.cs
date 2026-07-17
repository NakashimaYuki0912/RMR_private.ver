// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_CircusDawnCard1_0
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_CircusDawnCard1_0.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_CircusDawnCard1_0</summary>

public class DiceCardSelfAbility_CircusDawnCard1_0 : DiceCardSelfAbilityBase
{
  public int count;

  public override void OnUseCard()
  {
    base.OnUseCard();
    this.count = 0;
  }

  public override void OnLoseParrying()
  {
    base.OnLoseParrying();
    ++this.count;
  }

  public override void OnEndBattle()
  {
    base.OnEndBattle();
    if (this.count >= 3 || this.card.target == null || this.card.target.IsDead())
      return;
    CircusDawn1Buf.GiveBuf(this.owner, this.card.target);
  }
}
}
