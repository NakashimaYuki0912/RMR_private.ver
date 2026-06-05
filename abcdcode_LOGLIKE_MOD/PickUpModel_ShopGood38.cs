// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood38
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood38 : ShopPickUpModel
    {
        public PickUpModel_ShopGood38() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570038));
            this.id = new LorId(LogLikeMod.ModId, 90038);
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            MaxUpMinDownBuf.GiveBufThisRound(model, 2);
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood38.Shop38Effect());
        }

        public override string KeywordId => "GlobalEffect_G1sSteroids";
        public override string KeywordIconId => "ShopPassive38";

        public class Shop38Effect : OnceEffect
        {
            public static Rarity ItemRarity = Rarity.Rare;

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                    return;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90038));
                this.Use();
            }

            public override string KeywordId => "GlobalEffect_G1sSteroids";
            public override string KeywordIconId => "ShopPassive38";
        }
    }
}
