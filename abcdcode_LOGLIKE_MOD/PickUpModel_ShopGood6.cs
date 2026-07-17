// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood6
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood6.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood6 : ShopPickUpModel
    {
        public PickUpModel_ShopGood6()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570006));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90006);
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood6.GetOutInHere());
        }

        /// <summary>GetOutInHere</summary>

        public class GetOutInHere : OnceEffect
        {
            public static Rarity ItemRarity = Rarity.Rare;
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive6"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570006));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570006));
            }

            public override void OnClick()
            {
                base.OnClick();
                if (LogLikeMod.curstagetype > StageType.Normal || Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                    return;
                Singleton<StageController>.Instance.GetStageModel().GetWave(Singleton<StageController>.Instance.CurrentWave).Defeat();
                Singleton<StageController>.Instance.EndBattle();
                this.Use();
            }
        }
    }
}
