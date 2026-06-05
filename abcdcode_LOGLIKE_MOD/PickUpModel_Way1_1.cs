// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Way1_1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Way1_1 : PickUpModelBase
{
  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    if (model.UnitData != LogueBookModels.playerBattleModel[0])
      return;
    StageModel stageModel = Singleton<StageController>.Instance.GetStageModel();
    StageClassInfo data = Singleton<StageClassInfoList>.Instance.GetData(-855);
    StageWaveInfo wave = data.waveList[Random.Range(0, data.waveList.Count - 1)];
    StageWaveModel stageWaveModel = new StageWaveModel();
    stageWaveModel.Init(stageModel, wave);
    ((List<StageWaveModel>) typeof (StageModel).GetField("_waveList", AccessTools.all).GetValue( stageModel)).Add(stageWaveModel);
  }
}
}
