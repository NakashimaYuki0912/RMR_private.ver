// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood12
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood12.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood12 : ShopPickUpModel
    {
        public PickUpModel_ShopGood12() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570012));
            this.id = new LorId(LogLikeMod.ModId, 90012);
        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return base.IsCanPickUp(target) && !target.IsDead();
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(new LorId(LogLikeMod.ModId, 90012)));
            if (playingCard != null)
            {
                foreach (BattleDiceBehavior diceCardBehavior in playingCard.CreateDiceCardBehaviorList())
                {
                    diceCardBehavior.behaviourInCard.Min = (int)(2 + LogLikeMod.curchaptergrade);
                    diceCardBehavior.behaviourInCard.Dice = (int)(5 + LogLikeMod.curchaptergrade);
                    model.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(playingCard, diceCardBehavior);
                    SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(model, model.faction, model.hp, model.breakDetail.breakGauge, model.bufListDetail.GetBufUIDataList());
                }
            }
            Singleton<LogueSaveManager>.Instance.AddToObtainCount(this, -1);
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood12.TrashShield());
        }

        public override string KeywordId => "GlobalEffect_ScrapShield";
        public override string KeywordIconId => "ShopPassive12";
        /// <summary>TrashShield</summary>
        public class TrashShield : OnceEffect
        {
            public static Rarity ItemRarity = Rarity.Uncommon;

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                    return;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 90012));
                this.Use();
            }

            public override string KeywordId => "GlobalEffect_ScrapShield";
            public override string KeywordIconId => "ShopPassive12";
        }
    }
}
