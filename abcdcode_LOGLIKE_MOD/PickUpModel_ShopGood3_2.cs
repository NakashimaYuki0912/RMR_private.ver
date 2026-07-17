// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood3_2
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood3_2.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood3_2 : ShopPickUpModel
    {
        public PickUpModel_ShopGood3_2()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8573002));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 30002);
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood3_2.Shop3_2());
        }

        /// <summary>Shop component: Shop3_2</summary>

        public class Shop3_2 : GlobalLogueEffectBase
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive3_2"];

            public override string GetEffectName() => TextDataModel.GetText("Shop3_2_Name");

            public override string GetEffectDesc() => TextDataModel.GetText("Shop3_2_Desc");

            public override void OnStartBattle()
            {
                if (LogLikeMod.curstagetype != StageType.Normal)
                    return;
                LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 3001)));
            }
        }
    }
}
