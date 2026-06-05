// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGoodMook7
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGoodMook7 : ShopPickUpModel
    {
        public PickUpModel_ShopGoodMook7() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8583007));
            this.id = new LorId(LogLikeMod.ModId, 83007);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGoodMook7.Mook7Effect());
        }

        public override string KeywordId => "GlobalEffect_KillingAura";
        public override string KeywordIconId => "ShopPassiveMook7";

        public class Mook7Effect : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Rare;
            public override float DmgFactor(
              BattleUnitModel model,
              int dmg,
              DamageType type = DamageType.ETC,
              KeywordBuf keyword = KeywordBuf.None)
            {
                return type <= DamageType.Attack || model.faction != Faction.Enemy ? base.DmgFactor(model, dmg, type, keyword) : 2f;
            }

            public override string KeywordId => "GlobalEffect_KillingAura";
            public override string KeywordIconId => "ShopPassiveMook7";
        }
    }
}
