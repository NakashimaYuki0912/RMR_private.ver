// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch6_1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryModel_Mystery_Ch6_1 : MysteryBase
    {
        public override void SwapFrame(int id) => base.SwapFrame(id);

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0)
            {
                if (choiceid == 0)
                {
                    foreach (DiceCardItemModel diceCardItemModel in LogueBookModels.GetCardList(true, true).FindAll(x => x.ClassInfo.CheckCanUpgrade() && x.ClassInfo.Chapter < 6))
                    {
                        int num = diceCardItemModel.num;
                        LorId id = diceCardItemModel.ClassInfo.id;
                        if (num > 0)
                        {
                            for (int index = 0; index < num; ++index)
                            {
                                LogueBookModels.AddUpgradeCard(id, true);
                                LogueBookModels.DeleteCard(id);
                            }
                        }
                    }
                    Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new MysteryModel_Mystery_Ch6_1.Mystery6_1Effect1());
                }
                if (choiceid == 1)
                {
                    for (int index = 0; index < 3; ++index)
                    {
                        LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 6001)));
                        LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 6002)));
                        LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 6003)));
                    }
                    LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 6004)));
                }
                if (choiceid == 2)
                    Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new MysteryModel_Mystery_Ch6_1.Mystery6_1Effect2());
            }
            base.OnClickChoice(choiceid);
        }

        public class Mystery6_1Effect1 : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Special;

            public override void RewardClearStageInterrupt() => LogLikeMod.rewards.Clear();

            public override void RewardInStageInterrupt() => LogLikeMod.rewards.Clear();

            public override string KeywordId => "GlobalEffect_THEPAST";
            public override string KeywordIconId => "Ch6Event1Effect1";
        }

        public class Mystery6_1Effect2 : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Unique;

            public override LorId InvenAddCardChange(LorId baseid)
            {
                try
                {
                    if (ItemXmlDataList.instance.GetCardItem(baseid).CheckCanUpgrade())
                        return Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(baseid).id;
                }
                catch
                {
                    return baseid;
                }
                return baseid;
            }

            public override string KeywordIconId => "Ch6Event1Effect2";

            public override string KeywordId => "GlobalEffect_NineFourteen";
        }
    }
}
