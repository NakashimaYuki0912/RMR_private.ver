// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.ShopGoods_Card
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using LOR_DiceSystem;
using RogueLike_Mod_Reborn;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class ShopGoods_Card : ShopGoods
    {
        public LogLikeMod.UILogCardSlot CardSlot;

        public static int CalcPrice(DiceCardXmlInfo cardinfo)
        {
            Rarity rarity = cardinfo.Rarity;
            int chapter = cardinfo.Chapter;
            float num1 = 1f;
            float num2;
            switch (rarity)
            {
                case Rarity.Common:
                    num2 = 1f;
                    break;
                case Rarity.Uncommon:
                    num2 = 1.5f;
                    break;
                case Rarity.Rare:
                    num2 = 2f;
                    break;
                case Rarity.Unique:
                    num2 = 4f;
                    break;
                default:
                    num2 = 1f;
                    break;
            }
            switch (chapter)
            {
                case 1:
                    num1 = 1f;
                    break;
                case 2:
                    num1 = 1.1f;
                    break;
                case 3:
                    num1 = 1.3f;
                    break;
                case 4:
                    num1 = 1.5f;
                    break;
                case 5:
                    num1 = 1.8f;
                    break;
                case 6:
                    num1 = 2.5f;
                    break;
                case 7:
                    num1 = 3f;
                    break;
            }
            return Mathf.RoundToInt((float)(1.0 + 1.8 * (double)num2 * (double)num1 * (double)Random.Range(0.8f, 1.2f)));
        }

        public override SaveData GetSaveData()
        {
            SaveData saveData = base.GetSaveData();
            saveData.AddData("Id", this.CardSlot._cardModel.GetID().LogGetSaveData());
            saveData.AddData("num", new SaveData(this.count));
            return saveData;
        }

        public override void LoadFromSaveData(SaveData data)
        {
            base.LoadFromSaveData(data);
            this.Money.text = this.price.ToString();
            this.count = data.GetInt("num");
            this.CardSlot.txt_cardNumbers.text = this.count.ToString();
        }

        public void SetGoods(DiceCardXmlInfo cardinfo, int goods_count = 2, int price = -1)
        {
            LogLikeMod.UILogCardSlot CardSlot = LogLikeMod.UILogCardSlot.SlotCopyingByOrig();
            CardSlot.transform.SetParent(this.transform);
            CardSlot.transform.localScale = new Vector3(1.2f, 1.2f);
            CardSlot.transform.localPosition = new Vector3(0.0f, 0.0f);
            CardSlot.SetData(new DiceCardItemModel(cardinfo));
            CardSlot.selectable.SubmitEvent.RemoveAllListeners();
            CardSlot.selectable.SubmitEvent.AddListener((UnityAction<BaseEventData>)(e => this.OnClickCard()));
            CardSlot.selectable.SelectEvent.RemoveAllListeners();
            CardSlot.selectable.SelectEvent.AddListener((UnityAction<BaseEventData>)(e => this.OnPointerEnter(e)));
            CardSlot.selectable.DeselectEvent.RemoveAllListeners();
            CardSlot.selectable.DeselectEvent.AddListener((UnityAction<BaseEventData>)(e => CardSlot.OnPointerExit(e)));
            CardSlot.txt_cardNumbers.text = goods_count.ToString();
            this.parent.FrameObj.Add($"CardSlot{cardinfo.id.packageId}{cardinfo.id.id.ToString()}", CardSlot.gameObject);
            this.count = goods_count;
            this.CardSlot = CardSlot;
            Image image = ModdingUtils.CreateImage(this.transform, "MoneyIcon", new Vector2(1f, 1f), new Vector2(-10f, -100f));
            this.parent.FrameObj.Add($"moneyicon{cardinfo.id.packageId}{cardinfo.id.id.ToString()}", CardSlot.gameObject);
            this.moneyicon = image;
            TextMeshProUGUI textTmp = ModdingUtils.CreateText_TMP(image.transform, new Vector2(40f, 0.0f), 25, new Vector2(0.5f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.MidlineLeft, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
            textTmp.text = price == -1 ? ShopGoods_Card.CalcPrice(cardinfo).ToString() : price.ToString();
            this.parent.FrameObj.Add($"Money{cardinfo.id.packageId}{cardinfo.id.id.ToString()}", CardSlot.gameObject);
            this.price = int.Parse(textTmp.text);
            this.Money = textTmp;
        }

        public void OnPointerEnter(BaseEventData e)
        {
            this.CardSlot.OnPointerEnter(e);
            this.gameObject.transform.SetAsLastSibling();
        }

        public override void Purchase()
        {
            base.Purchase();
            LogueBookModels.AddCard(this.CardSlot._cardModel.GetID());
            CardAddVfx.RunCardVfx(this.CardSlot);
            --this.count;
            this.CardSlot.txt_cardNumbers.text = this.count.ToString();
            if (this.count <= 0)
            {
                this.gameObject.SetActive(false);
                LogLikeMod.UILogBattleDiceCardUI.Instance.gameObject.SetActive(false);
            }
            SingletonBehavior<BattleSoundManager>.Instance.PlaySound(EffectSoundType.CARD_APPLY, this.transform.position);
        }

        public virtual void OnClickCard()
        {
            if (this.count <= 0 || !this.CanPurchase() || PassiveAbility_MoneyCheck.GetMoney() < this.price)
                return;
            PassiveAbility_MoneyCheck.SubMoney(this.price);
            this.Purchase();
            if (this.parent == null)
                return;
            this.parent.MoneyChecking();
        }

        public class CardPerchaseEffect : MonoBehaviour
        {
            public float time = 3f;

            public void Awake() => this.time = 3f;

            public void Update()
            {
                this.time -= Time.deltaTime;
                if ((double)this.time >= 0.0)
                    return;
                Object.Destroy(this.gameObject);
            }
        }
    }
}
