// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood18
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

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
