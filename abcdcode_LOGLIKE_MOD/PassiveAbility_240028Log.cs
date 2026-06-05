// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_240028Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;

 
namespace abcdcode_LOGLIKE_MOD {

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
