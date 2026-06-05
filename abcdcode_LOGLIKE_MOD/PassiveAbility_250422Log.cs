// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_250422Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using LOR_XML;
using Sound;
using System;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_250422Log : PassiveAbilityBase
{
  public AudioClip[] _oldEnemytheme;
  public int _roundDamage;
  public bool _egoCancel;
  public string path = "6/RedHood_Emotion_Aura";
  public Battle.CreatureEffect.CreatureEffect aura;
  public bool _bDoneEffect;

  public override void OnRoundStart()
  {
    bool egoCancel = this._egoCancel;
    this.PlayChangingEffect();
    if (this.owner.faction != Faction.Enemy)
      return;
    int emotionTotalCoinNumber = Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionTotalCoinNumber;
    Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionTotalBonus = emotionTotalCoinNumber + 1;
  }

  public override void OnRoundStartAfter()
  {
  }

  public override void OnCreated()
  {
    if (this.owner.bufListDetail.HasAssimilation())
    {
      this.owner.passiveDetail.DestroyPassive((PassiveAbilityBase) this);
    }
    else
    {
      this.name = Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 250422));
      this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 250422));
      this.owner.bufListDetail.AddBufWithoutDuplication((BattleUnitBuf) new PassiveAbility_250422Log.BattleUnitBuf_RedMistEgo());
      if (this.owner.faction != Faction.Player)
        return;
      this.owner.personalEgoDetail.AddCard(new LorId(LogLikeMod.ModId, 607021));
    }
  }

  public override void BeforeRollDice(BattleDiceBehavior behavior)
  {
    behavior.ApplyDiceStatBonus(new DiceStatBonus()
    {
      power = 2
    });
  }

  public override bool OnBreakGageZero()
  {
    this._egoCancel = true;
    return false;
  }

  public override void AfterGiveDamage(int damage) => this._roundDamage += damage;

  public override void OnRoundEnd()
  {
    if (this._roundDamage < 40)
      this.owner.TakeBreakDamage((int) ((double) this.owner.breakDetail.GetDefaultBreakGauge() * 0.40000000596046448), DamageType.Passive);
    this._roundDamage = 0;
  }

  public override void OnRoundEndTheLast()
  {
    if (!this._egoCancel)
      return;
    if ( this.aura !=  null)
      this.aura.ManualDestroy();
    this.owner.personalEgoDetail.RemoveCard(new LorId(LogLikeMod.ModId, 607021));
    this.owner.passiveDetail.DestroyPassive((PassiveAbilityBase) this);
    this.owner.view.ResetSkin();
    this.owner.bufListDetail.RemoveBufAll(typeof (PassiveAbility_250422Log.BattleUnitBuf_RedMistEgo));
    List<PassiveAbilityBase> all = this.owner.passiveDetail.PassiveList.FindAll((Predicate<PassiveAbilityBase>) (x => x is PassiveAbility_250022Log));
    if (all.Count > 0)
    {
      foreach (PassiveAbilityBase passiveAbilityBase in all)
        (passiveAbilityBase as PassiveAbility_250022Log).StartEgoCount();
    }
    if (this._oldEnemytheme != null)
    {
      SingletonBehavior<BattleSoundManager>.Instance.SetEnemyTheme(this._oldEnemytheme);
      Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionTotalBonus = 0;
    }
    if (this.owner.faction == Faction.Enemy)
      this.owner.view.DisplayDlg(DialogType.SPECIAL_EVENT, "SPECIAL_EVENT_2");
    this.ResetResist();
  }

  public void ResetResist()
  {
    this.owner.Book.SetResistHP(BehaviourDetail.Slash, AtkResist.Normal);
    this.owner.Book.SetResistHP(BehaviourDetail.Penetrate, AtkResist.Normal);
    this.owner.Book.SetResistHP(BehaviourDetail.Hit, AtkResist.Endure);
    this.owner.Book.SetResistBP(BehaviourDetail.Slash, AtkResist.Normal);
    this.owner.Book.SetResistBP(BehaviourDetail.Penetrate, AtkResist.Normal);
    this.owner.Book.SetResistBP(BehaviourDetail.Hit, AtkResist.Endure);
  }

  public override void OnKill(BattleUnitModel target)
  {
    this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.RedMist, 1);
  }

  public void SetParticle()
  {
    UnityEngine.Object original = Resources.Load("Prefabs/Battle/SpecialEffect/RedMistRelease_ActivateParticle");
    if (original !=  null)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
      gameObject.transform.parent = this.owner.view.charAppearance.transform;
      gameObject.transform.localPosition = Vector3.zero;
      gameObject.transform.localRotation = Quaternion.identity;
      gameObject.transform.localScale = Vector3.one;
    }
    SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Battle/Kali_Change");
  }

  public void PlayChangingEffect()
  {
    if (this._bDoneEffect)
      return;
    this._bDoneEffect = true;
    this.owner.view.ChangeSkin("TheRedMist");
    this.owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
    if ( this.aura ==  null)
      this.aura = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect(this.path, 1f, this.owner.view, this.owner.view);
    this.SetParticle();
    int emotionTotalCoinNumber = Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionTotalCoinNumber;
    Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionTotalBonus = emotionTotalCoinNumber + 1;
    AudioClip[] enemyThemes = new AudioClip[3];
    AudioClip audioClip = Resources.Load<AudioClip>("Sounds/Battle/RedMistBgm");
    if ( audioClip !=  null)
    {
      for (int index = 0; index < enemyThemes.Length; ++index)
        enemyThemes[index] = audioClip;
      this._oldEnemytheme = SingletonBehavior<BattleSoundManager>.Instance.SetEnemyTheme(enemyThemes);
      SingletonBehavior<BattleSoundManager>.Instance.ChangeEnemyTheme(0);
    }
    else
      Debug.LogError( "Bgm Not found : red mist");
  }

  public class BattleUnitBuf_RedMistEgo : BattleUnitBuf
  {
    public override KeywordBuf bufType => KeywordBuf.RedMistEgo;

    public override string keywordId => "RedMistEgo";

    public override bool isAssimilation => true;

    public BattleUnitBuf_RedMistEgo() => this.stack = 0;
  }
}
}
