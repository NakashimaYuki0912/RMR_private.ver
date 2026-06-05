// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_nicolaiCard2Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_nicolaiCard2Log : DiceCardSelfAbilityBase
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
    this.owner.allyCardDetail.AddNewCard(Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 608004)).id).AddCost(-1);
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
