// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_RestGood6
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class PickUpModel_RestGood6 : RestPickUp
    {
        public static int count;

        public PickUpModel_RestGood6()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 800006));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = TextDataModel.GetText("RestGood6_Desc", PickUpModel_RestGood6.count);
            this.id = new LorId(LogLikeMod.ModId, 800006);
            this.type = RestPickUp.RestPickUpType.Sub;
        }

        public override void Init()
        {
            PickUpModel_RestGood6.count = 2;
            this.Desc = TextDataModel.GetText("RestGood6_Desc", PickUpModel_RestGood6.count);
        }

        public override bool CheckCondition() => LogueBookModels.GetCardList(true, true).Count > 0;

        public void ExChangeCard(MysteryModel_CardChoice model, DiceCardItemModel card)
        {
            LogueBookModels.DeleteCard(card.ClassInfo.id);
            Singleton<MysteryManager>.Instance.EndMystery((MysteryBase)model);
            int id = 0;
            switch (card.ClassInfo.Chapter)
            {
                case 1:
                    id = 1001;
                    break;
                case 2:
                    id += 1000;
                    goto default;
                case 3:
                    id += 2000;
                    goto default;
                case 4:
                    id += 3000;
                    goto default;
                case 5:
                    id += 4000;
                    goto default;
                case 6:
                    id += 5000;
                    goto default;
                case 7:
                    id += 6000;
                    goto default;
                default:
                    if (id == 0) id += 5000;
                    switch (card.ClassInfo.Rarity)
                    {
                        case Rarity.Common:
                            ++id;
                            break;
                        case Rarity.Uncommon:
                            id += 2;
                            break;
                        case Rarity.Rare:
                            id += 3;
                            break;
                        case Rarity.Unique:
                            id += 4;
                            break;
                        case Rarity.Special:
                            ++id;
                            break;
                    }
                    break;
            }
            LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, id)));
            MysteryModel_CardReward.PopupCardReward_AutoSave();
        }

        public override bool OnChoiceOther(RestGood other) => PickUpModel_RestGood6.count > 0;

        public override void OnChoice(RestGood good)
        {
            MysteryModel_CardChoice.PopupCardChoice(LogueBookModels.GetCardList(true, true), new MysteryModel_CardChoice.ChoiceResult(this.ExChangeCard), MysteryModel_CardChoice.ChoiceDescType.TransformDesc);
            --PickUpModel_RestGood6.count;
            this.Desc = TextDataModel.GetText("RestGood6_Desc", PickUpModel_RestGood6.count);
        }
    }
}
