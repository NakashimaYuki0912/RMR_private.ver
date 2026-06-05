// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PassiveAbility_150038Log
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PassiveAbility_150038Log : PassiveAbilityBase
{
  public int _patternCount;
  public int _speedDiceNumAdder = 2;
  public bool _bLoadParticle;
  public string _TRANSFORM_PARTICLE_PATH = "Prefabs/Battle/SpecialEffect/Xiao_EgoTransformParticle";
  public GameObject _aura;

  public override int SpeedDiceNumAdder() => this._speedDiceNumAdder;

  public override void OnWaveStart()
  {
    this._patternCount = 0;
    if (this.owner.emotionDetail.EmotionLevel >= 1)
      return;
    this.owner.emotionDetail.SetEmotionLevel(1);
  }

  public override void OnReleaseBreak()
  {
    this.owner.breakDetail.LoseBreakGauge(this.owner.breakDetail.breakGauge / 2);
  }

  public override void OnRoundStart()
  {
    this._speedDiceNumAdder = 3;
    if (Singleton<StageController>.Instance.RoundTurn >= 2)
      ++this._speedDiceNumAdder;
    if (this._bLoadParticle)
      return;
    this.LoadParticle();
    this._bLoadParticle = true;
  }

  public override void OnRoundStartAfter()
  {
    this.SetCards_phase2();
    ++this._patternCount;
    this._patternCount %= 6;
  }

  public void SetCards_phase2()
  {
    this.owner.allyCardDetail.ExhaustAllCards();
    bool flag = Singleton<StageController>.Instance.RoundTurn == 1;
    int num = this.owner.Book.GetSpeedDiceRule(this.owner).Roll(this.owner).Count - 4;
    if (this._patternCount == 0)
    {
      if (flag)
      {
        this.AddNewCard(610010);
      }
      else
      {
        this.AddNewCard(610006);
        this.AddNewCard(610008);
      }
      this.AddNewCard(601001);
      this.AddNewCard(610003);
    }
    else if (this._patternCount == 1)
    {
      this.AddNewCard(610010);
      this.AddNewCard(610008);
      this.AddNewCard(606003);
    }
    else if (this._patternCount == 2)
    {
      this.AddNewCard(610006);
      this.AddNewCard(610005);
      this.AddNewCard(610003);
    }
    else if (this._patternCount == 3)
    {
      this.AddNewCard(601001);
      this.AddNewCard(606003);
      this.AddNewCard(610006);
    }
    else if (this._patternCount == 4)
    {
      this.AddNewCard(610008);
      this.AddNewCard(610003);
      this.AddNewCard(610002);
    }
    else if (this._patternCount == 5)
    {
      this.AddNewCard(610009);
      this.AddNewCard(610006);
      this.AddNewCard(610003);
    }
    this.AddNewCard(this.GetDragonAngerId());
    for (int index = 0; index < num; ++index)
    {
      switch (index)
      {
        case 0:
          this.AddNewCard(610010);
          break;
        case 1:
          this.AddNewCard(606005);
          break;
        default:
          this.AddNewCard(601006);
          break;
      }
    }
  }

  public void AddNewCard(int id)
  {
    this.owner.allyCardDetail.AddTempCard(new LorId(LogLikeMod.ModId, id))?.SetCostToZero();
  }

  public void LoadParticle()
  {
    this.owner.view.ChangeSkin("XiaoEgo");
    Object original1 = Resources.Load(this._TRANSFORM_PARTICLE_PATH);
    if (original1 !=  null)
    {
      GameObject gameObject = Object.Instantiate(original1) as GameObject;
      gameObject.transform.parent = this.owner.view.charAppearance.atkEffectRoot;
      gameObject.transform.localPosition = Vector3.zero;
      gameObject.transform.localRotation = Quaternion.identity;
      gameObject.transform.localScale = Vector3.one;
    }
    Object original2 = Resources.Load("Prefabs/Battle/SpecialEffect/XiaoEgoAura");
    if (!(original2 !=  null))
      return;
    GameObject gameObject1 = Object.Instantiate(original2) as GameObject;
    if ( gameObject1 !=  null)
    {
      gameObject1.transform.parent = this.owner.view.charAppearance.transform;
      gameObject1.transform.localPosition = Vector3.zero;
      gameObject1.transform.localRotation = Quaternion.identity;
      gameObject1.transform.localScale = Vector3.one;
      XiaoFeatherAura component = gameObject1.GetComponent<XiaoFeatherAura>();
      if ( component !=  null)
        component.Init(this.owner.view);
      this._aura = gameObject1;
    }
  }

  public int GetDragonAngerId() => RandomUtil.SelectOne<int>(610012, 610013);
}
}
