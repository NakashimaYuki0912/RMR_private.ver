// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_thumbBullet1Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

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
