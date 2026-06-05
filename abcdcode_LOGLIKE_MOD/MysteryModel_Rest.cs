// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Rest
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

 
namespace abcdcode_LOGLIKE_MOD {

public class MysteryModel_Rest : MysteryBase
{
  public bool Inited;
  public List<RestGood> Choices;
  public static Dictionary<int, Vector2[]> ChoiceShape;

  public override void LoadFromSaveData(SaveData savedata)
  {
  }

  public MysteryModel_Rest()
  {
    this.Inited = false;
    if (MysteryModel_Rest.ChoiceShape != null)
      return;
    MysteryModel_Rest.ChoiceShape = new Dictionary<int, Vector2[]>();
    Vector2[] vector2Array1 = new Vector2[1]
    {
      new Vector2(0.0f, 250f)
    };
    MysteryModel_Rest.ChoiceShape.Add(1, vector2Array1);
    Vector2[] vector2Array2 = new Vector2[2]
    {
      new Vector2(-200f, 250f),
      new Vector2(200f, 250f)
    };
    MysteryModel_Rest.ChoiceShape.Add(2, vector2Array2);
    Vector2[] vector2Array3 = new Vector2[3]
    {
      new Vector2(-300f, 250f),
      new Vector2(0.0f, 250f),
      new Vector2(300f, 250f)
    };
    MysteryModel_Rest.ChoiceShape.Add(3, vector2Array3);
    Vector2[] vector2Array4 = new Vector2[4]
    {
      new Vector2(-360f, 250f),
      new Vector2(-120f, 250f),
      new Vector2(120f, 250f),
      new Vector2(360f, 250f)
    };
    MysteryModel_Rest.ChoiceShape.Add(4, vector2Array4);
    Vector2[] vector2Array5 = new Vector2[5]
    {
      new Vector2(-400f, 250f),
      new Vector2(-200f, 250f),
      new Vector2(0.0f, 250f),
      new Vector2(200f, 250f),
      new Vector2(400f, 250f)
    };
    MysteryModel_Rest.ChoiceShape.Add(5, vector2Array5);
    Vector2[] vector2Array6 = new Vector2[6]
    {
      new Vector2(-300f, 250f),
      new Vector2(0.0f, 250f),
      new Vector2(300f, 250f),
      new Vector2(-300f, -50f),
      new Vector2(0.0f, -50f),
      new Vector2(300f, -50f)
    };
    MysteryModel_Rest.ChoiceShape.Add(6, vector2Array6);
    Vector2[] vector2Array7 = new Vector2[7]
    {
      new Vector2(-360f, 250f),
      new Vector2(-120f, 250f),
      new Vector2(120f, 250f),
      new Vector2(360f, 250f),
      new Vector2(-300f, -50f),
      new Vector2(0.0f, -50f),
      new Vector2(300f, -50f)
    };
    MysteryModel_Rest.ChoiceShape.Add(7, vector2Array7);
    Vector2[] vector2Array8 = new Vector2[8]
    {
      new Vector2(-360f, 250f),
      new Vector2(-120f, 250f),
      new Vector2(120f, 250f),
      new Vector2(360f, 250f),
      new Vector2(-360f, -50f),
      new Vector2(-120f, -50f),
      new Vector2(120f, -50f),
      new Vector2(360f, -50f)
    };
    MysteryModel_Rest.ChoiceShape.Add(8, vector2Array8);
    Vector2[] vector2Array9 = new Vector2[9]
    {
      new Vector2(-400f, 250f),
      new Vector2(-200f, 250f),
      new Vector2(0.0f, 250f),
      new Vector2(200f, 250f),
      new Vector2(400f, 250f),
      new Vector2(-360f, -50f),
      new Vector2(-120f, -50f),
      new Vector2(120f, -50f),
      new Vector2(360f, -50f)
    };
    MysteryModel_Rest.ChoiceShape.Add(9, vector2Array9);
    Vector2[] vector2Array10 = new Vector2[10]
    {
      new Vector2(-400f, 250f),
      new Vector2(-200f, 250f),
      new Vector2(0.0f, 250f),
      new Vector2(200f, 250f),
      new Vector2(400f, 250f),
      new Vector2(-400f, -50f),
      new Vector2(-200f, -50f),
      new Vector2(0.0f, -50f),
      new Vector2(200f, -50f),
      new Vector2(400f, -50f)
    };
    MysteryModel_Rest.ChoiceShape.Add(10, vector2Array10);
  }

