// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch1_5
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using RogueLike_Mod_Reborn;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryModel_Mystery_Ch1_5 : MysteryBase
    {
        int money = 0;

        public bool CheckHas()
        {
            List<DiceCardItemModel> cardList = LogueBookModels.GetCardList(true, true);
            if (cardList.Find((Predicate<DiceCardItemModel>)(x => x.GetID() == new LorId(LogLikeMod.ModId, 101001))) == null)
            {
                LorId upid = Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 101001)).id;
                if (cardList.Find((Predicate<DiceCardItemModel>)(x => x.GetID() == upid)) == null)
                    return false;
            }
            return true;
        }

        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (id > 0)
            {
                this.ReformatButton(0, money);
            }
            if (id != 0 || this.CheckHas())
                return;
            this.FrameObj["choice_btn1"].GetComponent<Image>().sprite = LogLikeMod.ArtWorks["disabledButton"];
            this.FrameObj["Desc1"].GetComponent<TextMeshProUGUI>().text = TextDataModel.GetText("Mystery_RequireCondition") + this.FrameObj["Desc1"].GetComponent<TextMeshProUGUI>().text;
        }

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0)
            {
                if (choiceid == 0)
                {
                    money = UnityEngine.Random.Range(1, 11);
                    PassiveAbility_MoneyCheck.AddMoney(money);
                }
                if (choiceid == 1)
                {
                    if (!this.CheckHas())
                        return;
                    money = UnityEngine.Random.Range(5, 21);
                    PassiveAbility_MoneyCheck.AddMoney(money);
                    if (LogueBookModels.GetCardList(true, true).Find(x => x.GetID() == new LorId(LogLikeMod.ModId, 101001)) != null)
                        LogueBookModels.DeleteCard(new LorId(LogLikeMod.ModId, 101001));
                    else
                        LogueBookModels.DeleteCard(Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 101001)).id);
                }
            }
            base.OnClickChoice(choiceid);
        }
    }
}
