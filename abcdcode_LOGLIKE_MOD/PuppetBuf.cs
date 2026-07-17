// -----------------------------------------------------------------------------
// Battle buffer / status effect: PuppetBuf
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PuppetBuf.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using HarmonyLib;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>PuppetBuf</summary>

    public class PuppetBuf : BattleUnitBuf
    {
        public BattleUnitModel owner;

        public override string keywordId => "LogueLikeMod_PuppetBuf";

        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, LogLikeMod.ArtWorks["buff_Puppet"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
            this.owner = owner;
            owner.view.speedDiceSetterUI.SetSpeedDicesBeforeRoll(owner.Book.GetSpeedDiceRule(owner).speedDiceList, owner.faction);
            owner.view.speedDiceSetterUI.DeselectAll();
        }

        public override int SpeedDiceBreakedAdder() => 100;

        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.Destroy();
        }
    }
}
