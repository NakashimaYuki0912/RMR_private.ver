// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveStigma2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_ShopPassiveStigma2 : PassiveAbilityBase
{
  public override string debugDesc => "전투 책장으로 적에게 화상을 부여하면 4막 후에 부여한 수치만큼 다시 화상을 부여함";

  public override int OnGiveKeywordBufByCard(BattleUnitBuf buf, int stack, BattleUnitModel target)
  {
    target.bufListDetail.AddBuf((BattleUnitBuf) new PassiveAbility_ShopPassiveStigma2.Stigma2Buf(stack));
    return base.OnGiveKeywordBufByCard(buf, stack, target);
  }

  public class Stigma2Buf : BattleUnitBuf
  {
    public int fire;

    public Stigma2Buf(int fire)
    {
      this.stack = 4;
      this.fire = fire;
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      --this.stack;
      if (this.stack != 0)
        return;
      this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Burn, this.fire);
    }
  }
}
}
