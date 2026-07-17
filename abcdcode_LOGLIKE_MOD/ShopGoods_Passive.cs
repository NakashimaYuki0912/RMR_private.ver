// -----------------------------------------------------------------------------
// Shop system component: ShopGoods_Passive
// Namespace/file: ruina-roguelike-reborn-main\abcdcode_LOGLIKE_MOD\ShopGoods_Passive.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
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

    /// <summary>Shop component: ShopGoods_Passive</summary>

    public class ShopGoods_Passive : ShopGoods
    {
        public ShopPickUpModel GoodScript;
        public RewardPassiveInfo GoodInfo;
        public Sprite GoodSprite;
        public UILogCustomSelectable customSelectable;
        #region --- Save / load ---

 
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
        #endregion

        #region --- Getters / setters / checks ---


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
        #endregion

        #region --- Battle hooks ---


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
        #endregion

        #region --- Other helpers ---


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
        #endregion

        #region --- Getters / setters / checks ---


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
                if (RewardingModel.IsValidBookData(book))
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
        #endregion

        #region --- Other helpers ---


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
                    LogueBookModels.TryAddUniqueRoleBookToInventoryAndCompendium(this.GoodInfo.id);
                else if (this.GoodInfo.rewardtype == RewardType.Creature)
                    RMRAbnormalityUnlockManager.UnlockShopAbnormalityPage(this.GoodInfo);
            }
            this.gameObject.SetActive(false);
            SingletonBehavior<UIMainOverlayManager>.Instance.Close();
        }
        #endregion

        #region --- Battle hooks ---


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
        #endregion

        #region --- UI show / hide / build ---


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

            NudgeShopTooltip(instance);
        }
        #endregion

        #region --- Other helpers ---


        private static void NudgeShopTooltip(UIMainOverlayManager instance)
        {
            if (instance == null)
                return;
            try
            {
                var pivot = HarmonyLib.AccessTools.Field(typeof(UIMainOverlayManager), "tooltipPositionPivot")
                    ?.GetValue(instance) as RectTransform;
                if (pivot == null)
                    return;
                var curPos = pivot.anchoredPosition;
                pivot.anchoredPosition = new Vector2(curPos.x, curPos.y + 100f);
            }
            catch { }
        }
        #endregion

        #region --- UI show / hide / build ---


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
                if (RewardingModel.IsValidBookData(book))
                {
                    name = RewardingModel.GetLocalizedBookName(book);
                    desc = RewardingModel.GetAblilityText(book);
                }
                else
                {
                    // Should be filtered out of the shop pool; keep a safe tooltip fallback.
                    name = book != null ? RewardingModel.GetLocalizedBookName(book) : string.Empty;
                    if (string.IsNullOrEmpty(name) || string.Equals(name, "ModNeeded", System.StringComparison.OrdinalIgnoreCase))
                        name = this.GoodInfo.id.ToString();
                    desc = string.Empty;
                }
            }
            else if (this.GoodInfo.rewardtype == RewardType.Creature)
            {
                // Vanilla AbnormalityCards.xml is keyed by EmotionCard.Name (SnowWhite_Vine),
                // not script (snowwhite1). Looking up Script alone always yields "Not found".
                EmotionCardXmlInfo card = LogLikeMod.GetRegisteredPickUpXml(this.GoodInfo);
                string script = null;
                if (card != null && card.Script != null && card.Script.Count > 0)
                    script = card.Script[0];
                if (string.IsNullOrEmpty(script))
                    script = this.GoodInfo.script;

                PickUpModelBase pickUp = null;
                if (!string.IsNullOrEmpty(script))
                {
                    pickUp = LogLikeMod.FindPickUp(script);
                    if (pickUp == null)
                        pickUp = PickUpModel_RMRVanillaEmotion.TryCreate(script);
                }

                if (card != null)
                {
                    RMRAbnormalityUnlockManager.EnsureVanillaEmotionPresentation(this.GoodInfo, card);
                    PickUpModel_RMRVanillaEmotion.InjectResolvedDesc(card, pickUp);
                }

                AbnormalityCard abnormalityCard = null;
                if (!string.IsNullOrEmpty(script))
                    abnormalityCard = PickUpModel_RMRVanillaEmotion.ResolveAbnormalityDesc(script);

                // Prefer dictionary entry under vanilla Name (after presentation apply).
                if ((abnormalityCard == null || PickUpModel_RMRVanillaEmotion.IsMissingDesc(abnormalityCard))
                    && card != null && !string.IsNullOrEmpty(card.Name))
                {
                    AbnormalityCard byName = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(card.Name);
                    if (byName != null && !PickUpModel_RMRVanillaEmotion.IsMissingDesc(byName))
                        abnormalityCard = byName;
                }

                if (abnormalityCard != null && !PickUpModel_RMRVanillaEmotion.IsMissingDesc(abnormalityCard))
                {
                    name = abnormalityCard.cardName;
                    desc = string.IsNullOrEmpty(abnormalityCard.abilityDesc)
                        ? abnormalityCard.flavorText
                        : abnormalityCard.abilityDesc;
                    if (!string.IsNullOrEmpty(abnormalityCard.flavorText)
                        && !string.IsNullOrEmpty(abnormalityCard.abilityDesc)
                        && desc.IndexOf(abnormalityCard.flavorText, System.StringComparison.Ordinal) < 0)
                        desc = abnormalityCard.abilityDesc + "\n\n" + abnormalityCard.flavorText;
                }
                else if (pickUp != null && !PickUpModel_RMRVanillaEmotion.IsMissingText(pickUp.Name))
                {
                    name = pickUp.Name;
                    desc = pickUp.Desc ?? string.Empty;
                    if (!string.IsNullOrEmpty(pickUp.FlaverText))
                        desc = string.IsNullOrEmpty(desc) ? pickUp.FlaverText : desc + "\n\n" + pickUp.FlaverText;
                }
            }
            if (string.IsNullOrEmpty(name)
                || string.Equals(name, "Not found", System.StringComparison.OrdinalIgnoreCase))
                name = !string.IsNullOrEmpty(this.GoodInfo.script) ? this.GoodInfo.script : this.GoodInfo.id.ToString();
            if (string.Equals(desc, "Not found", System.StringComparison.OrdinalIgnoreCase))
                desc = string.Empty;
            instance.SetTooltip(name, desc, this.gameObject.transform as RectTransform, this.GoodInfo.passiverarity, UIToolTipPanelType.OnlyContent);
            NudgeShopTooltip(instance);
        }
        #endregion

        #region --- Battle hooks ---


        public void OnPointerExit()
        {
            SingletonBehavior<UIMainOverlayManager>.Instance.Close();
        }

        #endregion

    }
}
