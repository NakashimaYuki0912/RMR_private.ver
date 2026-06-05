// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_150051Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using UI;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_150051Log : PassiveAbilityBase
{
  public const int powerDownCardId = 611024;
  public const int hpLine = 300;
  public const int handStunLine = 30;
  public int _patternCountPhase1;
  public int _patternCountPhase2;
  public int _patternCountPhase3;
  public bool _unionState;
  public int _currentPhase = 1;
  public int _areaCool = 6;
  public List<BattleUnitModel> _powerDownHands = new List<BattleUnitModel>();
  public List<BattleUnitModel> _noPowerDownHands = new List<BattleUnitModel>();

  public override bool isTargetable => base.isTargetable;

  public override int SpeedDiceNumAdder()
  {
    int num = 2;
    int currentPhase = this._currentPhase;
    return num;
  }

  public override void OnRoundStart()
  {
    this._currentPhase = !this._unionState ? ((double) this.owner.hp <= 300.0 ? 3 : 1) : 2;
    if (this._currentPhase == 1)
      return;
    if (this._currentPhase == 2)
    {
      if (this._areaCool > 1)
        this._areaCool = 1;
    }
    else if (this._currentPhase == 3 && this._areaCool > 1)
      this._areaCool = 1;
  }

  public override void OnRoundStartAfter()
  {
    this.owner.allyCardDetail.ExhaustAllCards();
    if (this._currentPhase != 2)
      this.SetCardsInHands();
    this.SetCards();
    if (this._currentPhase == 1)
      ++this._patternCountPhase1;
    else if (this._currentPhase == 2)
    {
      ++this._patternCountPhase2;
    }
    else
    {
      if (this._currentPhase != 3)
        return;
      ++this._patternCountPhase3;
    }
  }

  public override void OnRoundEnd() => --this._areaCool;

  public override void OnRoundEndTheLast()
  {
    if (this.CanJoin())
    {
      this.Join();
    }
    else
    {
      if (!this.CanSplit())
        return;
      this.Split();
    }
  }

  public void SetCards()
  {
    if (this._currentPhase == 1)
    {
      this.AddNewCard(611031);
      this.AddNewCard(611032);
      this.AddNewCard(611033);
    }
    else if (this._currentPhase == 2)
    {
      this.AddNewCard(611035);
      this.AddNewCard(611035);
      this.AddNewCard(611035);
      this.AddNewCard(611035);
    }
    else if (this._currentPhase == 3)
    {
      if (this.ExistsAliveHands())
      {
        this.AddNewCard(611031);
        this.AddNewCard(611032);
        if (this.ExistsDeadHand())
          this.AddNewCard(611034);
        else
          this.AddNewCard(611032);
        this.AddNewCard(611033);
      }
      else
      {
        this.AddNewCard(611036);
        this.AddNewCard(611034);
        this.AddNewCard(611034);
      }
    }
    if (this._areaCool <= 0)
    {
      this.AddNewCard(611001);
      this._areaCool = 2;
    }
    BattleUnitBufListDetail bufListDetail = this.owner.bufListDetail;
    PassiveAbility_150051Log.BattleUnitBuf_yanAreaReady buf = new PassiveAbility_150051Log.BattleUnitBuf_yanAreaReady();
    buf.stack = this._areaCool;
    bufListDetail.AddBuf((BattleUnitBuf) buf);
  }

  public void SetDefenseCards()
  {
  }

  public void SetCardsInHands()
  {
    this.owner.emotionDetail.SpeedDiceNumAdder();
    List<PassiveAbility_150039Log> handPassiveList = this.GetHandPassiveList();
    int num = 0;
    foreach (PassiveAbility_150039Log ability150039Log in handPassiveList)
    {
      if (this._currentPhase == 1)
      {
        if (num == 0)
          ability150039Log.SetCardsVer1(this._patternCountPhase1 % 4, false);
        else
          ability150039Log.SetCardsVer2(this._patternCountPhase1 % 4, false);
      }
      else if (num == 0)
        ability150039Log.SetCardsVer1(this._patternCountPhase3 % 2 * 2 + 1, false);
      else
        ability150039Log.SetCardsVer2(this._patternCountPhase3 % 2 * 2 + 1, false);
      ++num;
    }
  }

  public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
  {
    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
    aliveList.RemoveAll((Predicate<BattleUnitModel>) (x => x == this.owner));
    List<BattleUnitModel> list = new List<BattleUnitModel>((IEnumerable<BattleUnitModel>) aliveList);
    if (this.IsPowerCard(card.GetID()))
    {
      aliveList.RemoveAll((Predicate<BattleUnitModel>) (x => (double) x.hp <= 30.0));
      if (aliveList.Count > 0)
        return RandomUtil.SelectOne<BattleUnitModel>(aliveList);
      if (list.Count > 0)
        return RandomUtil.SelectOne<BattleUnitModel>(list);
    }
    else if (!this.IsNullifyCard(card.GetID()))
    {
      if (this.IsRecoverCard(card.GetID()))
      {
        aliveList.RemoveAll((Predicate<BattleUnitModel>) (x => (double) x.hp > 50.0));
        if (aliveList.Count > 0)
          return RandomUtil.SelectOne<BattleUnitModel>(aliveList);
        if (list.Count > 0)
          return RandomUtil.SelectOne<BattleUnitModel>(list);
      }
      else if (card.GetID() == new LorId(LogLikeMod.ModId, 611032))
      {
        aliveList.RemoveAll((Predicate<BattleUnitModel>) (x => (double) x.hp <= 30.0));
        if (aliveList.Count > 0)
          return RandomUtil.SelectOne<BattleUnitModel>(aliveList);
        if (list.Count > 0)
          return RandomUtil.SelectOne<BattleUnitModel>(list);
      }
    }
    return base.ChangeAttackTarget(card, idx);
  }

  public bool IsPowerCard(LorId cardId) => cardId == new LorId(LogLikeMod.ModId, 611031);

  public bool IsNullifyCard(LorId cardId) => cardId == new LorId(LogLikeMod.ModId, 611031);

  public bool IsRecoverCard(LorId cardId) => cardId == new LorId(LogLikeMod.ModId, 611031);

  public void AddNewCard(int id)
  {
    this.owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, id))?.SetCostToZero();
  }

  public void Join()
  {
    foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(this.owner.faction).FindAll((Predicate<BattleUnitModel>) (x => x.passiveDetail.HasPassive<PassiveAbility_150039Log>())))
      battleUnitModel.isRegister = false;
    this._unionState = true;
    this.ChangeSkin("Yan_Union");
  }

  public void Split()
  {
    BattleObjectManager.instance.GetAliveList(Faction.Enemy);
    BattleUnitModel battleUnitModel1 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, new LorId(LogLikeMod.ModId, 50039), this.owner.index + 1);
    BattleUnitModel battleUnitModel2 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, new LorId(LogLikeMod.ModId, 50040), this.owner.index + 2);
    battleUnitModel1?.SetHp(80 /*0x50*/);
    battleUnitModel2?.SetHp(80 /*0x50*/);
    this._unionState = false;
    this.owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
    this.owner.RecoverBreakLife(1);
    this.owner.ResetBreakGauge();
    this.owner.breakDetail.nextTurnBreak = false;
    this.ChangeSkin("Yan_Distort");
    int num = 0;
    foreach (BattleUnitModel battleUnitModel3 in (IEnumerable<BattleUnitModel>) BattleObjectManager.instance.GetList())
      SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel3.UnitData.unitData, num++);
    BattleObjectManager.instance.InitUI();
  }

  public bool CanJoin()
  {
    return !this._unionState && this.ExistsHands() && !this.ExistsAliveHands() && (double) this.owner.hp > 300.0;
  }

  public bool CanSplit()
  {
    return this._unionState && !this.ExistsHands() && (double) this.owner.hp <= 300.0;
  }

  public bool ExistsHands()
  {
    return BattleObjectManager.instance.GetAliveList(this.owner.faction).FindAll((Predicate<BattleUnitModel>) (x => x.passiveDetail.HasPassive<PassiveAbility_150039Log>())).Count > 0;
  }

  public bool ExistsAliveHands()
  {
    return BattleObjectManager.instance.GetAliveList(this.owner.faction).FindAll((Predicate<BattleUnitModel>) (x => x.passiveDetail.HasPassive<PassiveAbility_150039Log>())).FindAll((Predicate<BattleUnitModel>) (x => (double) x.hp > 30.0)).Count > 0;
  }

  public bool ExistsDeadHand()
  {
    return BattleObjectManager.instance.GetAliveList(this.owner.faction).FindAll((Predicate<BattleUnitModel>) (x => x.passiveDetail.HasPassive<PassiveAbility_150039Log>())).FindAll((Predicate<BattleUnitModel>) (x => (double) x.hp <= 30.0)).Count > 0;
  }

  public List<BattleUnitModel> GetHandList()
  {
    return BattleObjectManager.instance.GetAliveList(this.owner.faction).FindAll((Predicate<BattleUnitModel>) (x => x.passiveDetail.HasPassive<PassiveAbility_150039Log>()));
  }

  public List<PassiveAbility_150039Log> GetHandPassiveList()
  {
    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
    List<PassiveAbility_150039Log> handPassiveList = new List<PassiveAbility_150039Log>();
    foreach (BattleUnitModel battleUnitModel in aliveList)
    {
      if (battleUnitModel.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>) (x => x is PassiveAbility_150039Log)) is PassiveAbility_150039Log ability150039Log)
        handPassiveList.Add(ability150039Log);
    }
    return handPassiveList;
  }

  public void ChangeSkin(string skinName)
  {
    int height = 170;
    this.owner.view.ChangeSkin(skinName);
    EnemyUnitClassInfo data = Singleton<EnemyUnitClassInfoList>.Instance.GetData(this.owner.UnitData.unitData.EnemyUnitId);
    if (data != null)
      height = data.height;
    switch (skinName)
    {
      case "Yan_Distort":
        this.owner.view.ChangeSkin(skinName);
        this.owner.view.ChangeHeight(height);
        break;
      case "Yan_Union":
        this.owner.view.ChangeSkin(skinName);
        this.owner.view.ChangeHeight((int) ((double) height * 1.5));
        break;
      default:
        Util.DebugEditorLog( ("Not fount skin : " + skinName));
        break;
    }
  }

  public class BattleUnitBuf_yanAreaReady : BattleUnitBuf
  {
    public override string keywordId => "YanArea_Ready";

    public override void OnRoundEnd() => this.Destroy();
  }
}
}
