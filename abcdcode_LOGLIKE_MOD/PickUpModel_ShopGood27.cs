// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood27
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood27 : ShopPickUpModel
    {
        public PickUpModel_ShopGood27() : base()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570027));
            this.id = new LorId(LogLikeMod.ModId, 90027);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood27.Shop27Effect());
        }

        public override string KeywordId => "GlobalEffect_BloodShed";
        public override string KeywordIconId => "ShopPassive27";
        public class Shop27Effect : GlobalLogueEffectBase
        {
            public bool CanUse;
            public bool EffectOn;
            public static Rarity ItemRarity = Rarity.Unique;

            public override void OnStartBattle()
            {
                base.OnStartBattle();
                this.CanUse = true;
                this.EffectOn = false;
            }

            public override void OnRoundStart(StageController stage)
            {
                base.OnRoundStart(stage);
                this.EffectOn = false;
                Singleton<GlobalLogueEffectManager>.Instance.UpdateSprites();
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (!this.EffectOn)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmgRate = 50,
                    breakRate = 50
                });
            }

            public override void OnClick()
            {
                base.OnClick();
                if (Singleton<StageController>.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase || !this.CanUse)
                    return;
                this.EffectOn = true;
                this.CanUse = false;
                Singleton<GlobalLogueEffectManager>.Instance.UpdateSprites();
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Wolf_FogChange", volume: 0.85f);
            }

            public override string KeywordId => "GlobalEffect_BloodShed";
            public override string KeywordIconId => this.EffectOn ? "ShopPassive27_On" : "ShopPassive27";
        }
    }
}
