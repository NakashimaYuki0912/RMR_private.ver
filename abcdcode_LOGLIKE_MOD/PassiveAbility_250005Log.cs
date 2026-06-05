// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_250005Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_250005Log : PassiveAbilityBase
{
  public int _count;

  public override void OnSucceedAttack(BattleDiceBehavior behavior)
  {
    if (behavior.card.card.GetSpec().Ranged != CardRange.Near)
      return;
    ++this._count;
    if (this._count >= 3)
    {
      this.owner.allyCardDetail.AddNewCard(ThumbBulletClassLog.GetRandomBulletId());
      this._count = 0;
    }
  }
}
}
