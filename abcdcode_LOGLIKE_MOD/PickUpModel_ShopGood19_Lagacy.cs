// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood19_Lagacy
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood19_Lagacy.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Pickup model: PickUpModel_ShopGood19_Lagacy</summary>

public class PickUpModel_ShopGood19_Lagacy : ShopPickUpModel
{
  public PickUpModel_ShopGood19_Lagacy()
  {
    this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570019));
    this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
    this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
    this.id = new LorId(LogLikeMod.ModId, 90019);
  }

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    this.GivePassive(new LorId(LogLikeMod.ModId, 8570019), model);
  }

  public override void OnPickUpShop(ShopGoods good)
  {
    ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90019));
  }
}
}
