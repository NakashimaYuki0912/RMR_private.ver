// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGoodLogic4
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGoodLogic4 : ShopPickUpModel
    {
        public PickUpModel_ShopGoodLogic4() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8580004));
            this.id = new LorId(LogLikeMod.ModId, 80004);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGoodLogic4.Logic4Effect());
        }

        public override string KeywordId => "GlobalEffect_SuppressiveFire";
        public override string KeywordIconId => "ShopPassiveLogic4";

        public class Logic4Effect : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Rare;

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (behavior.owner.faction == Faction.Enemy || ModdingUtils.GetFieldValue<List<BattlePlayingCardDataInUnitModel>>("_allCardList", Singleton<StageController>.Instance).Find(x => x.owner != behavior.owner && x.card.GetSpec().Ranged == CardRange.Far && behavior.card.target == x.target) == null)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = 1
                });
            }

            public override string KeywordId => "GlobalEffect_SuppressiveFire";
            public override string KeywordIconId => "ShopPassiveLogic4";
        }
    }
}
