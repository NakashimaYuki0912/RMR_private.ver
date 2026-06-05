// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood6_Lagacy
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_ShopGood6_Lagacy : ShopPickUpModel
    {
        public PickUpModel_ShopGood6_Lagacy()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570006));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90006);
        }

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            List<ShopGoods> shopGoodsList = Singleton<ShopManager>.Instance.curshop.Goods.FindAll((Predicate<ShopGoods>)(x => x != good && x.gameObject.activeSelf)).RandomPickUp<ShopGoods>(3);
            if (shopGoodsList.Count > 0)
            {
                foreach (ShopGoods shopGoods in shopGoodsList)
                {
                    ModdingUtils.CreateImage(shopGoods.transform.parent, LogLikeMod.ArtWorks["ShopPassive6"], new Vector2(1f, 1f), (Vector2)shopGoods.transform.localPosition, new Vector2(150f, 150f)).gameObject.AddComponent<PickUpModel_ShopGood6_Lagacy.AutoDestroy>();
                    shopGoods.Purchase();
                }
            }
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood6_Lagacy.ShopThiefPenalty());
        }

        public class AutoDestroy : MonoBehaviour
        {
            public float time;

            public void ChangeAlpha(GameObject obj, float alpha)
            {
                if (!(obj.GetComponent<Image>() != null))
                    return;
                Color color = obj.GetComponent<Image>().color;
                obj.GetComponent<Image>().color = new Color(color.r, color.g, color.b, alpha);
            }

            public void Awake() => this.time = 3f;

            public void Update()
            {
                this.time -= Time.deltaTime;
                this.ChangeAlpha(this.gameObject, this.time / 3f);
            }
        }

        [HideFromItemCatalog]
        public class ShopThiefPenalty : GlobalLogueEffectBase
        {
            public static Rarity ItemRarity = Rarity.Special;

            public override bool CanShopPurchase(ShopBase shop, ShopGoods goods) => false;

            public override void OnLeaveShop(ShopBase shop)
            {
                base.OnLeaveShop(shop);
                this.Destroy();
            }

            public override string KeywordId => "GlobalEffect_ShopThiefPenalty";
            public override string KeywordIconId => "ShopPassive6";
        }
    }
}
