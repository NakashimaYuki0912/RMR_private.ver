// -----------------------------------------------------------------------------
// Post-battle or shop pickup model: PickUpModel_BossReward1
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\PickUpModel_BossReward1.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using GameSave;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    
    /// <summary>Pickup model: PickUpModel_BossReward1</summary>
    
    public class PickUpModel_BossReward1 : PickUpModelBase
    {
        public override string KeywordId => "GlobalEffect_Bookbinder";
        public override string KeywordIconId => "BossReward1";

        public PickUpModel_BossReward1() : base()
        {

        }

        public override void OnPickUp()
        {
            base.OnPickUp();
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new PickUpModel_BossReward1.BossReward1Effect());
        }

        [HideFromItemCatalog]
        public class BossReward1Effect : GlobalLogueEffectBase
        {

            public override string KeywordId => "GlobalEffect_Bookbinder_Effect";
            public override string KeywordIconId => "BossReward1";

            public override int GetStack()
            {
                return this.stack * 2;
            }

            public int stack;

            public static Rarity ItemRarity = Rarity.Unique;

            public BossReward1Effect() => this.stack = 1;

            public override void LoadFromSaveData(SaveData save)
            {
                base.LoadFromSaveData(save);
                this.stack = save.GetInt("stack");
            }

            public override SaveData GetSaveData()
            {
                SaveData saveData = base.GetSaveData();
                saveData.AddData("stack", this.stack);
                return saveData;
            }

            public override bool CanDupliacte()
            {
                return true;
            }

            public override void AddedNew()
            {
                ++this.stack;
                Singleton<GlobalLogueEffectManager>.Instance.UpdateSprites();
            }

            public override int ChangeSuccCostValue() => this.stack * 2;
        }
    }
}
