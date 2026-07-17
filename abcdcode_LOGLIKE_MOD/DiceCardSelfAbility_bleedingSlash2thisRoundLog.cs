// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_bleedingSlash2thisRoundLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_bleedingSlash2thisRoundLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_bleedingSlash2thisRoundLog</summary>

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

  /// <summary>BleedingSlash2thisRoundBuf</summary>

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
