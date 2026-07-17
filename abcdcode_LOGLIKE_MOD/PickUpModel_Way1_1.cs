// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_Way1_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_Way1_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Pickup model: PickUpModel_Way1_1</summary>

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
