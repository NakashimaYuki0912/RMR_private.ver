// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardAbility_usett_eyeLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardAbility_usett_eyeLog : DiceCardAbilityBase
{
  public override string[] Keywords
  {
    get => new string[1]{ "Protection_Keyword" };
  }

  public override void OnSucceedAttack()
  {
    base.OnSucceedAttack();
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction))
    {
      alive.bufListDetail.AddKeywordBufByCard(KeywordBuf.Protection, 2, this.owner);
      alive.bufListDetail.AddKeywordBufByCard(KeywordBuf.BreakProtection, 2, this.owner);
    }
  }
}
}
