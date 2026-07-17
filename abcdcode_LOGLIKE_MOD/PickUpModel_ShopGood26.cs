// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood26
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood26.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_ShopGood26</summary>

    public class PickUpModel_ShopGood26 : ShopPickUpModel
    {
        public PickUpModel_ShopGood26()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570026));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90026);
        }

        public override bool IsEquipReward() => true;

        public override bool IsCanPickUp(UnitDataModel target) => base.IsCanPickUp(target);

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            this.GivePassive(new LorId(LogLikeMod.ModId, 8570026), model);
        }

        public override void OnPickUpShop(ShopGoods good) => ShopPickUpModel.AddEquipPage(this.id);
    }
}
