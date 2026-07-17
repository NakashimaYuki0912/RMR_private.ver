// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood25Effect2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood25Effect2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood25Effect2 : PickUpModelBase
    {
        public PickUpModel_ShopGood25Effect2()
        {
            this.Name = TextDataModel.GetText("ShopGood25_2Name");
            this.Desc = TextDataModel.GetText("ShopGood25_2Desc");
            this.id = new LorId(LogLikeMod.ModId, 900252);
        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return PickUpModel_ShopGood25.Shop25Effect.curpuppeteer.owner.UnitData.unitData != target && base.IsCanPickUp(target);
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            PuppetBuf buf = new PuppetBuf();
            model.bufListDetail.AddBuf((BattleUnitBuf)buf);
            PickUpModel_ShopGood25.Shop25Effect.curpuppet = buf;
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfileAll();
        }
    }
}
