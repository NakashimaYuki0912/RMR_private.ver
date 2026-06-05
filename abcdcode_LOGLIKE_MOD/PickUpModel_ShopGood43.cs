// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood43
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood43 : ShopPickUpModel
    {
        public PickUpModel_ShopGood43()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570043));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90043);
        }

        public override bool IsCanAddShop() => true;

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood43.JuMoney());
        }

        public class JuMoney : GlobalLogueEffectBase
        {
            public int stack;

            public JuMoney() => this.stack = 1;

            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive43"];

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570043));
            }

            public override string GetEffectDesc()
            {
                return TextDataModel.GetText("BossReward1Desc_Effect", this.stack);
            }

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

            public override void AddedNew()
            {
                ++this.stack;
                Singleton<GlobalLogueEffectManager>.Instance.UpdateSprites();
            }

            public override int ChangeSuccCostValue() => this.stack;
        }
    }
}
