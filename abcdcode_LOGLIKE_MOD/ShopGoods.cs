// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.ShopGoods
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class ShopGoods : MonoBehaviour
    {
        public Image moneyicon;
        public int price;
        public int count;
        public TextMeshProUGUI Money;
        public ShopBase parent;

        public virtual SaveData GetSaveData()
        {
            SaveData saveData = new SaveData();
            saveData.AddData("price", new SaveData(this.price));
            saveData.AddData("position_x", new SaveData((int)this.transform.localPosition.x));
            saveData.AddData("position_y", new SaveData((int)this.transform.localPosition.y));
            return saveData;
        }

        public virtual void LoadFromSaveData(SaveData data)
        {
            this.price = data.GetInt("price");
            this.transform.localPosition = new Vector3((float)data.GetInt("position_x"), (float)data.GetInt("position_y"));
        }

        public virtual bool CanPurchase()
        {
            return PassiveAbility_MoneyCheck.GetMoney() >= this.price && Singleton<GlobalLogueEffectManager>.Instance.CanShopPurchase(this.parent, this);
        }

        public virtual void Purchase()
        {
        }

        public virtual bool CheckEnoughMoney()
        {
            if (this.CanPurchase())
            {
                this.Money.color = LogLikeMod.DefFontColor;
                return true;
            }
            this.Money.color = Color.red;
            return false;
        }

        public virtual void SetShop(ShopBase p) => this.parent = p;

        public virtual void Update()
        {
        }
    }
}
