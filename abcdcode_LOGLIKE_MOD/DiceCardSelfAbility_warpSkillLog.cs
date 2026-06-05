// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_warpSkillLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_warpSkillLog : DiceCardSelfAbilityBase
{
  public int chargeStack;

  public override string[] Keywords
  {
    get
    {
      return new string[2]
      {
        "onlypage_warp_Keyword",
        "WarpCharge"
      };
    }
  }

  public override void OnUseCard()
  {
    if (this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.WarpCharge) is BattleUnitBuf_warpCharge activatedBuf && activatedBuf.stack > 0)
    {
      this.chargeStack = activatedBuf.stack;
      activatedBuf.UseStack(activatedBuf.stack, true);
    }
    if ((double) RandomUtil.valueForProb < (double) this.chargeStack * 0.0989999994635582)
    {
      this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
      {
        power = 10
      });
    }
    else
    {
      FilterUtil.ShowWarpBloodFilter();
      this.chargeStack = 0;
      this.owner.TakeDamage(15, DamageType.Card_Ability, this.owner);
    }
  }
}
}
