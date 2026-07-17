// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_Common1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_Common1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using HarmonyLib;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_Common1</summary>

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
