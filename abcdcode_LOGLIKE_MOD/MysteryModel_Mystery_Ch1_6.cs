// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch1_6
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryModel_Mystery_Ch1_6 : MysteryBase
    {
        public override void OnClickChoice(int choiceid)
        {
            if (choiceid == 0)
                MysteryBase.SetNextStageCustom(new LorId(LogLikeMod.ModId, 1100061), StageType.Normal);
            base.OnClickChoice(choiceid);
        }
    }
}
