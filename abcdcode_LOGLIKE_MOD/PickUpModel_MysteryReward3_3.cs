// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_MysteryReward3_3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

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
