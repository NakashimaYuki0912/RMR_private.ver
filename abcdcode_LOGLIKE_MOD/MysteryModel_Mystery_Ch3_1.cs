// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch3_1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class MysteryModel_Mystery_Ch3_1 : MysteryBase
{
  public override void SwapFrame(int id) => base.SwapFrame(id);

  public override void OnClickChoice(int choiceid)
  {
    MysteryBase.SetNextStageCustom(new LorId(LogLikeMod.ModId, 1300011), StageType.Normal);
    Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase) new MysteryModel_Mystery_Ch3_1.PlayerSetting3_1());
    base.OnClickChoice(choiceid);
  }

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
