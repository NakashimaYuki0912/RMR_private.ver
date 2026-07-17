// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ShopPassiveUnion6
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_ShopPassiveUnion6.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_ShopPassiveUnion6</summary>

public class PassiveAbility_ShopPassiveUnion6 : UnionWeaponPassiveAbilityBase
{
  public int count;

  public override string debugDesc
  {
    get => "(다른 생체무기와 중복 불가) 막 시작 시 체력을 5% 잃음. 근접 책장으로 일방공격을 당할 시 합을 즉시 종료시킴(막당 최대 2회)";
  }

  public override void OnRoundStartAfter()
  {
    base.OnRoundStartAfter();
    this.count = 2;
  }

  public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel attackerCard)
  {
    base.OnStartTargetedOneSide(attackerCard);
    if (this.count <= 0 || attackerCard.card.GetSpec().Ranged != CardRange.Near)
      return;
    attackerCard.cardBehaviorQueue.Clear();
    --this.count;
  }
}
}
