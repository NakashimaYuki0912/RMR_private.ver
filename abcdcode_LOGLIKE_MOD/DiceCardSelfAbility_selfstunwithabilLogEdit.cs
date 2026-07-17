// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardSelfAbility_selfstunwithabilLogEdit
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardSelfAbility_selfstunwithabilLogEdit.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Card self-ability: DiceCardSelfAbility_selfstunwithabilLogEdit</summary>

public class DiceCardSelfAbility_selfstunwithabilLogEdit : DiceCardSelfAbilityBase
{
  public const int _SPECIAL_CARD_ID = 616007;

  public override void OnUseInstance(
    BattleUnitModel unit,
    BattleDiceCardModel self,
    BattleUnitModel targetUnit)
  {
    unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Stun, 1, unit);
    unit.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 3, unit);
    unit.allyCardDetail.AddNewCard(new LorId(LogLikeMod.ModId, 616007));
    SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(unit, unit.faction, unit.hp, unit.breakDetail.breakGauge, unit.bufListDetail.GetBufUIDataList());
    if (unit.bufListDetail.GetActivatedBuf(KeywordBuf.Stun) == null)
      return;
    unit.view.speedDiceSetterUI.SetSpeedDicesBeforeRoll(unit.Book.GetSpeedDiceRule(unit).speedDiceList, unit.faction);
    unit.view.speedDiceSetterUI.DeselectAll();
  }
}
}
