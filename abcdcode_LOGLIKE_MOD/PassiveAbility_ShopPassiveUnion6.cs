// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassiveUnion6
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

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
