// -----------------------------------------------------------------------------
// Refactored LOGLIKE/RMR logic: DiceCardSelfAbility_choiceTest_0
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_Refactored\DiceCardSelfAbility_choiceTest_0.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Card self-ability: DiceCardSelfAbility_choiceTest_0</summary>

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
