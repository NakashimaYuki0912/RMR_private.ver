// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_EquipDefault
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_EquipDefault.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_EquipDefault</summary>

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
            BookXmlInfo bookXml = RewardingModel.GetBookDataOriginAware(this.id);
            if (bookXml == null)
                return;
            BookModel bookModel = new BookModel(bookXml);
            bookModel.instanceId = LogueBookModels.nextinstanceid++;
            bookModel.TryGainUniquePassive();
            LogueBookModels.booklist.Add(bookModel);
        }

        public override void OnPickUp(BattleUnitModel model)
        {
        }
    }
}
