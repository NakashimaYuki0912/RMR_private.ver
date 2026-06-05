// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood25Effect2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


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
