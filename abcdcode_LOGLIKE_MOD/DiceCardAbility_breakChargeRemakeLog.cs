// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardAbility_breakChargeRemakeLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardAbility_breakChargeRemakeLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "WarpCharge" };
  }

  public override void OnSucceedAttack()
  {
    for (int index = 0; index < 4; ++index)
    {
      if (this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.WarpCharge) is BattleUnitBuf_warpCharge activatedBuf && activatedBuf.stack >= 2)
      {
        activatedBuf.UseStack(2, true);
        if (this.card?.target != null)
          this.card.target.TakeBreakDamage(6, DamageType.Card_Ability, this.owner);
      }
    }
  }
}
}
