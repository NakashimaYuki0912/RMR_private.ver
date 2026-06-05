// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_RestGood2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Reflection;
using UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class PickUpModel_RestGood2 : RestPickUp
    {
        public PickUpModel_RestGood2()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 800002));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 800002);
            this.type = RestPickUp.RestPickUpType.Main;
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            LogStatAdder adder = new LogStatAdder()
            {
                maxbreakpercent = 5,
                maxhppercent = 5
            };
            LogueBookModels.AddPlayerStat(model.UnitData, adder);
        }

        public override bool CheckCondition()
        {
            bool condition = LogueBookModels.GetCardList(true, true).FindAll(x => x.ClassInfo.CheckCanUpgrade()).Count > 0;
            if (!condition)
                UI.UIAlarmPopup.instance.SetAlarmText(TextDataModel.GetText("CardCheckPopUp_CannotUpgrade"));
            return condition;
        }

        public override void OnChoice(RestGood good)
        {
            MysteryModel_CardChoice.PopupCardChoice(LogueBookModels.GetCardList(true, true).
                FindAll(x => x.ClassInfo.CheckCanUpgrade()), 
                new MysteryModel_CardChoice.ChoiceResult(this.UpgradeCard), 
                MysteryModel_CardChoice.ChoiceDescType.UpgradeDesc);
        }

        public void UpgradeCard(MysteryModel_CardChoice mystery, DiceCardItemModel model)
        {
            MysteryModel_UpgradeCheckPopup.PopupUpgradeCheck(
                model.GetID(), 
                my => this.UpgradedCard(mystery, my, model.GetID())
            );
        }

        public void UpgradedCard(
          MysteryModel_CardChoice mystery,
          MysteryModel_UpgradeCheckPopup mystery2,
          LorId cardid)
        {
            LogueBookModels.DeleteCard(cardid);
            LogueBookModels.AddCard(new LorId(mystery2.metadata.unparsedPid, cardid.id));
            UISoundManager.instance.PlayEffectSound(UISoundType.Card_Apply);
            Singleton<MysteryManager>.Instance.EndMystery(mystery);
            Singleton<MysteryManager>.Instance.EndMystery(mystery2);
        }
    }
}
