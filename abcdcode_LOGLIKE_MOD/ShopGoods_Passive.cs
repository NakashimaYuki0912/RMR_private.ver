// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.ShopGoods_Passive
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using RogueLike_Mod_Reborn;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class ShopGoods_Passive : ShopGoods
    {
        public ShopPickUpModel GoodScript;
        public Sprite GoodSprite;
        public UILogCustomSelectable customSelectable;
        // Direct equip page purchase support (for role books without PickUpModel scripts)
        public LorId directEquipId = LorId.None;
        // Direct abnormality page purchase support
        public bool isAbnormalityGood;
        public RewardPassiveInfo storedRewardInfo;
 
        public override SaveData GetSaveData()
        {
            SaveData saveData = base.GetSaveData();
            LorId saveId = LorId.None;
            if (this.GoodScript != null)
                saveId = this.GoodScript.id;
            else if (this.directEquipId != LorId.None)
                saveId = this.directEquipId;
            else if (this.storedRewardInfo != null)
                saveId = this.storedRewardInfo.id;
            saveData.AddData("Id", saveId.LogGetSaveData());
            return saveData;
        }

        public override void LoadFromSaveData(SaveData data)
        {
            base.LoadFromSaveData(data);
            this.Money.text = this.price.ToString();
        }

        public override bool CanPurchase()
        {
            // For direct equip/abnormality goods without PickUpModel, just check money
            if (this.GoodScript == null)
                return base.CanPurchase();
            using (List<BattleUnitModel>.Enumerator enumerator = BattleObjectManager.instance.GetList(Faction.Player).GetEnumerator())
            {
                do
                {
                    if (!enumerator.MoveNext())
                        goto label_5;
                }
                while (!this.GoodScript.IsCanPickUp(enumerator.Current.UnitData.unitData));
                goto label_6;
            }
        label_5:
            return false;
        label_6:
            return base.CanPurchase();
        }

        public int CalcPrice(RewardPassiveInfo cardinfo)
        {
            int num1;
            if (cardinfo.price != -1)
            {
                num1 = cardinfo.price;
            }
            else if (this.directEquipId != LorId.None)
            {
                // Role book pricing: based on book's own rarity and chapter
                BookXmlInfo book = Singleton<BookXmlList>.Instance.GetData(this.directEquipId);
                float rarityMult = 1f;
                if (book != null)
                {
                    switch (book.Rarity)
                    {
                        case Rarity.Common: rarityMult = 1f; break;
                        case Rarity.Uncommon: rarityMult = 1.8f; break;
                        case Rarity.Rare: rarityMult = 3f; break;
                        case Rarity.Unique: rarityMult = 5f; break;
                    }
                }
                num1 = Mathf.RoundToInt((float)(5.0 + 7.0 * (double)rarityMult) * Random.Range(0.85f, 1.15f));
            }
            else if (this.isAbnormalityGood)
            {
                // Abnormality page pricing: premium
                float rarityMult = 1f;
                switch (cardinfo.passiverarity)
                {
                    case Rarity.Common: rarityMult = 2f; break;
                    case Rarity.Uncommon: rarityMult = 3f; break;
                    case Rarity.Rare: rarityMult = 5f; break;
                    case Rarity.Unique: rarityMult = 8f; break;
                }
                num1 = Mathf.RoundToInt((float)(10.0 + 8.0 * (double)rarityMult) * Random.Range(0.85f, 1.15f));
            }
            else
            {
                ShopPickUpModel goodScript = this.GoodScript;
                if (goodScript == null)
                {
                    num1 = 1;
                }
                else
                {
                    float num2;
                    switch (goodScript.basepassive != null ? (int)goodScript.basepassive.rare : (int)cardinfo.passiverarity)
                    {
                        case 0:
                            num2 = 1f;
                            break;
                        case 1:
                            num2 = 1.5f;
                            break;
                        case 2:
                            num2 = 2.5f;
                            break;
                        case 3:
                            num2 = 3.5f;
                            break;
                        default:
                            num2 = 1f;
                            break;
                    }
                    int num3 = Mathf.RoundToInt((float)(9.0 + 6.5 * (double)num2) * Random.Range(0.85f, 1.15f));
                    if (goodScript.GetShopType() == ShopRewardType.Once)
                        num3 = Mathf.RoundToInt((float)(4.5 + 3.5 * (double)num2) * Random.Range(0.85f, 1.15f));
                    num1 = num3;
                }
            }
            return num1;
        }

        public void SetGoods(RewardPassiveInfo goodinfo)
        {
            this.storedRewardInfo = goodinfo;
            this.GoodScript = LogLikeMod.FindPickUp(goodinfo.script) as ShopPickUpModel;
            this.directEquipId = LorId.None;
            this.isAbnormalityGood = false;

            // Handle EquipPage rewards that don't have a ShopPickUpModel script
            if (this.GoodScript == null && goodinfo.rewardtype == RewardType.EquipPage)
            {
                this.directEquipId = goodinfo.id;
            }
            // Handle Creature/Abnormality rewards
            if (goodinfo.rewardtype == RewardType.Creature)
            {
                this.isAbnormalityGood = true;
            }
            this.GoodSprite = null;
            if (goodinfo.id.packageId == LogLikeMod.ModId)
            {
                if (LogLikeMod.ArtWorks.ContainsKey(goodinfo.artwork))
                    this.GoodSprite = LogLikeMod.ArtWorks[goodinfo.artwork];
                if (this.GoodSprite == null && !string.IsNullOrEmpty(goodinfo.iconartwork)
                    && LogLikeMod.ArtWorks.ContainsKey(goodinfo.iconartwork))
                    this.GoodSprite = LogLikeMod.ArtWorks[goodinfo.iconartwork];
                if (this.GoodSprite == null && LogLikeMod.ArtWorks.ContainsKey("Stage_Rest"))
                    this.GoodSprite = LogLikeMod.ArtWorks["Stage_Rest"];
            }
            else if (LogLikeMod.ModdedArtWorks.ContainsKey((goodinfo.id.packageId, goodinfo.artwork)))
                this.GoodSprite = LogLikeMod.ModdedArtWorks[(goodinfo.id.packageId, goodinfo.artwork)];
            customSelectable = null;
            if (this.GoodSprite != null)
                customSelectable = ModdingUtils.CreateLogSelectable(this.transform, this.GoodSprite, new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(180f, 180f));
            else if (goodinfo.id.packageId == LogLikeMod.ModId)
                customSelectable = ModdingUtils.CreateLogSelectable(this.transform, goodinfo.iconartwork, new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(180f, 180f));
            else if (LogLikeMod.ModdedArtWorks.ContainsKey((goodinfo.id.packageId, goodinfo.iconartwork)))
                customSelectable = ModdingUtils.CreateLogSelectable(this.transform, LogLikeMod.ModdedArtWorks[(goodinfo.id.packageId, goodinfo.iconartwork)], new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(180f, 180f));
            if (customSelectable == null)
            {
                Debug.LogError($"[ShopGoods_Passive] Missing artwork for shop passive good: {goodinfo.id} / {goodinfo.artwork}");
                return;
            }
            Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
            buttonClickedEvent.AddListener(() => this.OnClickGoods());
            customSelectable.onClick = buttonClickedEvent;
            customSelectable.SelectEvent = new UnityEventBasedata();
            customSelectable.SelectEvent.AddListener(e => this.OnPointerEnter());
            customSelectable.DeselectEvent = new UnityEventBasedata();
            customSelectable.DeselectEvent.AddListener(e => this.OnPointerExit());
            Image image = ModdingUtils.CreateImage(this.transform, "MoneyIcon", new Vector2(1f, 1f), new Vector2(-10f, -90f));
            this.moneyicon = image;
            TextMeshProUGUI textTmp = ModdingUtils.CreateText_TMP(image.transform, new Vector2(40f, 0.0f), 25, new Vector2(0.5f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.MidlineLeft, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
            textTmp.text = this.CalcPrice(goodinfo).ToString();
            this.price = int.Parse(textTmp.text);
            this.Money = textTmp;
        }

        public override void Purchase()
        {
            base.Purchase();
            if (this.GoodScript != null)
            {
                this.GoodScript.OnPickUpShop((ShopGoods)this);
                LogueBookModels.shopPick.Add(this.GoodScript.id);
            }
            else if (this.directEquipId != LorId.None)
            {
                // Direct role book purchase
                ShopPickUpModel.AddEquipPage(this.directEquipId);
                LogueBookModels.shopPick.Add(this.directEquipId);
            }
            else if (this.isAbnormalityGood && this.storedRewardInfo != null)
            {
                // Abnormality page purchase: add to EmotionCardList
                if (!LogueBookModels.EmotionCardList.Exists(x => x.id == this.storedRewardInfo.id))
                {
                    LogueBookModels.EmotionCardList.Add(this.storedRewardInfo);
                    LogueBookModels.shopPick.Add(this.storedRewardInfo.id);
                }
            }
            this.gameObject.SetActive(false);
            SingletonBehavior<UIMainOverlayManager>.Instance.Close();
        }

        public virtual void OnClickGoods()
        {
            if (!this.CanPurchase())
                return;
            PassiveAbility_MoneyCheck.SubMoney(this.price);
            this.Purchase();
            if (this.parent == null)
                return;
            this.parent.MoneyChecking();
        }

        public void OnPointerEnter()
        {
            this.ShowDesc();
        }

        public void ShowDesc()
        {
            var instance = SingletonBehavior<UIMainOverlayManager>.Instance;
            if (this.GoodScript == null)
            {
                // Direct equip page or abnormality page without PickUpModel
                if (this.directEquipId != LorId.None)
                {
                    BookXmlInfo book = Singleton<BookXmlList>.Instance.GetData(this.directEquipId);
                    if (book != null)
                    {
                        string desc = RewardingModel.GetAblilityText(book);
                        instance.SetTooltip(book.InnerName, desc,
                            this.gameObject.transform as RectTransform,
                            book.Rarity, UIToolTipPanelType.OnlyContent);
                    }
                }
                else if (this.isAbnormalityGood && this.storedRewardInfo != null)
                {
                    PickUpModelBase pickUp = LogLikeMod.FindPickUp(this.storedRewardInfo.script);
                    string abnoName = pickUp != null ? pickUp.Name : this.storedRewardInfo.script;
                    string desc = pickUp != null ? pickUp.Desc : string.Empty;
                    string flavor = pickUp != null ? pickUp.FlaverText : string.Empty;
                    string tooltipDesc = string.IsNullOrEmpty(flavor) ? RewardingModel.GetRaritytext(this.storedRewardInfo.passiverarity) : desc + "\n\n" + flavor + "\n" + RewardingModel.GetRaritytext(this.storedRewardInfo.passiverarity);
                    instance.SetTooltip(abnoName, tooltipDesc,
                        this.gameObject.transform as RectTransform,
                        this.storedRewardInfo.passiverarity, UIToolTipPanelType.OnlyContent);
                }
                var curPos2 = instance.tooltipPositionPivot.anchoredPosition;
                instance.tooltipPositionPivot.anchoredPosition = new Vector2(curPos2.x, curPos2.y + 100f);
                return;
            }
            bool flag = this.GoodScript.IsEquipReward();
            string name = this.GoodScript.Name;
            this.GoodScript.EditName(ref name);
            string desc1 = this.GoodScript.Desc;
            this.GoodScript.EditDesc(ref desc1);
            if (flag)
            {
                string desc2 = $"{TextDataModel.GetText("Shop_EquipPrevious", this.GoodScript.basepassive.cost)}{desc1}";
                instance.SetTooltip(name, desc2 + "\n" + "\n" + this.GoodScript.FlaverText + "\n"
                        + "<color=#" + ColorUtility.ToHtmlStringRGB(UIColorManager.Manager.GetEquipRarityColor(this.GoodScript.GetRarity())) + ">" + RewardingModel.GetRaritytext(this.GoodScript.GetRarity()) + "</color>",
                        this.gameObject.transform as RectTransform,
                        this.GoodScript.GetRarity(),
                        UIToolTipPanelType.OnlyContent);
            }
            else
                instance.SetTooltip(name, desc1 + "\n" + "\n" + this.GoodScript.FlaverText + "\n"
                        + "<color=#" + ColorUtility.ToHtmlStringRGB(UIColorManager.Manager.GetEquipRarityColor(this.GoodScript.GetRarity())) + ">" + RewardingModel.GetRaritytext(this.GoodScript.GetRarity()) + "</color>",
                        this.gameObject.transform as RectTransform,
                        this.GoodScript.GetRarity(),
                        UIToolTipPanelType.OnlyContent);

            var curPos = instance.tooltipPositionPivot.anchoredPosition;
            // additional adjustment for better placement specific to shops
            instance.tooltipPositionPivot.anchoredPosition = new Vector2(curPos.x, curPos.y + 100f);
        }

        public void OnPointerExit()
        {
            SingletonBehavior<UIMainOverlayManager>.Instance.Close();
        }

    }
}
