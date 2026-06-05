// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_EquipDefault
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    public class PickUpModel_EquipDefault : PickUpModelBase
    {
        public static int EquipLimit(Rarity rare)
        {
            switch (rare)
            {
                case Rarity.Common:
                    return 5;
                case Rarity.Uncommon:
                    return 4;
                case Rarity.Rare:
                    return 3;
                case Rarity.Unique:
                    return 1;
                default:
                    return 1;
            }
        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            List<BookModel> all = LogueBookModels.booklist.FindAll(x => x.ClassInfo.id == this.id);
            if (all.Count == 0)
                return true;
            Rarity rarity = all[0].Rarity;
            return all.Count < PickUpModel_EquipDefault.EquipLimit(rarity);
        }

        public virtual void EquipPage(BookXmlInfo equip, BattleUnitModel model)
        {
            LogLikeMod.PlayerEquipOrders.Add(new EquipChangeOrder()
            {
                equip = equip,
                model = model.UnitData
            });
        }

        public override void OnPickUp()
        {
            base.OnPickUp();
            BookModel bookModel = new BookModel(Singleton<BookXmlList>.Instance.GetData(this.id));
            bookModel.instanceId = LogueBookModels.nextinstanceid++;
            bookModel.TryGainUniquePassive();
            LogueBookModels.booklist.Add(bookModel);
        }

        public override void OnPickUp(BattleUnitModel model)
        {
        }
    }
}
