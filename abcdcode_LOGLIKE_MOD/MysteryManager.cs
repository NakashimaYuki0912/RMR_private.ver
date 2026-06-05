// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryManager
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;
using UI;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryManager : Singleton<MysteryManager>
    {
        public MysteryBase curMystery;
        public List<MysteryBase> interruptMysterys;

        public void StartMystery(LorId mysteryid)
        {
            this.StartMystery(Singleton<MysteryXmlList>.Instance.GetData(mysteryid));
        }

        public void StartMystery(MysteryXmlInfo info)
        {
            if (LogLikeMod.DefFont == null)
            {
                LogLikeMod.DefFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
                LogLikeMod.DefFontColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                LogLikeMod.DefFont_TMP = (SingletonBehavior<UIPopupWindowManager>.Instance.popupPanels[1] as UIOptionWindow).displayDropdown.itemText.font;
            }
            this.curMystery = MysteryBase.FindMystery(info.script);
            this.curMystery.xmlinfo = info;
            this.interruptMysterys = new List<MysteryBase>();
            this.curMystery.Init();
        }

        public void AddInterrupt(MysteryBase mystery)
        {
            if (this.interruptMysterys == null)
                this.interruptMysterys = new List<MysteryBase>();
            this.interruptMysterys.Add(mystery);
            mystery.Init();
        }

        public void EndMystery(MysteryBase mystery = null)
        {
            if (this.curMystery != null)
            {
                if (mystery == this.curMystery || mystery == null)
                {
                    this.curMystery.EndMystery();
                    this.curMystery = (MysteryBase)null;
                    if (this.interruptMysterys.Count <= 0)
                        return;
                    foreach (MysteryBase interruptMystery in this.interruptMysterys)
                        interruptMystery.EndMystery();
                    this.interruptMysterys.Clear();
                }
                else if (this.interruptMysterys.Contains(mystery))
                {
                    mystery.EndMystery();
                    this.interruptMysterys.Remove(mystery);
                }
            }
            else if (this.interruptMysterys != null && this.interruptMysterys.Contains(mystery))
            {
                mystery.EndMystery();
                this.interruptMysterys.Remove(mystery);
            }
        }
    }
}
