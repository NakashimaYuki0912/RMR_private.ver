// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGoodLogic6
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGoodLogic6.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Pickup model: PickUpModel_ShopGoodLogic6</summary>

public class PickUpModel_ShopGoodLogic6 : ShopPickUpModel
{
  public PickUpModel_ShopGoodLogic6()
  {
    this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8580006));
    this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
    this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
    this.id = new LorId(LogLikeMod.ModId, 80006);
  }

  public override bool IsEquipReward() => true;

  public override bool IsCanPickUp(UnitDataModel target) => base.IsCanPickUp(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    this.GivePassive(new LorId(LogLikeMod.ModId, 8580006), model);
  }

  public override void OnPickUpShop(ShopGoods good) => ShopPickUpModel.AddEquipPage(this.id);
}
}
