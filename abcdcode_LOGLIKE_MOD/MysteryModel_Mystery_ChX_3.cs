// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_ChX_3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using TMPro;


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryModel_Mystery_ChX_3 : MysteryBase
    {
        public int[] CardValue = new int[6] { 3, 4, 5, 6, 7, 8 };
        public int[] EquipValue = new int[6] { 1, 1, 2, 2, 3, 3 };
        public int[] MoneyValue = new int[6]
        {
            12,
            17,
            22,
            27,
            32,
            37
        };

        public override void SwapFrame(int id)
        {
            base.SwapFrame(id);
            if (id != 0)
                return;
            int curchaptergrade = (int)LogLikeMod.curchaptergrade;
            this.ReformatButton(0, this.CardValue[curchaptergrade]);
            this.ReformatButton(1, this.EquipValue[curchaptergrade]);
            this.ReformatButton(2, this.MoneyValue[curchaptergrade]);
        }

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 0)
            {
                int curchaptergrade = (int)LogLikeMod.curchaptergrade;
                if (choiceid == 0)
                {
                    for (int index = 0; index < this.CardValue[curchaptergrade]; ++index)
                        LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 1000 * (curchaptergrade + 1) + 1)));
                    MysteryModel_CardReward.PopupCardReward_AutoSave();
                }
                if (choiceid == 1)
                {
                    for (int index = 0; index < this.EquipValue[curchaptergrade]; ++index)
                        LogLikeMod.rewards_passive.Add(new RewardInfo()
                        {
                            grade = LogLikeMod.curchaptergrade,
                            rewards = Singleton<RewardPassivesList>.Instance.GetChapterData(LogLikeMod.curchaptergrade, PassiveRewardListType.CommonReward, new LorId(-1))
                        });
                }
                if (choiceid == 2)
                    LogueBookModels.AddMoney(this.MoneyValue[curchaptergrade]);
                if (choiceid == 3)
                {
                    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
                        alive.RecoverHP(alive.MaxHp / 4);
                }
            }
            base.OnClickChoice(choiceid);
        }
    }
}
