// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood41
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood41.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood41 : ShopPickUpModel
    {
        public PickUpModel_ShopGood41()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570041));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90041);
        }

        public override bool IsCanAddShop()
        {
            return !LogueBookModels.shopPick.Contains(this.id);
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new PickUpModel_ShopGood41.Shop41Effect());
        }

        /// <summary>Shop component: Shop41Effect</summary>

        public class Shop41Effect : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Uncommon;

            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive41"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570041));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570041));
            }

            public override void OnRoundStart(StageController stage)
            {
                base.OnRoundStart(stage);
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(Faction.Player);
                List<BattleUnitModel> all = aliveList.FindAll((Predicate<BattleUnitModel>)(x => x.MaxPlayPoint > x.PlayPoint));
                if (all.Count > 0)
                    all.RandomPickUp<BattleUnitModel>(1)[0].cardSlotDetail.RecoverPlayPoint(1);
                else
                    aliveList.RandomPickUp<BattleUnitModel>(1)[0].cardSlotDetail.RecoverPlayPoint(1);
            }
        }
    }
}
