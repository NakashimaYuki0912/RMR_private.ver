// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_thumbBullet3Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_thumbBullet3Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_thumbBullet3Log</summary>

public class DiceCardSelfAbility_thumbBullet3Log : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Burn_Keyword" };
  }

  public override void OnUseCard() => this.OnActivate(this.owner, this.card.card);

  public override void OnDiscard(BattleUnitModel unit, BattleDiceCardModel self)
  {
    this.OnActivate(unit, self);
  }

  public void OnActivate(BattleUnitModel unit, BattleDiceCardModel self)
  {
    self.exhaust = true;
    unit.bufListDetail.AddBuf((BattleUnitBuf) new DiceCardSelfAbility_thumbBullet3Log.BattleUnitBuf_bullet3());
    unit.passiveDetail.OnExhaustBullet();
  }

  /// <summary>BattleUnitBuf_bullet3</summary>

  public class BattleUnitBuf_bullet3 : BattleUnitBuf
  {
    public override void OnSuccessAttack(BattleDiceBehavior behavior)
    {
      if (behavior.card.card.GetSpec().Ranged != CardRange.Far)
        return;
      behavior.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 3, this._owner);
    }

    public override void OnRoundEnd() => this.Destroy();
  }
}
}
