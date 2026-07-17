// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_MysteryReward3_4
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_MysteryReward3_4.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_MysteryReward3_4 : PickUpModelBase
    {
        public PickUpModel_MysteryReward3_4()
        {
            this.Name = TextDataModel.GetText("MysteryCh3_4RewardName");
            this.Desc = TextDataModel.GetText("MysteryCh3_4RewardDesc");
            this.FlaverText = "";
            this.ArtWork = "Mystery_Ch3_4Reward";
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            model.RecoverHP(10000);
            UnitDataModel unitData = model.UnitData.unitData;
            LogueBookModels.playersperpassives[unitData].Clear();
            BookXmlInfo data = Singleton<BookXmlList>.Instance.GetData(RewardingModel.GetReward(Singleton<RewardPassivesList>.Instance.GetChapterData(ChapterGrade.Grade3, PassiveRewardListType.CommonReward, LorId.None)).id);
            if (data == null)
                return;
            LogLikeMod.PlayerEquipOrders.Add(new EquipChangeOrder()
            {
                equip = data,
                model = model.UnitData
            });
        }
    }
}
