// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_choiceTest_0
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class DiceCardSelfAbility_choiceTest_0 : DiceCardSelfAbilityBase
    {
        public static string Desc = "[장착 시 발동] 전투 책장 1개를 얻음";
        public static BattleUnitModel unit;

        public override void OnUseInstance(
          BattleUnitModel unit,
          BattleDiceCardModel self,
          BattleUnitModel targetUnit)
        {
            DiceCardSelfAbility_choiceTest_0.unit = unit;
            LogLikeMod.PauseBool = true;
            LogLikeMod.rewards_InStage.Add(new RewardInfo()
            {
                grade = ChapterGrade.GradeAll,
                rewards = Singleton<RewardPassivesList>.Instance.GetChapterData(ChapterGrade.GradeAll, PassiveRewardListType.Custom, new LorId(LogLikeMod.ModId, 6974))
            });
        }

        public static void GiveCard(LorId id)
        {
            if (DiceCardSelfAbility_choiceTest_0.unit != null)
            {
                DiceCardSelfAbility_choiceTest_0.unit.allyCardDetail.AddNewCard(id);
                LogLikeMod.PauseBool = false;
            }
            DiceCardSelfAbility_choiceTest_0.unit = (BattleUnitModel)null;
        }
    }
}
