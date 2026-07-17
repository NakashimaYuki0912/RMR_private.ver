// -----------------------------------------------------------------------------
// Library of Ruina mod script: MysteryManager
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryManager.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;
using UI;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>MysteryManager</summary>

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
            }
            // Do not copy options-dropdown font (Latin-only). Patch cnFont_* for mystery UI.
            LogLikeMod.EnsureLocalizedFonts("StartMystery", repairActiveUi: true);
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
