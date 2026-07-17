// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood40
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood40.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;
using System.Collections.Generic;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood40 : ShopPickUpModel
    {
        public PickUpModel_ShopGood40()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570040));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90040);
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood40.Shop40Effect());
        }

        /// <summary>Shop component: Shop40Effect</summary>

        public class Shop40Effect : OnceEffect
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive40"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570040));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570040));
            }

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                    return;
                List<DiceCardItemModel> cardlist = new List<DiceCardItemModel>();
                foreach (DiceCardXmlInfo pickUpCard in RewardingModel.PickUpCards(Singleton<CardDropValueList>.Instance.GetData(new LorId(LogLikeMod.ModId, 11001 + 1000 * (int)LogLikeMod.curchaptergrade))))
                    cardlist.Add(new DiceCardItemModel(pickUpCard)
                    {
                        num = 1
                    });
                MysteryModel_CardChoice.PopupCardChoice(cardlist, new MysteryModel_CardChoice.ChoiceResult(this.OnChoiceCard), MysteryModel_CardChoice.ChoiceDescType.ChooseDesc);
                this.Use();
            }

            public void OnChoiceCard(MysteryModel_CardChoice mystery, DiceCardItemModel model)
            {
                PickUpModel_ShopGood40Effect.curcard = model.ClassInfo;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 900401));
                Singleton<MysteryManager>.Instance.EndMystery(mystery);
            }
        }
    }
}
