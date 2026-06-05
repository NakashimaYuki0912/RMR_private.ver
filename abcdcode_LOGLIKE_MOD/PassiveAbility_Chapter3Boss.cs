// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_Chapter3Boss
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

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
