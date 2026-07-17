// -----------------------------------------------------------------------------
// Combat dice/card ability script: DiceCardAbility_carnivalLog
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\DiceCardAbility_carnivalLog.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Dice ability: DiceCardAbility_carnivalLog</summary>

    public class DiceCardAbility_carnivalLog : DiceCardAbilityBase
    {
        public override void AfterAction()
        {
            LorId id = Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(LogLikeMod.ModId, 401007)).id;
            this.owner.allyCardDetail.AddNewCard(id);
            this.card.target?.allyCardDetail.AddNewCard(id);
        }
    }
}
