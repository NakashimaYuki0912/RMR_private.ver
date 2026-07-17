// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGoodUnion8
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGoodUnion8.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Pickup model: PickUpModel_ShopGoodUnion8</summary>

public class PickUpModel_ShopGoodUnion8 : ShopPickUpModel
{
  public PickUpModel_ShopGoodUnion8()
  {
    this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8581008));
    this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
    this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
    this.id = new LorId(LogLikeMod.ModId, 81008);
  }

  public override bool IsCanPickUp(UnitDataModel target) => true;

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    LogueBookModels.AddPlayerStat(model.UnitData, new LogStatAdder()
    {
      maxhp = 10,
      maxbreak = 5
    });
  }

  public override void OnPickUpShop(ShopGoods good)
  {
    ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 81008));
  }
}
}
