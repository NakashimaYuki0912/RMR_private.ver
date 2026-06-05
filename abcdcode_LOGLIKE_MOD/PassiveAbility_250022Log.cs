// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_250022Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using LOR_XML;
using System;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_250022Log : PassiveAbilityBase
{
  public int addedDice = 2;
  public int _egoCondition = 350;
  public bool _egoReady;
  public int _egoCardCount;
  public int _awakenCount;
  public int _dmgReduction;

  public override void OnWaveStart()
  {
    int num1 = this.owner.UnitData.floorBattleData.param2;
    int num2 = this.owner.UnitData.floorBattleData.param3;
    if (num1 > 0)
      this._awakenCount = 0;
    this._egoCardCount = 0;
    this.InitDeck();
  }

  public void StartEgoCount() => this._awakenCount = 2;

  public void InitDeck()
  {
    int num1 = this.owner.UnitData.floorBattleData.param2;
    int num2 = this.owner.UnitData.floorBattleData.param3;
    this.owner.allyCardDetail.ExhaustAllCards();
    List<LorId> lorIdList = new List<LorId>();
    for (int index = 0; index < 8; ++index)
    {
      lorIdList.Add(new LorId(LogLikeMod.ModId, 607003));
      lorIdList.Add(new LorId(LogLikeMod.ModId, 607004));
      lorIdList.Add(new LorId(LogLikeMod.ModId, 607005));
    }
    this.owner.allyCardDetail.SetMaxHand(20);
    this.owner.allyCardDetail.SetMaxDrawHand(14);
    foreach (LorId cardId in lorIdList)
      this.owner.allyCardDetail.AddNewCardToDeck(cardId);
    for (int index = 0; index < 3; ++index)
      this.owner.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 607002));
    for (int index = 0; index < 3; ++index)
      this.owner.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 607007));
    this.owner.allyCardDetail.AddNewCardToDeck(new LorId(LogLikeMod.ModId, 607006));
    int num3 = 0;
    while (num3 < 4)
      ++num3;
    this.owner.allyCardDetail.DrawCards(5);
  }

  public override int SpeedDiceNumAdder() => this.addedDice;

  public override void OnRoundStart()
  {
    this.UpdateResist();
    int roundTurn = Singleton<StageController>.Instance.RoundTurn;
    int num1 = this.owner.UnitData.floorBattleData.param2;
    int num2 = this.owner.UnitData.floorBattleData.param3;
    if (num1 > 0 && !this.HasEgoPassive())
    {
      if (this._awakenCount <= 0)
        this.AwakenEgo();
      else
        --this._awakenCount;
    }
    if (this.HasEgoPassive())
    {
      if (this._egoCardCount <= 0)
      {
        if (this.owner.allyCardDetail.GetHand().FindAll((Predicate<BattleDiceCardModel>) (x => x.GetID() == new LorId(LogLikeMod.ModId, 607001))).Count == 0)
        {
          this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 607001)).AddBuf((BattleDiceCardBuf) new PassiveAbility_250022Log.BattleDiceCardBuf_temporary());
          this._egoCardCount = 2;
        }
      }
      else
        --this._egoCardCount;
    }
    else
      this.owner.allyCardDetail.ExhaustCard(new LorId(LogLikeMod.ModId, 607001));
    this.addedDice = Mathf.Min(3, roundTurn);
  }

  public override void OnRoundEnd()
  {
    this.owner.allyCardDetail.DrawCards(1);
    this.owner.cardSlotDetail.RecoverPlayPoint(1);
  }

  public override void OnRoundEndTheLast() => this.TryEgo();

  public bool HasEgoPassive() => this.owner.passiveDetail.HasPassive<PassiveAbility_250422Log>();

  public void TryEgo()
  {
    int num1 = this.owner.UnitData.floorBattleData.param1;
    int num2 = this.owner.UnitData.floorBattleData.param2;
    if (this.owner.UnitData.floorBattleData.param3 > 0 || num2 > 0)
      return;
    if (num1 >= 5)
    {
      this.AwakenEgo();
    }
    else
    {
      if (!this._egoReady)
        return;
      this.AwakenEgo();
    }
  }

  public void AwakenEgo()
  {
    if (this.owner.UnitData.floorBattleData.param2 <= 0)
    {
      if (this.owner.faction == Faction.Enemy)
      {
        SephirahType currentFloor = Singleton<StageController>.Instance.CurrentFloor;
        BattleUnitView view1 = this.owner.view;
        int num = UnityEngine.Random.Range(0, 2);
        string id1 = "SPECIAL_EVENT_" + num.ToString();
        view1.DisplayDlg(DialogType.SPECIAL_EVENT, id1);
        switch (currentFloor)
        {
          case SephirahType.Gebura:
            BattleUnitModel battleUnitModel1 = BattleObjectManager.instance.GetAliveList(Faction.Player).Find((Predicate<BattleUnitModel>) (x => x.UnitData.unitData.defaultBook.GetBookClassInfoId() == 6));
            if (battleUnitModel1 != null)
            {
              BattleUnitView view2 = battleUnitModel1.view;
              num = UnityEngine.Random.Range(0, 2);
              string id2 = "SPECIAL_EVENT_" + num.ToString();
              view2.DisplayDlg(DialogType.SPECIAL_EVENT, id2);
              break;
            }
            break;
          case SephirahType.Binah:
            BattleUnitModel battleUnitModel2 = BattleObjectManager.instance.GetAliveList(Faction.Player).Find((Predicate<BattleUnitModel>) (x => x.UnitData.unitData.defaultBook.GetBookClassInfoId() == 8));
            if (battleUnitModel2 != null)
            {
              battleUnitModel2.view.DisplayDlg(DialogType.SPECIAL_EVENT, "SPECIAL_EVENT_0");
              break;
            }
            break;
          case SephirahType.Keter:
            BattleObjectManager.instance.GetAliveList(Faction.Player).Find((Predicate<BattleUnitModel>) (x => x.UnitData.unitData.defaultBook.GetBookClassInfoId() == 10))?.view.DisplayDlg(DialogType.SPECIAL_EVENT, "SPECIAL_EVENT_0");
            break;
        }
      }
      this.owner.UnitData.floorBattleData.param2 = 1;
      this.owner.RecoverBreakLife(1);
      this.owner.ResetBreakGauge();
      this.owner.breakDetail.nextTurnBreak = false;
    }
    this.owner.passiveDetail.AddPassive((PassiveAbilityBase) new PassiveAbility_250422Log());
  }

  public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
  {
    this._dmgReduction = 0;
    if (this.owner.UnitData.floorBattleData.param2 <= 0 && (double) this.owner.hp - (double) dmg <= (double) this._egoCondition)
    {
      this._dmgReduction = (int) ((double) this._egoCondition - ((double) this.owner.hp - (double) dmg));
      this._egoReady = true;
    }
    return base.BeforeTakeDamage(attacker, dmg);
  }

  public override int GetDamageReductionAll()
  {
    int damageReductionAll;
    if (this.owner.UnitData.floorBattleData.param2 <= 0 && (double) this.owner.hp <= (double) this._egoCondition)
    {
      damageReductionAll = 9999;
      this._egoReady = true;
    }
    else
      damageReductionAll = this._dmgReduction;
    return damageReductionAll;
  }

  public void UpdateResist()
  {
    BehaviourDetail detail = RandomUtil.SelectOne<BehaviourDetail>(BehaviourDetail.Slash, BehaviourDetail.Penetrate, BehaviourDetail.Hit);
    if (!this.HasEgoPassive())
      return;
    this.owner.Book.SetResistHP(BehaviourDetail.Slash, AtkResist.Endure);
    this.owner.Book.SetResistHP(BehaviourDetail.Penetrate, AtkResist.Endure);
    this.owner.Book.SetResistHP(BehaviourDetail.Hit, AtkResist.Endure);
    this.owner.Book.SetResistBP(BehaviourDetail.Slash, AtkResist.Endure);
    this.owner.Book.SetResistBP(BehaviourDetail.Penetrate, AtkResist.Endure);
    this.owner.Book.SetResistBP(BehaviourDetail.Hit, AtkResist.Endure);
    this.owner.Book.SetResistHP(detail, AtkResist.Normal);
    this.owner.Book.SetResistBP(detail, AtkResist.Normal);
  }

  public class BattleDiceCardBuf_temporary : BattleDiceCardBuf
  {
    public override void OnUseCard(BattleUnitModel owner) => this._card.exhaust = true;
  }
}
}
