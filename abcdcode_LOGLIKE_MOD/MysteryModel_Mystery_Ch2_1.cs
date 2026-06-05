// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch2_1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using LOR_DiceSystem;
using System.Collections.Generic;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    public class MysteryModel_Mystery_Ch2_1 : MysteryBase
    {
        public override void SwapFrame(int id) => base.SwapFrame(id);

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 1)
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new MysteryModel_Mystery_Ch2_1.MeatPieQuest());
            if (this.curFrame.FrameID == 2)
                MysteryBase.SetNextStageCustom(new LorId(LogLikeMod.ModId, 1200011), StageType.Normal);
            base.OnClickChoice(choiceid);
        }

        [HideFromItemCatalog]
        public class MeatPieQuest : GlobalLogueEffectBase
        {
            public int stack;

            public MeatPieQuest() => this.stack = 0;

            public override SaveData GetSaveData()
            {
                SaveData saveData = base.GetSaveData();
                saveData.AddData("stack", new SaveData(this.stack));
                return saveData;
            }

            public override void LoadFromSaveData(SaveData save)
            {
                base.LoadFromSaveData(save);
                this.stack = save.GetInt("stack");
            }

            public override Sprite GetSprite() => LogLikeMod.ArtWorks["GlobalEffect_MeatPieQuest"];

            public override string GetEffectName() => TextDataModel.GetText("MeatPieQuest_Name");

            public override string GetEffectDesc()
            {
                return TextDataModel.GetText("MeatPieQuest_Desc", this.stack);
            }

            public override void OnSkipCardRewardChoose(List<DiceCardXmlInfo> cardlist)
            {
                base.OnSkipCardRewardChoose(cardlist);
                ++this.stack;
                if (this.stack < 5)
                    return;
                if (LogLikeMod.curstagetype == StageType.Boss)
                {
                    MysteryBase.AddStageList(Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 220001)), LogLikeMod.curchaptergrade + 1);
                    Singleton<GlobalLogueEffectManager>.Instance.RemoveEffect(this);
                }
                else
                {
                    MysteryBase.AddStageList(Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 220001)), LogLikeMod.curchaptergrade);
                    Singleton<GlobalLogueEffectManager>.Instance.RemoveEffect(this);
                }
            }
        }
    }
}
