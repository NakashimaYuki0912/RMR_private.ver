// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Common1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    public class PickUpModel_Common1 : PickUpModelBase
    {
        public override bool IsCanPickUp(UnitDataModel target)
        {
            foreach (AtkResist atkResist in new Dictionary<string, AtkResist>()
            {
              {
                "SResist",
                target.bookItem.equipeffect.SResist
              },
              {
                "SBResist",
                target.bookItem.equipeffect.SBResist
              },
              {
                "PResist",
                target.bookItem.equipeffect.PResist
              },
              {
                "PBResist",
                target.bookItem.equipeffect.PBResist
              },
              {
                "HResist",
                target.bookItem.equipeffect.HResist
              },
              {
                "HBResist",
                target.bookItem.equipeffect.HBResist
              }
            }.Values)
            {
                if (atkResist <= AtkResist.Normal)
                    return true;
            }
            return false;
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            Dictionary<string, AtkResist> dic = new Dictionary<string, AtkResist>();
            dic.Add("SResist", model.UnitData.unitData.bookItem.equipeffect.SResist);
            dic.Add("SBResist", model.UnitData.unitData.bookItem.equipeffect.SBResist);
            dic.Add("PResist", model.UnitData.unitData.bookItem.equipeffect.PResist);
            dic.Add("PBResist", model.UnitData.unitData.bookItem.equipeffect.PBResist);
            dic.Add("HResist", model.UnitData.unitData.bookItem.equipeffect.HResist);
            dic.Add("HBResist", model.UnitData.unitData.bookItem.equipeffect.HBResist);
            string str;
            do
            {
                str = ModdingUtils.PickoneKeyInDic<string, AtkResist>(dic);
                switch (dic[str])
                {
                    case AtkResist.Weak:
                    case AtkResist.Vulnerable:
                    case AtkResist.Normal:
                        goto label_1;
                    default:
                        dic.Remove(str);
                        continue;
                }
            }
            while (dic.Count != 0);
            goto label_3;
        label_1:
            typeof(BookEquipEffect).GetField(str, AccessTools.all).SetValue(model.UnitData.unitData.bookItem.equipeffect, (dic[str] + 1));
            return;
        label_3:;
        }
    }
}
