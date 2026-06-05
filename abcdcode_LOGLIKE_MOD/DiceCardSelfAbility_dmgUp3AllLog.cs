// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_dmgUp3AllLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_dmgUp3AllLog : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    List<BattleUnitModel> battleUnitModelList = new List<BattleUnitModel>();
    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction))
      alive.bufListDetail.AddKeywordBufByCard(KeywordBuf.DmgUp, 3, this.owner);
  }
}
}
