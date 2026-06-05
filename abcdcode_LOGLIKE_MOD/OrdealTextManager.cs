// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.OrdealTextManager
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

 
namespace abcdcode_LOGLIKE_MOD {

public class OrdealTextManager : Singleton<OrdealTextManager>
{
  public OrdealTextManager.OrdealTextBehaviour action;
  public GameObject root;

  public void ObjectInit()
  {
    this.root = LogLikeMod.LogUIObjs[110];
    this.action = ModdingUtils.CreateImage(this.root.transform, "Ordeal_Frame", new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(1920f, 400f)).gameObject.AddComponent<OrdealTextManager.OrdealTextBehaviour>();
    this.action.Init();
    this.action.gameObject.SetActive(false);
  }

  public void SetOrdeal(
    string id,
    Color color,
    string audioclip,
    OrdealTextManager.OrdealEnd endfunc = null)
  {
    if ( this.root ==  null)
      this.ObjectInit();
    AudioClip audioClip = Singleton<LogSoundEffectManager>.Instance.GetAudioClip(audioclip);
    if ( audioClip !=  null)
      SingletonBehavior<SoundEffectManager>.Instance.PlayClip(audioClip, volume: 3f);
    else
      this.Log($"Error : {audioclip} is NULL");
    this.root.gameObject.SetActive(true);
    this.action.gameObject.SetActive(true);
    this.action.SetOrdeal(id, color, endfunc);
  }

  public delegate void OrdealEnd();

  public class OrdealTextBehaviour : MonoBehaviour
  {
    public OrdealTextManager.OrdealEnd endFunc;
    public List<GameObject> graphic;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Desc;
    public Image Frame;
    public Image Vertical1;
    public Image Vertical2;
    public float timer;

    public void Init()
    {
      this.graphic = new List<GameObject>();
      this.Frame = this.gameObject.GetComponent<Image>();
      this.Vertical1 = ModdingUtils.CreateImage(this.Frame.transform, "Ordeal_VerticalBar", new Vector2(1f, 1f), new Vector2(-940f, 0.0f), new Vector2(30f, 390f));
      this.Vertical2 = ModdingUtils.CreateImage(this.Frame.transform, "Ordeal_VerticalBar", new Vector2(1f, 1f), new Vector2(940f, 0.0f), new Vector2(30f, 390f));
      this.Title = ModdingUtils.CreateText_TMP(this.Frame.transform, new Vector2(0.0f, 120f), 20, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, Color.black, LogLikeMod.DefFont_TMP);
      this.Name = ModdingUtils.CreateText_TMP(this.Frame.transform, new Vector2(0.0f, 60f), 70, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, Color.black, LogLikeMod.DefFont_TMP);
      this.Desc = ModdingUtils.CreateText_TMP(this.Frame.transform, new Vector2(0.0f, -50f), 20, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, Color.black, LogLikeMod.DefFont_TMP);
      this.graphic.Add(this.Frame.gameObject);
      this.graphic.Add(this.Vertical1.gameObject);
      this.graphic.Add(this.Vertical2.gameObject);
      this.graphic.Add(this.Title.gameObject);
      this.graphic.Add(this.Name.gameObject);
      this.graphic.Add(this.Desc.gameObject);
      this.timer = -1f;
    }

    public void SetColor(Color color)
    {
      this.Title.color = color;
      this.Name.color = color;
      this.Desc.color = color;
      this.Frame.color = color;
      this.Vertical1.color = color;
      this.Vertical2.color = color;
    }

    public void SetOrdeal(string id, Color color, OrdealTextManager.OrdealEnd func = null)
    {
      string text1 = TextDataModel.GetText(id + "Ordeal_Title");
      string text2 = TextDataModel.GetText(id + "Ordeal_Name");
      string text3 = TextDataModel.GetText(id + "Ordeal_Desc");
      this.Title.text = text1;
      this.Name.text = text2;
      this.Desc.text = text3;
      this.Title.color = color;
      this.Name.color = color;
      this.Desc.color = color;
      this.Frame.color = color;
      this.Vertical1.color = color;
      this.Vertical2.color = color;
      this.timer = 0.0f;
      this.endFunc = func;
    }

    public void ChangeAlpha(GameObject obj, float alpha)
    {
      if ( obj.GetComponent<Image>() !=  null)
      {
        Color color = obj.GetComponent<Image>().color;
        obj.GetComponent<Image>().color = new Color(color.r, color.g, color.b, alpha);
      }
      if (!( obj.GetComponent<TextMeshProUGUI>() !=  null))
        return;
      Color color1 = obj.GetComponent<TextMeshProUGUI>().color;
      obj.GetComponent<TextMeshProUGUI>().color = new Color(color1.r, color1.g, color1.b, alpha);
    }

    public void ChangeAlpha(float alpha)
    {
      foreach (GameObject gameObject in this.graphic)
        this.ChangeAlpha(gameObject, alpha);
    }

    public void Update()
    {
      if ((double) this.timer < 0.0)
        return;
      this.timer += Time.deltaTime;
      if ((double) this.timer < 0.5)
        this.ChangeAlpha(this.timer * 2f);
      else if ((double) this.timer < 4.5)
        this.ChangeAlpha(1f);
      else if ((double) this.timer < 5.0)
      {
        this.ChangeAlpha((5f - this.timer) * 2f);
      }
      else
      {
        this.ChangeAlpha(0.0f);
        this.timer = -1f;
        if (this.endFunc != null)
          this.endFunc();
        this.gameObject.SetActive(false);
      }
    }
  }
}
}
