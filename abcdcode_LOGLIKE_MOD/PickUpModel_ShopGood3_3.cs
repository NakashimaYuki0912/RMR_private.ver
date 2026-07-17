// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood3_3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood3_3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_ShopGood3_3</summary>

    public class PickUpModel_ShopGood3_3 : ShopPickUpModel
    {
        public PickUpModel_ShopGood3_3()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8573003));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 30003);
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            this.GivePassive(new LorId(LogLikeMod.ModId, 8573003), model);
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 30003));
        }
    }
}
