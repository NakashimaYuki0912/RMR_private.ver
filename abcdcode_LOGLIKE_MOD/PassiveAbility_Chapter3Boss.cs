// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_Chapter3Boss
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_Chapter3Boss.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_Chapter3Boss</summary>

public class PassiveAbility_Chapter3Boss : PassiveAbilityBase
{
  public override string debugDesc => "감정 단계가 2 이상일때 막 시작 시 책장을 1장 추가로 뽑고 속도 주사위 + 1";

  public override void OnRoundStart()
  {
    base.OnRoundStart();
    if (this.owner.emotionDetail.EmotionLevel < 2)
      return;
    this.owner.allyCardDetail.DrawCards(1);
  }

  public override int SpeedDiceNumAdder()
  {
    return this.owner.emotionDetail.EmotionLevel >= 2 ? 1 : base.SpeedDiceNumAdder();
  }
}
}
