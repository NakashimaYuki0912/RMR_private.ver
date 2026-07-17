// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGoodUnion10
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGoodUnion10.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Pickup model: PickUpModel_ShopGoodUnion10</summary>

public class PickUpModel_ShopGoodUnion10 : ShopPickUpModel
{
  public PickUpModel_ShopGoodUnion10()
  {
    this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8581010));
    this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
    this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
    this.id = new LorId(LogLikeMod.ModId, 81010);
  }

  public override bool IsEquipReward() => true;

  public override bool IsCanPickUp(UnitDataModel target) => base.IsCanPickUp(target);

  public override string[] Keywords
  {
    get => new string[1]{ "LogueLikeMod_LuckyBuf_Page" };
  }

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    this.GivePassive(new LorId(LogLikeMod.ModId, 8581010), model);
  }

  public override void OnPickUpShop(ShopGoods good) => ShopPickUpModel.AddEquipPage(this.id);
}
}
