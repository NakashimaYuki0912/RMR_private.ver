// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood32
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood32.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD {

/// <summary>Pickup model: PickUpModel_ShopGood32</summary>

public class PickUpModel_ShopGood32 : ShopPickUpModel
{
  public PickUpModel_ShopGood32()
  {
    this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570032));
    this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
    this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
    this.id = new LorId(LogLikeMod.ModId, 90032);
  }

  public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

  public override void OnPickUpShop(ShopGoods good)
  {
    ShopPickUpModel.AddPassiveReward(RewardInfo.GetCurChapterCommonReward(LogLikeMod.curchaptergrade));
  }
}
}
