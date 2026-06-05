// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood34
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using RogueLike_Mod_Reborn;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood34 : ShopPickUpModel
    {
        public PickUpModel_ShopGood34()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570034));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90034);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override string[] Keywords
        {
            get => new string[1] { "LogueLikeMod_LuckyBuf_Page" };
        }

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood34.MenualGuardEffect());
        }

        public class MenualGuardEffect : MenualGlobalEffect
        {
            public override Sprite GetSprite()
            {
                return MenualGlobalEffect.CurEffect == this ? LogLikeMod.ArtWorks["ShopPassive34_on"] : LogLikeMod.ArtWorks["ShopPassive34"];
            }

            public override string GetEffectName()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetName(new LorId(LogLikeMod.ModId, 8570034));
            }

            public override string GetEffectDesc()
            {
                return Singleton<PassiveDescXmlList>.Instance.GetDesc(new LorId(LogLikeMod.ModId, 8570034));
            }

            public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
            {
                if (MenualGlobalEffect.CurEffect != this || behavior.owner.faction != Faction.Player || behavior.behaviourInCard.Detail != BehaviourDetail.Guard)
                    return;
                BattleUnitBuf_RMR_Luck.ChangeDiceResult(behavior, 1, ref diceResult);
            }
        }
    }
}
