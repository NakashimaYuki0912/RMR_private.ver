// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Common3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{

    public class PickUpModel_Common3 : PickUpModelBase
    {
        public override bool IsCanPickUp(UnitDataModel target)
        {
            int num = 0;
            for (int index = 0; index < LogueBookModels.playersPick[target].Count; ++index)
            {
                if (LogueBookModels.playersPick[target][index] == 15210003)
                    ++num;
                if (num == 3)
                    return false;
            }
            return true;
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            ++model.UnitData.unitData.bookItem.equipeffect.SpeedMin;
            ++model.UnitData.unitData.bookItem.equipeffect.Speed;
        }
    }
}
