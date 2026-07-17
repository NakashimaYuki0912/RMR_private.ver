// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveUnion5
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveUnion5.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveUnion5</summary>

public class PassiveAbility_ShopPassiveUnion5 : UnionWeaponPassiveAbilityBase
{
  public BattleDiceBehavior last;

  public override string debugDesc
  {
    get => "(다른 생체무기와 중복 불가) 막 시작 시 체력을 5% 잃음. 합 패배 시 현재 주사위를 재사용(주사위 당 1번)";
  }

  public override void OnLoseParrying(BattleDiceBehavior behavior)
  {
    base.OnLoseParrying(behavior);
    if (this.last == behavior)
      return;
    behavior.isBonusAttack = true;
    this.last = behavior;
  }
}
}
