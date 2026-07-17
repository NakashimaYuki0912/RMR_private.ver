// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_RestGood6
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_RestGood6.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_RestGood6</summary>

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
