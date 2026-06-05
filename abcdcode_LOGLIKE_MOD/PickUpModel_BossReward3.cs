// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BossReward3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_BossReward3 : PickUpModelBase
    {
        public static int[] MoneyRewardTable = new int[6]
        {
            30,
            40,
            50,
            60,
            70,
            80
        };
        public override string KeywordId => "GlobalEffect_LuckyTael";
        public override string KeywordIconId => "BossReward3";
        public PickUpModel_BossReward3()
        {
            int index = (int)LogLikeMod.curchaptergrade;
            if (index > 5)
                index = 5;
            var effectText = LogueEffectXmlList.Instance.GetEffectInfo(this.KeywordId, LogLikeMod.ModId, MoneyRewardTable[index]);
            this.Desc = effectText.Desc;
            this.Name = effectText.Name;
            this.FlaverText = effectText.FlavorText;
            this.ArtWork = this.KeywordIconId;
        }

        public override void OnPickUp()
        {
            base.OnPickUp();
            int index = (int)LogLikeMod.curchaptergrade;
            if (index > 5)
                index = 5;
            LogueBookModels.AddMoney(PickUpModel_BossReward3.MoneyRewardTable[index]);
            MysteryBase.AddStageList(Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 111001)), LogLikeMod.curchaptergrade + 1);
        }
    }
}
