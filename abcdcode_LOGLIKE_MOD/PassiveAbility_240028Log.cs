// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_240028Log
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PassiveAbility_240028Log.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Passive ability: PassiveAbility_240028Log</summary>

public class PassiveAbility_240028Log : PassiveAbilityBase
{
  public int _diceAdder = 1;
  public int _patternCount;

  public override int SpeedDiceNumAdder() => this._diceAdder;

  public override void OnRoundStart()
  {
    this.SetCards();
    ++this._patternCount;
    if (!this.owner.allyCardDetail.GetHand().Exists((Predicate<BattleDiceCardModel>) (x => x.GetID() == new LorId(LogLikeMod.ModId, 508001))))
      return;
    MapManager currentMapObject = SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject;
    if (currentMapObject is CryingChildMapManager)
      (currentMapObject as CryingChildMapManager).PlayAreaLaserSound();
  }

  public void SetCards()
  {
    this.owner.allyCardDetail.ExhaustAllCards();
    if (!(Singleton<StageController>.Instance.EnemyStageManager is EnemyTeamStageManager_TheCryingLog enemyStageManager))
      return;
    if (enemyStageManager.Stack >= 5)
    {
      this._diceAdder = 2;
      this.AddNewCard(508003);
      this.AddNewCard(508004);
      if ((double) RandomUtil.valueForProb < 0.5)
        this.AddNewCard(508003);
      else
        this.AddNewCard(508004);
    }
    else if (enemyStageManager.Stack >= 3)
    {
      this._diceAdder = 3;
      this.AddNewCard(508002);
      this.AddNewCard(508003);
      this.AddNewCard(508004);
      this.AddNewCard(508005);
    }
    else
    {
      this._diceAdder = 4;
      this.AddNewCard(508001);
      this.AddNewCard(508002);
      this.AddNewCard(508002);
      if ((double) RandomUtil.valueForProb < 0.5)
        this.AddNewCard(508003);
      else
        this.AddNewCard(508004);
      this.AddNewCard(508005);
    }
  }

  public void AddNewCard(int id)
  {
    this.owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, id))?.SetCostToZero();
  }
}
}
