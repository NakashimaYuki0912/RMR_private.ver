// -----------------------------------------------------------------------------
// Battle buffer / status effect: MaxUpMinDownBuf
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\MaxUpMinDownBuf.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using HarmonyLib;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>MaxUpMinDownBuf</summary>

    public class MaxUpMinDownBuf : BattleUnitBuf
    {
        public override string keywordId => "LogueLikeMod_MaxUpMinDownBuf";

        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            this._bufIcon = LogLikeMod.ArtWorks["buff_MaxUpMinDown"];
            this._iconInit = true;
        }

        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            --this.stack;
            if (this.stack > 0)
                return;
            this.Destroy();
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                min = -1,
                max = 2
            });
        }

        public static MaxUpMinDownBuf IshaveBuf(BattleUnitModel target, bool findready = false)
        {
            foreach (BattleUnitBuf activatedBuf in target.bufListDetail.GetActivatedBufList())
            {
                if (activatedBuf is MaxUpMinDownBuf)
                    return activatedBuf as MaxUpMinDownBuf;
            }
            if (findready)
            {
                foreach (BattleUnitBuf readyBuf in target.bufListDetail.GetReadyBufList())
                {
                    if (readyBuf is MaxUpMinDownBuf)
                        return readyBuf as MaxUpMinDownBuf;
                }
            }
            return (MaxUpMinDownBuf)null;
        }

        public static void GiveBufThisRound(BattleUnitModel target, int stack)
        {
            MaxUpMinDownBuf maxUpMinDownBuf = MaxUpMinDownBuf.IshaveBuf(target);
            if (maxUpMinDownBuf != null)
            {
                maxUpMinDownBuf.stack += stack;
            }
            else
            {
                MaxUpMinDownBuf buf = new MaxUpMinDownBuf();
                buf.stack = stack;
                buf.Init(target);
                target.bufListDetail.AddBuf((BattleUnitBuf)buf);
            }
        }
    }
}
