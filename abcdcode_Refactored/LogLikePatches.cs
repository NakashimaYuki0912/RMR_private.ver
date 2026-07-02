using BattleCharacterProfile;
using HarmonyLib;
using LOR_DiceSystem;
using LOR_XML;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Workshop;
using RogueLike_Mod_Reborn;


namespace abcdcode_LOGLIKE_MOD
{
    public static class LogLikeRoutines
    {
        public static IEnumerator DisableRoutine(LevelUpUI self)
        {
            self.cardHidingGroup.alpha = 0.0f;
            float elapsed = 0.0f;
            while ((double)elapsed < 1.0)
            {
                elapsed += TimeManager.GetUIDeltaTime() * 2f;
                self.cardSelectionGroup.alpha = 1f - elapsed;
                yield return null;
            }
            self.SetRootCanvas(false);
            for (int i = 0; i < self.candidates.Length; ++i)
                self.candidates[i].gameObject.SetActive(false);
            for (int j = 0; j < self.egoSlotList.Length; ++j)
                self.egoSlotList[j].gameObject.SetActive(false);
        }

        public static void HideRewardSelectionImmediately(LevelUpUI self)
        {
            if (self == null)
                return;

            self.SetRootCanvas(false);
            if (LogLikeMod.skipPanel != null)
                LogLikeMod.skipPanel.gameObject.SetActive(false);
            if (LogLikeMod.StageRemainPanel != null)
                LogLikeMod.StageRemainPanel.gameObject.SetActive(false);
        }

        public static IEnumerator TranslateRoutine(bool hide, LevelUpUI self)
        {
            float translateDelay = (float)typeof(LevelUpUI).GetField("translateDelay", AccessTools.all).GetValue(self);
            float translateSpeed = (float)typeof(LevelUpUI).GetField("translateSpeed", AccessTools.all).GetValue(self);
            float elapsed = 0.0f;
            Vector2 v = new Vector2(-2027f, -213f);
            Vector2 vector2 = new Vector2(751f, 79f);
            if (hide)
            {
                self.showTranslator.anchoredPosition = Vector2.zero;
                self.cardSelectionGroup.interactable = false;
            }
            else
                self.showTranslator.anchoredPosition = v;
            yield return YieldCache.WaitForSeconds(translateDelay);
            if (hide)
            {
                while ((double)elapsed < 1.0)
                {
                    elapsed += Time.deltaTime * translateSpeed;
                    self.showTranslator.anchoredPosition = Vector2.Lerp(Vector2.zero, v, elapsed * elapsed);
                    yield return YieldCache.waitFrame;
                }
            }
            else
            {
                while ((double)elapsed < 1.0)
                {
                    elapsed += Time.deltaTime * translateSpeed;
                    self.showTranslator.anchoredPosition = Vector2.Lerp(v, Vector2.zero, elapsed);
                    yield return YieldCache.waitFrame;
                }
                self.cardSelectionGroup.interactable = true;
            }
        }

        /// <summary>
        /// Safely extracts the UITextDataLoader and frame Image from a cloned button,
        /// without relying on fragile child-index assumptions.
        /// Returns the loader (or null) and the frame (or null).
        /// </summary>
        public static void SafeGetButtonComponents(Button button, out UITextDataLoader loader, out Image frame)
        {
            loader = null;
            frame = null;
            if (button == null)
                return;
            loader = button.GetComponentInChildren<UITextDataLoader>(true);
            // Frame is typically the first Image child that isn't the button's own targetGraphic.
            var images = button.GetComponentsInChildren<Image>(true);
            if (images != null)
            {
                foreach (var img in images)
                {
                    if (img != null && img != button.targetGraphic)
                    {
                        frame = img;
                        break;
                    }
                }
            }
        }

        private static string GetRMRText(string key, string fallback)
        {
            try
            {
                string text = TextDataModel.GetText(key);
                if (!string.IsNullOrEmpty(text) && text != key)
                    return text;
            }
            catch
            {
            }
            string lang = TextDataModel.CurrentLanguage.ToString().ToLowerInvariant();
            if (key == "ui_Realization")
            {
                if (lang.Contains("en")) return "Realization";
                if (lang.Contains("kr")) return "\ud574\ubc29\uc804";
                if (lang.Contains("jp") || lang.Contains("ja")) return "\u89e3\u653e\u6226";
                return "\u89e3\u653e\u6218";
            }
            if (key == "ui_RealizationDesc")
            {
                if (lang.Contains("en")) return "Challenge a floor realization. Victory permanently unlocks that floor's abnormality and E.G.O pages in the atlas and reward pools.";
                if (lang.Contains("kr")) return "\uac01 \uce35\uc758 \ud574\ubc29\uc804\uc5d0 \ub3c4\uc804\ud558\uc138\uc694. \uc2b9\ub9ac \uc2dc \ud574\ub2f9 \uce35\uc758 \ud658\uc0c1\uccb4 \ud398\uc774\uc9c0\uc640 E.G.O \ud398\uc774\uc9c0\uac00 \ub3c4\uac10\uacfc \ubcf4\uc0c1 \ud480\uc5d0 \uc601\uad6c \ud574\uae08\ub429\ub2c8\ub2e4.";
                if (lang.Contains("jp") || lang.Contains("ja")) return "\u5404\u968e\u306e\u89e3\u653e\u6226\u306b\u6311\u6226\u3059\u308b\u3002\u52dd\u5229\u3059\u308b\u3068\u3001\u305d\u306e\u968e\u306e\u5e7b\u60f3\u4f53\u30da\u30fc\u30b8\u3068E.G.O\u30da\u30fc\u30b8\u304c\u56f3\u9451\u3068\u5831\u916c\u30d7\u30fc\u30eb\u306b\u6c38\u4e45\u89e3\u653e\u3055\u308c\u308b\u3002";
                return "\u6311\u6218\u5404\u5c42\u89e3\u653e\u6218\u3002\u80dc\u5229\u540e\uff0c\u8be5\u5c42\u5f02\u60f3\u4f53\u4e66\u9875\u4e0eE.G.O\u4e66\u9875\u4f1a\u6c38\u4e45\u89e3\u9501\u5230\u56fe\u9274\u548c\u5956\u52b1\u6c60\u3002";
            }
            return fallback;
        }

        private static TextMeshProUGUI GetOrCreateRealizationButtonLabel(Button button)
        {
            if (button == null)
                return null;
            Transform existing = button.transform.Find("RMR_RealizationLabel");
            TextMeshProUGUI label = existing == null ? null : existing.GetComponent<TextMeshProUGUI>();
            if (label == null)
            {
                GameObject go = new GameObject("RMR_RealizationLabel");
                go.transform.SetParent(button.transform, false);
                label = go.AddComponent<TextMeshProUGUI>();
            }
            RectTransform rect = label.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;
            label.transform.SetAsLastSibling();
            label.gameObject.SetActive(true);
            return label;
        }

        public static void ApplyRealizationButtonText(Button button)
        {
            TextMeshProUGUI label = GetOrCreateRealizationButtonLabel(button);
            if (label == null)
                return;
            label.text = GetRMRText("ui_Realization", "\u89e3\u653e\u6218");
            label.enableWordWrapping = false;
            label.fontSize = 32f;
            label.font = LogLikeMod.DefFont_TMP;
            label.color = LogLikeMod.DefFontColor;
            label.alignment = TextAlignmentOptions.Center;
            label.raycastTarget = false;
        }

        public static void OnClickCraftTab(UIBattleSettingEditPanel __instance)
        {
            UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click);
            __instance.SetBUttonState((UIBattleSettingEditTap)4);
        }

