// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch6_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch6_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Mystery node model: MysteryModel_Mystery_Ch6_1</summary>

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

        /// <summary>Mystery6_1Effect1</summary>

        public class Mystery6_1Effect1 : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Special;

            public override void RewardClearStageInterrupt() => LogLikeMod.rewards.Clear();

            public override void RewardInStageInterrupt() => LogLikeMod.rewards.Clear();

            public override string KeywordId => "GlobalEffect_THEPAST";
            public override string KeywordIconId => "Ch6Event1Effect1";
        }

        /// <summary>Mystery6_1Effect2</summary>

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
