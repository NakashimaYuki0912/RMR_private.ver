// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryAnimatorDefault
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryAnimatorDefault
    {
        public bool started = false;
        public float curanimremaintime;
        public MysteryAnimOrderData curanimorder;
        public Queue<MysteryAnimOrderData> animorders;
        public MysteryBase curMystery;

        public virtual void Init(MysteryBase mystery)
        {
            this.started = true;
            this.curMystery = mystery;
            this.curanimremaintime = 0.0f;
            foreach (KeyValuePair<string, GameObject> keyValuePair in this.curMystery.FrameObj)
            {
                if ((keyValuePair.Key == "Mask" || keyValuePair.Key == "Frame" || keyValuePair.Key.Contains("Title") ? 1 : (keyValuePair.Key.Contains("FrameArt") ? 1 : 0)) == 0)
                    this.ChangeAlpha(keyValuePair.Value, 0.0f);
            }
            this.animorders = new Queue<MysteryAnimOrderData>();
            this.animorders.Enqueue(new MysteryAnimOrderData(1.2f, MysteryAnimOrderData.AnimType.FadeInGroup, new List<string>()
    {
      "Dia",
      "TextUpDown_Up",
      "TextUpDown_Down"
    }));
            this.animorders.Enqueue(new MysteryAnimOrderData(0.8f, MysteryAnimOrderData.AnimType.FadeInGroup, new List<string>()
    {
      "choice_btn",
      "Desc"
    }));
        }

        public void ChangeAlpha(GameObject obj, float alpha)
        {
            if (obj.GetComponent<Image>() != null)
            {
                Color color = obj.GetComponent<Image>().color;
                obj.GetComponent<Image>().color = new Color(color.r, color.g, color.b, alpha);
            }
            if (!(obj.GetComponent<TextMeshProUGUI>() != null))
                return;
            Color color1 = obj.GetComponent<TextMeshProUGUI>().color;
            obj.GetComponent<TextMeshProUGUI>().color = new Color(color1.r, color1.g, color1.b, alpha);
        }

        public void Next()
        {
            this.curanimorder = this.animorders.Dequeue();
            this.curanimremaintime = this.curanimorder.time;
        }

        public virtual void Update()
        {
            if (!this.started)
                return;
            if (this.curanimorder == null)
            {
                if (this.animorders == null || this.animorders.Count == 0)
                {
                    this.started = false;
                    this.curMystery.animator.animator = (MysteryAnimatorDefault)null;
                    return;
                }
                this.Next();
            }
            if ((double)this.curanimremaintime <= 0.0)
            {
                if (this.curanimorder.type == MysteryAnimOrderData.AnimType.FadeIn)
                {
                    foreach (string key in this.curanimorder.targetobjgroup)
                        this.ChangeAlpha(this.curMystery.FrameObj[key], 1f);
                }
                else if (this.curanimorder.type == MysteryAnimOrderData.AnimType.FadeInGroup)
                {
                    foreach (string str in this.curanimorder.targetobjgroup)
                    {
                        foreach (KeyValuePair<string, GameObject> keyValuePair in this.curMystery.FrameObj)
                        {
                            if (keyValuePair.Key.Contains(str))
                                this.ChangeAlpha(keyValuePair.Value, 1f);
                        }
                    }
                }
                this.curanimorder = (MysteryAnimOrderData)null;
            }
            else
            {
                if (this.curanimorder.type == MysteryAnimOrderData.AnimType.FadeIn)
                {
                    foreach (string key in this.curanimorder.targetobjgroup)
                        this.ChangeAlpha(this.curMystery.FrameObj[key], (this.curanimorder.time - this.curanimremaintime) / this.curanimorder.time);
                }
                else if (this.curanimorder.type == MysteryAnimOrderData.AnimType.FadeInGroup)
                {
                    foreach (string str in this.curanimorder.targetobjgroup)
                    {
                        foreach (KeyValuePair<string, GameObject> keyValuePair in this.curMystery.FrameObj)
                        {
                            if (keyValuePair.Key.Contains(str))
                                this.ChangeAlpha(keyValuePair.Value, (this.curanimorder.time - this.curanimremaintime) / this.curanimorder.time);
                        }
                    }
                }
                this.curanimremaintime -= Time.deltaTime;
            }
        }
    }
}
