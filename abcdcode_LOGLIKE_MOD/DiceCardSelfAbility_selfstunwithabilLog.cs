// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_selfstunwithabilLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_selfstunwithabilLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_selfstunwithabilLog</summary>

public class DiceCardSelfAbility_selfstunwithabilLog : DiceCardSelfAbilityBase
{
  public const int _SPECIAL_CARD_ID = 616007;

  public override void OnUseInstance(
    BattleUnitModel unit,
    BattleDiceCardModel self,
    BattleUnitModel targetUnit)
  {
    unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Stun, 1, unit);
    unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 4, unit);
    LorId id = Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 616007)).id;
    unit.allyCardDetail.AddNewCard(id);
    SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(unit, unit.faction, unit.hp, unit.breakDetail.breakGauge, unit.bufListDetail.GetBufUIDataList());
    if (unit.bufListDetail.GetActivatedBuf(KeywordBuf.Stun) == null)
      return;
    unit.view.speedDiceSetterUI.SetSpeedDicesBeforeRoll(unit.Book.GetSpeedDiceRule(unit).speedDiceList, unit.faction);
    unit.view.speedDiceSetterUI.DeselectAll();
  }
}
}
