// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_MysteryReward3_3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_MysteryReward3_3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_MysteryReward3_3 : PickUpModelBase
    {
        public PickUpModel_MysteryReward3_3()
        {
            this.Name = TextDataModel.GetText("MysteryCh3_3RewardName");
            this.Desc = TextDataModel.GetText("MysteryCh3_3RewardDesc");
            this.FlaverText = "";
            this.ArtWork = "Mystery_Ch3_3Reward";
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            LogueBookModels.AddPlayerStat(model.UnitData, new LogStatAdder()
            {
                maxhp = Random.Range(5, 11),
                maxbreak = Random.Range(5, 11),
                speedmin = -1,
                speedmax = -1
            });
        }
    }
}
