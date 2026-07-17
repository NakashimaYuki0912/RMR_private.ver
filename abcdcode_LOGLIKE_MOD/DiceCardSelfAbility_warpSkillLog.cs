// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_warpSkillLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_warpSkillLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_warpSkillLog</summary>

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
