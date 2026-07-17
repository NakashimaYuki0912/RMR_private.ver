// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGoodUnion3
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGoodUnion3.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Pickup model: PickUpModel_ShopGoodUnion3</summary>

public class PickUpModel_ShopGoodUnion3 : ShopPickUpModel
{
  public PickUpModel_ShopGoodUnion3()
  {
    this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8581003));
    this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
    this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
    this.id = new LorId(LogLikeMod.ModId, 81003);
  }

  public override bool IsEquipReward() => true;

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    this.GivePassive(new LorId(LogLikeMod.ModId, 8581003), model);
  }

  public override void OnPickUpShop(ShopGoods good) => ShopPickUpModel.AddEquipPage(this.id);
}
}
