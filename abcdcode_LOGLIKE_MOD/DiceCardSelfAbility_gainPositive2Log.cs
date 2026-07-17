// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_gainPositive2Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_gainPositive2Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_gainPositive2Log</summary>

public class DiceCardSelfAbility_gainPositive2Log : DiceCardSelfAbilityBase
{
  public override void OnUseCard()
  {
    this.owner.battleCardResultLog?.SetUseCardEvent(new BattleCardBehaviourResult.BehaviourEvent(this.PrintEffect));
  }

  public void PrintEffect()
  {
    SingletonBehavior<BattleManagerUI>.Instance.ui_battleEmotionCoinUI.OnAcquireCoin(this.owner, EmotionCoinType.Positive, this.owner.emotionDetail.CreateEmotionCoin(EmotionCoinType.Positive, 2));
  }
}
}
