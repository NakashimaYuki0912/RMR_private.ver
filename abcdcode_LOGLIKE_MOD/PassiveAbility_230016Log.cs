// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_230016Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_230016Log : PassiveAbilityBase
{
  public int _patternCount;
  public bool _is2phase;
  public int _lastCardId = -1;
  public bool _bPhase2Initialized;

  public override string debugDesc => "토머리 전용";

  public bool is2Phase => this._is2phase;

  public override void OnWaveStart()
  {
    this.owner.Book.SetOriginalResists();
    this._patternCount = 0;
    this.UpdatePhase();
    if (!this._is2phase)
      this.Execute1PhasePattern();
    else
      this.Execute2PhasePattern();
  }

  public void Execute1PhasePattern()
  {
    if (this._patternCount <= 2)
    {
      if (this.owner.breakDetail.nextTurnBreak)
        return;
      List<int> list = new List<int>();
      list.Add(405011);
      list.Add(405013);
      if (BattleObjectManager.instance.GetAliveList(this.owner.faction).Count >= 2 && this._patternCount <= 1)
        list.Add(405012);
      if (list.Contains(this._lastCardId))
        list.Remove(this._lastCardId);
      this.owner.allyCardDetail.ExhaustAllCards();
      for (int index = 0; index < 2 && list.Count != 0; ++index)
      {
        int id = RandomUtil.SelectOne<int>(list);
        this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, id))?.SetCostToZero();
        list.Remove(id);
      }
      ++this._patternCount;
    }
    else if (this._patternCount == 3)
    {
      if (this.owner.breakDetail.nextTurnBreak)
        return;
      this.StartEffect();
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction))
      {
        if (alive != this.owner)
        {
          alive.breakDetail.TakeBreakDamage(50, DamageType.Passive);
          alive.breakDetail.nextTurnBreak = true;
        }
      }
      this.owner.Book.SetResistBP(BehaviourDetail.Slash, AtkResist.Weak);
      this.owner.Book.SetResistBP(BehaviourDetail.Penetrate, AtkResist.Weak);
      this.owner.Book.SetResistBP(BehaviourDetail.Hit, AtkResist.Weak);
      this.owner.allyCardDetail.ExhaustAllCards();
      this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 405009))?.SetCostToZero();
      ++this._patternCount;
    }
    else
    {
      this._patternCount = 0;
      if (this.owner.breakDetail.nextTurnBreak)
        return;
      this.owner.allyCardDetail.ExhaustAllCards();
      for (int index = 0; index < 2; ++index)
        this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 405010))?.SetCostToZero();
    }
  }

  public void Execute2PhasePattern()
  {
    this.owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this.owner);
    this.owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, 2, this.owner);
    if (this.owner.breakDetail.nextTurnBreak)
      return;
    bool flag = (double) this.owner.hp > (double) this.owner.MaxHp * 0.25;
    this.owner.allyCardDetail.ExhaustAllCards();
    this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 405014))?.SetCostToZero();
    (!flag ? this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 405015)) : this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 405016)))?.SetCostToZero();
    this._patternCount = 0;
  }

  public void UpdatePhase()
  {
    if (this._is2phase || (double) this.owner.hp > (double) this.owner.MaxHp * 0.5)
      return;
    this._patternCount = 0;
    this._is2phase = true;
  }

  public override void OnRoundStart()
  {
    this.Log("ActivePassive");
    int emotionTotalCoinNumber = Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionTotalCoinNumber;
    Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionTotalBonus = emotionTotalCoinNumber + 1;
    this.Log("ActivePassive2");
    if (!this._bPhase2Initialized && this._is2phase)
    {
      this.owner.view.ChangeSkin("TomerryPhase2");
      this.owner.UnitData.unitData.InitBattleDialogByDefaultBook(new LorId(LogLikeMod.ModId, 130017));
      UnityEngine.Object original = Resources.Load("Prefabs/Battle/SpecialEffect/TomerryChangeAppearanceEffect");
      SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Battle/Tomary_Phase2");
      if (original !=  null)
        (UnityEngine.Object.Instantiate(original, this.owner.view.charAppearance.transform) as GameObject).transform.localPosition = new Vector3(0.0f, 3f, 0.0f);
      this._bPhase2Initialized = true;
    }
    if (this.owner.allyCardDetail.GetHand().Exists((Predicate<BattleDiceCardModel>) (x => x.GetID() == 405010)))
      this.owner.view.charAppearance.ChangeMotion(ActionDetail.Move);
    foreach (BattleDiceCardModel battleDiceCardModel in this.owner.allyCardDetail.GetHand())
      battleDiceCardModel.temporary = true;
  }

  public override void OnRoundEnd()
  {
    this.UpdatePhase();
    this.owner.Book.SetOriginalResists();
    if (!this._is2phase)
      this.Execute1PhasePattern();
    else
      this.Execute2PhasePattern();
  }

  public void StartEffect()
  {
    BattleCamManager instance1 = SingletonBehavior<BattleCamManager>.Instance;
    CameraFilterPack_Blur_Radial r =  instance1 !=  null ? instance1.AddCameraFilter<CameraFilterPack_Blur_Radial>() : (CameraFilterPack_Blur_Radial) null;
    BattleCamManager instance2 = SingletonBehavior<BattleCamManager>.Instance;
    if ( instance2 ==  null)
      return;
    instance2.StartCoroutine(this.Pinpong(r));
  }

  public IEnumerator Pinpong(CameraFilterPack_Blur_Radial r)
  {
    float elapsedTime = 0.0f;
    while ((double) elapsedTime < 1.0)
    {
      elapsedTime += Time.deltaTime;
      r.Intensity = Mathf.PingPong(Time.time, 0.05f);
      yield return  new WaitForEndOfFrame();
    }
    BattleCamManager instance = SingletonBehavior<BattleCamManager>.Instance;
    if ( instance !=  null)
      instance.RemoveCameraFilter<CameraFilterPack_Blur_Radial>();
  }
}
}
