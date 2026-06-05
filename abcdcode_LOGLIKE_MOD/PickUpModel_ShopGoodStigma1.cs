// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGoodStigma1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGoodStigma1 : ShopPickUpModel
    {
        public PickUpModel_ShopGoodStigma1() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8582001));
            this.id = new LorId(LogLikeMod.ModId, 82001);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new PickUpModel_ShopGoodStigma1.Stigma1Effect());
        }
        public override string KeywordId => "GlobalEffect_SlowBurn";
        public override string KeywordIconId => "ShopPassiveStigma1";

        public class Stigma1Effect : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Unique;

            public override string KeywordId => "GlobalEffect_SlowBurn";
            public override string KeywordIconId => "ShopPassiveStigma1";
        }
    }
}
