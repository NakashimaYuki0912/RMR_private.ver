// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardSelfAbility_selfstunwithabilLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

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
