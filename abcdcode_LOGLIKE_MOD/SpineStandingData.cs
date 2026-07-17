// -----------------------------------------------------------------------------
// Library of Ruina mod script: SpineStandingData
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\SpineStandingData.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using Spine.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>SpineStandingData</summary>

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
