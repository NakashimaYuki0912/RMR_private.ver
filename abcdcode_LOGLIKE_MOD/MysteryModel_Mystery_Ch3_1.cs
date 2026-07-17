// -----------------------------------------------------------------------------
// Mystery / event node model: MysteryModel_Mystery_Ch3_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch3_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Mystery node model: MysteryModel_Mystery_Ch3_1</summary>

public class MysteryModel_Mystery_Ch3_1 : MysteryBase
{
  public override void SwapFrame(int id) => base.SwapFrame(id);

  public override void OnClickChoice(int choiceid)
  {
    MysteryBase.SetNextStageCustom(new LorId(LogLikeMod.ModId, 1300011), StageType.Normal);
    Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase) new MysteryModel_Mystery_Ch3_1.PlayerSetting3_1());
    base.OnClickChoice(choiceid);
  }

  /// <summary>PlayerSetting3_1</summary>

  public class PlayerSetting3_1 : GlobalLogueEffectBase
  {
    public override void OnStartBattleAfter()
    {
      base.OnStartBattleAfter();
      foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList(Faction.Player))
      {
        FormationModel formationModel = new FormationModel(Singleton<FormationXmlList>.Instance.GetData(230001));
        battleUnitModel.formation = formationModel.PostionList[battleUnitModel.index];
      }
      this.Destroy();
    }
  }
}
}
