// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_thumbBullet2Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

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
