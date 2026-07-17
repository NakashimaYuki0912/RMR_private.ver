// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_BossReward5
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_BossReward5.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    /// <summary>Pickup model: PickUpModel_BossReward5</summary>
    public class PickUpModel_BossReward5 : PickUpModelBase
    {
        public override string KeywordId => "GlobalEffect_CupOfGreed";
        public override string KeywordIconId => "BossReward5";
        public PickUpModel_BossReward5():base()
        {

        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find(x => x is PickUpModel_BossReward5.WildCardEffect) == null;
        }

        public override void OnPickUp()
        {
            base.OnPickUp();
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new PickUpModel_BossReward5.WildCardEffect());
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        [HideFromItemCatalog]
        public class WildCardEffect : GlobalLogueEffectBase
        {
            public override string KeywordId => "GlobalEffect_CupOfGreed_Effect";
            public override string KeywordIconId => "BossReward5";

            public bool CanUse;

            public static Rarity ItemRarity = Rarity.Rare;

            public override void OnStartBattle()
            {
                base.OnStartBattle();
                this.CanUse = true;
            }

            public override void OnClick()
            {
                base.OnClick();
                if (!this.CanUse)
                    return;
                MysteryModel_CardChoice.PopupCardChoice(LogueBookModels.GetCardList(), new MysteryModel_CardChoice.ChoiceResult(this.ChooseCard), MysteryModel_CardChoice.ChoiceDescType.ChooseDesc);
                this.CanUse = false;
            }

            public override string GetEffectDesc()
            {
                LogueEffectXmlInfo info = LogueEffectXmlList.Instance.GetEffectInfo(this.KeywordId, LogLikeMod.ModId, PickUpModel_BossReward5_1.card == null ? "X" : PickUpModel_BossReward5_1.card.Name);
                return info.Desc;
            }

            public void ChooseCard(MysteryModel_CardChoice mystery, DiceCardItemModel model)
            {
                PickUpModel_BossReward5_1.card = model.ClassInfo;
                ShopPickUpModel.AddPassiveReward(new LorId(LogLikeMod.ModId, 1000051));
                Singleton<MysteryManager>.Instance.EndMystery(mystery);
            }
        }
    }
}
