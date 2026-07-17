// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_150039Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_150039Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_150039Log</summary>

public class PassiveAbility_150039Log : PassiveAbilityBase
{
  public bool _speedDiceAdded;

  public override int SpeedDiceNumAdder() => this._speedDiceAdded ? 3 : 2;

  public override void OnWaveStart()
  {
  }

  public override void OnRoundStart() => this.owner.emotionDetail.SetMaxEmotionLevel(0);

  public void SetCardsVer1(int patternCount, bool speedDiceadded)
  {
    speedDiceadded = false;
    this.owner.allyCardDetail.ExhaustAllCards();
    switch (patternCount)
    {
      case 0:
        this.AddNewCard(611021);
        this.AddNewCard(611023);
        if (Singleton<StageController>.Instance.RoundTurn > 1)
        {
          this.AddNewCard(611022);
          break;
        }
        break;
      case 1:
        this.AddNewCard(611002);
        this.AddNewCard(611023);
        this.AddNewCard(611021);
        break;
      case 2:
        this.AddNewCard(611023);
        this.AddNewCard(611022);
        this.AddNewCard(611023);
        break;
      case 3:
        this.AddNewCard(611021);
        this.AddNewCard(611022);
        this.AddNewCard(611003);
        break;
    }
    if (!speedDiceadded)
      return;
    this._speedDiceAdded = true;
    this.AddNewCard(RandomUtil.SelectOne<int>(611021, 611022));
  }

  public void SetCardsVer2(int patternCount, bool speedDiceadded)
  {
    speedDiceadded = false;
    this.owner.allyCardDetail.ExhaustAllCards();
    switch (patternCount)
    {
      case 0:
        this.AddNewCard(611022);
        this.AddNewCard(611023);
        if (Singleton<StageController>.Instance.RoundTurn > 1)
        {
          this.AddNewCard(611021);
          break;
        }
        break;
      case 1:
        this.AddNewCard(611021);
        this.AddNewCard(611022);
        this.AddNewCard(611023);
        break;
      case 2:
        this.AddNewCard(611021);
        this.AddNewCard(611023);
        this.AddNewCard(611023);
        break;
      case 3:
        this.AddNewCard(611003);
        this.AddNewCard(611022);
        this.AddNewCard(611023);
        break;
    }
    if (!speedDiceadded)
      return;
    this._speedDiceAdded = true;
    this.AddNewCard(RandomUtil.SelectOne<int>(611021, 611022));
  }

  public void AddNewCard(int id)
  {
    this.owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, id))?.SetCostToZero();
  }

  public override bool OnBreakGageZero() => true;
}
}
