// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MaxUpMinDownBuf
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;


namespace abcdcode_LOGLIKE_MOD
{

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
