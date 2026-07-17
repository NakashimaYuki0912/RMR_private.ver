// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood25Effect1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood25Effect1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood25Effect1 : PickUpModelBase
    {
        public PickUpModel_ShopGood25Effect1()
        {
            this.Name = TextDataModel.GetText("ShopGood25_1Name");
            this.Desc = TextDataModel.GetText("ShopGood25_1Desc");
            this.id = new LorId(LogLikeMod.ModId, 900251);
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            PuppeteerBuf buf = new PuppeteerBuf();
            model.bufListDetail.AddBuf((BattleUnitBuf)buf);
            PickUpModel_ShopGood25.Shop25Effect.curpuppeteer = buf;
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfileAll();
        }
    }
}
