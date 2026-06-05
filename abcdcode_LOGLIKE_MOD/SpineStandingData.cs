// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.SpineStandingData
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Spine.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class SpineStandingData
{
  public SkeletonDataAsset asset;
  public Dictionary<ActionDetail, string> AnimDic;
  public Dictionary<ActionDetail, float> AnimSpeed;
  public Dictionary<ActionDetail, bool> AnimLoop;
  public Dictionary<ActionDetail, Vector3> AnimScale;

  public SpineStandingData(SkeletonDataAsset Asset)
  {
    this.asset = Asset;
    this.AnimDic = new Dictionary<ActionDetail, string>();
    this.AnimSpeed = new Dictionary<ActionDetail, float>();
    this.AnimLoop = new Dictionary<ActionDetail, bool>();
    this.AnimScale = new Dictionary<ActionDetail, Vector3>();
  }

  public void SetDic(ActionDetail detail, string anim, float speed = 1f, bool IsLoop = true)
  {
    this.AnimDic[detail] = anim;
    this.AnimSpeed[detail] = speed;
    this.AnimLoop[detail] = IsLoop;
    this.AnimScale[detail] = new Vector3(1f, 1f);
  }

  public void SetScale(ActionDetail detail, Vector3 vec) => this.AnimScale[detail] = vec;

  public void SetScale(Vector3 vec)
  {
    foreach (KeyValuePair<ActionDetail, Vector3> keyValuePair in this.AnimScale.ToList<KeyValuePair<ActionDetail, Vector3>>())
      this.AnimScale[keyValuePair.Key] = vec;
  }
}
}
