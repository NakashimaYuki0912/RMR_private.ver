// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood3_1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood3_1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood3_1 : ShopPickUpModel
    {
        public PickUpModel_ShopGood3_1()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8573001));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 30001);
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood3_1.Shop3_1());
        }

        /// <summary>Shop component: Shop3_1</summary>

        public class Shop3_1 : GlobalLogueEffectBase
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive3_1"];

            public override string GetEffectName() => TextDataModel.GetText("Shop3_1_Name");

            public override string GetEffectDesc() => TextDataModel.GetText("Shop3_1_Desc");

            public override void OnStartBattle()
            {
                if (LogLikeMod.curstagetype != StageType.Mystery)
                    return;
                LogLikeMod.rewards.Add(Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 3001)));
            }
        }
    }
}