        public static void OnClickCreatureTab(UIBattleSettingEditPanel __instance)
        {
            UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click);
            __instance.SetBUttonState((UIBattleSettingEditTap)3);
        }

        public static void OnClickAtlasTab(UIBattleSettingEditPanel __instance)
        {
            UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click);
            __instance.SetBUttonState((UIBattleSettingEditTap)5);
        }

        public static void OnClickRealization(UIBattleSettingEditPanel __instance)
        {
            UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click);
            if (LogLikeMod.CheckStage())
            {
                // Open the realization selection panel
                var panel = Singleton<LogRealizationPanel>.Instance;
                if (panel == null)
                {
                    GameObject go = new GameObject("LogRealizationPanel");
                    panel = go.AddComponent<LogRealizationPanel>();
                }
                panel.Show(__instance);
            }
        }

        public static void OnClickInventory(UIBattleSettingEditPanel __instance)
        {
            UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click);
            __instance.SetBUttonState((UIBattleSettingEditTap)2);
        }

        public static void SetBattleSettingCardPanelVisible(bool visible)
        {
            UIBattleSettingPanel panel = UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel;
            if (panel == null || panel.EditPanel == null)
                return;
            UISettingCardInvenPanel battleCardPanel = LogLikeMod.GetFieldValue<UISettingCardInvenPanel>(panel.EditPanel, "_battleCardPanel");
            if (battleCardPanel != null)
                battleCardPanel.SetActivePanel(visible);
        }

        public static void InitUIBattleSettingWaveSlot(
          UIBattleSettingWaveSlot slot,
          UIBattleSettingWaveList list)
        {
            FieldInfo field1 = slot.GetType().GetField("panel", AccessTools.all);
            FieldInfo field2 = slot.GetType().GetField("rect", AccessTools.all);
            FieldInfo field3 = slot.GetType().GetField("img_circle", AccessTools.all);
            FieldInfo field4 = slot.GetType().GetField("img_circleglow", AccessTools.all);
            FieldInfo field5 = slot.GetType().GetField("img_Icon", AccessTools.all);
            FieldInfo field6 = slot.GetType().GetField("img_IconGlow", AccessTools.all);
            FieldInfo field7 = slot.GetType().GetField("hsv_Icon", AccessTools.all);
            FieldInfo field8 = slot.GetType().GetField("hsv_IconGlow", AccessTools.all);
            FieldInfo field9 = slot.GetType().GetField("hsv_Circle", AccessTools.all);
            FieldInfo field10 = slot.GetType().GetField("hsv_CircleGlow", AccessTools.all);
            FieldInfo field11 = slot.GetType().GetField("txt_Alarm", AccessTools.all);
            FieldInfo field12 = slot.GetType().GetField("materialsetter_txtAlarm", AccessTools.all);
            FieldInfo field13 = slot.GetType().GetField("arrow", AccessTools.all);
            FieldInfo field14 = slot.GetType().GetField("defeatColor", AccessTools.all);
            FieldInfo field15 = slot.GetType().GetField("anim", AccessTools.all);
            FieldInfo field16 = slot.GetType().GetField("cg", AccessTools.all);
            field1.SetValue(slot, list);
            RectTransform transform = slot.transform as RectTransform;
            field2.SetValue(slot, transform);
            field3.SetValue(slot, slot.gameObject.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Image>());
            field4.SetValue(slot, slot.gameObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>());
            field5.SetValue(slot, slot.gameObject.transform.GetChild(1).GetChild(3).gameObject.GetComponent<Image>());
            field6.SetValue(slot, slot.gameObject.transform.GetChild(1).GetChild(2).gameObject.GetComponent<Image>());
            field7.SetValue(slot, slot.gameObject.transform.GetChild(1).GetChild(3).gameObject.GetComponent<_2dxFX_HSV>());
            field8.SetValue(slot, slot.gameObject.transform.GetChild(1).GetChild(2).gameObject.GetComponent<_2dxFX_HSV>());
            field9.SetValue(slot, slot.gameObject.transform.GetChild(1).GetChild(1).gameObject.GetComponent<_2dxFX_HSV>());
            field10.SetValue(slot, slot.gameObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<_2dxFX_HSV>());
            field11.SetValue(slot, slot.gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>());
            field12.SetValue(slot, slot.gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProMaterialSetter>());
            field13.SetValue(slot, slot.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>());
            Color color = new Color(0.454902f, 0.1098039f, 0.0f, 1f);
            field14.SetValue(slot, color);
            field15.SetValue(slot, slot.gameObject.GetComponent<Animator>());
            field16.SetValue(slot, slot.gameObject.GetComponent<CanvasGroup>());
            slot.transform.localPosition = (Vector3)new Vector2(120f, 0.0f);
            slot.gameObject.SetActive(false);
        }

        public static void InitUIBattleSettingWaveSlots(
          List<UIBattleSettingWaveSlot> slots,
          UIBattleSettingWaveList __instance)
        {
            float num = 5f / (float)slots.Count;
            for (int index = 0; index < slots.Count; ++index)
                slots[index].gameObject.transform.localScale = new Vector3(1f, 1f);
        }

        public static void ChangeEPCUTransform(EmotionPassiveCardUI __instance)
        {
            LevelUpUI uiLevelup = SingletonBehavior<BattleManagerUI>.Instance.ui_levelup;
            if ((double)uiLevelup.selectedEmotionCard.transform.localPosition.x > 0.0)
            {
                uiLevelup.selectedEmotionCard.transform.localPosition = new Vector3(-410f, -510f);
                uiLevelup.selectedEmotionCardBg.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                uiLevelup.selectedEmotionCard.transform.localPosition = new Vector3(410f, -510f);
                uiLevelup.selectedEmotionCardBg.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }

    public class LogLikeHooks
    {
        private static readonly FieldInfo UIBattleSettingPanelSephirahButtonsField = typeof(UIBattleSettingPanel).GetField("SephirahButtons", AccessTools.all);
        private static readonly FieldInfo BattleUnitEmotionDetailSelfField = typeof(BattleUnitEmotionDetail).GetField("_self", AccessTools.all);
        private static readonly MethodInfo BattleUnitEmotionDetailGetEmotionCoinAdderMethod = typeof(BattleUnitEmotionDetail).GetMethod("GetEmotionCoinAdder", AccessTools.all);
        private static readonly FieldInfo BattleUnitEmotionDetailEmotionCoinsField = typeof(BattleUnitEmotionDetail).GetField("_emotionCoins", AccessTools.all);
        private static readonly FieldInfo EmotionCoinCoinTypeField = typeof(EmotionCoin).GetField("_coinType", AccessTools.all);
        private static readonly FieldInfo BattleUnitCardsInHandUIRootObjField = typeof(BattleUnitCardsInHandUI).GetField("_rootObj", AccessTools.all);
        private static readonly FieldInfo BattleUnitCardsInHandUIToggleShowEgoField = typeof(BattleUnitCardsInHandUI).GetField("toggle_ShowEgo", AccessTools.all);
        private static readonly FieldInfo BattleUnitCardsInHandUIIsOverOnEgoToggleField = typeof(BattleUnitCardsInHandUI).GetField("isOverOnEgoToggle", AccessTools.all);
        private static readonly FieldInfo BattleUnitCardsInHandUISelectedUnitField = typeof(BattleUnitCardsInHandUI).GetField("_selectedUnit", AccessTools.all);
        private static readonly FieldInfo BattleUnitCardsInHandUIHoveredUnitField = typeof(BattleUnitCardsInHandUI).GetField("_hOveredUnit", AccessTools.all);
        private static readonly FieldInfo BattleUnitCardsInHandUIHandStateField = typeof(BattleUnitCardsInHandUI).GetField("_handState", AccessTools.all);
        private static readonly MethodInfo BattleUnitCardsInHandUISetEgoToggleStateMethod = typeof(BattleUnitCardsInHandUI).GetMethod("SetEgoToggleState", AccessTools.all);

        private static void QueueDropBookReward(DropBookXmlInfo reward, LorId sourceId = null)
        {
            if (LogLikeMod.rewards == null)
                LogLikeMod.rewards = new List<DropBookXmlInfo>();
            LorId detectedId = sourceId ?? reward?.id;
            if (detectedId != null && (string.IsNullOrEmpty(detectedId.packageId) || detectedId.packageId == "@origin"))
            {
                LorId normalizedId = GetCurrentChapterDropValueRewardId();
                DropBookXmlInfo normalized = Singleton<DropBookXmlList>.Instance.GetData(normalizedId);
                if (normalized != null)
                    reward = normalized;
                else
                    Debug.LogWarning($"[RMR RewardFlow] Chapter drop book not found for vanilla enemy reward {detectedId}: {normalizedId}.");
            }
            if (reward == null)
                return;
            LogLikeMod.rewards.Add(reward);
        }

        private static LorId GetCurrentChapterDropValueRewardId()
        {
            int suffix = 1;
            if (LogLikeMod.curstagetype == StageType.Elite)
                suffix = 2;
            else if (LogLikeMod.curstagetype == StageType.Boss)
                suffix = 4;
            int chapterNumber = (int)LogLikeMod.curchaptergrade + 1;
            return new LorId(LogLikeMod.ModId, chapterNumber * 1000 + suffix);
        }

        private static void SkipCurrentRewardSelection(LevelUpUI self)
        {
            if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.CardReward)
            {
                List<DiceCardXmlInfo> cardlist = new List<DiceCardXmlInfo>();
                if (self != null && self.egoSlotList != null)
                {
                    foreach (BattleDiceCardUI egoSlot in self.egoSlotList)
                    {
                        if (egoSlot != null && egoSlot.CardModel != null && egoSlot.CardModel.XmlData != null)
                            cardlist.Add(egoSlot.CardModel.XmlData);
                    }
                }
                Singleton<GlobalLogueEffectManager>.Instance.OnSkipCardRewardChoose(cardlist);
                if (LogLikeMod.rewards != null && LogLikeMod.rewards.Count > 0)
                    LogLikeMod.rewards.RemoveAt(0);
            }
            else if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.EgoCardReward)
            {
                if (LogLikeMod.egoSelectionQueue != null && LogLikeMod.egoSelectionQueue.Count > 0)
                    LogLikeMod.egoSelectionQueue.RemoveAt(0);
            }
            else if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.PassiveReward)
            {
                if (LogLikeMod.rewards_passive != null && LogLikeMod.rewards_passive.Count > 0)
                    LogLikeMod.rewards_passive.RemoveAt(0);
            }
            else
            {
                return;
            }

            if (self != null)
                self.SetRootCanvas(false);
            RewardingModel.StartPickReward();
        }

        /// <summary>
        /// Hook for hijacing Crrying Children's map manager.
        /// </summary>
        public void CryingChildMapManager_InitInvitationMap(Action<BattleSceneRoot, MapManager> orig, BattleSceneRoot self, MapManager m)
        {
            if (LogLikeMod.CheckStage(true) && m is CryingChildMapManager)
            {
                var newmap = m.gameObject.AddComponent<CryingChildrenLogKys>();
                UnityEngine.Object.Destroy(m);
                m = newmap;
            }
            orig(self, m);
        }

        /// <summary>
        /// Hook for fixing an annoying passive attribution bug where the game fails to find a passive.
        /// </summary>
        public void BookModel_ChangePassive(Action<BookModel, PassiveModel, PassiveModel> orig, BookModel self, PassiveModel currentBookPassive, PassiveModel changeBookPassive)
        {
            if (!LogLikeMod.CheckStage())
            {
                orig(self, currentBookPassive, changeBookPassive);
                return;
            }
            currentBookPassive.SuccessionPassiveForReserved(changeBookPassive);
        }

        /// <summary>
        /// Hook for changing empty/partially empty decks giving out default pages.
        /// </summary>
        public List<DiceCardXmlInfo> UnitDataModel_GetDeckForBattle(Func<UnitDataModel, int, List<DiceCardXmlInfo>> orig, UnitDataModel self, int index)
        {
            if (LogueBookModels.TryGetGrade6SpecialBuiltInDeckCards(self, out List<DiceCardXmlInfo> builtInDeck))
                return builtInDeck;

            if (LogLikeMod.CheckStage(true) && RMRCore.CurrentGamemode.ReplaceBaseDeck)
            {
                List<DiceCardXmlInfo> list = new List<DiceCardXmlInfo>();
                list.AddRange(self.GetCardList(index));
                int deckSize = self.GetDeckSize();
                int curSize = list.Count;
                if (self.bookItem.ClassInfo.RangeType != EquipRangeType.Range && curSize < deckSize)
                {
                    var defaultDeck = DeckXmlList.Instance.GetData(RMRCore.CurrentGamemode.BaseDeckReplacement).cardIdList;
                    for (int i = curSize; i < deckSize; i++)
                    {
                        list.Add(ItemXmlDataList.instance.GetCardItem(defaultDeck[i], false));
                    }
                }
                return list;
            }
            return orig(self, index);
        }

        /// <summary>
        /// Preserves the vanilla fixed-deck behavior for Black Silence and Binah while
        /// their source BookXmlInfo is projected onto a Roguelike unit.
        /// </summary>
        public bool BookModel_IsFixedDeck(Func<BookModel, bool> orig, BookModel self)
        {
            if (LogueBookModels.HasGrade6SpecialBuiltInDeck(self))
                return true;
            return orig(self);
        }

        /// <summary>
        /// Displays the original vanilla fixed deck instead of the Roguelike starter deck.
        /// </summary>
        public List<DiceCardXmlInfo> BookModel_GetCardListFromCurrentDeck(
            Func<BookModel, List<DiceCardXmlInfo>> orig,
            BookModel self)
        {
            if (LogueBookModels.TryGetGrade6SpecialBuiltInDeckCards(self, out List<DiceCardXmlInfo> builtInDeck))
                return builtInDeck;
            return orig(self);
        }

        /// <summary>
        /// Hook for relocalizing abnormality pages.
        /// </summary>
        public AbnormalityCard AbnormalityCardDescXmlList_GetAbnormalityCard(
          Func<AbnormalityCardDescXmlList, string, AbnormalityCard> orig,
          AbnormalityCardDescXmlList self,
          string cardID)
        {
            if (!LogLikeMod.CheckStage())
                return orig(self, cardID);
            Dictionary<string, AbnormalityCard> dictionary = (Dictionary<string, AbnormalityCard>)typeof(AbnormalityCardDescXmlList).GetField("_dictionary", AccessTools.all).GetValue(self);
            AbnormalityCard abnormalityCard;
            if (dictionary.ContainsKey(cardID))
            {
                abnormalityCard = dictionary[cardID];
            }
            else
            {
                abnormalityCard = new AbnormalityCard()
                {
                    id = cardID,
                    abnormalityName = "Not found",
                    cardName = "Not found",
                    abilityDesc = "Not found",
                    flavorText = "Not found",
                    dialogues = (List<AbnormalityCardDialog>)null
                };
                dictionary.Add(cardID, abnormalityCard);
            }
            return abnormalityCard;
        }

        /// <summary>
        /// Hook for closing the abno page UI.
        /// </summary>
        public void UIGetAbnormalityPanel_PointerClickButton(
          Action<UIGetAbnormalityPanel> orig,
          UIGetAbnormalityPanel self)
        {
            if (LogLikeMod.CheckStage() || LogLikeMod.CheckStage(true))
                self.Close();
            else
                orig(self);
        }

        /// <summary>
        /// Hook for pausing the battle scene whenever:<br></br>
        /// - <see cref="LogLikeMod.PauseBool"/> is true<br></br>
        /// - A mystery event is currently running<br></br>
        /// - An event interrupt is currently running<br></br>
        /// - A shop is currently running
        /// </summary>
        public void BattleSceneRoot_Update(Action<BattleSceneRoot> orig, BattleSceneRoot self)
        {
            if (LogLikeMod.CheckStage() && (Singleton<MysteryManager>.Instance.curMystery != null || Singleton<MysteryManager>.Instance.interruptMysterys != null && Singleton<MysteryManager>.Instance.interruptMysterys.Count > 0 || Singleton<ShopManager>.Instance.curshop != null || LogLikeMod.PauseBool))
                return;
            orig(self);
        }

        /// <summary>
        /// Hook for giving newly-generated units their adequate default deck replacements.
        /// </summary>
        public LorId BookXmlInfo_get_DeckId(Func<BookXmlInfo, LorId> orig, BookXmlInfo self)
        {
            var stage = Singleton<StageController>.Instance.GetStageModel();
            if (self.id.packageId == LogLikeMod.ModId // check for roguelike 
                && ((stage == null || !RoguelikeGamemodeController.Instance.isContinue) && self.id.id == -854) // check for wave start + main player
                || (LogLikeMod.AddPlayer && self.id.id >= -858 && self.id.id <= -855)) // if not above, then check if player is just being added
            {
                return RMRCore.CurrentGamemode.BaseDeckReplacement;
            }
            return orig(self);
        }

        /// <summary>
        /// Hook for disabling the sephirah buttons.
        /// </summary>
        public void UIBattleSettingPanel_SetCurrentSephirahButton(
          Action<UIBattleSettingPanel> orig,
          UIBattleSettingPanel self)
        {
            orig(self);
            // (UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel).cg_NormalFrame
            // attach funny background to object above
            if (!LogLikeMod.CheckStage())
                return;
            // Keep Sephirah buttons available in RMR so the selected floor can continue
            // controlling reception BGM across later acts.
        }

        /* UNUSED
        public void WorkshopSkinDataSetter_LateInit(
          Action<WorkshopSkinDataSetter> orig,
          WorkshopSkinDataSetter self)
        {
            orig(self);
        }
        */

        /// <summary>
        /// Hook for limiting emotion level gain depending on the chapter.
        /// </summary>
        public int BattleUnitEmotionDetail_CreateEmotionCoin(
          Func<BattleUnitEmotionDetail, EmotionCoinType, int, int> orig,
          BattleUnitEmotionDetail self,
          EmotionCoinType coinType,
          int count = 1)
        {
            if (!LogLikeMod.CheckStage(true))
                return orig(self, coinType, count);
            self.SetMaxEmotionLevel(Math.Min((int)(LogLikeMod.curchaptergrade + 1), 5));
            BattleUnitModel battleUnitModel = (BattleUnitModel)BattleUnitEmotionDetailSelfField.GetValue(self);
            if (battleUnitModel.faction == Faction.Player && battleUnitModel.UnitData.unitData.gender == Gender.Creature)
                return 0;
            count += (int)BattleUnitEmotionDetailGetEmotionCoinAdderMethod.Invoke(self, new object[1]
            {
       count
            });
            if (battleUnitModel.faction == Faction.Player)
                battleUnitModel.personalEgoDetail.AddEgoCoolTime(count);
            if (self.EmotionLevel >= self.MaximumEmotionLevel)
                return 0;
            List<EmotionCoin> emotionCoinList = (List<EmotionCoin>)BattleUnitEmotionDetailEmotionCoinsField.GetValue(self);
            for (int index = 0; index < count; ++index)
            {
                if (emotionCoinList.Count < self.MaximumCoinNumber)
                {
                    //if (!self.OnGetEmotionCoin(coinType))
                    //  ;
                    EmotionCoin uninitializedObject = (EmotionCoin)FormatterServices.GetUninitializedObject(typeof(EmotionCoin));
                    EmotionCoinCoinTypeField.SetValue(uninitializedObject, coinType);
                    emotionCoinList.Add(uninitializedObject);
                    self.totalEmotionCoins.Add(uninitializedObject);
                }
            }
            return count;
        }

        /// <summary>
        /// Hook for resetting the emotion level of all librarians and clearing out abno pages.
        /// </summary>
        public void StageController_EndBattlePhase(
          Action<StageController, float> orig,
          StageController self,
          float deltaTime)
        {
            // Realization battles have independent reward handling — skip Roguelike reward chain
            if (RMRRealizationManager.InRealizationBattle)
            {
                orig(self, deltaTime);
                return;
            }

            if (LogLikeMod.CheckStage(true))
            {
                if (LogLikeMod.purpleexcept)
                {
                    orig(self, deltaTime);
                    return;
                }
                RMRAbnormalityUnlockManager.GrantRedMistChallengeVictoryRewards();
                RMRAbnormalityUnlockManager.RecordBlackSilenceVictoryUnlock();
                RMRAbnormalityUnlockManager.GrantDistortedEnsembleVictoryRewards();
                if (!RewardingModel.RewardClearStage(self))
                    return;
                if (!LogLikeMod.EndBattle)
                {
                    if (LogLikeMod.curstagetype == StageType.Boss)
                        RMRAbnormalityUnlockManager.RecordPermanentClear(LogLikeMod.curchaptergrade);
                    self.EndBattle();
                    Singleton<GlobalLogueEffectManager>.Instance.OnEndBattle();
                    LogLikeMod.EndBattle = true;
                    foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList(Faction.Player))
                    {
                        battleUnitModel.emotionDetail.SetEmotionLevel(0);
                        battleUnitModel.emotionDetail.PassiveList.Clear();
                    }
                }
            }
            orig(self, deltaTime);
        }

        /// <summary>
        /// Returns true for Grade4, Grade5, Grade6 — the high chapters that receive an extra EquipPage reward.
        /// </summary>
        private static bool IsHighChapterExtraEquipRewardChapter(ChapterGrade grade)
        {
            return grade == ChapterGrade.Grade4 || grade == ChapterGrade.Grade5 || grade == ChapterGrade.Grade6;
        }

        /// <summary>
        /// Creates an extra EquipPage-only reward by filtering the current chapter's CommonReward pool.
        /// Returns null if no EquipPage entries exist in the pool.
        /// </summary>
        private static RewardInfo CreateExtraEquipPageReward(ChapterGrade grade)
        {
            var allRewards = Singleton<RewardPassivesList>.Instance.GetChapterData(grade, PassiveRewardListType.CommonReward, LorId.None);
            if (allRewards == null)
                return null;
            var equipPages = allRewards.Where(x => x != null && x.rewardtype == RewardType.EquipPage).ToList();
            if (equipPages.Count == 0)
                return null;
            return new RewardInfo { grade = grade, rewards = equipPages };
        }

        /// <summary>
        /// Adds an extra EquipPage reward to <see cref="LogLikeMod.rewards_passive"/> if the current chapter
        /// is Grade4/5/6 and the CommonReward pool contains EquipPage entries.
        /// Call this only after a battle reward (Common/Elite/Boss) has already been added.
        /// </summary>
        private static void TryAddExtraEquipPageReward()
        {
            if (!IsHighChapterExtraEquipRewardChapter(LogLikeMod.curchaptergrade))
                return;
            RewardInfo extraReward = CreateExtraEquipPageReward(LogLikeMod.curchaptergrade);
            if (extraReward != null)
                LogLikeMod.rewards_passive.Add(extraReward);
        }

        /// <summary>
        /// Hook for initializing stage rewards and enabling money UI.
        /// Also runs <see cref="GlobalLogueEffectManager.OnStartBattle"/> (and OnStartBattleAfter).
        /// </summary>
        private static void ResetBattleEgoSelectionState(StageController controller)
        {
            Singleton<SpecialCardListModel>.Instance.Init();
            StageModel stage = controller?.GetStageModel();
            if (stage == null)
                return;
            foreach (StageLibraryFloorModel floor in stage.GetAvailableFloorList())
            {
                ModdingUtils.GetFieldValue<List<EmotionEgoXmlInfo>>("_selectedEgoList", floor)?.Clear();
                ModdingUtils.GetFieldValue<List<EmotionEgoXmlInfo>>("_selectableEgoList", floor)?.Clear();
                if (floor.team != null)
                    floor.team.egoSelectionPoint = 0;
            }
        }

        private sealed class PurpleTransitionEmotionState
        {
            public int EmotionLevel;
            public int MaxEmotionLevel;
        }

        private static Dictionary<UnitDataModel, PurpleTransitionEmotionState> CapturePurpleTransitionEmotionState()
        {
            Dictionary<UnitDataModel, PurpleTransitionEmotionState> result = new Dictionary<UnitDataModel, PurpleTransitionEmotionState>();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetList(Faction.Player))
            {
                if (unit?.UnitData?.unitData == null || unit.emotionDetail == null)
                    continue;
                result[unit.UnitData.unitData] = new PurpleTransitionEmotionState
                {
                    EmotionLevel = unit.emotionDetail.EmotionLevel,
                    MaxEmotionLevel = unit.emotionDetail.MaximumEmotionLevel
                };
            }
            return result;
        }

        private static void RestorePurpleTransitionEmotionState(Dictionary<UnitDataModel, PurpleTransitionEmotionState> states)
        {
            if (states == null || states.Count == 0)
                return;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetList(Faction.Player))
            {
                if (unit?.UnitData?.unitData == null || unit.emotionDetail == null)
                    continue;
                if (!states.TryGetValue(unit.UnitData.unitData, out PurpleTransitionEmotionState state))
                    continue;
                int maxLevel = Math.Max(state.MaxEmotionLevel, Math.Min((int)(LogLikeMod.curchaptergrade + 1), 5));
                unit.emotionDetail.SetMaxEmotionLevel(maxLevel);
                unit.emotionDetail.SetEmotionLevel(Math.Min(state.EmotionLevel, maxLevel));
            }
        }

        public void StageController_StartBattle(Action<StageController> orig, StageController self)
        {
            // If a realization battle is pending, activate the flag now that the
            // realization stage is actually loading. This prevents the EndBattle hook
            // from treating the event-transition EndBattle as a realization end.
            if (RMRRealizationManager.PendingRealizationBattle)
            {
                RMRRealizationManager.ActivatePendingRealization();
            }

            // Realization battles skip the normal Roguelike reward initialization
            if (RMRRealizationManager.InRealizationBattle)
            {
                orig(self);
                return;
            }

            // Purple Tear's half-HP teleport restarts the current floor battle without
            // ending the reception. Keep librarian emotion, selected abnormality pages,
            // EGO cards and reward queues intact until the forced transition completes.
            bool isPurpleTransition = LogLikeMod.CheckStage() && LogLikeMod.purpleexcept;
            if (isPurpleTransition)
            {
                Dictionary<UnitDataModel, PurpleTransitionEmotionState> emotionStates = CapturePurpleTransitionEmotionState();
                orig(self);
                RestorePurpleTransitionEmotionState(emotionStates);
                LogLikeMod.purpleexcept = false;
                return;
            }

            if (LogLikeMod.CheckStage())
            {
                ResetBattleEgoSelectionState(self);
                RewardingModel.ResetDropBookRewardNormalization();
                RMRAbnormalityUnlockManager.ResetRedMistChallengeBattleState();
                LogLikeMod.BattleMoneyUI.Active();
                LogueBookModels.selectedEmotion = new List<RewardPassiveInfo>();
                LogueBookModels.EmotionCardList = RMRAbnormalityUnlockManager.GetUnlockedEmotionCardsForBattle();
                LogLikeMod.curemotion = 0;
                LogLikeMod.purpleexcept = false;
                LogLikeMod.rewards_InStage = new List<RewardInfo>();
                LogLikeMod.rewards = new List<DropBookXmlInfo>();
                LogLikeMod.rewards_lastKill = new List<DropBookXmlInfo>();
                LogLikeMod.rewards_passive = new List<RewardInfo>();
                LogLikeMod.rewardsMystery = new List<LorId>();
                LogLikeMod.egoSelectionQueue = new List<List<LorId>>();
                //if (!LogLikeMod.Debugging)
                //  ;
                LogLikeMod.PlayerEquipOrders = new List<EquipChangeOrder>();
                if (LogLikeMod.curstagetype == StageType.Normal)
                {
                    if (LogLikeMod.NormalRewardCool <= 0)
                    {
                        LogLikeMod.rewards_passive.Add(RewardInfo.GetCurChapterCommonReward(LogLikeMod.curchaptergrade));
                        LogLikeMod.NormalRewardCool = 0;
                        TryAddExtraEquipPageReward();
                    }
                    else
                        --LogLikeMod.NormalRewardCool;
                }
                if (LogLikeMod.curstagetype == StageType.Elite)
                {
                    RewardInfo eliteReward = RewardInfo.GetCurChapterEliteReward(LogLikeMod.curchaptergrade);
                    if (eliteReward != null)
                    {
                        LogLikeMod.rewards_passive.Add(eliteReward);
                        TryAddExtraEquipPageReward();
                    }
                }
                if (LogLikeMod.curstagetype == StageType.Creature)
                    LogLikeMod.NormalRewardCool = 0;
                if (LogLikeMod.curChStageStep != 0 && LogLikeMod.curstagetype == StageType.Boss && LogueBookModels.RemainStageList.ContainsKey(LogLikeMod.curchaptergrade + 1))
                {
                    RewardInfo chapterBossReward = RewardInfo.GetCurChapterBossReward(LogLikeMod.curchaptergrade);
                    if (chapterBossReward != null)
                    {
                        LogLikeMod.rewards_passive.Add(chapterBossReward);
                        TryAddExtraEquipPageReward();
                    }
                }
                RMRAbnormalityUnlockManager.EnqueueBattleClearRewards();
                RMRAbnormalityUnlockManager.SuppressRedMistChallengeGenericRewards();
                LogLikeMod.ResetNextStage();
                if (LogLikeMod.curstagetype == StageType.Boss)
                {
                    if (LogLikeMod.curchaptergrade != ChapterGrade.Grade7 && LogueBookModels.RemainStageList.ContainsKey(LogLikeMod.curchaptergrade + 1))
                        LogLikeMod.nextlist = LogueBookModels.GetNextList(LogLikeMod.curchaptergrade + 1, true);
                    else
                        LogLikeMod.nextlist.Clear();
                }
                else
                    LogLikeMod.nextlist = LogueBookModels.GetNextList(LogLikeMod.curchaptergrade, LogLikeMod.curstagetype == StageType.Start);
                Singleton<GlobalLogueEffectManager>.Instance.OnStartBattle();
                Singleton<GlobalLogueEffectManager>.Instance.UpdateSprites();
            }
            else
            {
                LogLikeMod.ResetUIs();
            }
            orig(self);
            if (!LogLikeMod.CheckStage())
                return;
            Singleton<GlobalLogueEffectManager>.Instance.OnStartBattleAfter();
        }

        /// <summary>
        /// Hook for initializing librarian units in roguelike.
        /// </summary>
        public void StageController_CreateLibrarianUnit(
          Action<StageController, SephirahType> orig,
          StageController self,
          SephirahType sephirah)
        {
            if (RMRRealizationManager.InRealizationBattle)
            {
                CreateRoguelikeLibrarianUnits(self, sephirah, false);
                return;
            }

            if (LogLikeMod.CheckStage(true))
            {
                CreateRoguelikeLibrarianUnits(self, sephirah, true);
            }
            else
                orig(self, sephirah);
        }

        private static void CreateRoguelikeLibrarianUnits(
            StageController self,
            SephirahType sephirah,
            bool applyLogueEffects)
        {
            BattleTeamModel battleTeamModel = (BattleTeamModel)typeof(StageController).GetField("_librarianTeam", AccessTools.all).GetValue(self);
            int num = 0;
            foreach (UnitBattleDataModel unitBattleData in LogueBookModels.playerBattleModel.FindAll(x => x.IsAddedBattle))
            {
                StageLibraryFloorModel floor = self.GetStageModel().GetFloor(sephirah);
                UnitDataModel unitData = unitBattleData.unitData;
                BattleUnitModel defaultUnit = BattleObjectManager.CreateDefaultUnit(Faction.Player);
                defaultUnit.index = num;
                defaultUnit.grade = unitData.grade;
                defaultUnit.formation = floor.GetFormationPosition(defaultUnit.index);
                defaultUnit.SetUnitData(unitBattleData);
                defaultUnit.OnCreated();
                battleTeamModel.AddUnit(defaultUnit);
                BattleObjectManager.instance.RegisterUnit(defaultUnit);
                defaultUnit.passiveDetail.OnUnitCreated();
                defaultUnit.SetDeadSceneBlock(true);
                ++num;
                if (applyLogueEffects)
                    Singleton<GlobalLogueEffectManager>.Instance.OnCreateLibrarian(defaultUnit);
            }
            if (applyLogueEffects)
                Singleton<GlobalLogueEffectManager>.Instance.OnCreateLibrarians();
        }

        /// <summary>
        /// Hook for prompting abno page choice screens.
        /// </summary>
        public bool StageController_RoundEndPhase_ChoiceEmotionCard(
          Func<StageController, bool> orig,
          StageController self)
        {
            if (!LogLikeMod.CheckStage(true))
                return orig(self);
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (!alive.view.FormationReturned)
                    return false;
            }
            return RewardingModel.EmotionChoice();
        }

        /* UNUSED
        public void StageController_InitStageByInvitation(
          Action<StageController, StageClassInfo, List<LorId>> orig,
          StageController self,
          StageClassInfo stage,
          List<LorId> books = null)
        {
            orig(self, stage, books);
        }
        */

        /// <summary>
        /// Hook for transferring enemy book rewards into the reward pool at the end of the Act.
        /// </summary>
        public void StageController_OnEnemyDropBookForAdded(
          Action<StageController, DropBookDataForAddedReward> orig,
          StageController self,
          DropBookDataForAddedReward data)
        {
            if (LogLikeMod.CheckStage(true))
            {
                LogLikeMod.rewards_lastKill = new List<DropBookXmlInfo>();
                DropBookXmlInfo data1 = Singleton<DropBookXmlList>.Instance.GetData(data.id);
                QueueDropBookReward(data1, data.id);
                if (data1 != null)
                    LogLikeMod.rewards_lastKill.Add(data1);
            }
            else
                orig(self, data);
        }

        /// <summary>
        /// Hook for handling Roguelike acts actually ending.
        /// </summary>
        public void StageController_EndBattle(Action<StageController> orig, StageController self)
        {
            // Handle realization battle end
            if (RMRRealizationManager.InRealizationBattle && self.Phase != StageController.StagePhase.EndBattle)
            {
                bool victory = BattleObjectManager.instance.GetAliveListWithAvailable(Faction.Player).Count > 0
                    && BattleObjectManager.instance.GetAliveListWithAvailable(Faction.Enemy).Count == 0;
                RMRRealizationManager.OnRealizationBattleEnded(victory);
                orig(self);
                // Clear InRealizationBattle AFTER vanilla EndBattle completes,
                // so that ClearBattle/EndBattlePhase postfixes still see the guard
                // during the vanilla cleanup sequence.
                RMRRealizationManager.ClearRealizationFlag();
                return;
            }

            if (LogLikeMod.CheckStage(true) && self.Phase != StageController.StagePhase.EndBattle)
            {
                LogLikeMod.EndBattle = false;
                LogLikeMod.SetStagePhase(self, StageController.StagePhase.EndBattle);
            }
            else
                orig(self);
        }

        /// <summary>
        /// Hook for manipulating the UI for PickUpModel abno page popup screen.
        /// </summary>
        public IEnumerator LevelUpUI_OnSelectRoutine(Func<LevelUpUI, IEnumerator> orig, LevelUpUI self)
        {
            if (LogLikeMod.CheckStage())
            {
                FieldInfo _needUnitSelection = typeof(LevelUpUI).GetField("_needUnitSelection", AccessTools.all);
                FieldInfo _selectedCard = typeof(LevelUpUI).GetField("_selectedCard", AccessTools.all);
                self.cardSelectionGroup.interactable = false;
                yield return YieldCache.WaitForSeconds(0.1f);
                if ((bool)_needUnitSelection.GetValue(self))
                {
                    EmotionCardXmlInfo einfo = _selectedCard.GetValue(self) as EmotionCardXmlInfo;
                    LorId id = new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(einfo), einfo.id);
                    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetList(Faction.Player);
                    RewardPassiveInfo pinfo = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id);
                    PickUpModelBase pickupmodel = null;
                    if (pinfo != null)
                        pickupmodel = LogLikeMod.FindPickUp(pinfo.script);
                    if (aliveList != null)
                    {
                        if (pickupmodel != null)
                        {
                            if (pickupmodel.GetPickupTarget() != null)
                            {
                                aliveList = pickupmodel.GetPickupTarget();
                                goto label_11;
                            }
                            EmotionCardXmlInfo info = _selectedCard.GetValue(self) as EmotionCardXmlInfo;
                            pickupmodel.id = new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(info), info.id);
                            aliveList.RemoveAll(x => !pickupmodel.IsCanPickUp(x.UnitData.unitData));
                            info = null;
                        }
                        else
                            aliveList.RemoveAll(x => x.IsDead());
                    }
                label_11:
                    if (aliveList.Count > 0)
                    {
                        if (_selectedCard.GetValue(self) != null)
                            self.selectedEmotionCard.Init((EmotionCardXmlInfo)_selectedCard.GetValue(self), true);
                        if (Singleton<StageController>.Instance.AllyFormationDirection == Direction.LEFT)
                        {
                            self.selectedEmotionCard.transform.localPosition = new Vector3(410f, -510f);
                            self.selectedEmotionCardBg.transform.localScale = new Vector3(-1f, 1f, 1f);
                        }
                        else
                        {
                            self.selectedEmotionCard.transform.localPosition = new Vector3(-410f, -510f);
                            self.selectedEmotionCardBg.transform.localScale = new Vector3(1f, 1f, 1f);
                        }
                        List<UICustomSelectable_autofind> list = new List<UICustomSelectable_autofind>();
                        foreach (BattleUnitModel battleUnitModel in aliveList)
                        {
                            battleUnitModel.view.abCardSelector.Init(battleUnitModel, Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionLevel);
                            list.Add(battleUnitModel.view.abCardSelector.selectable);
                        }
                        foreach (UICustomSelectable_autofind uicustomSelectable_autofind in list)
                            uicustomSelectable_autofind.SetActivatedCharacterSelectables(list);
                        if (list.Count > 0)
                            BattleUIInputController.Instance.SelectSelectableForcely(list[0]);
                        self.OnSelectHide();
                        list = null;
                    }
                    else
                    {
                        self.StartCoroutine(LogLikeRoutines.DisableRoutine(self));
                        self.StartCoroutine(LogLikeRoutines.TranslateRoutine(true, self));
                    }
                    einfo = null;
                    id = null;
                    aliveList = null;
                    pinfo = null;
                }
                else
                {
                    self.StartCoroutine(LogLikeRoutines.DisableRoutine(self));
                    self.StartCoroutine(LogLikeRoutines.TranslateRoutine(true, self));
                }
            }
            else
                yield return orig(self);
        }

        /// <summary>
        /// Hook for manipulating the UI for PickUpModel abno page popup screen.
        /// </summary>
        public void LevelUpUI_SetEmotionPerDataUI(
          Action<LevelUpUI, float, float> orig,
          LevelUpUI self,
          float positivevalue,
          float negativevalue)
        {
            try
            {
                if (LogLikeMod.CheckStage(true))
                {
                    TextMeshProUGUI textMeshProUgui1 = (TextMeshProUGUI)typeof(LevelUpUI).GetField("txt_PositiveValueText", AccessTools.all).GetValue(self);
                    TextMeshProUGUI textMeshProUgui2 = (TextMeshProUGUI)typeof(LevelUpUI).GetField("txt_NegativeValueText", AccessTools.all).GetValue(self);
                    RectTransform rectTransform = (RectTransform)typeof(LevelUpUI).GetField("rect_BarContent", AccessTools.all).GetValue(self);
                    textMeshProUgui1.text = "0";
                    textMeshProUgui2.text = "0";
                    rectTransform.anchoredPosition = Vector2.zero;
                    rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
                    return;
                }
            }
            catch
            {
                orig(self, positivevalue, negativevalue);
            }
            orig(self, positivevalue, negativevalue);
        }

        /// <summary>
        /// Hook for manipulating the UI for rewards. All of them.
        /// </summary>
        public void LevelUpUI_InitBase(
          Action<LevelUpUI, int, bool> orig,
          LevelUpUI self,
          int selectedCount,
          bool isEgo = false)
        {
            orig(self, selectedCount, isEgo);
            if (LogLikeMod.CheckStage(true))
            {
                TextMeshProUGUI textMeshProUgui1 = (TextMeshProUGUI)typeof(LevelUpUI).GetField("txt_SelectDesc", AccessTools.all).GetValue(self);
                TextMeshProUGUI textMeshProUgui2 = (TextMeshProUGUI)typeof(LevelUpUI).GetField("txt_BtnSelectDesc", AccessTools.all).GetValue(self);
                if (LogLikeMod.skipPanel == null)
                {
                    Button button = ModdingUtils.CreateButton(self.selectablePanel.transform.parent, "AbCardSelection_Skip", new Vector2(1f, 1f), new Vector2(650f, 385f));
                    LogLikeMod.skipPanel = button;
                    LogLikeMod.skipPanelText = ModdingUtils.CreateText_TMP(button.transform, new Vector2(-10f, 0.0f), (int)textMeshProUgui2.fontSize, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, textMeshProUgui2.color, textMeshProUgui2.font);
                }
                LogLikeMod.skipPanel.onClick.RemoveAllListeners();
                LogLikeMod.skipPanel.onClick.AddListener(() => SkipCurrentRewardSelection(self));
                if (LogLikeMod.StageRemainText == null)
                {
                    UILogCustomSelectable BackGround = ModdingUtils.CreateLogSelectable(self.selectablePanel.transform.parent, "AbCardSelection_Skip", new Vector2(1f, 1f), new Vector2(0.0f, 500f));
                    BackGround.SelectEvent = new UnityEventBasedata();
                    BackGround.SelectEvent.AddListener((UnityAction<BaseEventData>)(e =>
                    {
                        string name = TextDataModel.GetText("ui_stageremain") + LogueBookModels.RemainStageList[LogLikeMod.curchaptergrade].Count.ToString();
                        Dictionary<string, int> dictionary = new Dictionary<string, int>();
                        foreach (LogueStageInfo info in LogueBookModels.RemainStageList[LogLikeMod.curchaptergrade])
                        {
                            LogueBookModels.CreateStageDesc(info);
                            EmotionCardXmlInfo pickUpXml = LogLikeMod.GetRegisteredPickUpXml(info);
                            if (pickUpXml == null)
                                continue;
                            AbnormalityCard abnormalityCard = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(pickUpXml.Name);
                            if (abnormalityCard == null)
                                continue;
                            if (!dictionary.ContainsKey(abnormalityCard.cardName))
                                dictionary[abnormalityCard.cardName] = 1;
                            else
                                ++dictionary[abnormalityCard.cardName];
                        }
                        string desc = string.Empty;
                        foreach (KeyValuePair<string, int> keyValuePair in dictionary)
                            desc = desc + keyValuePair.Key + keyValuePair.Value.ToString() + Environment.NewLine;
                        SingletonBehavior<UIBattleOverlayManager>.Instance.EnableBufOverlay(name, desc, (Sprite)null, BackGround.gameObject);
                    }));
                    BackGround.DeselectEvent = new UnityEventBasedata();
                    BackGround.DeselectEvent.AddListener((UnityAction<BaseEventData>)(e => SingletonBehavior<UIBattleOverlayManager>.Instance.DisableOverlay()));
                    TextMeshProUGUI textTmp = ModdingUtils.CreateText_TMP(BackGround.transform, new Vector2(-10f, 0.0f), (int)textMeshProUgui2.fontSize, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, textMeshProUgui2.color, textMeshProUgui2.font);
                    LogLikeMod.StageRemainPanel = BackGround;
                    LogLikeMod.StageRemainText = textTmp;
                }
                int num;
                switch (RewardingModel.rewardFlag)
                {
                    case RewardingModel.RewardFlag.CardReward:
                        num = 1;
                        break;
                    case RewardingModel.RewardFlag.PassiveReward:
                        RewardPassiveInfo firstPassive = LogLikeMod.rewards_passive != null
                            && LogLikeMod.rewards_passive.Count > 0
                            && LogLikeMod.rewards_passive[0] != null
                            && LogLikeMod.rewards_passive[0].rewards != null
                            && LogLikeMod.rewards_passive[0].rewards.Count > 0
                            ? LogLikeMod.rewards_passive[0].rewards[0]
                            : null;
                        num = firstPassive != null && firstPassive.rewardtype != RewardType.Creature ? 1 : 0;
                        break;
                    default:
                        num = 0;
                        break;
                }
                if (num != 0)
                {
                    LogLikeMod.skipPanel.gameObject.SetActive(true);
                    LogLikeMod.skipPanelText.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("ui_selectskip");
                }
                else
                    LogLikeMod.skipPanel.gameObject.SetActive(false);
                if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.NextStageChoose)
                {
                    LogLikeMod.StageRemainPanel.gameObject.SetActive(true);
                    TextMeshProUGUI stageRemainText1 = LogLikeMod.StageRemainText;
                    string text1 = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("ui_stageremain");
                    int count = LogueBookModels.RemainStageList.TryGetValue(LogLikeMod.curchaptergrade, out List<LogueStageInfo> curChapterList) && curChapterList != null
                        ? curChapterList.Count : 0;
                    string str1 = count.ToString();
                    string str2 = text1 + str1;
                    stageRemainText1.text = str2;
                    if (LogLikeMod.curstagetype == StageType.Boss && LogLikeMod.curchaptergrade < ChapterGrade.Grade7)
                    {
                        TextMeshProUGUI stageRemainText2 = LogLikeMod.StageRemainText;
                        string text2 = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("ui_stageremain");
                        if (LogueBookModels.RemainStageList.TryGetValue(LogLikeMod.curchaptergrade + 1, out List<LogueStageInfo> nextChapterList) && nextChapterList != null)
                            count = nextChapterList.Count;
                        else
                            count = 0;
                        string str3 = count.ToString();
                        string str4 = text2 + str3;
                        stageRemainText2.text = str4;
                    }
                }
                else
                    LogLikeMod.StageRemainPanel.gameObject.SetActive(false);
                if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.CardReward)
                {
                    textMeshProUgui1.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("BattleEnd_CardReward");
                    textMeshProUgui2.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("BattleEnd_CardReward");
                    if (!Singleton<TutorialManager>.Instance.IsSeeTuto("tutorial_BattlePage1_1"))
                        Singleton<TutorialManager>.Instance.LoadTuto("tutorial_BattlePage1_1");
                }
                if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.PassiveReward)
                {
                    textMeshProUgui1.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("BattleEnd_PassiveReward");
                    textMeshProUgui2.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("BattleEnd_PassiveReward");
                    if (self.candidates != null && self.candidates.Length > 0 && self.candidates[0].Card != null)
                    {
                        var candidateCard = self.candidates[0].Card;
                        var passiveInfo = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(candidateCard), candidateCard.id));
                        if (passiveInfo != null)
                        {
                            if (!Singleton<TutorialManager>.Instance.IsSeeTuto("tutorial_EquipPage1_1new") && passiveInfo.rewardtype == RewardType.EquipPage)
                                Singleton<TutorialManager>.Instance.LoadTuto("tutorial_EquipPage1_1new");
                            if (!Singleton<TutorialManager>.Instance.IsSeeTuto("tutorial_EmotionPage1_1") && passiveInfo.rewardtype == RewardType.Creature)
                                Singleton<TutorialManager>.Instance.LoadTuto("tutorial_EmotionPage1_1");
                        }
                    }
                }
                if (RewardingModel.rewardFlag != RewardingModel.RewardFlag.NextStageChoose)
                    return;
                textMeshProUgui1.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("BattleEnd_NextStage");
                textMeshProUgui2.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("BattleEnd_NextStage");
            }
            else if (LogLikeMod.skipPanel != null)
                LogLikeMod.skipPanel.gameObject.SetActive(false);
        }

        /* UNUSED
        public bool RoundEndPhase_ReturnUnit(
          Func<StageController, float, bool> orig,
          StageController self,
          float deltaTime)
        {
            return orig(self, deltaTime);
        }
        */

        /// <summary>
        /// Hook for hijacking EGO page screen to give card rewards instead.
        /// </summary>
        public void StageLibraryFloorModel_OnPickEgoCard(
          Action<StageLibraryFloorModel, EmotionEgoXmlInfo> orig,
          StageLibraryFloorModel self,
          EmotionEgoXmlInfo egoCard)
        {
            if (LogLikeMod.CheckStage(true)
                && RewardingModel.rewardFlag == RewardingModel.RewardFlag.EgoCardReward
                && LogLikeMod.egoSelectionQueue != null
                && LogLikeMod.egoSelectionQueue.Count > 0)
            {
                List<DiceCardXmlInfo> cardlist = new List<DiceCardXmlInfo>();
                foreach (BattleDiceCardUI egoSlot in SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.egoSlotList)
                {
                    if (egoSlot?.CardModel?.XmlData != null)
                        cardlist.Add(egoSlot.CardModel.XmlData);
                }
                Singleton<GlobalLogueEffectManager>.Instance.OnPickCardReward(cardlist, ItemXmlDataList.instance.GetCardItem(egoCard.CardId));
                RMRAbnormalityUnlockManager.UnlockEgoForCurrentRoute(egoCard.CardId);
                LogueBookModels.RecordAtlasEgoPage(egoCard.CardId);
                LogLikeMod.egoSelectionQueue.RemoveAt(0);
            }
            else
                orig(self, egoCard);
        }

        /// <summary>
        /// Hook for handling next stage choice and passive reward giving.
        /// </summary>
        public void StageLibraryFloorModel_OnPickPassiveCard(
          Action<StageLibraryFloorModel, EmotionCardXmlInfo, BattleUnitModel> orig,
          StageLibraryFloorModel self,
          EmotionCardXmlInfo card,
          BattleUnitModel target = null)
        {
            var stage = Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Stage(card), card.id));
            if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.NextStageChoose && stage != null)
            {
                if (stage.type == StageType.Creature)
                {
                    Debug.Log($"[RMR AbnoRoute] Creature stage selected. placeholder={stage.Id}, chapter={LogLikeMod.curchaptergrade}");
                    StageClassInfo abnormalityStage = RMRAbnormalityBattleRouter.PickStageForChapter(LogLikeMod.curchaptergrade);
                    if (abnormalityStage != null)
                    {
                        Debug.Log($"[RMR AbnoRoute] Routed to vanilla abnormality stage: {abnormalityStage.id}");
                        LogLikeMod.SetNextStage(abnormalityStage.id, StageType.Creature);
                        LogueBookModels.RemoveStageInlist(stage.Id, LogLikeMod.curchaptergrade);
                        return;
                    }
                    else
                    {
                        // No usable vanilla abnormality stage found.
                        // DO NOT fall through to the placeholder 99110x stage (which contains normal enemies).
                        // Instead, remove the placeholder and regenerate the next list.
                        Debug.LogError($"[RMR AbnoRoute] No vanilla abnormality stage for chapter {LogLikeMod.curchaptergrade}. Removing placeholder {stage.Id} from all chapters to prevent normal-battle routing.");
                        LogueBookModels.RemoveStageInlist(stage.Id);
                        LogLikeMod.nextlist = LogueBookModels.GetNextList(LogLikeMod.curchaptergrade);
                        // Pick a safe non-Creature fallback from the remain list so the game doesn't get stuck.
                        List<LogueStageInfo> remainList = LogueBookModels.RemainStageList[LogLikeMod.curchaptergrade];
                        LogueStageInfo fallbackStage = remainList.Find(s => s.type != StageType.Creature);
                        if (fallbackStage != null)
                        {
                            Debug.Log($"[RMR AbnoRoute] Fallback to non-Creature stage: {fallbackStage.Id} type={fallbackStage.type}");
                            LogLikeMod.SetNextStage(fallbackStage.Id, fallbackStage.type);
                            LogueBookModels.RemoveStageInlist(fallbackStage.Id, LogLikeMod.curchaptergrade);
                        }
                        else
                        {
                            Debug.LogError($"[RMR AbnoRoute] CRITICAL: No fallback stage available for chapter {LogLikeMod.curchaptergrade}!");
                        }
                        return;
                    }
                }
                LogLikeMod.SetNextStage(stage.Id, stage.type);
                RMRCore.ApplyBinahRedMistProgressionState();
            } else if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.NextStageChoose && stage == null)
            {
                Debug.Log("NULL STAGE ERROR!!: " + card.Name + " --- " + card.id + " --- Generated PID: " + LogLikeMod.GetPickUpXmlWorkShopId_Stage(card) ?? "NULL");
            }
            else if (Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(card), card.id)) != null)
            {
                RewardPassiveInfo pickedRewardInfo = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(card), card.id));
                if (pickedRewardInfo == null)
                {
                    Debug.LogWarning("[StageLibraryFloorModel_OnPickPassiveCard] pickedRewardInfo is null, cannot process reward.");
                    return;
                }
                if (pickedRewardInfo.rewardtype == RewardType.Creature && RewardingModel.rewardFlag == RewardingModel.RewardFlag.PassiveReward)
                {
                    RMRAbnormalityUnlockManager.OnEmotionPagePicked(card);
                    LogLikeMod.rewards_passive.RemoveAt(0);
                    return;
                }
                if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.EmtoionChoose)
                {
                    RMRAbnormalityUnlockManager.OnEmotionPagePicked(card);
                    if (RMRAbnormalityUnlockManager.IsNoAbnormalityFallback(card))
                        return;
                }
                if (card.Script == null || card.Script.Count == 0)
                {
                    Debug.LogError("[StageLibraryFloorModel_OnPickPassiveCard] card.Script is null or empty, cannot resolve PickUp.");
                    return;
                }
                PickUpModelBase pickUp = LogLikeMod.FindPickUp(card.Script[0]);
                if (pickUp != null)
                {
                    pickUp.rewardinfo = pickedRewardInfo;
                    pickUp.id = new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(card), card.id);
                    if (card.TargetType == EmotionTargetType.All || card.TargetType == EmotionTargetType.AllIncludingEnemy)
                    {
                        foreach (BattleUnitModel model in BattleObjectManager.instance.GetList(Faction.Player))
                        {
                            pickUp.OnPickUp(model);
                            if (LogueBookModels.playersPick.ContainsKey(model.UnitData.unitData))
                                LogueBookModels.playersPick[model.UnitData.unitData].Add(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(card), card.id));
                        }
                        if (card.TargetType == EmotionTargetType.AllIncludingEnemy)
                        {
                            foreach (BattleUnitModel model in BattleObjectManager.instance.GetList(Faction.Enemy))
                                pickUp.OnPickUp(model);
                        }
                    }
                    if (target != null)
                    {
                        pickUp.OnPickUp(target);
                        if (LogueBookModels.playersPick.ContainsKey(target.UnitData.unitData))
                            LogueBookModels.playersPick[target.UnitData.unitData].Add(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(card), card.id));
                    }
                    pickUp.OnPickUp();
                    Singleton<LogueSaveManager>.Instance.AddToObtainCount(pickUp);
                }
                switch (RewardingModel.rewardFlag)
                {
                    case RewardingModel.RewardFlag.PassiveReward:
                        LogLikeMod.rewards_passive.RemoveAt(0);
                        break;
                    case RewardingModel.RewardFlag.RewardInStage:
                        LogLikeMod.rewards_InStage.RemoveAt(0);
                        if (Singleton<ShopManager>.Instance.curshop != null && LogLikeMod.rewards_InStage.Count == 0)
                            Singleton<ShopManager>.Instance.curshop.HideShop();
                        if (Singleton<MysteryManager>.Instance.curMystery == null || !(Singleton<MysteryManager>.Instance.curMystery is MysteryModel_Rest))
                            break;
                        (Singleton<MysteryManager>.Instance.curMystery as MysteryModel_Rest).HideRest();
                        break;
                }
            }
            else
                orig(self, card, target);
        }

        /// <summary>
        /// Hook for initializing inventory, abnormality and crafting screens.
        /// </summary>
        public void UIBattleSettingEditPanel_Open(
          Action<UIBattleSettingEditPanel, UIBattleSettingEditTap> orig,
          UIBattleSettingEditPanel self,
          UIBattleSettingEditTap state)
        {
            if (LogLikeMod.CheckStage())
            {
                if (LogLikeMod.InvenBtn == null)
                {
                    Button fieldValue = LogLikeMod.GetFieldValue<Button>(self, "button_BattleCard");
                    LogLikeMod.InvenBtn = UnityEngine.Object.Instantiate<Button>(fieldValue, fieldValue.transform.parent);
                    LogLikeMod.InvenBtn.transform.localPosition = fieldValue.transform.localPosition + new Vector3(200f, 0.0f);
                    Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
                    buttonClickedEvent.AddListener((UnityAction)(() => LogLikeRoutines.OnClickInventory(self)));
                    LogLikeMod.InvenBtn.onClick = buttonClickedEvent;
                    LogLikeRoutines.SafeGetButtonComponents(LogLikeMod.InvenBtn, out UITextDataLoader component, out Image invenFrame);
                    if (component != null)
                    {
                        component.key = "ui_Inventory";
                        component.SetText();
                    }
                    LogLikeMod.InvenBtnFrame = invenFrame;
                    if (LogLikeMod.InvenBtnFrame != null)
                        LogLikeMod.InvenBtnFrame.enabled = false;
                }
                if (LogLikeMod.CreatureBtn == null)
                {
                    Button fieldValue = LogLikeMod.GetFieldValue<Button>(self, "button_BattleCard");
                    LogLikeMod.CreatureBtn = UnityEngine.Object.Instantiate<Button>(fieldValue, fieldValue.transform.parent);
                    LogLikeMod.CreatureBtn.transform.localPosition = fieldValue.transform.localPosition + new Vector3(400f, 0.0f);
                    Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
                    buttonClickedEvent.AddListener((UnityAction)(() => LogLikeRoutines.OnClickAtlasTab(self)));
                    LogLikeMod.CreatureBtn.onClick = buttonClickedEvent;
                    LogLikeRoutines.SafeGetButtonComponents(LogLikeMod.CreatureBtn, out UITextDataLoader component, out Image creatureFrame);
                    if (component != null)
                    {
                        component.key = "ui_AtlasTab";
                        component.SetText();
                    }
                    LogLikeMod.CreatureBtnFrame = creatureFrame;
                    if (LogLikeMod.CreatureBtnFrame != null)
                        LogLikeMod.CreatureBtnFrame.enabled = false;
                    LogLikeMod.AtlasBtn = LogLikeMod.CreatureBtn;
                    LogLikeMod.AtlasBtnFrame = LogLikeMod.CreatureBtnFrame;
                }
                if (LogLikeMod.CraftBtn == null)
                {
                    Button fieldValue = LogLikeMod.GetFieldValue<Button>(self, "button_BattleCard");
                    LogLikeMod.CraftBtn = UnityEngine.Object.Instantiate<Button>(fieldValue, fieldValue.transform.parent);
                    LogLikeMod.CraftBtn.transform.localPosition = fieldValue.transform.localPosition + new Vector3(600f, 0.0f);
                    Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
                    buttonClickedEvent.AddListener((UnityAction)(() => LogLikeRoutines.OnClickCraftTab(self)));
                    LogLikeMod.CraftBtn.onClick = buttonClickedEvent;
                    LogLikeRoutines.SafeGetButtonComponents(LogLikeMod.CraftBtn, out UITextDataLoader component, out Image craftFrame);
                    if (component != null)
                    {
                        component.key = "ui_CraftTab";
                        component.SetText();
                    }
                    LogLikeMod.CraftBtnFrame = craftFrame;
                    if (LogLikeMod.CraftBtnFrame != null)
                        LogLikeMod.CraftBtnFrame.enabled = false;
                }
                if (LogLikeMod.AtlasBtn == null && LogLikeMod.CreatureBtn != null)
                {
                    LogLikeMod.AtlasBtn = LogLikeMod.CreatureBtn;
                    LogLikeMod.AtlasBtnFrame = LogLikeMod.CreatureBtnFrame;
                }
                if (LogLikeMod.RealizationBtn == null)
                {
                    Button fieldValue = LogLikeMod.GetFieldValue<Button>(self, "button_BattleCard");
                    LogLikeMod.RealizationBtn = UnityEngine.Object.Instantiate<Button>(fieldValue, fieldValue.transform.parent);
                    LogLikeMod.RealizationBtn.transform.localPosition = fieldValue.transform.localPosition + new Vector3(800f, 0.0f);
                    Button.ButtonClickedEvent btnEvent = new Button.ButtonClickedEvent();
                    btnEvent.AddListener((UnityAction)(() => LogLikeRoutines.OnClickRealization(self)));
                    LogLikeMod.RealizationBtn.onClick = btnEvent;
                    LogLikeRoutines.SafeGetButtonComponents(LogLikeMod.RealizationBtn, out UITextDataLoader txtLoader, out Image realizationFrame);
                    if (txtLoader != null)
                    {
                        txtLoader.key = "ui_Realization";
                        txtLoader.SetText();
                    }
                    LogLikeRoutines.ApplyRealizationButtonText(LogLikeMod.RealizationBtn);
                    LogLikeMod.RealizationBtnFrame = realizationFrame;
                    if (LogLikeMod.RealizationBtnFrame != null)
                        LogLikeMod.RealizationBtnFrame.enabled = false;
                }
                LogLikeMod.InvenBtn.gameObject.SetActive(true);
                LogLikeMod.CreatureBtn.gameObject.SetActive(true);
                LogLikeMod.CraftBtn.gameObject.SetActive(true);
                LogLikeMod.AtlasBtn.gameObject.SetActive(true);
                LogLikeRoutines.ApplyRealizationButtonText(LogLikeMod.RealizationBtn);
                if (LogLikeMod.RealizationBtn != null)
                    LogLikeMod.RealizationBtn.gameObject.SetActive(RMRRealizationManager.InitialRelicEntryAvailable);
                Singleton<GlobalLogueInventoryPanel>.Instance.SetActive(false);
                Singleton<LogCreatureTabPanel>.Instance.SetActive(false);
                Singleton<LogCraftPanel>.Instance.SetActive(false);
                Singleton<LogAtlasPanel>.Instance.SetActive(false);
                Image image = (Image)typeof(UIBattleSettingEditPanel).GetField("img_BlockBackGroundBg", AccessTools.all).GetValue(self);
                self.SetBUttonState(state);
                image.raycastTarget = true;
                self.SetActivePanel(true);
            }
            else
            {
                LogLikeMod.InvenBtn.gameObject.SetActive(false);
                LogLikeMod.CreatureBtn.gameObject.SetActive(false);
                LogLikeMod.CraftBtn.gameObject.SetActive(false);
                LogLikeMod.AtlasBtn.gameObject.SetActive(false);
                if (LogLikeMod.RealizationBtn != null)
                    LogLikeMod.RealizationBtn.gameObject.SetActive(false);
                orig(self, state);
            }
        }

        /// <summary>
        /// Hook for unlocking BattleSetting combat page customization even after an Act.
        /// </summary>
        public void UIBattleSettingLibrarianInfoPanel_SetBattleCardSlotState(
          Action<UIBattleSettingLibrarianInfoPanel> orig,
          UIBattleSettingLibrarianInfoPanel self)
        {
            orig(self);
            if (!LogLikeMod.CheckStage())
                return;
            Color uiColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
            typeof(UIBattleSettingLibrarianInfoPanel).GetField("isBattlePageLock", AccessTools.all).SetValue(self, false);
            self.SetBattlePageSlotColor(uiColor);
        }

        /// <summary>
        /// Hook for unlocking BattleSetting key page customization even after an Act.
        /// </summary>
        public void UIBattleSettingLibrarianInfoPanel_SetEquipPageSlotState(
          Action<UIBattleSettingLibrarianInfoPanel> orig,
          UIBattleSettingLibrarianInfoPanel self)
        {
            orig(self);
            if (!LogLikeMod.CheckStage())
                return;
            Color uiColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
            typeof(UIBattleSettingLibrarianInfoPanel).GetField("isEquipPageLock", AccessTools.all).SetValue(self, false);
            self.SetEquipPageSlotColor(uiColor);
        }

        /* NOT IMPLEMENTED
        public void BattleDiceCardUI_ShowDetail(Action<BattleDiceCardUI> orig, BattleDiceCardUI self)
        {
            orig(self);
            if (!LogLikeMod.CheckStage(true))
                return;
            self.KeywordListUI.Activate();
        }
        */

        /// <summary>
        /// Hijacks and adjusts the ego card popup UI for miscellaneous purposes.
        /// </summary>
        public void BattleDiceCardUI_SetEgoCardForPopup(
          Action<BattleDiceCardUI, EmotionEgoXmlInfo> orig,
          BattleDiceCardUI self,
          EmotionEgoXmlInfo egoxmlinfo)
        {
            orig(self, egoxmlinfo);
            if (!LogLikeMod.CheckStage(true))
                return;
            foreach (GameObject gameObject in (GameObject[])typeof(BattleDiceCardUI).GetField("ob_NormalFrames", AccessTools.all).GetValue(self))
                gameObject.SetActive(true);
            foreach (GameObject gameObject in (GameObject[])typeof(BattleDiceCardUI).GetField("ob_EgoFrames", AccessTools.all).GetValue(self))
                gameObject.SetActive(false);
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(egoxmlinfo.CardId);
            FieldInfo field1 = typeof(BattleDiceCardUI).GetField("colorFrame", AccessTools.all);
            FieldInfo field2 = typeof(BattleDiceCardUI).GetField("colorLineardodge", AccessTools.all);
            field1.SetValue(self, UIColorManager.Manager.GetCardRarityColor(cardItem.Rarity));
            field2.SetValue(self, UIColorManager.Manager.GetCardRarityLinearColor(cardItem.Rarity));
            typeof(BattleDiceCardUI).GetMethod("SetRangeIconHsv", AccessTools.all).Invoke(self, new object[1]
            {
       UIColorManager.Manager.CardRangeHsvValue[(int) cardItem.Rarity]
            });
            typeof(BattleDiceCardUI).GetMethod("SetFrameColor", AccessTools.all).Invoke(self, new object[1]
            {
      field1.GetValue( self)
            });
            self.SetLinearDodgeColor(true);
        }

        /// <summary>
        /// Sets card states for UI purposes.<br></br>
        /// Not entirely sure if this is necessary. Won't bother touching it though. -CalmMagma
        /// </summary>
        public void BattleUnitCardsInHandUI_SetCardsObject(
          Action<BattleUnitCardsInHandUI, BattleUnitModel, bool> orig,
          BattleUnitCardsInHandUI self,
          BattleUnitModel unitModel,
          bool isClicked = true)
        {
            if (LogLikeMod.CheckStage(true))
            {
                GameObject gameObject = (GameObject)BattleUnitCardsInHandUIRootObjField.GetValue(self);
                Toggle toggle = (Toggle)BattleUnitCardsInHandUIToggleShowEgoField.GetValue(self);
                gameObject.SetActive(true);
                BattleUnitCardsInHandUIIsOverOnEgoToggleField.SetValue(self, false);
                if (isClicked)
                    BattleUnitCardsInHandUISelectedUnitField.SetValue(self, unitModel);
                else
                    BattleUnitCardsInHandUIHoveredUnitField.SetValue(self, unitModel);
                BattleUnitCardsInHandUI.EgoToggleState egoToggleState = BattleUnitCardsInHandUI.EgoToggleState.Hide;
                if (unitModel.personalEgoDetail.ExistsCard() || Singleton<SpecialCardListModel>.Instance.ExistEgoCardBySelected())
                    egoToggleState = toggle.isOn ? BattleUnitCardsInHandUI.EgoToggleState.On : BattleUnitCardsInHandUI.EgoToggleState.Off;
                else
                    BattleUnitCardsInHandUIHandStateField.SetValue(self, BattleUnitCardsInHandUI.HandState.BattleCard);
                if (!PlatformManager.Instance.AchievementUnlocked(AchievementEnum.ONCE_COPY) && unitModel.allyCardDetail.Exsist6CardsInHand_andCopy())
                    PlatformManager.Instance.UnlockAchievement(AchievementEnum.ONCE_COPY);
                BattleUnitCardsInHandUISetEgoToggleStateMethod.Invoke(self, new object[1]
                {
         egoToggleState
                });
            }
            else
                orig(self, unitModel, isClicked);
        }

        /// <summary>
        /// Hook for de-equipping combat pages from the deck.
        /// </summary>
        public bool DeckModel_MoveCardToInventory(
          Func<DeckModel, LorId, bool> orig,
          DeckModel self,
          LorId cardId)
        {
            if (!LogLikeMod.CheckStage())
                return orig(self, cardId);
            if (!((List<DiceCardXmlInfo>)typeof(DeckModel).GetField("_deck", AccessTools.all).GetValue(self)).Remove(ItemXmlDataList.instance.GetCardItem(cardId)))
                return false;
            LogueBookModels.AddCard(cardId, 1, false);
            return true;
        }

        /// <summary>
        /// Hook for equipping combat pages from the inventory.
        /// </summary>
        public CardEquipState DeckModel_AddCardFromInventory(
          Func<DeckModel, LorId, CardEquipState> orig,
          DeckModel self,
          LorId cardId)
        {
            CardEquipState cardEquipState;
            if (LogLikeMod.CheckStage())
            {
                List<DiceCardXmlInfo> diceCardXmlInfoList = (List<DiceCardXmlInfo>)typeof(DeckModel).GetField("_deck", AccessTools.all).GetValue(self);
                if (diceCardXmlInfoList.Count >= 9)
                {
                    cardEquipState = CardEquipState.FullOfDeck;
                }
                else
                {
                    DiceCardXmlInfo card = ItemXmlDataList.instance.GetCardItem(cardId);
                    DiceCardSelfAbilityBase diceCardSelfAbility = Singleton<AssemblyManager>.Instance.CreateInstance_DiceCardSelfAbility(card.Script);
                    CardEquipState state;
                    if (diceCardSelfAbility != null && diceCardSelfAbility is LogDiceCardSelfAbility && !(diceCardSelfAbility as LogDiceCardSelfAbility).CanAddDeck(self, out state))
                        return state;
                    if (diceCardXmlInfoList.FindAll(x => x.id.GetOriginalId() == card.id.GetOriginalId()).Count >= card.Limit)
                        cardEquipState = CardEquipState.OverCardLimit;
                    else if (!LogueBookModels.RemoveCard(card.id))
                    {
                        cardEquipState = CardEquipState.LackOfCards;
                    }
                    else
                    {
                        Singleton<LibraryQuestManager>.Instance.OnEditCard();
                        diceCardXmlInfoList.Add(card);
                        cardEquipState = CardEquipState.Equippable;
                    }
                }
            }
            else
                cardEquipState = orig(self, cardId);
            return cardEquipState;
        }

        /// <summary>
        /// Hook for overriding combat page list with the roguelike's.
        /// </summary>
        public void UIInvenCardListScroll_SetData(
          Action<UIInvenCardListScroll, List<DiceCardItemModel>, UnitDataModel> orig,
          UIInvenCardListScroll self,
          List<DiceCardItemModel> cards,
          UnitDataModel unitData)
        {
            if (LogLikeMod.CheckStage())
                cards = LogueBookModels.GetCardListForInven();
            orig(self, cards, unitData);
        }

        /// <summary>
        /// Hook for equipping a combat page into an unit-<br></br>
        /// SPECIFICALLY, defers control of AddCardFromInventory to BookModel.
        /// </summary>
        public CardEquipState UnitDataModel_AddCardFromInventory(
          Func<UnitDataModel, LorId, CardEquipState> orig,
          UnitDataModel self,
          LorId cardId)
        {
            if (!LogLikeMod.CheckStage())
                return orig(self, cardId);
            return ItemXmlDataList.instance.GetCardItem(cardId) == null ? 
                CardEquipState.ERROR : 
                self.bookItem.AddCardFromInventoryToCurrentDeck(cardId);
        }

        /// <summary>
        /// Hook for equipping a combat page into an unit.
        /// </summary>
        public CardEquipState BookModel_AddCardFromInventory(
            Func<BookModel, LorId, CardEquipState> orig, 
            BookModel self, 
            LorId cardId)
        {
            if (!LogLikeMod.CheckStage())
                return orig(self, cardId);
            if (self.IsFixedDeck())
            {
                return CardEquipState.ERROR;
            }
            if (self.IsLockByBluePrimary())
            {
                return CardEquipState.ERROR;
            }
            DiceCardXmlInfo cardXmlInfo = ItemXmlDataList.instance.GetCardItem(cardId, false);
            if (cardXmlInfo == null)
            {
                return CardEquipState.ERROR;
            }
            if (cardXmlInfo.optionList.Contains(CardOption.OnlyPage))
            {
                if (!self.GetOnlyCards().Exists((DiceCardXmlInfo x) => x.id.GetOriginalId() == cardXmlInfo.id.GetOriginalId()))
                {
                    return CardEquipState.OnlyPageLimit;
                }
            }
            else if (self.ClassInfo.RangeType == EquipRangeType.Melee)
            {
                if (cardXmlInfo.Spec.Ranged == CardRange.Far)
                {
                    return CardEquipState.FarTypeLimit;
                }
            }
            else if (self.ClassInfo.RangeType == EquipRangeType.Range && cardXmlInfo.Spec.Ranged == CardRange.Near)
            {
                return CardEquipState.NearTypeLimit;
            }
            return self._deck.AddCardFromInventory(cardId);
        }

        /// <summary>
        /// Hook for overriding keypage list with Roguelike's.
        /// </summary>
        public List<BookModel> BookInventoryModel_GetBookList_equip(
          Func<BookInventoryModel, List<BookModel>> orig,
          BookInventoryModel self)
        {
            return LogLikeMod.CheckStage() ? LogueBookModels.booklist : orig(self);
        }

        /* DEPRECATED, MOVED TO RMRCORE.BOOKSTOADDTOINVENTORY
        public List<LorId> DropBookInventoryModel_GetBookList_invitationBookList(
          Func<DropBookInventoryModel, List<LorId>> orig,
          DropBookInventoryModel self)
        {
            List<LorId> invitationBookList = orig(self);

            if (LoguePlayDataSaver.CheckPlayerData())
                invitationBookList.Add(new LorId(LogLikeMod.ModId, -855));
            invitationBookList.Add(new LorId(LogLikeMod.ModId, -2854));
            invitationBookList.Add(new LorId(LogLikeMod.ModId, -3854));
            invitationBookList.Add(new LorId(LogLikeMod.ModId, -4854));
            invitationBookList.Add(new LorId(LogLikeMod.ModId, -5854));
            return invitationBookList;
        }
        */

        /// <summary>
        /// Hook to disable the card equip info button.
        /// </summary>
        public void UIInvenCardSlot_OnClickCardEquipInfoButton(
          Action<UIInvenCardSlot> orig,
          UIInvenCardSlot self)
        {
            if (LogLikeMod.CheckStage())
                return;
            orig(self);
        }

        /// <summary>
        /// Hook to make exclusive combat pages show up properly in roguelike's card inventory
        /// </summary>
        public void UIInvenCardScrollList_ApplyFilterAll(Action<UIInvenCardListScroll> orig, UIInvenCardListScroll self)
        {
            if (!LogLikeMod.CheckStage())
            {
                orig(self);
                return;
            }
            self._currentCardListForFilter.Clear();
            List<DiceCardItemModel> cardsByDetailFilterUI = self.GetCardsByDetailFilterUI(self.GetCardBySearchFilterUI(self.GetCardsByCostFilterUI(self.GetCardsByGradeFilterUI(self._originCardList))));
            cardsByDetailFilterUI.Sort(new Comparison<DiceCardItemModel>(SortUtil.CardItemCompByCost));
            float y;
            if (self._unitdata != null)
            {
                Predicate<DiceCardItemModel> cond1 = (DiceCardItemModel x) => true;
                Predicate<DiceCardItemModel> cond2 = (DiceCardItemModel x) => true;
                switch (self._unitdata.bookItem.ClassInfo.RangeType)
                {
                    case EquipRangeType.Melee:
                        cond1 = ((DiceCardItemModel x) => x.GetSpec().Ranged == CardRange.Near);
                        break;
                    case EquipRangeType.Range:
                        cond1 = ((DiceCardItemModel x) => x.GetSpec().Ranged == CardRange.Far);
                        break;
                    case EquipRangeType.Hybrid:
                        cond1 = (DiceCardItemModel x) => true;
                        break;
                }
                List<DiceCardXmlInfo> onlyCards = self._unitdata.bookItem.GetOnlyCards();
                cond2 = (DiceCardItemModel x) => onlyCards.Exists((DiceCardXmlInfo z) => z.id.GetOriginalId() == x.GetID().GetOriginalId());
                foreach (DiceCardItemModel item in cardsByDetailFilterUI.FindAll((DiceCardItemModel x) => x.ClassInfo.optionList.Contains(CardOption.OnlyPage) && !cond2(x)))
                {
                    cardsByDetailFilterUI.Remove(item);
                }
                self._currentCardListForFilter.AddRange(cardsByDetailFilterUI.FindAll(delegate (DiceCardItemModel x)
                {
                    if (!x.ClassInfo.optionList.Contains(CardOption.OnlyPage))
                    {
                        return cond1(x);
                    }
                    return cond2(x);
                }));
                self._currentCardListForFilter.AddRange(cardsByDetailFilterUI.FindAll((DiceCardItemModel x) => !(x.ClassInfo.optionList.Contains(CardOption.OnlyPage) ? cond2(x) : cond1(x))));
            }
            else
            {
                self._currentCardListForFilter.AddRange(cardsByDetailFilterUI);
            }
            float x2 = (float)self.column * self.slotWidth;
            y = (float)(self.GetMaxRow() + self.row - 1) * self.slotHeight;
            self.scrollBar.SetScrollRectSize(x2, y);
            self.scrollBar.SetWindowPosition(0f, 0f);
            if (self.slotList != null && self.slotList.Count > 0)
                self.selectablePanel.ChildSelectable = self.slotList[0].selectable;
            self.SetCardsData(self.GetCurrentPageList());
        }

        /// <summary>
        /// Hook to manipulate card slot states to refer to the roguelike inventory.
        /// </summary>
        public void UIInvenCardSlot_SetSlotState(Action<UIInvenCardSlot> orig, UIInvenCardSlot self)
        {
            if (LogLikeMod.CheckStage())
            {
                TextMeshProUGUI textMeshProUgui = (TextMeshProUGUI)typeof(UIInvenCardSlot).GetField("txt_deckLimit", AccessTools.all).GetValue(self);
                GameObject gameObject = (GameObject)typeof(UIInvenCardSlot).GetField("deckLimitRoot", AccessTools.all).GetValue(self);
                DiceCardItemModel _cardModel = (DiceCardItemModel)typeof(UIOriginCardSlot).GetField("_cardModel", AccessTools.all).GetValue(self);
                FieldInfo field = typeof(UIInvenCardSlot).GetField("slotState", AccessTools.all);
                field.SetValue(self, UIINVENCARD_STATE.None);
                if (_cardModel.num <= 0)
                    field.SetValue(self, UIINVENCARD_STATE.NumberZero);
                if (UI.UIController.Instance.CurrentUnit.GetDeckAll().FindAll(x => x.id.GetOriginalId() == _cardModel.GetID().GetOriginalId()).Count >= _cardModel.GetLimit())
                    field.SetValue(self, UIINVENCARD_STATE.LimitedDeck);
                UnitDataModel currentUnit = UI.UIController.Instance.CurrentUnit;
                if (currentUnit != null)
                {
                    BookModel bookItem = currentUnit.bookItem;
                    List<DiceCardXmlInfo> onlyCards = bookItem.GetOnlyCards();
                    if (_cardModel.ClassInfo.optionList.Contains(CardOption.OnlyPage))
                    {
                        if (!onlyCards.Exists(y => y.id.GetOriginalId() == _cardModel.GetID().GetOriginalId()))
                            field.SetValue(self, UIINVENCARD_STATE.OnlyPage);
                    }
                    else if (bookItem.ClassInfo.RangeType == EquipRangeType.Melee && _cardModel.GetSpec().Ranged == CardRange.Far)
                        field.SetValue(self, UIINVENCARD_STATE.RangeCard);
                    else if (bookItem.ClassInfo.RangeType == EquipRangeType.Range && _cardModel.GetSpec().Ranged == CardRange.Near)
                        field.SetValue(self, UIINVENCARD_STATE.MeleeCard);
                }
                gameObject.gameObject.SetActive((UIINVENCARD_STATE)field.GetValue(self) > UIINVENCARD_STATE.None);
                self.SetGrayScale((UIINVENCARD_STATE)field.GetValue(self) > UIINVENCARD_STATE.None);
                switch ((UIINVENCARD_STATE)field.GetValue(self))
                {
                    case UIINVENCARD_STATE.LimitedDeck:
                        textMeshProUgui.text = TextDataModel.GetText("ui_card_equipstate_overcardlimit");
                        break;
                    case UIINVENCARD_STATE.LimitedFloor:
                        textMeshProUgui.text = TextDataModel.GetText("ui_card_equipstate_overfloorlimit");
                        break;
                    case UIINVENCARD_STATE.NumberZero:
                        textMeshProUgui.text = TextDataModel.GetText("ui_card_equipstate_lackofcards");
                        break;
                    case UIINVENCARD_STATE.OnlyPage:
                        textMeshProUgui.text = TextDataModel.GetText("ui_card_equipstate_onlypagelimit");
                        break;
                    case UIINVENCARD_STATE.RangeCard:
                        textMeshProUgui.text = TextDataModel.GetText("ui_card_equipstate_fartypelimit");
                        break;
                    case UIINVENCARD_STATE.MeleeCard:
                        textMeshProUgui.text = TextDataModel.GetText("ui_card_equipstate_neartypelimit");
                        break;
                }
                self.RefreshNumbersData();
            }
            else
                orig(self);
        }

        /// <summary>
        /// Hook to initialize roguelike reception and gamemodes.
        /// </summary>
        public void UIInvitationRightMainPanel_ConfirmSendInvitation(
          Action<UIInvitationRightMainPanel> orig,
          UIInvitationRightMainPanel self)
        {
            StageClassInfo bookRecipe = self.GetBookRecipe();
            Singleton<StagesXmlList>.Instance.RestoreToDefault();
            Singleton<RewardPassivesList>.Instance.RestoreToDefault();
            Singleton<MysteryXmlList>.Instance.RestoreToDefault();
            Singleton<CardDropValueList>.Instance.RestoreToDefault();

            RMRCore.CurrentGamemode = null;

            LorId invitation = bookRecipe.id;
            bool succes = false;

            if (invitation == new LorId(RMRCore.packageId, -855))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(invitation, true);
                RMRCore.CurrentGamemode.FilterContent();

                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadPlayData();
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                this.Log("CONTINUED ROGUELIKE RUN! " + RMRCore.CurrentGamemode.SaveDataString);
                return;
            }
            else if (bookRecipe.id == new LorId(LogLikeMod.ModId, -2854))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(new LorId(RMRCore.packageId, -2854), false);
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadChDebugData(ChapterGrade.Grade2);
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter1());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter2());
                LoguePlayDataSaver.RemovePlayerData();
            }
            else if (bookRecipe.id == new LorId(LogLikeMod.ModId, -3854))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(new LorId(RMRCore.packageId, -3854), false);
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadChDebugData(ChapterGrade.Grade3);
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter1());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter2());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter3());
                LoguePlayDataSaver.RemovePlayerData();
            }
            else if (bookRecipe.id == new LorId(LogLikeMod.ModId, -4854))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(new LorId(RMRCore.packageId, -4854), false);
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadChDebugData(ChapterGrade.Grade4);
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter1());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter2());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter3());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter4());
                LoguePlayDataSaver.RemovePlayerData();
            }
            else if (bookRecipe.id == new LorId(LogLikeMod.ModId, -5854))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(new LorId(RMRCore.packageId, -5854), false);
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadChDebugData(ChapterGrade.Grade5);
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter1());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter2());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter3());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter4());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter5());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftExclusiveCardChapter5());
                LoguePlayDataSaver.RemovePlayerData();
            }
            else if (bookRecipe.id == new LorId(LogLikeMod.ModId, -6854))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(new LorId(RMRCore.packageId, -6854), false);
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadChDebugData(ChapterGrade.Grade6);
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter1());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter2());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter3());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter4());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter5());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftExclusiveCardChapter5());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftExclusiveCardChapter6());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter6());
                LoguePlayDataSaver.RemovePlayerData();
            }
            else if (bookRecipe.id == new LorId(LogLikeMod.ModId, -7854))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(new LorId(RMRCore.packageId, -7854), false);
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadChDebugData(ChapterGrade.Grade7);
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter1());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter2());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter3());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter4());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter5());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter6());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftExclusiveCardChapter5());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftExclusiveCardChapter6());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter7());
                LoguePlayDataSaver.RemovePlayerData();
            }

            try
            {
                succes = RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(invitation, false);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            if (RMRCore.CurrentGamemode == null)
                RMRCore.CurrentGamemode = new RoguelikeGamemode_RMR_Default();

            if (succes)
            {
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                LogueBookModels.CreatePlayer();
                orig(self);
                LogueBookModels.CreatePlayerBattle();
                LoguePlayDataSaver.RemovePlayerData();
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                GlobalLogueEffectManager.Instance.AddEffects(new RMREffect_ExtendedFunctionalityEffect());
                this.Log("NEW RUN! " + RMRCore.CurrentGamemode.SaveDataString);
            }
            else
            {
                orig(self);
                this.Log("REGULAR RECEPTION!");
            }
        }

        /// <summary>
        /// Hook to force the character list panel to render unitData based on <see cref="LogueBookModels.playerBattleModel"/>.
        /// </summary>
        public void UILibrarianCharacterListPanel_SetLibrarianCharacterListPanel_Battle(
          Action<UILibrarianCharacterListPanel> orig,
          UILibrarianCharacterListPanel self)
        {
            if (LogLikeMod.CheckStage())
            {
                UICharacterList uiCharacterList = (UICharacterList)typeof(UILibrarianCharacterListPanel).GetField("CharacterList", AccessTools.all).GetValue(self);
                List<UnitBattleDataModel> playerBattleModel = LogueBookModels.playerBattleModel;
                self.SetCharacterRenderer(playerBattleModel, false);
                uiCharacterList.InitUnitListFromBattleData(playerBattleModel);
                typeof(UILibrarianCharacterListPanel).GetMethod("UpdateFrameToSephirah", AccessTools.all).Invoke(self, new object[1]
                {
                     SephirahType.None
                });
            }
            else
                orig(self);
        }

        /// <summary>
        /// Hook to disable sephirot selection buttons.
        /// </summary>
        public void UILibrarianCharacterListPanel_InitSephirahSelectionButtonsInBattle(
          Action<UILibrarianCharacterListPanel, List<StageLibraryFloorModel>> orig,
          UILibrarianCharacterListPanel self,
          List<StageLibraryFloorModel> floors)
        {
            if (LogLikeMod.CheckStage())
            {
                
                foreach (UISephirahSelectionButton sephirahSelectionButton in (List<UISephirahSelectionButton>)typeof(UILibrarianCharacterListPanel).GetField("SephirahSelectionButtons", AccessTools.all).GetValue(self))
                {
                    sephirahSelectionButton.Deactivate();
                    sephirahSelectionButton.SetLock();
                }
            }
            else
                orig(self, floors);
        }

    }

    [HarmonyPatch]
    public class LogLikePatches
    {
        #region PREFIXES 
        /// <summary>
        /// Prefix patch to hijack the abnormality page get card id method to support the reward dictionary.
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(EmotionEgoXmlInfo), "CardId", MethodType.Getter)]
        public static bool EmotionEgoXmlInfo_get_CardId(EmotionEgoXmlInfo __instance, ref LorId __result)
        {
            if (LogLikeMod.RewardCardDic_Dummy == null)
                return true;
            foreach (KeyValuePair<string, List<EmotionEgoXmlInfo>> keyValuePair in LogLikeMod.RewardCardDic_Dummy)
            {
                if (keyValuePair.Value.Contains(__instance))
                {
                    __result = new LorId(keyValuePair.Key, __instance._CardId);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Important prefix patch to correctly get upgraded cards from the inventory/card dictionary.
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(ItemXmlDataList), nameof(ItemXmlDataList.GetCardItem), new Type[2] { typeof(LorId), typeof(bool) })]
        public static bool ItemXmlDataList_GetCardItem(
          ItemXmlDataList __instance,
          LorId id,
          bool errNull = false)
        {
            UpgradeMetadata metadata;
            if (LogLikeMod.CheckStage() && UpgradeMetadata.UnpackPid(id.packageId, out metadata))
                Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(metadata.actualPid, id.id), metadata.index, metadata.count);
            return true;
        }

        /// <summary>
        /// Ancient prefix which just made abcdcode's TextDataModel actually import stuff and override vanilla if necessary.
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(TextDataModel), nameof(TextDataModel.GetText))]
        public static bool TextDataModel_GetText(string id, ref string __result, params object[] args)
        {
            if (!ShouldOverrideVanillaTextWithRmrText(id))
                return true;

            string text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText(id, args);
            if (!(text != string.Empty))
                return true;
            __result = text;
            return false;
        }

        private static bool ShouldOverrideVanillaTextWithRmrText(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            if (IsExplicitRmrTextKey(id))
                return true;

            return LogLikeMod.CheckStage()
                || RMRRealizationManager.PendingRealizationBattle
                || RMRRealizationManager.InRealizationBattle;
        }

        private static bool IsExplicitRmrTextKey(string id)
        {
            if (id == "ui_ExceptionWithLog")
                return true;

            return id.StartsWith("ui_RMR_", StringComparison.Ordinal)
                || id.StartsWith("RMR_", StringComparison.Ordinal)
                || id.StartsWith("LogueLike", StringComparison.Ordinal)
                || id.StartsWith("BossReward", StringComparison.Ordinal)
                || id.StartsWith("MysteryCh", StringComparison.Ordinal)
                || id.StartsWith("StartBoost", StringComparison.Ordinal)
                || id.StartsWith("MemberShip", StringComparison.Ordinal);
        }

        [HarmonyPrefix, HarmonyPatch(typeof(StageController), nameof(StageController.RoundStartPhase_System))]
        public static bool StageController_RoundStartPhase_System(StageController __instance)
        {
            if (!LogLikeMod.GetFieldValue<bool>(__instance, "_bCalledRoundStart_system") && LogLikeMod.CheckStage())
                Singleton<GlobalLogueEffectManager>.Instance.OnRoundStart(__instance);
            return true;
        }

        /*
        [HarmonyPrefix, HarmonyPatch(typeof(StageController), nameof(StageController.StartParrying))]
        public static bool StageController_StartParrying(
          StageController __instance,
          BattlePlayingCardDataInUnitModel cardA,
          BattlePlayingCardDataInUnitModel cardB)
        {
            if (cardA.owner.passiveDetail.HasPassive<PassiveAbility_ShopPassiveMook4>() && cardB is BattleKeepedCardDataInUnitModel)
            {
                LogLikeMod.SetStagePhase(__instance, StageController.StagePhase.ExecuteOneSideAction);
                cardA.owner.turnState = BattleUnitTurnState.DOING_ACTION;
                cardA.target.turnState = BattleUnitTurnState.DOING_ACTION;
                Singleton<BattleOneSidePlayManager>.Instance.StartOneSidePlay(cardA);
                return false;
            }
            if (!cardB.owner.passiveDetail.HasPassive<PassiveAbility_ShopPassiveMook4>() || !(cardA is BattleKeepedCardDataInUnitModel))
                return true;
            LogLikeMod.SetStagePhase(__instance, StageController.StagePhase.ExecuteOneSideAction);
            cardB.owner.turnState = BattleUnitTurnState.DOING_ACTION;
            cardB.target.turnState = BattleUnitTurnState.DOING_ACTION;
            Singleton<BattleOneSidePlayManager>.Instance.StartOneSidePlay(cardB);
            return false;
        }
        */

        /// <summary>
        /// Currently unused (?) patch to add support for modded story icons (strictly for Roguelike).
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(UISpriteDataManager), nameof(UISpriteDataManager.GetStoryIcon))]
        public static bool UISpriteDataManager_GetStoryIcon(
          string story,
          ref UIIconManager.IconSet __result)
        {
            if (!LogLikeMod.CheckStage() || !story.Contains("<LogLike>"))
                return true;
            string key = story.Remove(0, 9);
            Sprite artWork = LogLikeMod.ArtWorks[key];
            Sprite artWorkGlow = LogLikeMod.ArtWorks[key + "_glow"];
            if (artWork != null)
                __result = new UIIconManager.IconSet()
                {
                    icon = artWork,
                    iconGlow = artWorkGlow ?? artWork
                };
            return false;
        }

        /// <summary>
        /// Patch to hijack the BattleSetting screen to add the inventory, abnormality and crafting tabs.
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(UIBattleSettingEditPanel), nameof(UIBattleSettingEditPanel.SetBUttonState))]
        public static bool UIBattleSettingEditPanel_SetBUttonState(
          UIBattleSettingEditPanel __instance,
          UIBattleSettingEditTap state)
        {
            if (!LogLikeMod.CheckStage())
            {
                Singleton<GlobalLogueInventoryPanel>.Instance.SetActive(false);
                Singleton<LogAtlasPanel>.Instance.SetActive(false);
                return true;
            }
            Button fieldValue1 = LogLikeMod.GetFieldValue<Button>(__instance, "button_EquipPage");
            Button fieldValue2 = LogLikeMod.GetFieldValue<Button>(__instance, "button_BattleCard");
            Image fieldValue3 = LogLikeMod.GetFieldValue<Image>(__instance, "img_equippageFrame");
            Image fieldValue4 = LogLikeMod.GetFieldValue<Image>(__instance, "img_battlecardFrame");
            UISettingEquipPageInvenPanel fieldValue5 = LogLikeMod.GetFieldValue<UISettingEquipPageInvenPanel>(__instance, "_equipPagePanel");
            UISettingCardInvenPanel fieldValue6 = LogLikeMod.GetFieldValue<UISettingCardInvenPanel>(__instance, "_battleCardPanel");
            RectTransform fieldValue7 = LogLikeMod.GetFieldValue<RectTransform>(__instance, "rect_LeftBg");
            switch (state)
            {
                case (UIBattleSettingEditTap)2:
                    fieldValue7.localPosition = (Vector3)new Vector2(0.0f, 0.0f);
                    ColorBlock colors1 = fieldValue1.colors;
                    ColorBlock colors2 = fieldValue2.colors;
                    colors1.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    colors2.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    fieldValue1.colors = colors1;
                    fieldValue2.colors = colors2;
                    fieldValue3.enabled = false;
                    fieldValue4.enabled = false;
                    fieldValue5.SetActivePanel(false);
                    fieldValue6.SetActivePanel(false);
                    LogLikeMod.InvenBtnFrame.enabled = true;
                    LogLikeMod.CreatureBtnFrame.enabled = false;
                    LogLikeMod.CraftBtnFrame.enabled = false;
                    LogLikeMod.AtlasBtnFrame.enabled = false;
                    Singleton<GlobalLogueInventoryPanel>.Instance.SetActive(true);
                    Singleton<LogCreatureTabPanel>.Instance.SetActive(false);
                    Singleton<LogCraftPanel>.Instance.SetActive(false);
                    Singleton<LogAtlasPanel>.Instance.SetActive(false);
                    return false;
                case (UIBattleSettingEditTap)3:
                    fieldValue7.localPosition = (Vector3)new Vector2(0.0f, 0.0f);
                    ColorBlock colors3 = fieldValue1.colors;
                    ColorBlock colors4 = fieldValue2.colors;
                    colors3.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    colors4.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    fieldValue1.colors = colors3;
                    fieldValue2.colors = colors4;
                    fieldValue3.enabled = false;
                    fieldValue4.enabled = false;
                    fieldValue5.SetActivePanel(false);
                    fieldValue6.SetActivePanel(false);
                    LogLikeMod.InvenBtnFrame.enabled = false;
                    LogLikeMod.CreatureBtnFrame.enabled = true;
                    LogLikeMod.CraftBtnFrame.enabled = false;
                    LogLikeMod.AtlasBtnFrame.enabled = false;
                    Singleton<GlobalLogueInventoryPanel>.Instance.SetActive(false);
                    Singleton<LogCreatureTabPanel>.Instance.SetActive(true);
                    Singleton<LogCraftPanel>.Instance.SetActive(false);
                    Singleton<LogAtlasPanel>.Instance.SetActive(false);
                    return false;
                case (UIBattleSettingEditTap)4:
                    fieldValue7.localPosition = (Vector3)new Vector2(0.0f, 0.0f);
                    ColorBlock colors5 = fieldValue1.colors;
                    ColorBlock colors6 = fieldValue2.colors;
                    colors5.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    colors6.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    fieldValue1.colors = colors5;
                    fieldValue2.colors = colors6;
                    fieldValue3.enabled = false;
                    fieldValue4.enabled = false;
                    fieldValue5.SetActivePanel(false);
                    fieldValue6.SetActivePanel(false);
                    LogLikeMod.InvenBtnFrame.enabled = false;
                    LogLikeMod.CreatureBtnFrame.enabled = false;
                    LogLikeMod.CraftBtnFrame.enabled = true;
                    LogLikeMod.AtlasBtnFrame.enabled = false;
                    Singleton<GlobalLogueInventoryPanel>.Instance.SetActive(false);
                    Singleton<LogCreatureTabPanel>.Instance.SetActive(false);
                    Singleton<LogCraftPanel>.Instance.SetActive(true);
                    Singleton<LogAtlasPanel>.Instance.SetActive(false);
                    return false;
                case (UIBattleSettingEditTap)5:
                    fieldValue7.localPosition = (Vector3)new Vector2(0.0f, 0.0f);
                    ColorBlock colors7 = fieldValue1.colors;
                    ColorBlock colors8 = fieldValue2.colors;
                    colors7.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    colors8.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    fieldValue1.colors = colors7;
                    fieldValue2.colors = colors8;
                    fieldValue3.enabled = false;
                    fieldValue4.enabled = false;
                    fieldValue5.SetActivePanel(false);
                    fieldValue6.SetActivePanel(false);
                    LogLikeMod.InvenBtnFrame.enabled = false;
                    LogLikeMod.CreatureBtnFrame.enabled = false;
                    LogLikeMod.CraftBtnFrame.enabled = false;
                    LogLikeMod.AtlasBtnFrame.enabled = true;
                    Singleton<GlobalLogueInventoryPanel>.Instance.SetActive(false);
                    Singleton<LogCreatureTabPanel>.Instance.SetActive(false);
                    Singleton<LogCraftPanel>.Instance.SetActive(false);
                    Singleton<LogAtlasPanel>.Instance.SetActive(true);
                    return false;
                default:
                    Singleton<GlobalLogueInventoryPanel>.Instance.SetActive(false);
                    Singleton<LogCreatureTabPanel>.Instance.SetActive(false);
                    Singleton<LogCraftPanel>.Instance.SetActive(false);
                    Singleton<LogAtlasPanel>.Instance.SetActive(false);
                    LogLikeMod.InvenBtnFrame.enabled = false;
                    LogLikeMod.CreatureBtnFrame.enabled = false;
                    LogLikeMod.CraftBtnFrame.enabled = false;
                    LogLikeMod.AtlasBtnFrame.enabled = false;
                    return true;
            }
        }

        /// <summary>
        /// Prevents the reception from ending immediately upon clicking the back button. (I THINK)
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(UIBattleSettingPanel), nameof(UIBattleSettingPanel.OnClickBackButton))]
        public static bool UIBattleSettingPanel_OnClickBackButton()
        {
            if (LogRealizationPanel.Instance != null && LogRealizationPanel.Instance.TryHandleBack())
                return false;
            return !LogLikeMod.CheckStage() || !UIPassiveSuccessionPopup.Instance.isActiveAndEnabled;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIPassiveSuccessionPopup), nameof(UIPassiveSuccessionPopup.Open))]
        public static void UIPassiveSuccessionPopup_Open()
        {
            if (!LogLikeMod.CheckStage())
                return;
            LogLikeRoutines.SetBattleSettingCardPanelVisible(false);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIPassiveSuccessionPopup), nameof(UIPassiveSuccessionPopup.Close))]
        public static void UIPassiveSuccessionPopup_Close()
        {
            if (!LogLikeMod.CheckStage())
                return;
            LogLikeRoutines.SetBattleSettingCardPanelVisible(true);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIPassiveSuccessionPopup), nameof(UIPassiveSuccessionPopup.CloseDefault))]
        public static void UIPassiveSuccessionPopup_CloseDefault()
        {
            if (!LogLikeMod.CheckStage())
                return;
            LogLikeRoutines.SetBattleSettingCardPanelVisible(true);
        }

        /*
        /// <summary>
        /// Patch for moving the positioning of enemy profiles.
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(BattleUnitInfoManagerUI), nameof(BattleUnitInfoManagerUI.Initialize))]
        public static bool BattleUnitInfoManagerUI_Initialize(BattleUnitInfoManagerUI __instance)
        {
            if (__instance.enemyProfileArray.Length < 10)
            {
                List<Vector2> vector2List = new List<Vector2>()
                  {
                    new Vector2(121f, 410f),
                    new Vector2(121f, 480f),
                    new Vector2(121f, 550f),
                    new Vector2(121f, 620f),
                    new Vector2(121f, 690f)
                  };
                BattleCharacterProfileUI enemyProfile = __instance.enemyProfileArray[0];
                int index = 0;
                while (__instance.enemyProfileArray.Length < 10)
                {
                    __instance.enemyProfileArray = __instance.enemyProfileArray.AddToArray<BattleCharacterProfileUI>(LogLikeMod.LogBattleCharacterProfileUI.BattleUnitInfoManagerUI_Copying(enemyProfile));
                    __instance.enemyProfileArray[__instance.enemyProfileArray.Length - 1].gameObject.transform.localPosition = (Vector3)vector2List[index];
                    __instance.enemyProfileArray[__instance.enemyProfileArray.Length - 1].gameObject.SetActive(false);
                    ++index;
                }
            }
            return true;
        }
        */

        /// <summary>
        /// Patch for hijacking UI popups to show in the BattleSetting screen. (unsure)
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(UIPopupWindowManager), nameof(UIPopupWindowManager.Update))]
        public static bool UIPopupWindowManager_Update()
        {
            return !LogLikeMod.CheckStage() || UI.UIController.Instance.CurrentUIPhase != UIPhase.BattleSetting;
        }


        [HarmonyPrefix, HarmonyPatch(typeof(UIPassiveSuccessionSlot), nameof(UIPassiveSuccessionSlot.SetDataModel))]
        public static bool UIPassiveSuccessionSlot_SetDataModel(
          UIPassiveSuccessionSlot __instance,
          PassiveModel passive,
          ref bool __result)
        {
            if (passive == null)
            {
                __result = false;
                return false;
            }
            if (passive.reservedData == null)
            {
                __result = false;
                return false;
            }
            if (!(passive.reservedData.currentpassive.id == new LorId(LogLikeMod.ModId, 1)))
                return true;
            __result = false;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(UICharacterListPanel), nameof(UICharacterListPanel.RefreshBattleUnitDataModel))]
        public static bool UICharacterListPanel_RefreshBattleUnitDataModel(
          UICharacterListPanel __instance,
          UnitDataModel data)
        {
            if (!LogLikeMod.CheckStage())
                return true;
            __instance.Log("Refrash Character start");
            UnitBattleDataModel battledata = LogueBookModels.playerBattleModel.Find(x => x.unitData == data);
            if (battledata != null)
            {
                UICharacterSlot uiCharacterSlot = LogLikeMod.GetFieldValue<UICharacterList>(__instance, "CharacterList").slotList.Find(x => x.unitBattleData == battledata);
                if (uiCharacterSlot != null && uiCharacterSlot.unitBattleData != null)
                {
                    uiCharacterSlot.ReloadHpBattleSettingSlot();
                    __instance.Log("Refrash Character success");
                }
            }
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(UnitDataModel), nameof(UnitDataModel.EquipBookForUI))]
        public static bool UnitDataModel_EquipBookForUI(
          UnitDataModel __instance,
          BookModel newBook,
          ref bool __result,
          bool isEnemySetting = false,
          bool force = false)
        {
            if (!LogLikeMod.CheckStage() || isEnemySetting)
                return true;
            int num = LogueBookModels.playerBattleModel.IndexOf(LogueBookModels.playerBattleModel.Find((Predicate<UnitBattleDataModel>)(x => x.unitData == __instance)));
            if (newBook != null)
            {
                __instance.Log("newBook not null");
                LogueBookModels.EquipNewPage(LogueBookModels.playerBattleModel.Find((Predicate<UnitBattleDataModel>)(x => x.unitData == __instance)), newBook.ClassInfo);
                LogueBookModels.RemoveEquip(__instance);
                newBook.SetOwner(__instance);
                __result = true;
                return false;
            }
            __instance.Log("newBook null");
            LogueBookModels.EquipNewPage(LogueBookModels.playerBattleModel.Find((Predicate<UnitBattleDataModel>)(x => x.unitData == __instance)), LogueBookModels.BaseXmlInfo);
            if (num != 0)
            {
                __instance.bookItem.ClassInfo.CharacterSkin[0] = "KetherLibrarian";
                typeof(BookModel).GetField("_selectedOriginalSkin", AccessTools.all).SetValue(__instance.bookItem, __instance.bookItem.ClassInfo.CharacterSkin[0]);
                typeof(BookModel).GetField("_characterSkin", AccessTools.all).SetValue(__instance.bookItem, __instance.bookItem.ClassInfo.CharacterSkin[0]);
            }
            LogueBookModels.RemoveEquip(__instance);
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BattleEmotionInfo_CenterBtn), nameof(BattleEmotionInfo_CenterBtn.OnPointerUp))]
        public static bool BattleEmotionInfo_CenterBtn_OnPointerUp()
        {
            return !LogLikeMod.CheckStage() || Singleton<MysteryManager>.Instance.curMystery == null && (Singleton<MysteryManager>.Instance.interruptMysterys == null || Singleton<MysteryManager>.Instance.interruptMysterys.Count <= 0) && Singleton<ShopManager>.Instance.curshop == null && !LogLikeMod.PauseBool;
        }

        /*
        [HarmonyPostfix, HarmonyPatch(typeof(BattleDiceCardUI), nameof(BattleDiceCardUI.GetClickableState))]
        public static void BattleDiceCardUI_GetClickableState(
          BattleDiceCardUI __instance,
          ref BattleDiceCardUI.ClickableState __result)
        {
            BattleUnitModel owner = __instance.CardModel.owner;
            if (owner == null || PassiveAbility_ShopPassiveMook9.HasPassive(owner) == null || __result != BattleDiceCardUI.ClickableState.NotEnoughCost)
                return;
            __result = BattleDiceCardUI.ClickableState.CanClick;
        } 
        */

        /// <summary>
        /// Harmony patch to handle Mook9
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(SpecialCardListModel), nameof(SpecialCardListModel.ReturnCardToHand))]
        public static bool SpecialCardListModel_ReturnCardToHand(
          SpecialCardListModel __instance,
          BattleUnitModel unit,
          BattleDiceCardModel appliedCard)
        {
            List<BattleDiceCardModel> fieldValue1 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInUse", __instance);
            List<BattleDiceCardModel> fieldValue2 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInReserved", __instance);
            List<BattleDiceCardModel> fieldValue3 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInHand", __instance);
            PassiveAbility_ShopPassiveMook9 shopPassiveMook9 = PassiveAbility_ShopPassiveMook9.HasPassive(unit);
            if (shopPassiveMook9 == null || !shopPassiveMook9.cards.ContainsKey(appliedCard))
                return true;
            unit.cardSlotDetail.ReserveCost(-(appliedCard.GetCost() - shopPassiveMook9.cards[appliedCard]));
            fieldValue1.Remove(appliedCard);
            fieldValue2.Remove(appliedCard);
            fieldValue3.Add(appliedCard);
            shopPassiveMook9.cards.Remove(appliedCard);
            return false;
        }

        /// <summary>
        /// Harmony patch to handle Mook9
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(BattlePersonalEgoCardDetail), nameof(BattlePersonalEgoCardDetail.ReturnCardToHand))]
        public static bool BattlePersonalEgoCardDetail_ReturnCardToHand(
          BattlePersonalEgoCardDetail __instance,
          BattleDiceCardModel appliedCard)
        {
            BattleUnitModel fieldValue1 = ModdingUtils.GetFieldValue<BattleUnitModel>("_self", __instance);
            List<BattleDiceCardModel> fieldValue2 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInUse", __instance);
            List<BattleDiceCardModel> fieldValue3 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInReserved", __instance);
            List<BattleDiceCardModel> fieldValue4 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInHand", __instance);
            PassiveAbility_ShopPassiveMook9 shopPassiveMook9 = PassiveAbility_ShopPassiveMook9.HasPassive(fieldValue1);
            if (shopPassiveMook9 == null || !shopPassiveMook9.cards.ContainsKey(appliedCard))
                return true;
            fieldValue1.cardSlotDetail.ReserveCost(-(appliedCard.GetCost() - shopPassiveMook9.cards[appliedCard]));
            fieldValue2.Remove(appliedCard);
            fieldValue3.Remove(appliedCard);
            fieldValue4.Add(appliedCard);
            shopPassiveMook9.cards.Remove(appliedCard);
            return false;
        }

        /// <summary>
        /// Harmony patch to handle Mook9
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(BattleAllyCardDetail), nameof(BattleAllyCardDetail.ReturnCardToHand))]
        public static bool BattleAllyCardDetail_ReturnCardToHand(
          BattleAllyCardDetail __instance,
          BattleDiceCardModel appliedCard)
        {
            BattleUnitModel fieldValue1 = ModdingUtils.GetFieldValue<BattleUnitModel>("_self", __instance);
            List<BattleDiceCardModel> fieldValue2 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInUse", __instance);
            List<BattleDiceCardModel> fieldValue3 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInReserved", __instance);
            List<BattleDiceCardModel> fieldValue4 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInHand", __instance);
            PassiveAbility_ShopPassiveMook9 shopPassiveMook9 = PassiveAbility_ShopPassiveMook9.HasPassive(fieldValue1);
            if (shopPassiveMook9 == null || !shopPassiveMook9.cards.ContainsKey(appliedCard))
                return true;
            fieldValue1.cardSlotDetail.ReserveCost(-(appliedCard.GetCost() - shopPassiveMook9.cards[appliedCard]));
            fieldValue2.Remove(appliedCard);
            fieldValue3.Remove(appliedCard);
            fieldValue4.Add(appliedCard);
            shopPassiveMook9.cards.Remove(appliedCard);
            return false;
        }

        /// <summary>
        /// Patch for handling Mook9
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(BattlePlayingCardSlotDetail), nameof(BattlePlayingCardSlotDetail.OnApplyCard))]
        public static bool BattlePlayingCardSlotDetail_OnApplyCard(
          BattlePlayingCardSlotDetail __instance,
          BattleDiceCardModel card,
          ref bool __result)
        {
            PassiveAbility_ShopPassiveMook9 shopPassiveMook9 = PassiveAbility_ShopPassiveMook9.HasPassive(ModdingUtils.GetFieldValue<BattleUnitModel>("_self", __instance));
            if (shopPassiveMook9 == null)
                return true;
            int cost = card.GetCost();
            if (__instance.ReservedPlayPoint + cost > __instance.GetMaxPlayPoint())
            {
                int num1 = __instance.GetMaxPlayPoint() - __instance.ReservedPlayPoint;
                int num2 = cost - num1;
                shopPassiveMook9.cards[card] = num2;
                __result = __instance.ReserveCost(num1);
                return false;
            }
            card.costSpended = false;
            __result = __instance.ReserveCost(cost);
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BattleUnitBuf_burn), nameof(BattleUnitBuf_burn.OnRoundEnd))]
        public static bool BattleUnitBuf_burn_OnRoundEnd(BattleUnitBuf_burn __instance)
        {
            if (!LogLikeMod.CheckStage())
                return true;
            BattleUnitModel fieldValue = ModdingUtils.GetFieldValue<BattleUnitModel>("_owner", __instance);
            if (fieldValue.passiveDetail.PassiveList.Find(x => x is PassiveAbility_ShopPassiveStigma5) != null)
            {
                PassiveAbility_ShopPassiveStigma5 shopPassiveStigma5 = fieldValue.passiveDetail.PassiveList.Find(x => x is PassiveAbility_ShopPassiveStigma5) as PassiveAbility_ShopPassiveStigma5;
                if (shopPassiveStigma5.stack < __instance.stack)
                    shopPassiveStigma5.stack = __instance.stack;
            }
            if (Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find(x => x is PickUpModel_ShopGoodStigma1.Stigma1Effect) == null)
                return true;
            if (!fieldValue.IsImmune(__instance.bufType))
            {
                int v = __instance.stack - fieldValue.bufListDetail.GetActivatedBufList().FindAll((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_burnDown)).Count * 10;
                if (v < 0)
                    v = 0;
                fieldValue.TakeDamage(v, DamageType.Buf, keyword: __instance.bufType);
                __instance.GetType().GetMethod("PrintEffect", AccessTools.all).Invoke(__instance, (object[])null);
                if (fieldValue.bufListDetail.GetActivatedBuf(KeywordBuf.BurnBreak) != null)
                    fieldValue.TakeBreakDamage(v / 2, DamageType.Buf, keyword: __instance.bufType);
                if (fieldValue.faction == Faction.Enemy && fieldValue.IsDead())
                    Singleton<StageController>.Instance.GetStageModel().AddBurnKillCount();
            }
            __instance.stack = __instance.stack * 3 / 4;
            if (__instance.stack <= 0)
                __instance.Destroy();
            return false;
        }


        [HarmonyPrefix, HarmonyPatch(typeof(BattleUnitBufListDetail), nameof(BattleUnitBufListDetail.ChangeDiceResult))]
        public static void BattleUnitBufListDetail_ChangeDiceResult(
          BattleUnitBufListDetail __instance,
          BattleDiceBehavior behavior,
          ref int diceResult)
        {
            if (!LogLikeMod.CheckStage())
                return;
            Singleton<GlobalLogueEffectManager>.Instance.ChangeDiceResult(behavior, ref diceResult);
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.BeforeRollDice))]
        public static bool BattleUnitModel_BeforeRollDice(
          BattleUnitModel __instance,
          BattleDiceBehavior behavior)
        {
            Singleton<GlobalLogueEffectManager>.Instance.BeforeRollDice(behavior);
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BattlePlayingCardDataInUnitModel), nameof(BattlePlayingCardDataInUnitModel.OnUseCard))]
        public static bool BattlePlayingCardDataInUnitModel_OnUseCard(
          BattlePlayingCardDataInUnitModel __instance)
        {
            Singleton<GlobalLogueEffectManager>.Instance.OnUseCard(__instance);
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BookPassiveInfo), "name", MethodType.Getter)]
        public static bool BookPassiveInfo_get_name(BookPassiveInfo __instance, ref string __result)
        {
            string name = Singleton<PassiveDescXmlList>.Instance.GetName(__instance.passive.id);
            if (!(name != string.Empty))
                return true;
            __result = name;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BookPassiveInfo), "desc", MethodType.Getter)]
        public static bool BookPassiveInfo_get_desc(BookPassiveInfo __instance, ref string __result)
        {
            string desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(__instance.passive.id);
            if (desc == string.Empty)
                return true;
            __result = desc;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BattleEmotionRewardInfoUI), nameof(BattleEmotionRewardInfoUI.SetData))]
        public static bool BattleEmotionRewardInfoUI_SetData(BattleEmotionRewardInfoUI __instance)
        {
            List<BattleEmotionRewardSlotUI> fieldValue = LogLikeMod.GetFieldValue<List<BattleEmotionRewardSlotUI>>(__instance, "slots");
            if (fieldValue.Count < 10)
            {
                while (fieldValue.Count < 10)
                {
                    BattleEmotionRewardSlotUI emotionRewardSlotUi = LogLikeMod.LogBattleEmotionRewardSlotUI.BattleEmotionRewardSlotUI_Copying(fieldValue[0]);
                    fieldValue.Add(emotionRewardSlotUi);
                }
            }
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BattleEmotionCoinUI), nameof(BattleEmotionCoinUI.Init))]
        public static bool BattleEmotionCoinUI_Init(BattleEmotionCoinUI __instance)
        {
            if (__instance.enermy.Length < 10)
            {
                List<Vector2> vector2List = new List<Vector2>()
                  {
                    new Vector2(121f, 410f),
                    new Vector2(121f, 480f),
                    new Vector2(121f, 550f),
                    new Vector2(121f, 620f),
                    new Vector2(121f, 690f)
                  };
                int index = 0;
                while (__instance.enermy.Length < 10)
                {
                    BattleEmotionCoinUI.BattleEmotionCoinData battleEmotionCoinData = __instance.enermy[0];
                    battleEmotionCoinData.target = UnityEngine.Object.Instantiate<RectTransform>(battleEmotionCoinData.target, battleEmotionCoinData.target.parent);
                    battleEmotionCoinData.target.localPosition = (Vector3)vector2List[index];
                    __instance.enermy = __instance.enermy.AddToArray<BattleEmotionCoinUI.BattleEmotionCoinData>(battleEmotionCoinData);
                    ++index;
                }
            }
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(UIBattleSettingWaveList), nameof(UIBattleSettingWaveList.SetData))]
        public static bool UIBattleSettingWaveList_SetData(
          UIBattleSettingWaveList __instance,
          StageModel stage)
        {
            try
            {
                if (__instance.gameObject.GetComponent<ScrollRect>() == null)
                {
                    List<Transform> transformList = new List<Transform>();
                    Texture2D texture2D = new Texture2D(2, 2);
                    texture2D.LoadImage(File.ReadAllBytes(Application.dataPath + "/Managed/Image/Mask.png"));
                    Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                    Image image = __instance.gameObject.AddComponent<Image>();
                    image.sprite = sprite;
                    image.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                    (__instance.transform as RectTransform).sizeDelta = new Vector2(400f, (float)((double)stage.waveList.Count * 250.0 / 3.0));
                    ScrollRect scrollRect = __instance.gameObject.AddComponent<ScrollRect>();
                    scrollRect.scrollSensitivity = 15f;
                    scrollRect.content = __instance.transform as RectTransform;
                    scrollRect.horizontal = false;
                    scrollRect.movementType = ScrollRect.MovementType.Elastic;
                    scrollRect.elasticity = 0.1f;
                }
                if (stage.waveList.Count > __instance.waveSlots.Count)
                {
                    int num = stage.waveList.Count - __instance.waveSlots.Count;
                    for (int index = 0; index < num; ++index)
                    {
                        UIBattleSettingWaveSlot slot = UnityEngine.Object.Instantiate<UIBattleSettingWaveSlot>(__instance.waveSlots[0], __instance.waveSlots[0].transform.parent);
                        LogLikeRoutines.InitUIBattleSettingWaveSlot(slot, __instance);
                        List<UIBattleSettingWaveSlot> battleSettingWaveSlotList = new List<UIBattleSettingWaveSlot>();
                        battleSettingWaveSlotList.Add(slot);
                        battleSettingWaveSlotList.AddRange((IEnumerable<UIBattleSettingWaveSlot>)__instance.waveSlots);
                        __instance.waveSlots = battleSettingWaveSlotList;
                    }
                }
                if (stage.waveList.Count < __instance.waveSlots.Count)
                {
                    int num = __instance.waveSlots.Count - stage.waveList.Count;
                    for (int index = 0; index < num && __instance.waveSlots.Count != 5; ++index)
                    {
                        UIBattleSettingWaveSlot waveSlot = __instance.waveSlots[__instance.waveSlots.Count - 1];
                        __instance.waveSlots.Remove(waveSlot);
                        UnityEngine.Object.DestroyImmediate(waveSlot);
                    }
                }
                LogLikeRoutines.InitUIBattleSettingWaveSlots(__instance.waveSlots, __instance);
                foreach (Component waveSlot in __instance.waveSlots)
                    waveSlot.gameObject.SetActive(false);
                for (int index = 0; index < stage.waveList.Count; ++index)
                {
                    UIBattleSettingWaveSlot waveSlot = __instance.waveSlots[index];
                    waveSlot.SetData(stage.waveList[index]);
                    waveSlot.gameObject.SetActive(true);
                    if (stage.waveList[index].IsUnavailable())
                        waveSlot.SetDefeat();
                    if (index == stage.waveList.Count - 1)
                        waveSlot.ActivateArrow(false);
                }
                int index1 = Singleton<StageController>.Instance.CurrentWave - 1;
                if (index1 < 0 || __instance.waveSlots.Count <= index1)
                    Debug.LogError("Index Error");
                else
                    __instance.waveSlots[index1].SetHighlighted();
            }
            catch
            {
            }
          (__instance.transform as RectTransform).sizeDelta = new Vector2(400f, (float)((double)stage.waveList.Count * 250.0 / 3.0));
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(CharacterAppearance), nameof(CharacterAppearance.ChangeMotion))]
        public static bool CharacterAppearance_ChangeMotion_prefix(ActionDetail detail, ActionDetail __state)
        {
            __state = detail;
            return true;
        }

        // Patch to make upgraded combat pages consult their original PackageIds for combat page art
        [HarmonyPrefix, HarmonyPatch(typeof(CustomizingCardArtworkLoader), nameof(CustomizingCardArtworkLoader.GetSpecificArtworkSprite))]
        public static void CustomizingCardArtworkLoader_GetSpecificArtworkSprite(
          ref string id,
          string name)
        {
            if (!LogLikeMod.CheckStage())
                return;
            if (id.Contains(LogCardUpgradeManager.UpgradeKeyword))
                id = UpgradeMetadata.UnpackPidUnsafe(id).actualPid;
        }

        #endregion

        #region POSTFIXES 

        [HarmonyPostfix, HarmonyPatch(typeof(BattleUnitBuf), nameof(BattleUnitBuf.Destroy))]
        public static void BattleUnitBuf_Destroy(BattleUnitBuf __instance)
        {
            if (!LogLikeMod.CheckStage())
                return;
            BattleUnitModel fieldValue = ModdingUtils.GetFieldValue<BattleUnitModel>("_owner", __instance);
            if (!(__instance is BattleUnitBuf_burn) || fieldValue.passiveDetail.PassiveList.Find(x => x is PassiveAbility_ShopPassiveStigma5) == null)
                return;
            (fieldValue.passiveDetail.PassiveList.Find(x => x is PassiveAbility_ShopPassiveStigma5) as PassiveAbility_ShopPassiveStigma5).Recovering();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(BattleDiceCardUI), nameof(BattleDiceCardUI.GetClickableState))]
        public static void BattleDiceCardUI_GetClickableState(
          BattleDiceCardUI __instance,
          ref BattleDiceCardUI.ClickableState __result)
        {
            BattleUnitModel owner = __instance.CardModel.owner;
            if (owner == null || PassiveAbility_ShopPassiveMook9.HasPassive(owner) == null || __result != BattleDiceCardUI.ClickableState.NotEnoughCost)
                return;
            __result = BattleDiceCardUI.ClickableState.CanClick;
        }
        [HarmonyPostfix, HarmonyPatch(typeof(BattleUnitPassiveDetail), nameof(BattleUnitPassiveDetail.OnKill))]
        public static void BattleUnitPassiveDetail_OnKill(
          BattleUnitPassiveDetail __instance,
          BattleUnitModel target)
        {
            Singleton<GlobalLogueEffectManager>.Instance.OnKillUnit(ModdingUtils.GetFieldValue<BattleUnitModel>("_self", __instance), target);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(BattleUnitPassiveDetail), nameof(BattleUnitPassiveDetail.OnDie))]
        public static void BattleUnitPassiveDetail_OnDie(BattleUnitPassiveDetail __instance)
        {
            Singleton<GlobalLogueEffectManager>.Instance.OnDieUnit(ModdingUtils.GetFieldValue<BattleUnitModel>("_self", __instance));
        }

        /// <summary>
        /// Postfix patch for Union2's effect
        /// </summary>
        [HarmonyPostfix, HarmonyPatch(typeof(BattleUnitBufListDetail), nameof(BattleUnitBufListDetail.CheckGift))]
        public static void BattleUnitBufListDetail_CheckGift(
          BattleUnitBufListDetail __instance,
          KeywordBuf bufType,
          int stack,
          BattleUnitModel actor)
        {
            if (bufType != KeywordBuf.Bleeding || actor.passiveDetail.PassiveList.Find(x => x is PassiveAbility_ShopPassiveUnion2) == null)
                return;
            ModdingUtils.GetFieldValue<BattleUnitModel>("_self", __instance).TakeDamage(stack);
        }

        /// <summary>
        /// Postfix patch to hook GlobalLogueEffectBase.DmgFactor
        /// </summary>
        [HarmonyPostfix, HarmonyPatch(typeof(BattleUnitPassiveDetail), nameof(BattleUnitPassiveDetail.DmgFactor))]
        public static void BattleUnitPassiveDetail_DmgFactor(
          BattleUnitPassiveDetail __instance,
          ref float __result,
          int dmg,
          DamageType type = DamageType.ETC,
          KeywordBuf keyword = KeywordBuf.None)
        {
            __result = Singleton<GlobalLogueEffectManager>.Instance.DmgFactor(ModdingUtils.GetFieldValue<BattleUnitModel>("_self", __instance), dmg, type, keyword);
        }

        /// <summary>
        /// Postfix patch to set abnormality combat page arts
        /// </summary>
        [HarmonyPostfix, HarmonyPatch(typeof(EmotionPassiveCardUI), nameof(EmotionPassiveCardUI.SetSprites))]
        public static void EmotionPassiveCardUI_SetSprites(EmotionPassiveCardUI __instance)
        {
            if (!LogLikeMod.CheckStage())
                return;
            Image image = (Image)typeof(EmotionPassiveCardUI).GetField("_artwork", AccessTools.all).GetValue(__instance);
            EmotionCardXmlInfo emotionCardXmlInfo = (EmotionCardXmlInfo)typeof(EmotionPassiveCardUI).GetField("_card", AccessTools.all).GetValue(__instance);
            if (LogLikeMod.ArtWorks.ContainsKey(emotionCardXmlInfo.Artwork))
                image.sprite = LogLikeMod.ArtWorks[emotionCardXmlInfo.Artwork];
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIOptionWindow), nameof(UIOptionWindow.Open))]
        public static void UIOptionWindow_Open(UIOptionWindow __instance)
        {
            if (!(LogLikeMod.DefFont == null))
                return;
            LogLikeMod.DefFont = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
            LogLikeMod.DefFontColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
            LogLikeMod.DefFont_TMP = __instance.displayDropdown.itemText.font;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIBattleSettingEditPanel), nameof(UIBattleSettingEditPanel.Close))]
        public static void UIBattleSettingEditPanel_Close()
        {
            if (!LogLikeMod.CheckStage())
                return;
            LoguePlayDataSaver.SavePlayData_Menu();
        }

        /*
        [HarmonyPostfix, HarmonyPatch(typeof(BattleDiceCardUI), nameof(BattleDiceCardUI.SetCard))]
        public static void BattleDiceCardUI_SetCard(
          BattleDiceCardUI __instance,
          BattleDiceCardModel cardModel)
        {
            if (!LogLikeMod.CheckStage() || !LogLikeMod.ArtWorks.ContainsKey(cardModel.GetArtworkSrc()))
                return;
            __instance.img_artwork.sprite = LogLikeMod.ArtWorks[cardModel.GetArtworkSrc()];
        }
        */

        /*
        [HarmonyPostfix, HarmonyPatch(typeof(UIOriginCardSlot), nameof(UIOriginCardSlot.SetData))]
        public static void UIOriginCardSlot_SetData(
          UIOriginCardSlot __instance,
          DiceCardItemModel cardmodel)
        {
            if (!LogLikeMod.CheckStage())
                return;
            Image image = (Image)typeof(UIOriginCardSlot).GetField("img_Artwork", AccessTools.all).GetValue(__instance);
            bool flag = cardmodel.GetID().packageId == LogLikeMod.ModId;
            if (LogLikeMod.ArtWorks.ContainsKey(cardmodel.GetArtworkSrc()) & flag)
            {
                image.sprite = LogLikeMod.ArtWorks[cardmodel.GetArtworkSrc()];
            }
        }
        */

        [HarmonyPostfix, HarmonyPatch(typeof(EmotionPassiveCardUI), nameof(EmotionPassiveCardUI.Init))]
        public static void EmotionPassiveCardUI_Init(EmotionPassiveCardUI __instance)
        {
            if (!LogLikeMod.CheckStage())
                return;
            bool fieldValue = LogLikeMod.GetFieldValue<bool>(__instance, "_isForceOpen");
            if (LogLikeMod.ChangeEmotinCardBtn == null)
            {
                LogLikeMod.ChangeEmotinCardBtn = ModdingUtils.CreateButton(SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.selectedEmotionCardBg.transform.parent.parent, "AbCardSelection_Skip", new Vector2(1f, 1f), new Vector2(0.0f, -480f));
                LogLikeMod.ChangeEmotinCardBtn.onClick.AddListener(() => LogLikeRoutines.ChangeEPCUTransform(__instance));
                ModdingUtils.CreateText_TMP(LogLikeMod.ChangeEmotinCardBtn.transform, new Vector2(-30f, 0.0f), 40, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP).text = TextDataModel.GetText("ui_EmotionPositionChange");
            }
            LogLikeMod.ChangeEmotinCardBtn.gameObject.SetActive(fieldValue);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(BookInventoryModel), nameof(BookInventoryModel.GetAllBookByInstanceId))]
        public static void BookInventoryModel_GetAllBookByInstanceId(
          ref BookModel __result,
          int bookInstanceId)
        {
            if (!LogLikeMod.CheckStage())
                return;
            BookModel bookModel = LogueBookModels.booklist.Find(x => x.instanceId == bookInstanceId);
            if (bookModel != null)
            {
                __result = bookModel;
            }
            else
            {
                BookModel bookItem = LogueBookModels.playerModel.Find(x => x.bookItem.instanceId == bookInstanceId).bookItem;
                if (bookItem == null)
                    return;
                __result = bookItem;
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(BookInventoryModel), nameof(BookInventoryModel.GetBookByInstanceId))]
        public static void BookInventoryModel_GetBookByInstanceId(
          ref BookModel __result,
          int bookInstanceId)
        {
            if (!LogLikeMod.CheckStage())
                return;
            BookModel bookModel = LogueBookModels.booklist.Find(x => x.instanceId == bookInstanceId);
            if (bookModel != null)
            {
                __result = bookModel;
            }
            else
            {
                BookModel bookItem = LogueBookModels.playerModel.Find(x => x.bookItem.instanceId == bookInstanceId).bookItem;
                if (bookItem == null)
                    return;
                __result = bookItem;
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(BookInventoryModel), nameof(BookInventoryModel.GetBookListAll))]
        public static void BookInventoryModel_GetBookListAll(ref List<BookModel> __result)
        {
            if (!LogLikeMod.CheckStage())
                return;
            List<BookModel> bookModelList = new List<BookModel>();
            foreach (UnitDataModel unitDataModel in LogueBookModels.playerModel)
                bookModelList.Add(unitDataModel.defaultBook);
            bookModelList.AddRange(LogueBookModels.booklist);
            __result = bookModelList;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(BookInventoryModel), nameof(BookInventoryModel.GetBookList_PassiveEquip))]
        public static void BookInventoryModel_GetBookList_PassiveEquip(
          ref List<BookModel> __result,
          BookModel booktobeEquiped)
        {
            if (!LogLikeMod.CheckStage())
                return;
            List<BookModel> bookModelList1 = new List<BookModel>();
            List<BookModel> bookModelList2 = new List<BookModel>();
            bookModelList2.AddRange((IEnumerable<BookModel>)LogueBookModels.booklist);
            foreach (BookModel bookModel in bookModelList2)
            {
                if (bookModel.owner == null && bookModel.GetPassiveInfoList().Count != 0)
                    bookModelList1.Add(bookModel);
            }
            __result = bookModelList1;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CharacterAppearance), nameof(CharacterAppearance.ChangeMotion))]
        public static void CharacterAppearance_ChangeMotion(
          CharacterAppearance __instance,
          ActionDetail detail,
          ActionDetail __state)
        {
            if (LogLikeMod.Temp)
            {
                for (int index = 0; index < SingletonBehavior<UICharacterRenderer>.Instance.cameraList.Count; ++index)
                {
                    Camera camera = SingletonBehavior<UICharacterRenderer>.Instance.cameraList[index];
                    camera.cullingMask = -1;
                    if (camera.gameObject.GetComponent("CameraRender") == null)
                        camera.gameObject.AddComponent<CameraRender>().index = index;
                }
                LogLikeMod.Temp = false;
            }
            WorkshopSkinDataSetter setter = __instance.GetComponent<WorkshopSkinDataSetter>();
            if (setter == null)
                return;
            WorkshopSkinData workshopSkinData = Singleton<CustomizingBookSkinLoader>.Instance.GetWorkshopBookSkinData(LogLikeMod.ModId).Find((Predicate<WorkshopSkinData>)(x => x.dic == setter.dic));
            if (workshopSkinData == null)
                return;
            string dataName = workshopSkinData.dataName;
            if (dataName == string.Empty)
                return;
            SpineStandingData spineStandingData = (SpineStandingData)null;
            if (LogLikeMod.spinedatas.TryGetValue(dataName, out spineStandingData))
            {
                if (spineStandingData.AnimDic.ContainsKey(__state))
                    detail = __state;
                if (spineStandingData.AnimDic.ContainsKey(detail))
                {
                    if (!LogLikeMod.spinemotions.ContainsKey(dataName))
                        LogLikeMod.spinemotions.Add(dataName, new Dictionary<ActionDetail, Dictionary<GameObject, SkeletonAnimation>>());
                    if (!LogLikeMod.spinemotions[dataName].ContainsKey(detail))
                        LogLikeMod.spinemotions[dataName].Add(detail, new Dictionary<GameObject, SkeletonAnimation>());
                    if (!LogLikeMod.spinemotions[dataName][detail].ContainsKey(__instance.gameObject))
                    {
                        SkeletonAnimation skeletonAnimation = SkeletonRenderer.NewSpineGameObject<SkeletonAnimation>(spineStandingData.asset);
                        __instance.AddChild(skeletonAnimation.gameObject);
                        skeletonAnimation.gameObject.transform.localScale = new Vector3(1f, 1f);
                        LogLikeMod.spinemotions[dataName][detail].Add(__instance.gameObject, skeletonAnimation);
                    }
                    LogLikeMod.spinemotions[dataName][detail][__instance.gameObject].gameObject.SetActive(true);
                    LogLikeMod.spinemotions[dataName][detail][__instance.gameObject].gameObject.transform.localPosition = new Vector3(0.0f, 0.0f);
                    LogLikeMod.spinemotions[dataName][detail][__instance.gameObject].gameObject.transform.localScale = spineStandingData.AnimScale[detail];
                    LogLikeMod.spinemotions[dataName][detail][__instance.gameObject].state.SetAnimation(0, spineStandingData.AnimDic[detail], spineStandingData.AnimLoop[detail]);
                    LogLikeMod.spinemotions[dataName][detail][__instance.gameObject].state.TimeScale = spineStandingData.AnimSpeed[detail];
                    if (UIPanel.Controller.GetUIPanel(UIPanelType.CharacterList).IsActivated || UIPanel.Controller.GetUIPanel(UIPanelType.CharacterList_Right).IsActivated)
                        LogLikeMod.spinemotions[dataName][detail][__instance.gameObject].gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 10f);
                    //else if (detail == ActionDetail.Standing)
                    //  ;
                }
            }
            if (!LogLikeMod.spinemotions.ContainsKey(dataName))
                return;
            foreach (KeyValuePair<ActionDetail, Dictionary<GameObject, SkeletonAnimation>> keyValuePair in LogLikeMod.spinemotions[dataName])
            {
                if (keyValuePair.Key != detail && keyValuePair.Value.ContainsKey(__instance.gameObject))
                    keyValuePair.Value[__instance.gameObject].gameObject.SetActive(false);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UILibrarianEquipInfoSlot), nameof(UILibrarianEquipInfoSlot.SetData))]
        public static void UILibrarianEquipInfoSlot_SetData(
          UILibrarianEquipInfoSlot __instance,
          BookPassiveInfo passive)
        {
            if (!(passive.passive.id == new LorId(LogLikeMod.ModId, 1)))
                return;
            __instance.txt_cost.text = "";
        }

        [HarmonyPostfix, HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnDie))]
        public static void BattleUnitModel_OnDie(BattleUnitModel __instance)
        {
            if (!LogLikeMod.CheckStage(true))
                return;
            if (__instance.faction == Faction.Enemy && __instance.UnitData.unitData.ExpDrop > 0)
            {
                int moneyDrop = __instance.UnitData.unitData.ExpDrop;
                if (LogLikeMod.curstagetype == StageType.Normal)
                {
                    float moneyScale = 1f;
                    switch (LogLikeMod.curchaptergrade)
                    {
                        case ChapterGrade.Grade1:
                        case ChapterGrade.Grade2:
                            moneyScale = 1.5f;
                            break;
                        case ChapterGrade.Grade3:
                            moneyScale = 0.9f;
                            break;
                        case ChapterGrade.Grade4:
                            moneyScale = 0.85f;
                            break;
                        case ChapterGrade.Grade5:
                            moneyScale = 0.8f;
                            break;
                        case ChapterGrade.Grade6:
                            moneyScale = 0.75f;
                            break;
                        case ChapterGrade.Grade7:
                            moneyScale = 0.7f;
                            break;
                    }
                    moneyDrop = Mathf.Max(1, Mathf.RoundToInt(moneyDrop * moneyScale));
                }
                PassiveAbility_MoneyCheck.AddMoney(moneyDrop);
            }
            else if (__instance.faction == Faction.Player && LogueBookModels.playerBattleModel.Contains(__instance.UnitData))
                LogueBookModels.playerBattleModel.Find(x => x == __instance.UnitData).isDead = true;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(BookPassiveInfo), "desc", MethodType.Getter)]
        public static void BookPassiveInfo_get_desc_postfix(
          BookPassiveInfo __instance,
          ref string __result)
        {
            if (__instance.passive == null || !(__instance.passive.id == new LorId(LogLikeMod.ModId, 1)))
                return;
            __result = PassiveAbility_MoneyCheck.GetMoney().ToString();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(LocalizedTextLoader), nameof(LocalizedTextLoader.LoadOthers))]
        public static void LocalizedTextLoader_LoadOthers(string language)
        {
            LogLikeMod.LoadTextData(language);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIInvitationRightMainPanel), nameof(UIInvitationRightMainPanel.OpenInit))]
        public static void UIInvitationRightMainPanel_OpenInit(UIInvitationRightMainPanel __instance)
        {
            if (LogLikeMod.LogOpenButton == null)
            {
                LogLikeMod.LogOpenButton = ModdingUtils.CreateLogSelectable(__instance.transform, "LogLikeModIcon", new Vector2(1f, 1f), new Vector2(-70f, 350f), new Vector2(100f, 100f));
                LogLikeMod.LogOpenButton.gameObject.AddComponent<FrameDummy>();
                Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
                buttonClickedEvent.AddListener(() =>
                {
                    SingletonBehavior<UIMainOverlayManager>.Instance.Close();
                    List<string> ExceptModNames;
                    if (LoguePlayDataSaver.CheckPlayerData()) { 
                        UIAlarmPopup.instance.SetChoiceAlarmText(
                        TextDataModel.GetText("ui_RMR_ConfirmStartNewRun"),
                        (bool yes) => {
                            if (yes)
                            {
                                if (LogLikeMod.CheckExceptionModExist(out ExceptModNames))
                                {
                                    string text = TextDataModel.GetText("ui_ExceptionWithLog") + Environment.NewLine;
                                    foreach (string str in ExceptModNames)
                                        text = $"{text}-{str}{Environment.NewLine}";
                                    UIAlarmPopup.instance.SetAlarmText(text);
                                }
                                else
                                {
                                    bool flag = true;
                                    __instance.SetCustomInvToggle(true);
                                    foreach (UIInvitationBookSlot invitationbookSlot in __instance.invitationbookSlots)
                                    {
                                        if (flag)
                                            invitationbookSlot.ApplySlotid(new LorId(LogLikeMod.ModId, -853), true);
                                        else
                                            invitationbookSlot.SetEmptySlot();
                                        flag = false;
                                    }
                                    __instance.ConfirmSendInvitation();
                                }
                            } 
                        });
                    } else
                    {
                        if (LogLikeMod.CheckExceptionModExist(out ExceptModNames))
                        {
                            string text = TextDataModel.GetText("ui_ExceptionWithLog") + Environment.NewLine;
                            foreach (string str in ExceptModNames)
                                text = $"{text}-{str}{Environment.NewLine}";
                            UIAlarmPopup.instance.SetAlarmText(text);
                        }
                        else
                        {
                            bool flag = true;
                            __instance.SetCustomInvToggle(true);
                            foreach (UIInvitationBookSlot invitationbookSlot in __instance.invitationbookSlots)
                            {
                                if (flag)
                                    invitationbookSlot.ApplySlotid(new LorId(LogLikeMod.ModId, -853), true);
                                else
                                    invitationbookSlot.SetEmptySlot();
                                flag = false;
                            }
                            __instance.ConfirmSendInvitation();
                        }
                    }
                });
                LogLikeMod.LogOpenButton.onClick = buttonClickedEvent;
                LogLikeMod.LogOpenButton.SelectEvent = new UnityEventBasedata();
                LogLikeMod.LogOpenButton.SelectEvent.AddListener((BaseEventData e) =>
                {
                    SingletonBehavior<UIMainOverlayManager>.Instance.SetTooltip(
                        TextDataModel.GetText("ui_RMR_StartNewRun"),
                        TextDataModel.GetText("ui_RMR_StartNewRunDesc"), 
                        LogLikeMod.LogOpenButton.transform as RectTransform);
                });
                LogLikeMod.LogOpenButton.DeselectEvent = new UnityEventBasedata();
                LogLikeMod.LogOpenButton.DeselectEvent.AddListener((BaseEventData e) =>
                {
                    SingletonBehavior<UIMainOverlayManager>.Instance.Close();
                });
            }
            if (LogLikeMod.LogContinueButton == null)
            {
                LogLikeMod.LogContinueButton = ModdingUtils.CreateLogSelectable(__instance.transform, "LogLikeModIcon_Continue", new Vector2(1f, 1f), new Vector2(-70f, 250f), new Vector2(100f, 100f));
                LogLikeMod.LogContinueButton.gameObject.AddComponent<FrameDummy>();
                Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
                buttonClickedEvent.AddListener(() =>
                {
                    List<string> ExceptModNames;
                    SingletonBehavior<UIMainOverlayManager>.Instance.Close();
                    if (LogLikeMod.CheckExceptionModExist(out ExceptModNames))
                    {
                        string text = TextDataModel.GetText("ui_ExceptionWithLog") + Environment.NewLine;
                        foreach (string str in ExceptModNames)
                            text = $"{text}-{str}{Environment.NewLine}";
                        UIAlarmPopup.instance.SetAlarmText(text);
                    }
                    else
                    {
                        bool flag = true;
                        __instance.SetCustomInvToggle(true);
                        foreach (UIInvitationBookSlot invitationbookSlot in __instance.invitationbookSlots)
                        {
                            if (flag)
                                invitationbookSlot.ApplySlotid(new LorId(LogLikeMod.ModId, -855), true);
                            else
                                invitationbookSlot.SetEmptySlot();
                            flag = false;
                        }
                        __instance.ConfirmSendInvitation();
                    }
                });
                LogLikeMod.LogContinueButton.onClick = buttonClickedEvent;
                LogLikeMod.LogContinueButton.SelectEvent = new UnityEventBasedata();
                LogLikeMod.LogContinueButton.SelectEvent.AddListener((BaseEventData e) =>
                {
                    SingletonBehavior<UIMainOverlayManager>.Instance.SetTooltip(
                        TextDataModel.GetText("ui_RMR_ContinueRun"),
                        TextDataModel.GetText("ui_RMR_ContinueRunDesc"),
                        LogLikeMod.LogContinueButton.transform as RectTransform);
                });
                LogLikeMod.LogContinueButton.DeselectEvent = new UnityEventBasedata();
                LogLikeMod.LogContinueButton.DeselectEvent.AddListener((BaseEventData e) =>
                {
                    SingletonBehavior<UIMainOverlayManager>.Instance.Close();
                });
            }
            LogLikeMod.LogContinueButton.gameObject.SetActive(LoguePlayDataSaver.CheckPlayerData());
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIEmotionPassiveCardInven), nameof(UIEmotionPassiveCardInven.SetSprites))]
        public static void UIEmotionPassiveCardInven_SetSprites(
          UIEmotionPassiveCardInven __instance,
          MentalState state)
        {
            if (!LogLikeMod.CheckStage())
                return;
            LogLikeMod.GetFieldValue<Image>(__instance, "_artwork").sprite = LogLikeMod.ArtWorks[__instance.Card.Artwork];
        }


        /// <summary>
        /// Patch responsible for equipping Key Pages(??)
        /// </summary>
        [HarmonyPostfix, HarmonyPatch(typeof(UIBattleSettingLibrarianInfoPanel), nameof(UIBattleSettingLibrarianInfoPanel.SetData))]
        public static void UIBattleSettingLibrarianInfoPanel_SetData(
          UIBattleSettingLibrarianInfoPanel __instance,
          UnitDataModel data)
        {
            __instance.PassiveListSelectable.SubmitEvent.RemoveAllListeners();
            if (!LogLikeMod.CheckStage() || !LogueBookModels.playerModel.Contains(data))
                return;
            __instance.PassiveListSelectable.SubmitEvent.AddListener((UnityAction<BaseEventData>)(e => UIPassiveSuccessionPopup.Instance.SetData(data, (UIPassiveSuccessionPopup.ApplyEvent)(() =>
            {
                __instance.passiveSlotsPanel.SetStatsDataInEquipBook(data.bookItem);
                (UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel).EditPanel.EquipPagePanel.ChangeEquipBook(null);
                UIControlManager.Instance.SelectSelectableForcely(__instance.PassiveListSelectable);
                LoguePlayDataSaver.SavePlayData_Menu();
            }))));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIPassiveSuccessionPopup), nameof(UIPassiveSuccessionPopup.InitReservedData))]
        public static void UIPassiveSuccessionPopup_InitReservedData()
        {
            if (!LogLikeMod.CheckStage())
                return;
            foreach (BookModel bookModel in LogueBookModels.booklist)
            {
                bookModel.InitReservedDataForPassiveSuccession();
                foreach (PassiveModel passiveModel in bookModel.GetPassiveModelList())
                    passiveModel.InitReservedData();
            }
            foreach (UnitDataModel unitDataModel in LogueBookModels.playerModel)
            {
                BookModel bookItem = unitDataModel.bookItem;
                bookItem.InitReservedDataForPassiveSuccession();
                foreach (PassiveModel passiveModel in bookItem.GetPassiveModelList())
                    passiveModel.InitReservedData();
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(StageWaveModel), nameof(StageWaveModel.GetUnitBattleDataListByFormation))]
        public static void StageWaveModel_GetUnitBattleDataListByFormation(
          StageWaveModel __instance,
          ref List<UnitBattleDataModel> __result)
        {
            List<UnitBattleDataModel> unitBattleDataModelList = new List<UnitBattleDataModel>();
            for (int i = 0; i < 10; ++i)
            {
                int formationIndex = __instance.GetFormationIndex(i);
                if (formationIndex < __instance.UnitList.Count)
                    unitBattleDataModelList.Add(__instance.UnitList[formationIndex]);
            }
            __result = unitBattleDataModelList;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(StageWaveModel), nameof(StageWaveModel.Init))]
        public static void StageWaveModel_Init(StageWaveModel __instance)
        {
            List<int> fieldValue = ModdingUtils.GetFieldValue<List<int>>("_formationIndex", __instance);
            fieldValue.Add(5);
            fieldValue.Add(6);
            fieldValue.Add(7);
            fieldValue.Add(8);
            fieldValue.Add(9);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIManualScreenPage), nameof(UIManualScreenPage.LoadContent))]
        public static void UIManualScreenPage_LoadContent(UIManualScreenPage __instance)
        {
            TutorialManager.TutoInfo logTuto = Singleton<TutorialManager>.Instance.FindLogTuto(__instance);
            if (logTuto == null)
                return;
            __instance.img_screenShot.sprite = LogLikeMod.ArtWorks[logTuto.ArtWork];
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIManualContentPanel), nameof(UIManualContentPanel.SetData))]
        public static void UIManualContentPanel_SetData(UIManualContentPanel __instance)
        {
            if (Singleton<TutorialManager>.Instance.Inited)
                return;
            Singleton<TutorialManager>.Instance.Init(__instance);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UI.UIController), nameof(UI.UIController.CallUIPhase), new Type[1] { typeof(UIPhase) })]
        public static void UIController_CallUIPhase(UIController __instance, UIPhase phase)
        {
            if (phase != UIPhase.BattleSetting || MysteryBase.curinfo == null)
                return;
            MysteryBase.LoadGetAbnomalityPanel(MysteryBase.curinfo.abnormal, MysteryBase.curinfo.level);
            MysteryBase.curinfo = (MysteryBase.MysteryAbnormalInfo)null;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UI.UIController), nameof(UI.UIController.Awake))]
        public static void UIController_Awake()
        {
            if (!(LogLikeMod.UILogCardSlot.Original == null))
                return;
            LogLikeMod.UILogCardSlot.Original = LogLikeMod.UILogCardSlot.SlotCopying();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(LevelUpUI), nameof(LevelUpUI.OnClickTargetUnit))]
        public static void LevelUpUI_OnClickTargetUnit()
        {
            if (!LogLikeMod.CheckStage())
                return;
            foreach (BattleUnitModel battleUnitModel in (IEnumerable<BattleUnitModel>)BattleObjectManager.instance.GetList())
            {
                if (battleUnitModel.view.abCardSelector.isInitialized)
                    battleUnitModel.view.abCardSelector.TurnOffUI();
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(StageModel), nameof(StageModel.GetFrontAvailableFloor))]
        public static void StageModel_GetFrontAvailableFloor(
          StageModel __instance,
          ref StageLibraryFloorModel __result)
        {
            if (!LogLikeMod.CheckStage() || __instance.floorList.Find((Predicate<StageLibraryFloorModel>)(x => x.IsUnavailable())) == null)
                return;
            __result = (StageLibraryFloorModel)null;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIColorManager), nameof(UIColorManager.GetSephirahColor))]
        public static void UIColorManager_GetSephirahColor(SephirahType sephirah, ref Color __result)
        {
            //if (!LogLikeMod.CheckStage() || sephirah != SephirahType.None)
            // ;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIColorManager), nameof(UIColorManager.GetSephirahGlowColor))]
        public static void UIColorManager_GetSephirahGlowColor(SephirahType sephirah, ref Color __result)
        {
            //if (!LogLikeMod.CheckStage() || sephirah != SephirahType.None)
            //  ;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UICharacterStatInfoPanel), nameof(UICharacterStatInfoPanel.SetData), new Type[1] { typeof(UnitDataModel) })]
        public static void UICharacterStatInfoPanel_SetData(
          UICharacterStatInfoPanel __instance,
          UnitDataModel data)
        {
            if (LogueBookModels.playerModel == null || !LogueBookModels.playerModel.Contains(data) || LogueBookModels.playersstatadders == null || !LogueBookModels.playersstatadders.TryGetValue(data, out var statAdders) || statAdders.Count <= 0)
                return;
            BookXmlInfo bookXmlInfo = LogueBookModels.CurPlayerEquipInfo(data);
            int hp = bookXmlInfo.EquipEffect.Hp;
            hp.Log("chp : " + hp.ToString());
            hp.Log("mhp : " + data.MaxHp.ToString());
            int num1;
            if (data.MaxHp > hp)
            {
                TextMeshProUGUI hpText = __instance.hpText;
                string str1 = hp.ToString();
                num1 = data.MaxHp - hp;
                string str2 = num1.ToString();
                string str3 = $"{str1} <color=#22FFE4>+ {str2}</color>";
                hpText.text = str3;
            }
            else if (data.MaxHp < hp)
            {
                TextMeshProUGUI hpText = __instance.hpText;
                string str4 = hp.ToString();
                num1 = hp - data.MaxHp;
                string str5 = num1.ToString();
                string str6 = $"{str4} <color=red>- {str5}</color>";
                hpText.text = str6;
            }
            int num2 = bookXmlInfo.EquipEffect.Break;
            if (data.Break > num2)
            {
                TextMeshProUGUI breakText = __instance.breakText;
                string str7 = num2.ToString();
                num1 = data.Break - num2;
                string str8 = num1.ToString();
                string str9 = $"{str7} <color=#22FFE4>+ {str8}</color>";
                breakText.text = str9;
            }
            else if (data.Break < num2)
            {
                TextMeshProUGUI breakText = __instance.breakText;
                string str10 = num2.ToString();
                num1 = num2 - data.Break;
                string str11 = num1.ToString();
                string str12 = $"{str10} <color=red>- {str11}</color>";
                breakText.text = str12;
            }
            AtkResist sresist1 = data.bookItem.equipeffect.SResist;
            AtkResist sresist2 = bookXmlInfo.EquipEffect.SResist;
            if (sresist1 > sresist2)
                __instance.resistSlash.text = $"<color=#22FFE4>{data.bookItem.GetResistHP_Text(BehaviourDetail.Slash)}</color>";
            else if (sresist1 < sresist2)
                __instance.resistSlash.text = $"<color=red>{data.bookItem.GetResistHP_Text(BehaviourDetail.Slash)}</color>";
            AtkResist presist1 = data.bookItem.equipeffect.PResist;
            AtkResist presist2 = bookXmlInfo.EquipEffect.PResist;
            if (presist1 > presist2)
                __instance.resistSlash.text = $"<color=#22FFE4>{data.bookItem.GetResistHP_Text(BehaviourDetail.Penetrate)}</color>";
            else if (presist1 < presist2)
                __instance.resistSlash.text = $"<color=red>{data.bookItem.GetResistHP_Text(BehaviourDetail.Penetrate)}</color>";
            AtkResist hresist1 = data.bookItem.equipeffect.HResist;
            AtkResist hresist2 = bookXmlInfo.EquipEffect.HResist;
            if (hresist1 > hresist2)
                __instance.resistSlash.text = $"<color=#22FFE4>{data.bookItem.GetResistHP_Text(BehaviourDetail.Hit)}</color>";
            else if (hresist1 < hresist2)
                __instance.resistSlash.text = $"<color=red>{data.bookItem.GetResistHP_Text(BehaviourDetail.Hit)}</color>";
            AtkResist sbResist1 = data.bookItem.equipeffect.SBResist;
            AtkResist sbResist2 = bookXmlInfo.EquipEffect.SBResist;
            if (sbResist1 > sbResist2)
                __instance.resistSlash.text = $"<color=#22FFE4>{data.bookItem.GetResistBP_Text(BehaviourDetail.Slash)}</color>";
            else if (sbResist1 < sbResist2)
                __instance.resistSlash.text = $"<color=red>{data.bookItem.GetResistBP_Text(BehaviourDetail.Slash)}</color>";
            AtkResist pbResist1 = data.bookItem.equipeffect.PBResist;
            AtkResist pbResist2 = bookXmlInfo.EquipEffect.PBResist;
            if (pbResist1 > pbResist2)
                __instance.resistSlash.text = $"<color=#22FFE4>{data.bookItem.GetResistBP_Text(BehaviourDetail.Penetrate)}</color>";
            else if (pbResist1 < pbResist2)
                __instance.resistSlash.text = $"<color=red>{data.bookItem.GetResistBP_Text(BehaviourDetail.Penetrate)}</color>";
            AtkResist hbResist1 = data.bookItem.equipeffect.HBResist;
            AtkResist hbResist2 = bookXmlInfo.EquipEffect.HBResist;
            if (hbResist1 > hbResist2)
                __instance.resistSlash.text = $"<color=#22FFE4>{data.bookItem.GetResistBP_Text(BehaviourDetail.Hit)}</color>";
            else if (hbResist1 < hbResist2)
                __instance.resistSlash.text = $"<color=red>{data.bookItem.GetResistBP_Text(BehaviourDetail.Hit)}</color>";
        }

        [HarmonyPostfix, HarmonyPatch(typeof(BookModel), nameof(BookModel.GetMaxPassiveCost))]
        public static void BookModel_GetMaxPassiveCost(ref int __result)
        {
            if (!LogLikeMod.CheckStage())
                return;
            int num = 6 + Singleton<GlobalLogueEffectManager>.Instance.ChangeSuccCostValue();
            if (num < 0)
                num = 0;
            __result = num;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(BookModel), nameof(BookModel.GetThumbSprite))]
        public static void BookModel_GetThumbSprite(BookModel __instance, ref Sprite __result)
        {
            if (!LogLikeMod.CheckStage() || __instance.ClassInfo.CharacterSkin == null)
                return;
            if (!__instance.ClassInfo.CharacterSkin.Any<string>())
                return;
            try
            {
                if (__instance.ClassInfo.skinType == "Lor")
                {
                    BookXmlInfo bookXmlInfo = Singleton<BookXmlList>.Instance.GetList().Find((Predicate<BookXmlInfo>)(x => x.CharacterSkin[0] == __instance.ClassInfo.CharacterSkin[0] && !x.id.IsWorkshop()));
                    __result = UnityEngine.Resources.Load<Sprite>("Sprites/Books/Thumb/" + bookXmlInfo.id.id.ToString());
                }
                else
                {
                    if (!(__instance.ClassInfo.skinType == "CUSTOM") || !__instance.ClassInfo.CharacterSkin[0].Contains("<LogLike>"))
                        return;
                    string key = __instance.ClassInfo.CharacterSkin[0].Remove(0, 9);
                    if (!LogLikeMod.ArtWorks.ContainsKey(key))
                        return;
                    __result = LogLikeMod.ArtWorks[key];
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Failed to load thumbnail: " + ex);
                __result = UnityEngine.Resources.Load<Sprite>("Sprites/Books/Thumb/1");
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(StageController), nameof(StageController.GameOver))]
        public static void StageController_GameOver(ref bool iswin, ref bool isbackbutton)
        {
            if (!RMRRealizationManager.ForceReturnAsDefeatPending)
                return;
            iswin = false;
            isbackbutton = true;
            RMRRealizationManager.ConsumeForceReturnAsDefeat();
            Debug.Log("[RMRRealizationManager] Routed realization completion through the vanilla defeat return flow.");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(StageController), nameof(StageController.ActivateStartBattleEffectPhase))]
        public static void StageController_ActivateStartBattleEffectPhase(StageController __instance)
        {
            var list = ModdingUtils.GetFieldValue<List<BattlePlayingCardDataInUnitModel>>("_allCardList", __instance);
            var list2 = new List<BattlePlayingCardDataInUnitModel>();
            list2.AddRange(list); // prevent collection modified exception
            foreach (BattlePlayingCardDataInUnitModel card in list2)
                Singleton<GlobalLogueEffectManager>.Instance.OnStartBattle_AfterCardSet(card);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(StageController), nameof(StageController.OnFixedUpdateLate))]
        public static void StageController_OnFixedUpdateLate()
        {
            if (LogLikeMod.CheckStage())
                LogLikeMod.PauseBool = RewardingModel.RewardInStage();
            else
                LogLikeMod.PauseBool = false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(StageController), nameof(StageController.ClearBattle))]
        public static void StageController_ClearBattle(StageController __instance)
        {
            // Skip Roguelike clear-battle processing during realization battles
            if (RMRRealizationManager.InRealizationBattle)
                return;

            if (!LogLikeMod.CheckStage(true) || Environment.StackTrace.Contains("UIBgScreenChangeAnim"))
                return;
            if (!LogLikeMod.purpleexcept)
            {
                if (LogLikeMod.AddPlayer)
                    LogueBookModels.AddSubPlayer();
                if (LogLikeMod.RecoverPlayers)
                {
                    foreach (UnitBattleDataModel unitBattleDataModel in LogueBookModels.playerBattleModel)
                    {
                        unitBattleDataModel.isDead = false;
                        unitBattleDataModel.hp += (unitBattleDataModel.MaxHp - (int)unitBattleDataModel.hp) * 0.75f; // recover 75% of missing hp
                        unitBattleDataModel.Init();
                        unitBattleDataModel.emotionDetail.Reset();
                    }
                }
            }
            LogLikeMod.AddPlayer = false;
            LogLikeMod.RecoverPlayers = false;
            if (!(LogLikeMod.curstageid != (LorId)null))
                return;
            LoguePlayDataSaver.SavePlayData();
            LoguePlayDataSaver.RemoveFlashData();
            StageModel stageModel = __instance.GetStageModel();
            if ((stageModel.GetFrontAvailableWave() == null ? 1 : (stageModel.GetFrontAvailableFloor() == null ? 1 : 0)) != 0)
                LoguePlayDataSaver.RemovePlayerData();
        }

        /// <summary>
        /// A patch to be able to disable units when unchecked.
        /// </summary>
        [HarmonyPostfix, HarmonyPatch(typeof(UICharacterSlot), nameof(UICharacterSlot.SetNoToggleState))]
        public static void UICharacterSlot_SetToggleStateFalse(UICharacterSlot __instance)
        {
            if (!LogLikeMod.CheckStage())
                return;
            var unit = LogueBookModels.playerBattleModel.Find(x => x.unitData == __instance.unitBattleData.unitData);
            if (unit != null)
                unit.IsAddedBattle = false;
        }

        /// <summary>
        /// A patch to be able to enable units when checked.
        /// </summary>
        [HarmonyPostfix, HarmonyPatch(typeof(UICharacterSlot), nameof(UICharacterSlot.SetYesToggleState))]
        public static void UICharacterSlot_SetToggleStateTrue(UICharacterSlot __instance)
        {
            if (!LogLikeMod.CheckStage())
                return;
            var unit = LogueBookModels.playerBattleModel.Find(x => x.unitData == __instance.unitBattleData.unitData);
            if (unit != null)
                unit.IsAddedBattle = true;
        }

        #endregion

        #region FINALIZERS
        [HarmonyFinalizer, HarmonyPatch(typeof(BattleUnitEmotionDetail), nameof(BattleUnitEmotionDetail.Reset))]
        static Exception BattleUnitEmotionDetail_Reset(Exception __exception)
        {
            return null;
        }


        #endregion
    }
}
