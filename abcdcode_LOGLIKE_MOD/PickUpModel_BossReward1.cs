// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BossReward1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    
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