  public override void SwapFrame(int id)
  {
    this.RemoveCurFrame();
    this.FrameObj.Add("Frame", ModdingUtils.CreateImage(LogLikeMod.LogUIObjs[90].transform, "RestPanel0", new Vector2(1f, 1f), new Vector2(0.0f, 0.0f)).gameObject);
    Button button1 = ModdingUtils.CreateButton(LogLikeMod.LogUIObjs[90].transform, "AbCardSelection_Skip", new Vector2(1f, 1f), new Vector2(-590f, -480f));
    button1.onClick.AddListener(new UnityAction(this.HideRest));
    this.FrameObj.Add("RestHide", button1.gameObject);
    TextMeshProUGUI textTmp1 = ModdingUtils.CreateText_TMP(button1.transform, new Vector2(-30f, 0.0f), 40, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
    textTmp1.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("ui_RestHide");
    textTmp1.transform.Rotate(0.0f, 0.0f, 2.5f);
    this.FrameObj.Add("RestHideText", textTmp1.gameObject);
    Button button2 = ModdingUtils.CreateButton(LogLikeMod.LogUIObjs[90].transform, "AbCardSelection_Skip", new Vector2(1f, 1f), new Vector2(590f, -480f));
    TextMeshProUGUI textTmp2 = ModdingUtils.CreateText_TMP(button2.transform, new Vector2(-30f, 0.0f), 40, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
    this.FrameObj.Add("LeaveButton", button2.gameObject);
    button2.onClick.AddListener(new UnityAction(this.LeaveRest));
    textTmp2.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("ui_RestLeave");
    textTmp2.transform.Rotate(0.0f, 0.0f, 2.5f);
    this.Choices = new List<RestGood>();
    List<RewardPassiveInfo> chapterData = Singleton<RewardPassivesList>.Instance.GetChapterData(ChapterGrade.GradeAll, PassiveRewardListType.Custom, new LorId(LogLikeMod.ModId, 800000));
    Singleton<GlobalLogueEffectManager>.Instance.ChangeRestChoice((MysteryBase) this, ref chapterData);
    this.RestChoiceCreating(chapterData);
    this.Inited = true;
  }

  public void ChoiceRest(RestGood choicegood)
  {
    List<RewardPassiveInfo> list = new List<RewardPassiveInfo>();
    foreach (RestGood choice in this.Choices)
    {
      if (choice.OnChoiceOther(choicegood))
        list.Add(choice.goodinfo);
    }
    foreach (string key in this.FrameObj.Keys.ToList<string>())
    {
      if (key.Contains("RestGoods"))
      {
        Object.Destroy( this.FrameObj[key]);
        this.FrameObj.Remove(key);
      }
    }
    this.Choices.Clear();
    if (list.Count > 0)
      this.RestChoiceCreating(list);
    else
      this.FrameObj["Frame"].GetComponent<Image>().sprite = LogLikeMod.ArtWorks["RestPanel1"];
  }

  public void RestChoiceCreating(List<RewardPassiveInfo> list)
  {
    int count = list.Count;
    for (int index = 0; index < list.Count; ++index)
      this.RestChoiceCreating(list[index], this.GetChoiceShape(count, index), index);
  }

  public RestGood RestChoiceCreating(RewardPassiveInfo passive, Vector2 position, int id)
  {
    GameObject gameObject = new GameObject("");
    gameObject.transform.SetParent(this.FrameObj["Frame"].transform);
    gameObject.transform.localScale = new Vector3(1f, 1f);
    RestGood restGood = gameObject.AddComponent<RestGood>();
    restGood.gameObject.transform.localPosition = (Vector3) position;
    RewardPassiveInfo goodinfo = passive;
    restGood.Set(goodinfo);
    restGood.SetParent(this);
    this.Choices.Add(restGood);
    this.FrameObj.Add("RestGoods" + id.ToString(), restGood.gameObject);
    return restGood;
  }

  public void LeaveRest()
  {
    Singleton<StageController>.Instance.GetStageModel().GetWave(Singleton<StageController>.Instance.CurrentWave).Defeat();
    Singleton<StageController>.Instance.EndBattle();
    Singleton<MysteryManager>.Instance.EndMystery((MysteryBase) this);
  }

  public void HideRest()
  {
    GameObject gameObject1 = this.FrameObj["Frame"];
    GameObject gameObject2 = this.FrameObj["LeaveButton"];
    if (gameObject1.activeSelf)
    {
      this.FrameObj["RestHideText"].GetComponent<TextMeshProUGUI>().text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("ui_RestShow");
      gameObject1.SetActive(false);
      gameObject2.SetActive(false);
    }
    else
    {
      this.FrameObj["RestHideText"].GetComponent<TextMeshProUGUI>().text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("ui_RestHide");
      gameObject1.SetActive(true);
      gameObject2.SetActive(true);
    }
    if (LogLikeMod.rewards_InStage.Count <= 0)
      return;
    this.FrameObj["RestHideText"].GetComponent<TextMeshProUGUI>().text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("ui_RestShow");
    gameObject1.SetActive(false);
    gameObject2.SetActive(false);
  }

  public Vector2 GetChoiceShape(int num, int id) => MysteryModel_Rest.ChoiceShape[num][id];
}
}
