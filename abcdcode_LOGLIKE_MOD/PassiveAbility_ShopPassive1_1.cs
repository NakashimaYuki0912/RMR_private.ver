// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassive1_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassive1_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassive1_1</summary>

public class PassiveAbility_ShopPassive1_1 : PassiveAbilityBase
{
  public override string debugDesc => "출혈이 부여된 적을 처치하면 추가로 1안과 뜬소문 전투책장 보상 1개를 획득함";

  public override void OnKill(BattleUnitModel target)
  {
    base.OnKill(target);
    if (!target.bufListDetail.HasBuf<BattleUnitBuf_bleeding>())
      return;
    PassiveAbility_MoneyCheck.AddMoney(1);
    LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 1001)));
  }
}
}
