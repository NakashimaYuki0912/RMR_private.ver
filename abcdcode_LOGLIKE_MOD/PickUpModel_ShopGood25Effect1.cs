// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood25Effect1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


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
