// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_thumbBullet2Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_thumbBullet2Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_thumbBullet2Log</summary>

public class DiceCardSelfAbility_thumbBullet2Log : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[2]
      {
        "Paralysis_Keyword",
        "Disarm_Keyword"
      };
    }
  }

  public override void OnUseCard() => this.OnActivate(this.owner, this.card.card);

  public override void OnDiscard(BattleUnitModel unit, BattleDiceCardModel self)
  {
    this.OnActivate(unit, self);
  }

  public void OnActivate(BattleUnitModel unit, BattleDiceCardModel self)
  {
    self.exhaust = true;
    unit.bufListDetail.AddBuf((BattleUnitBuf) new DiceCardSelfAbility_thumbBullet2Log.BattleUnitBuf_bullet2());
    unit.passiveDetail.OnExhaustBullet();
  }

  /// <summary>BattleUnitBuf_bullet2</summary>

  public class BattleUnitBuf_bullet2 : BattleUnitBuf
  {
    public override void OnSuccessAttack(BattleDiceBehavior behavior)
    {
      if (behavior.card.card.GetSpec().Ranged != CardRange.Far)
        return;
      behavior.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Paralysis, 2, this._owner);
      behavior.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Disarm, 1, this._owner);
    }

    public override void OnRoundEnd() => this.Destroy();
  }
}
}
