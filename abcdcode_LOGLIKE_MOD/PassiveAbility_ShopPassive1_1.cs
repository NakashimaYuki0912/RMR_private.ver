// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_ShopPassive1_1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

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
