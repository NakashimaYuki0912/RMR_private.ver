// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch5_1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;
using UI;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryModel_Mystery_Ch5_1 : MysteryBase
    {
        public override void SwapFrame(int id) => base.SwapFrame(id);

        public override void OnEnterChoice(int choiceid)
        {
            base.OnEnterChoice(choiceid);
            if (this.curFrame.FrameID != 0 || choiceid != 0)
                return;
            SingletonBehavior<UIBattleOverlayManager>.Instance.EnableBufOverlay(TextDataModel.GetText("Ch5Event1Effect_Name"), TextDataModel.GetText("Ch5Event1Effect_Desc"), LogLikeMod.ArtWorks["GlobalEffect_Ch5Event1Effect"], this.FrameObj["choice_btn" + choiceid.ToString()]);
        }

        public override void OnExitChoice(int choiceid)
        {
            base.OnExitChoice(choiceid);
            SingletonBehavior<UIBattleOverlayManager>.Instance.DisableOverlay();
        }

        public override void OnClickChoice(int choiceid)
        {
            if (this.curFrame.FrameID == 1)
            {
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
                    alive.LoseHp((int)alive.hp / 5);
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new MysteryModel_Mystery_Ch5_1.Mystery5_1Effect());
            }
            base.OnClickChoice(choiceid);
        }

        public class Mystery5_1Effect : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Rare;

            public override void OnStartBattle()
            {
                base.OnStartBattle();
                if (LogLikeMod.curchaptergrade == ChapterGrade.Grade5)
                    return;
                this.Destroy();
            }

            public override void OnRoundStart(StageController stage)
            {
                base.OnRoundStart(stage);
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(Faction.Player);
                ModdingUtils.SuffleList<BattleUnitModel>(aliveList);
                List<MysteryModel_Mystery_Ch5_1.Mystery5_1Effect.KeywordBufValue> keywordBufValueList = new List<MysteryModel_Mystery_Ch5_1.Mystery5_1Effect.KeywordBufValue>();
                keywordBufValueList.Add(new MysteryModel_Mystery_Ch5_1.Mystery5_1Effect.KeywordBufValue()
                {
                    type = KeywordBuf.Strength,
                    value = 3
                });
                keywordBufValueList.Add(new MysteryModel_Mystery_Ch5_1.Mystery5_1Effect.KeywordBufValue()
                {
                    type = KeywordBuf.Endurance,
                    value = 3
                });
                keywordBufValueList.Add(new MysteryModel_Mystery_Ch5_1.Mystery5_1Effect.KeywordBufValue()
                {
                    type = KeywordBuf.Protection,
                    value = 3
                });
                keywordBufValueList.Add(new MysteryModel_Mystery_Ch5_1.Mystery5_1Effect.KeywordBufValue()
                {
                    type = KeywordBuf.Vulnerable,
                    value = 3
                });
                keywordBufValueList.Add(new MysteryModel_Mystery_Ch5_1.Mystery5_1Effect.KeywordBufValue()
                {
                    type = KeywordBuf.Stun,
                    value = 1
                });
                for (int index = 0; index < (aliveList.Count > 5 ? 5 : aliveList.Count); ++index)
                {
                    MysteryModel_Mystery_Ch5_1.Mystery5_1Effect.KeywordBufValue keywordBufValue = keywordBufValueList[index];
                    aliveList[index].bufListDetail.AddKeywordBufThisRoundByEtc(keywordBufValue.type, keywordBufValue.value);
                }
            }

            public override string KeywordIconId => "GlobalEffect_Ch5Event1Effect";

            public override string KeywordId => "GlobalEffect_Inspiration";

            public class KeywordBufValue
            {
                public KeywordBuf type;
                public int value;
            }
        }
    }
}
