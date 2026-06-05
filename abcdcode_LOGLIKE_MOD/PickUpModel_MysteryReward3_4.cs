// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_MysteryReward3_4
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


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
