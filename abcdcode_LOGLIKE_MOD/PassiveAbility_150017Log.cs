// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_150017Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_150017Log : PassiveAbilityBase
{
  public int _patternCount;
  public bool pattern1;
  public List<LorId> _usedCardType = new List<LorId>();
  public int _onlyCardCooltime;

  public override void OnWaveStart()
  {
    this.pattern1 = false;
    this._usedCardType = new List<LorId>();
  }

  public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
  {
    LorId id = curCard.card.GetID();
    if (this._usedCardType.Contains(id))
      return;
    this._usedCardType.Add(id);
  }

  public LorId GetReleaseCard()
  {
    if (this._usedCardType.Contains(new LorId(LogLikeMod.ModId, 605009)))
      return new LorId(LogLikeMod.ModId, 605010);
    return this._usedCardType.Contains(new LorId(LogLikeMod.ModId, 605008)) ? new LorId(LogLikeMod.ModId, 605009) : new LorId(LogLikeMod.ModId, 605008);
  }

  public override int SpeedDiceNumAdder() => 1;

  public override void OnRoundStartAfter()
  {
    if (this.owner.IsBreakLifeZero())
      return;
    this.SetCards();
    ++this._patternCount;
    if (this._patternCount < 4)
      return;
    this._patternCount = 0;
  }

  public void SetCards()
  {
    this.owner.allyCardDetail.ExhaustAllCards();
    bool flag = this.CheckOtherNamed();
    if (TheIndexCardUtilLog.IsActivatedRelease(this.owner))
    {
      --this._onlyCardCooltime;
      if (this._onlyCardCooltime <= 0)
      {
        this.AddNewCard(new LorId(LogLikeMod.ModId, 605001));
        this._onlyCardCooltime = 4;
      }
      if (flag && this._onlyCardCooltime >= 3)
        this._onlyCardCooltime = 2;
    }
    if (this.CheckOtherNamed() || !TheIndexCardUtilLog.IsActivatedRelease(this.owner))
    {
      if (this._patternCount == 0)
      {
        if (!this.pattern1)
        {
          this.AddNewCard(new LorId(LogLikeMod.ModId, 605006));
          this.AddNewCard(this.GetReleaseCard());
          this.pattern1 = true;
        }
        else
          ++this._patternCount;
      }
      if (this._patternCount == 1)
      {
        this.AddNewCard(new LorId(LogLikeMod.ModId, 605022));
        this.AddNewCard(this.GetReleaseCard());
      }
      if (this._patternCount == 2)
      {
        this.AddNewCard(new LorId(LogLikeMod.ModId, 605021));
        this.AddNewCard(new LorId(LogLikeMod.ModId, 605007));
      }
      if (this._patternCount == 3)
      {
        this.AddNewCard(new LorId(LogLikeMod.ModId, 605007));
        this.AddNewCard(TheIndexCardUtilLog.GetCardIdByBuf(this.owner));
      }
    }
    else
    {
      this.AddNewCard(new LorId(LogLikeMod.ModId, 605021));
      this.AddNewCard(TheIndexCardUtilLog.GetCardIdByBuf(this.owner));
    }
    int num = 0;
    List<SpeedDice> speedDiceList = this.owner.Book.GetSpeedDiceRule(this.owner).Roll(this.owner);
    if (speedDiceList.Count >= 3)
      num = speedDiceList.Count - 2;
    if (num <= 0)
      return;
    this.AddNewCard(TheIndexCardUtilLog.GetAddedCardIdByBuf(this.owner));
  }

  public override void OnRoundEndTheLast()
  {
  }

  public void AddNewCard(LorId id) => this.owner.allyCardDetail.AddTempCard(id)?.SetCostToZero();

  public bool CheckOtherNamed()
  {
    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
    bool flag = false;
    foreach (BattleUnitModel battleUnitModel in aliveList)
    {
      if (battleUnitModel.passiveDetail.HasPassive<PassiveAbility_150015Log>())
      {
        flag = true;
        break;
      }
      if (battleUnitModel.passiveDetail.HasPassive<PassiveAbility_150016Log>())
      {
        flag = true;
        break;
      }
    }
    return flag;
  }
}
}
