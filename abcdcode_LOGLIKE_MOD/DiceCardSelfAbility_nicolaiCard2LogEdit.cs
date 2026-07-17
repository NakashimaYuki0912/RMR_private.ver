// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_nicolaiCard2LogEdit
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_nicolaiCard2LogEdit.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_nicolaiCard2LogEdit</summary>

public class DiceCardSelfAbility_nicolaiCard2LogEdit : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "WarpCharge" };
  }

  public override bool OnChooseCard(BattleUnitModel owner)
  {
    BattleUnitBuf activatedBuf = owner.bufListDetail.GetActivatedBuf(KeywordBuf.WarpCharge);
    return activatedBuf != null && activatedBuf.stack >= 20;
  }

  public override void OnUseCard() => this.card.card.exhaust = true;

  public override void OnEndBattle()
  {
    if (this.card.target == null || !this.card.target.IsDead())
      return;
    this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 608004)).AddCost(-1);
  }

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    if (this.card.target == null || (double) this.card.target.hp > (double) this.card.target.MaxHp * 0.5)
      return;
    behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      dmgRate = 100
    });
  }
}
}
