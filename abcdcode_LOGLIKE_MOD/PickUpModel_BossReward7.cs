// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_BossReward7
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_BossReward7.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_BossReward7 : PickUpModelBase
    {
        public PickUpModel_BossReward7()
        {
            this.Name = TextDataModel.GetText("BossReward7Name");
            this.Desc = TextDataModel.GetText("BossReward7Desc");
            this.FlaverText = TextDataModel.GetText("BossRewardFlaverText");
            this.ArtWork = "BossReward7";
        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find((Predicate<GlobalLogueEffectBase>)(x => x is PickUpModel_BossReward7.LimitedEditionEffect)) == null;
        }

        public override void OnPickUp()
        {
            base.OnPickUp();
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_BossReward7.LimitedEditionEffect());
        }

        [HideFromItemCatalog]
        public class LimitedEditionEffect : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Unique;

            public override Sprite GetSprite() => LogLikeMod.ArtWorks["BossReward7"];

            public override string GetEffectName() => TextDataModel.GetText("BossReward7Name");

            public override string GetEffectDesc() => TextDataModel.GetText("BossReward7Desc");

            public override void ChangeShopCard(ref DiceCardXmlInfo card)
            {
                base.ChangeShopCard(ref card);
                if (!card.CheckCanUpgrade() || (double)UnityEngine.Random.value >= 0.15000000596046448)
                    return;
                card = Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(card.id);
            }

            public override void ChangeCardReward(ref List<DiceCardXmlInfo> cardlist)
            {
                List<DiceCardXmlInfo> diceCardXmlInfoList = new List<DiceCardXmlInfo>();
                foreach (DiceCardXmlInfo info in cardlist)
                {
                    if (!info.CheckCanUpgrade())
                        diceCardXmlInfoList.Add(info);
                    else if ((double)UnityEngine.Random.value < 0.15)
                        diceCardXmlInfoList.Add(Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(info.id));
                    else
                        diceCardXmlInfoList.Add(info);
                }
                cardlist = diceCardXmlInfoList;
            }
        }
    }
}
