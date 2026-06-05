// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_230028Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_230028Log : PassiveAbilityBase
{
  public int _patternCount;
  public int _speedDiceAdder;
  public int _defaultDiceAdder = 2;
  public GameObject _protectionEffect;

  public override string debugDesc => "필립 전용";

  public GameObject protectionEffect => this._protectionEffect;

  public override int SpeedDiceNumAdder() => this._speedDiceAdder + this._defaultDiceAdder;

  public override void OnWaveStart()
  {
    this._speedDiceAdder = 0;
    this._patternCount = 0;
    if ( this._protectionEffect !=  null)
      Object.Destroy( this._protectionEffect);
    this._protectionEffect = (GameObject) null;
  }

  public void BurnAll()
  {
    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction == Faction.Player ? Faction.Enemy : Faction.Player);
    if (this.owner.IsBreakLifeZero())
      return;
    foreach (BattleUnitModel battleUnitModel in aliveList)
      battleUnitModel.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, 3, this.owner);
  }

  public override void OnRoundStart()
  {
    this.BurnAll();
    int emotionTotalCoinNumber = Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionTotalCoinNumber;
    Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionTotalBonus = emotionTotalCoinNumber + 1;
    this._speedDiceAdder = 0;
    this.owner.Book.SetOriginalResists();
    if (this.owner.IsBreakLifeZero())
    {
      this.owner.bufListDetail.AddBuf((BattleUnitBuf) new PassiveAbility_230028.PhilipBuf4(3 - this._patternCount + 1));
    }
    else
    {
      if (this._patternCount == 0 || this._patternCount == 1)
      {
        this.owner.bufListDetail.AddBuf((BattleUnitBuf) new PassiveAbility_230028.PhilipBuf2(2 - this._patternCount));
        this.owner.bufListDetail.AddBuf((BattleUnitBuf) new PassiveAbility_230028.PhilipBuf4(3 - this._patternCount));
        Singleton<StageController>.Instance.GetStageModel().SetCurrentMapInfo(0);
        this.owner.view.ChangeSkin("PhilipEgoProtection");
        if ( this._protectionEffect ==  null)
        {
          Object original = Resources.Load("Prefabs/Battle/BufEffect/PhilipProtectionEffect");
          if (original !=  null)
            this._protectionEffect = Object.Instantiate(original, this.owner.view.atkEffectRoot) as GameObject;
        }
        this.owner.allyCardDetail.ExhaustAllCards();
        for (int index = 0; index < 2; ++index)
          this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 408010))?.SetCostToZero();
        for (int index = 0; index < 1; ++index)
          this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 408011))?.SetCostToZero();
      }
      else if (this._patternCount == 2)
      {
        this.owner.bufListDetail.AddBuf((BattleUnitBuf) new PassiveAbility_230028.PhilipBuf1(3 - this._patternCount));
        this.owner.bufListDetail.AddBuf((BattleUnitBuf) new PassiveAbility_230028.PhilipBuf4(3 - this._patternCount));
        Singleton<StageController>.Instance.GetStageModel().SetCurrentMapInfo(1);
        this.owner.view.ChangeSkin("PhilipEgo");
        if ( this._protectionEffect !=  null)
        {
          Object.Destroy( this._protectionEffect);
          this._protectionEffect = (GameObject) null;
        }
        this.owner.allyCardDetail.ExhaustAllCards();
        for (int index = 0; index < 2; ++index)
          this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 408008))?.SetCostToZero();
        for (int index = 0; index < 1; ++index)
          this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 408009))?.SetCostToZero();
      }
      else if (this._patternCount == 3)
      {
        this.owner.bufListDetail.AddBuf((BattleUnitBuf) new PassiveAbility_230028.PhilipBuf3());
        Singleton<StageController>.Instance.GetStageModel().SetCurrentMapInfo(2);
        this.owner.view.ChangeSkin("PhilipEgoFury");
        SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Battle/Philip_MapChange_Strong");
        if ( this._protectionEffect !=  null)
        {
          Object.Destroy( this._protectionEffect);
          this._protectionEffect = (GameObject) null;
        }
        this._speedDiceAdder = 2;
        this.owner.allyCardDetail.ExhaustAllCards();
        for (int index = 0; index < 1; ++index)
          this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 408007))?.SetCostToZero();
        for (int index = 0; index < 4; ++index)
          this.owner.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 408008))?.SetCostToZero();
        this._patternCount = 0;
        return;
      }
      ++this._patternCount;
    }
  }

  public class PhilipBuf1 : BattleUnitBuf
  {
    public override string keywordId => "Philip_Attack";

    public PhilipBuf1(int n) => this.stack = n;

    public override void OnSuccessAttack(BattleDiceBehavior behavior)
    {
      behavior.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, 4, this._owner);
    }

    public override void OnRoundEnd() => this.Destroy();
  }

  public class PhilipBuf2 : BattleUnitBuf
  {
    public override string keywordId => "Philip_Defense";

    public PhilipBuf2(int n) => this.stack = n;

    public override int GetDamageReduction(BattleDiceBehavior behavior) => 5;

    public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
    {
      atkDice.owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, 2, this._owner);
    }

    public override void OnRoundEnd() => this.Destroy();
  }

  public class PhilipBuf3 : BattleUnitBuf
  {
    public override string keywordId => "Philip_Strong";

    public override void OnRoundEnd() => this.Destroy();
  }

  public class PhilipBuf4 : BattleUnitBuf
  {
    public override string keywordId => "Philip_Ready";

    public override string keywordIconId => "Philip_Strong";

    public PhilipBuf4(int n) => this.stack = n;

    public override void OnRoundEnd() => this.Destroy();
  }
}
}
