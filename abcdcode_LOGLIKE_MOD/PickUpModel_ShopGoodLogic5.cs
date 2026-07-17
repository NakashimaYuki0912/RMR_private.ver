// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGoodLogic5
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGoodLogic5.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_ShopGoodLogic5</summary>

    public class PickUpModel_ShopGoodLogic5 : ShopPickUpModel
    {
        public PickUpModel_ShopGoodLogic5()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8580005));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 80005);
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            LogueBookModels.AddCard(new LorId(LogLikeMod.ModId, 706108), 3);
            MysteryModel_CardCheck.PopupCardCheck(new List<LorId>()
            {
                new LorId(LogLikeMod.ModId, 706108),
                new LorId(LogLikeMod.ModId, 706108),
                new LorId(LogLikeMod.ModId, 706108)
            }, MysteryModel_CardCheck.CheckDescType.RewardDesc);
        }
    }
}
