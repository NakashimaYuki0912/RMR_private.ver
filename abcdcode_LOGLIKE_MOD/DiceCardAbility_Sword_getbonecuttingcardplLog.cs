// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardAbility_Sword_getbonecuttingcardplLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class DiceCardAbility_Sword_getbonecuttingcardplLog : DiceCardAbilityBase
{
  public const int _bonecutting_ID = 512006;

  public override string[] Keywords
  {
    get => new string[1]{ "Giveuptheflesh_Keyword" };
  }

  public override void OnLoseParrying()
  {
    base.OnLoseParrying();
    this.owner.allyCardDetail.AddNewCard(Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 512006)).id);
  }
}
}
