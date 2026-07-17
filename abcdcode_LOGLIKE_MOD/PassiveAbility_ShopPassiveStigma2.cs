// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveStigma2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveStigma2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveStigma2</summary>

public class PassiveAbility_ShopPassiveStigma2 : PassiveAbilityBase
{
  public override string debugDesc => "전투 책장으로 적에게 화상을 부여하면 4막 후에 부여한 수치만큼 다시 화상을 부여함";

  public override int OnGiveKeywordBufByCard(BattleUnitBuf buf, int stack, BattleUnitModel target)
  {
    target.bufListDetail.AddBuf((BattleUnitBuf) new PassiveAbility_ShopPassiveStigma2.Stigma2Buf(stack));
    return base.OnGiveKeywordBufByCard(buf, stack, target);
  }

  /// <summary>Stigma2Buf</summary>

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
