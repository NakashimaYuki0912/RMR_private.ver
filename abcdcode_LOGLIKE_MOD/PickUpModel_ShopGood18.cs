// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood18
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood18.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using GameSave;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood18 : ShopPickUpModel
    {
        public PickUpModel_ShopGood18() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570018));
            this.id = new LorId(LogLikeMod.ModId, 90018);
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new PickUpModel_ShopGood18.TongJang());
        }

        public override string KeywordId => "GlobalEffect_RolandNft";
        public override string KeywordIconId => "ShopPassive18";

        /// <summary>TongJang</summary>

        public class TongJang : OnceEffect
        {
            public static Rarity ItemRarity = Rarity.Rare;

            public TongJang() => this.stack = 0;

            public override int GetStack() => this.stack * 3;

            public override bool CanDupliacte() => false;

            public override void OnStartBattleAfter()
            {
                base.OnStartBattleAfter();
                if (LogLikeMod.curstagetype != StageType.Normal)
                    return;
                this.AddedNew();
            }

            public override void OnClick()
            {
                base.OnClick();
                PassiveAbility_MoneyCheck.AddMoney(this.stack * 3);
                this.Destroy();
            }

            public override string KeywordId => "GlobalEffect_RolandNft";
            public override string KeywordIconId => "ShopPassive18";
        }
    }
}
