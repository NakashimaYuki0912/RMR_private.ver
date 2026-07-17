// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_Sword_getbonecuttingcardplLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_Sword_getbonecuttingcardplLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Dice ability: DiceCardAbility_Sword_getbonecuttingcardplLog</summary>

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
