// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_ShopGood10
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_ShopGood10.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    /// <summary>Pickup model: PickUpModel_ShopGood10</summary>

    public class PickUpModel_ShopGood10 : ShopPickUpModel
    {
        public static float[] ValueTable = new float[20]
        {
            0.1f,
            0.1f,
            0.1f,
            0.1f,
            0.1f,
            0.2f,
            0.2f,
            0.4f,
            0.5f,
            0.7f,
            0.8f,
            0.9f,
            0.9f,
            1f,
            1f,
            1.1f,
            1.3f,
            1.7f,
            2f,
            3f
        };

        public PickUpModel_ShopGood10()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570010));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90010);
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            float num = PickUpModel_ShopGood10.ValueTable[Random.Range(0, PickUpModel_ShopGood10.ValueTable.Length)];
            PassiveAbility_MoneyCheck.AddMoney((int)((float)good.price * num));
        }
    }
}
