// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.ShopGoods_Passive
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using LOR_DiceSystem;
using LOR_XML;
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
        public RewardPassiveInfo GoodInfo;
        public Sprite GoodSprite;
        public UILogCustomSelectable customSelectable;
 
        public override SaveData GetSaveData()
        {
            SaveData saveData = base.GetSaveData();
            LorId id = this.GoodInfo != null ? this.GoodInfo.id : this.GoodScript.id;
            saveData.AddData("Id", id.LogGetSaveData());
            return saveData;
        }

        public override void LoadFromSaveData(SaveData data)
        {
            base.LoadFromSaveData(data);
            this.Money.text = this.price.ToString();
        }

        public override bool CanPurchase()
        {
            if (this.GoodScript == null)
                return this.GoodInfo != null && !LogueBookModels.HasObtainedReward(this.GoodInfo) && base.CanPurchase();
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

        private static int GetChapterPriceBase()
        {
            switch (LogLikeMod.curchaptergrade)
            {
                case ChapterGrade.Grade1:
                    return 9;
                case ChapterGrade.Grade2:
                    return 10;
                case ChapterGrade.Grade3:
                    return 12;
                case ChapterGrade.Grade4:
                    return 14;
                case ChapterGrade.Grade5:
                    return 16;
                case ChapterGrade.Grade6:
                case ChapterGrade.Grade7:
                    return 18;
                default:
                    return 12;
            }
        }

        private static int GetRarityPriceBonus(Rarity rarity)
        {
            switch (rarity)
            {
                case Rarity.Common:
                    return -2;
                case Rarity.Uncommon:
                    return 0;
                case Rarity.Rare:
                    return 3;
                case Rarity.Unique:
                    return 7;
                default:
                    return 0;
            }
        }

        public int CalcPrice(RewardPassiveInfo cardinfo)
        {
            int num1;
            if (cardinfo.price != -1)
            {
                num1 = cardinfo.price;
            }
            else
            {
                if (cardinfo.rewardtype == RewardType.EquipPage || cardinfo.rewardtype == RewardType.Creature)
                {
                    int levelBonus = cardinfo.rewardtype == RewardType.Creature ? Mathf.Max(0, cardinfo.level - 1) : 0;
                    num1 = Mathf.Max(1, Mathf.RoundToInt((GetChapterPriceBase() + GetRarityPriceBonus(cardinfo.passiverarity) + levelBonus) * Random.Range(0.9f, 1.1f)));
                    return num1;
                }
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
            this.GoodInfo = goodinfo;
            this.GoodScript = LogLikeMod.FindPickUp(goodinfo.script) as ShopPickUpModel;
            if (goodinfo.id.packageId == LogLikeMod.ModId)
                this.GoodSprite = LogLikeMod.ArtWorks[goodinfo.artwork];
            else if (LogLikeMod.ModdedArtWorks.ContainsKey((goodinfo.id.packageId, goodinfo.artwork)))
                this.GoodSprite = LogLikeMod.ModdedArtWorks[(goodinfo.id.packageId, goodinfo.artwork)];
            customSelectable = null;
            if (goodinfo.id.packageId == LogLikeMod.ModId)
                customSelectable = ModdingUtils.CreateLogSelectable(this.transform, goodinfo.iconartwork, new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(180f, 180f));
            else if (LogLikeMod.ModdedArtWorks.ContainsKey((goodinfo.id.packageId, goodinfo.iconartwork)))
                customSelectable = ModdingUtils.CreateLogSelectable(this.transform, LogLikeMod.ModdedArtWorks[(goodinfo.id.packageId, goodinfo.iconartwork)], new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(180f, 180f));
            else if (goodinfo.rewardtype == RewardType.EquipPage)
            {
                BookXmlInfo book = RewardingModel.GetBookDataOriginAware(goodinfo.id);
                if (book != null)
                    customSelectable = ModdingUtils.CreateLogSelectable(this.transform, new BookModel(book).GetThumbSprite(), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(180f, 180f));
            }
            if (customSelectable == null)
                customSelectable = ModdingUtils.CreateLogSelectable(this.transform, goodinfo.iconartwork, new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(180f, 180f));
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
            else if (this.GoodInfo != null)
            {
                if (this.GoodInfo.rewardtype == RewardType.EquipPage)
                    LogueBookModels.TryAddUniqueRoleBookToInventoryAndAtlas(this.GoodInfo.id);
                else if (this.GoodInfo.rewardtype == RewardType.Creature)
                    RMRAbnormalityUnlockManager.UnlockShopAbnormalityPage(this.GoodInfo);
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
            if (this.GoodScript == null)
            {
                this.ShowRewardInfoDesc();
                return;
            }
            bool flag = this.GoodScript.IsEquipReward();
            string name = this.GoodScript.Name;
            this.GoodScript.EditName(ref name);
            string desc1 = this.GoodScript.Desc;
            this.GoodScript.EditDesc(ref desc1);
            var instance = SingletonBehavior<UIMainOverlayManager>.Instance;
            if (flag)
            {
                string desc2 = $"{TextDataModel.GetText("Shop_EquipPrevious", this.GoodScript.basepassive.cost)}{desc1}";
                instance.SetTooltip(name, desc2 + "\n" + "\n" + this.GoodScript.FlaverText + "\n"
                        + "<color=#" + ColorUtility.ToHtmlStringRGB(UIColorManager.Manager.GetEquipRarityColor(this.GoodScript.GetRarity())) + ">" + this.GoodScript.GetRarity().ToString() + "</color>",
                        this.gameObject.transform as RectTransform,
                        this.GoodScript.GetRarity(),
                        UIToolTipPanelType.OnlyContent);
            }
            else
                instance.SetTooltip(name, desc1 + "\n" + "\n" + this.GoodScript.FlaverText + "\n"
                        + "<color=#" + ColorUtility.ToHtmlStringRGB(UIColorManager.Manager.GetEquipRarityColor(this.GoodScript.GetRarity())) + ">" + this.GoodScript.GetRarity().ToString() + "</color>",
                        this.gameObject.transform as RectTransform,
                        this.GoodScript.GetRarity(),
                        UIToolTipPanelType.OnlyContent);

            var curPos = instance.tooltipPositionPivot.anchoredPosition;
            // additional adjustment for better placement specific to shops
            instance.tooltipPositionPivot.anchoredPosition = new Vector2(curPos.x, curPos.y + 100f);
        }

        private void ShowRewardInfoDesc()
        {
            if (this.GoodInfo == null)
                return;
            var instance = SingletonBehavior<UIMainOverlayManager>.Instance;
            string name = string.Empty;
            string desc = string.Empty;
            if (this.GoodInfo.rewardtype == RewardType.EquipPage)
            {
                BookXmlInfo book = RewardingModel.GetBookDataOriginAware(this.GoodInfo.id);
                if (book != null)
                {
                    name = RewardingModel.GetLocalizedBookName(book);
                    desc = RewardingModel.GetAblilityText(book);
                }
            }
            else if (this.GoodInfo.rewardtype == RewardType.Creature)
            {
                EmotionCardXmlInfo card = LogLikeMod.GetRegisteredPickUpXml(this.GoodInfo);
                if (card != null && card.Script != null && card.Script.Count > 0)
                {
                    AbnormalityCard abnormalityCard = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(card.Script[0]);
                    if (abnormalityCard != null)
                    {
                        name = abnormalityCard.cardName;
                        desc = abnormalityCard.abilityDesc;
                    }
                }
            }
            if (string.IsNullOrEmpty(name))
                name = this.GoodInfo.id.ToString();
            instance.SetTooltip(name, desc, this.gameObject.transform as RectTransform, this.GoodInfo.passiverarity, UIToolTipPanelType.OnlyContent);
            var curPos = instance.tooltipPositionPivot.anchoredPosition;
            instance.tooltipPositionPivot.anchoredPosition = new Vector2(curPos.x, curPos.y + 100f);
        }

        public void OnPointerExit()
        {
            SingletonBehavior<UIMainOverlayManager>.Instance.Close();
        }

    }
}
