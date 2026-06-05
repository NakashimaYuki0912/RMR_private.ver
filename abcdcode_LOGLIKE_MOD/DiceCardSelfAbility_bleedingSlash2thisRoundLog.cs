// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_bleedingSlash2thisRoundLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardSelfAbility_bleedingSlash2thisRoundLog : DiceCardSelfAbilityBase
{
  public override string[] Keywords
  {
    get
    {
      return new string[2]
      {
        "bstart_Keyword",
        "Bleeding_Keyword"
      };
    }
  }

  public override void OnStartBattle()
  {
    this.owner.bufListDetail.AddBuf((BattleUnitBuf) new DiceCardSelfAbility_bleedingSlash2thisRoundLog.BleedingSlash2thisRoundBuf());
  }

  public class BleedingSlash2thisRoundBuf : BattleUnitBuf
  {
    public override void OnSuccessAttack(BattleDiceBehavior behavior)
    {
      if (behavior.Detail != BehaviourDetail.Slash || behavior.card.target == null)
        return;
      behavior.card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 2, this._owner);
    }

    public override void OnRoundEnd() => this.Destroy();
  }
}
}
