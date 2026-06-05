// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BossReward2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_BossReward2 : PickUpModelBase
    {
        public static int[] EquipRewardTable = new int[6]
        {
            1,
            2,
            3,
            4,
            5,
            5
        };
        public static int[] CardRewardTable = new int[6]
        {
            3,
            5,
            7,
            9,
            11,
            13
        };

        public override string KeywordId => "GlobalEffect_Retrospection";
        public override string KeywordIconId 
        { 
            get
            { 
                switch (LogLikeMod.curchaptergrade)
                {
                    case ChapterGrade.Grade1:
                    case ChapterGrade.Grade2:
                        return "BossReward2_1";
                        
                    case ChapterGrade.Grade3:
                        return "BossReward2_2";
                        
                    case ChapterGrade.Grade4:
                    case ChapterGrade.Grade5:
                        return "BossReward2_" + ((double)Random.value > 0.5 ? "3" : "4");
                        
                    default:
                        return "BossReward2_1";

                }
            } 
        }
        public PickUpModel_BossReward2() 
        {
            int index1 = (int)LogLikeMod.curchaptergrade;
            if (index1 > 5)
                index1 = 5;
            LogueEffectXmlInfo info = LogueEffectXmlList.Instance.GetEffectInfo(this.KeywordId, LogLikeMod.ModId, EquipRewardTable[index1], CardRewardTable[index1]);
            this.Desc = info.Desc;
            this.Name = info.Name;
            this.FlaverText = info.FlavorText;
            this.ArtWork = this.KeywordIconId;
        } // please work God I beg you

        public override void OnPickUp()
        {
            base.OnPickUp();
            int index1 = (int)LogLikeMod.curchaptergrade;
            if (index1 > 5)
                index1 = 5;
            for (int index2 = 0; index2 < PickUpModel_BossReward2.CardRewardTable[index1]; ++index2)
            {
                int num1 = 1;
                float num2 = Random.value;
                if ((double)num2 > 0.9)
                    num1 = 4;
                else if ((double)num2 > 0.7)
                    num1 = 3;
                else if ((double)num2 > 0.4)
                    num1 = 2;
                LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 1000 * (index1 + 1) + num1)));
            }
            for (int index3 = 0; index3 < PickUpModel_BossReward2.EquipRewardTable[index1]; ++index3)
                LogLikeMod.rewards_passive.Add(new RewardInfo()
                {
                    grade = LogLikeMod.curchaptergrade,
                    rewards = Singleton<RewardPassivesList>.Instance.GetChapterData(LogLikeMod.curchaptergrade, PassiveRewardListType.CommonReward, new LorId(-1))
                });
        }
    }
}
