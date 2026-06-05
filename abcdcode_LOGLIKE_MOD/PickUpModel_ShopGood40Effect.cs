// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood40Effect
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood40Effect : PickUpModelBase
    {
        public static DiceCardXmlInfo curcard;

        public PickUpModel_ShopGood40Effect()
        {
            this.ArtWork = "ShopPassive40";
            this.Name = TextDataModel.GetText("ShopGood40_1Name");
            if (PickUpModel_ShopGood40Effect.curcard != null)
                this.Desc = TextDataModel.GetText("ShopGood40_1Desc", PickUpModel_ShopGood40Effect.curcard.Name);
            else
                this.Desc = TextDataModel.GetText("ShopGood40_1Desc");
            this.id = new LorId(LogLikeMod.ModId, 900401);
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            model.allyCardDetail.AddNewCard(PickUpModel_ShopGood40Effect.curcard.id).AddBuf((BattleDiceCardBuf)new PickUpModel_ShopGood40Effect.BattleDiceCardBuf_Shop40costZeroCard());
            Singleton<LogAssetBundleManager>.Instance.LoadEffect(model.view.transform, (Vector3)new Vector2(1f, 1f), (Vector3)new Vector2(0.0f, -5f), "loglike_immediately_craft");
            PickUpModel_ShopGood40Effect.curcard = (DiceCardXmlInfo)null;
        }

        public class BattleDiceCardBuf_Shop40costZeroCard : BattleDiceCardBuf
        {
            public override DiceCardBufType bufType => DiceCardBufType.CostDecrease;

            public override void OnUseCard(BattleUnitModel owner)
            {
                base.OnUseCard(owner);
                this.Destroy();
            }

            public override int GetCost(int oldCost) => 0;
        }
    }
}
