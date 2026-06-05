// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.CameraRender
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using UI;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class CameraRender : MonoBehaviour
{
  public bool Actived;
  public int index;
  public int skip = 0;

  public bool CheckingRender()
  {
    return LogLikeMod.spinedatas.ContainsKey(SingletonBehavior<UICharacterRenderer>.Instance.characterList[this.index].unitModel.CustomBookItem.GetCharacterName());
  }

  public void Update()
  {
    try
    {
      --this.skip;
      if (!LogLikeMod.CheckStage() || !this.CheckingRender() || this.skip >= 0)
        return;
      this.gameObject.GetComponent<Camera>().Render();
      this.skip = 2;
    }
    catch
    {
    }
  }
}
}
