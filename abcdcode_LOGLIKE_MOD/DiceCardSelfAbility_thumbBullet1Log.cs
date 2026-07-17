// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_thumbBullet1Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_thumbBullet1Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_thumbBullet1Log</summary>

public class DiceCardSelfAbility_thumbBullet1Log : DiceCardSelfAbilityBase
{
  public override void OnUseCard() => this.OnActivate(this.owner, this.card.card);

  public override void OnDiscard(BattleUnitModel unit, BattleDiceCardModel self)
  {
    this.OnActivate(unit, self);
  }

  public void OnActivate(BattleUnitModel unit, BattleDiceCardModel self)
  {
    self.exhaust = true;
    unit.bufListDetail.AddBuf((BattleUnitBuf) new DiceCardSelfAbility_thumbBullet1Log.BattleUnitBuf_bullet1());
    unit.passiveDetail.OnExhaustBullet();
  }

  /// <summary>BattleUnitBuf_bullet1</summary>

  public class BattleUnitBuf_bullet1 : BattleUnitBuf
  {
    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      if (behavior.card.card.GetSpec().Ranged != CardRange.Far || behavior.TargetDice == null)
        return;
      behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = -1,
        dmg = -1
      });
    }

    public override void OnRoundEnd() => this.Destroy();
  }
}
}
