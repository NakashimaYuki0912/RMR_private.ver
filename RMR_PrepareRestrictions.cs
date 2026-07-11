using System;
using System.Collections.Generic;
using System.Linq;
using abcdcode_LOGLIKE_MOD;
using LOR_DiceSystem;
using UI;
using UnityEngine;

namespace RogueLike_Mod_Reborn
{
    /// <summary>
    /// Deck / key-page filters for RMR prepare and Floor Realization (解放战).
    /// Vanilla EGO pages live in personalEgo / emotion EGO UI — never the combat deck inventory.
    /// </summary>
    public static class RMRPrepareRestrictions
    {
        /// <summary>
        /// Urban Nightmare = Chapter 5, Star of the City = 6, Impurity = 7 (same as BookXmlInfo.Chapter).
        /// </summary>
        public const int ChapterUrbanNightmare = 5;
        public const int ChapterUrbanStar = 6;
        public const int ChapterImpurity = 7;

        public static bool IsEgoCombatPage(DiceCardXmlInfo card)
        {
            if (card == null)
                return false;
            try
            {
                if (card.IsEgo())
                    return true;
            }
            catch { /* IsEgo missing on some stubs */ }
            if (card.optionList == null)
                return false;
            return card.optionList.Contains(CardOption.EGO)
                || card.optionList.Contains(CardOption.EgoPersonal)
                || card.optionList.Contains(CardOption.EgoChange);
        }

        public static bool IsAllowedInCombatDeckInventory(DiceCardXmlInfo card)
        {
            if (card == null)
                return false;
            if (IsEgoCombatPage(card))
                return false;
            if (card.optionList != null && card.optionList.Contains(CardOption.NoInventory))
                return false;
            return true;
        }

        /// <summary>
        /// 解放战出战层书页章节上限：
        /// 历史/科技/文学/艺术 → 都市梦魇(5)；总类 → 杂质(7)；其余 → 都市之星(6)。
        /// </summary>
        public static int GetRealizationMaxChapter(SephirahType floor)
        {
            switch (floor)
            {
                case SephirahType.Malkuth: // 历史
                case SephirahType.Yesod:   // 科技
                case SephirahType.Hod:     // 文学
                case SephirahType.Netzach: // 艺术
                    return ChapterUrbanNightmare;
                case SephirahType.Keter:   // 总类
                    return ChapterImpurity;
                default:
                    return ChapterUrbanStar;
            }
        }

        public static SephirahType GetPrepareFloor()
        {
            try
            {
                if (Singleton<StageController>.Instance != null)
                {
                    SephirahType f = Singleton<StageController>.Instance.CurrentFloor;
                    if (f != SephirahType.None && (int)f != 11)
                        return f;
                }
            }
            catch { /* ignore */ }
            try
            {
                if (UI.UIController.Instance != null)
                {
                    SephirahType f = UI.UIController.Instance.CurrentSephirah;
                    if (f != SephirahType.None && (int)f != 11)
                        return f;
                }
            }
            catch { /* ignore */ }
            return SephirahType.Malkuth;
        }

        public static bool IsRealizationPrepareContext()
        {
            return RMRRealizationManager.IsRealizationPreparationActive
                || RMRRealizationManager.InRealizationBattle
                || RMRRealizationManager.RealizationReceptionActive;
        }

        public static int GetContentChapter(BookXmlInfo book)
        {
            if (book == null)
                return 99;
            int ch = book.Chapter;
            if (ch <= 0)
                ch = 1;
            return ch;
        }

        public static int GetContentChapter(DiceCardXmlInfo card)
        {
            if (card == null)
                return 99;
            int ch = card.Chapter;
            if (ch <= 0)
                ch = 1;
            return ch;
        }

        public static bool IsBookAllowedInCurrentPrepare(BookXmlInfo book)
        {
            if (book == null || !RewardingModel.IsValidBookData(book))
                return false;
            if (!IsRealizationPrepareContext())
                return true;
            int max = GetRealizationMaxChapter(GetPrepareFloor());
            return GetContentChapter(book) <= max;
        }

        public static bool IsCardAllowedInCurrentPrepare(DiceCardXmlInfo card)
        {
            if (!IsAllowedInCombatDeckInventory(card))
                return false;
            if (!IsRealizationPrepareContext())
                return true;
            int max = GetRealizationMaxChapter(GetPrepareFloor());
            return GetContentChapter(card) <= max;
        }

        public static List<DiceCardItemModel> FilterCombatInventoryCards(List<DiceCardItemModel> source)
        {
            if (source == null)
                return new List<DiceCardItemModel>();
            var result = new List<DiceCardItemModel>();
            foreach (DiceCardItemModel item in source)
            {
                if (item?.ClassInfo == null)
                    continue;
                if (!IsCardAllowedInCurrentPrepare(item.ClassInfo))
                    continue;
                result.Add(item);
            }
            return result;
        }

        public static List<BookModel> FilterEquipInventoryBooks(List<BookModel> source)
        {
            if (source == null)
                return new List<BookModel>();
            if (!IsRealizationPrepareContext())
                return source;
            var result = new List<BookModel>();
            foreach (BookModel book in source)
            {
                if (book?.ClassInfo == null)
                    continue;
                if (!IsBookAllowedInCurrentPrepare(book.ClassInfo))
                    continue;
                result.Add(book);
            }
            return result;
        }

        /// <summary>
        /// Remove illegal EGO pages already stuffed into decks (atlas/EGO reward path mistakes).
        /// </summary>
        public static void StripEgoPagesFromPlayerDecks()
        {
            if (LogueBookModels.playerModel == null)
                return;
            foreach (UnitDataModel unit in LogueBookModels.playerModel)
            {
                if (unit?.bookItem == null)
                    continue;
                try
                {
                    List<DiceCardXmlInfo> deck = unit.bookItem.GetCardListFromCurrentDeck();
                    if (deck == null)
                        continue;
                    var toRemove = deck.Where(c => IsEgoCombatPage(c)).Select(c => c.id).Distinct().ToList();
                    foreach (LorId id in toRemove)
                    {
                        try { unit.bookItem.MoveCardFromCurrentDeckToInventory(id); }
                        catch { /* ignore */ }
                    }
                    if (toRemove.Count > 0)
                        Debug.Log($"[RMR] Stripped {toRemove.Count} EGO page id(s) from deck of {unit.name}.");
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("[RMR] StripEgoPagesFromPlayerDecks: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Collect EGO ids owned this run (route unlock + combat inventory entries).
        /// </summary>
        public static List<LorId> GetOwnedEgoIdsForBattle()
        {
            var ids = new List<LorId>();
            var seen = new HashSet<string>();
            void TryAdd(LorId id)
            {
                if (id == null || id == LorId.None)
                    return;
                string key = (id.packageId ?? "") + ":" + id.id;
                if (!seen.Add(key))
                    return;
                DiceCardXmlInfo card = null;
                try
                {
                    card = ItemXmlDataList.instance.GetCardItem(id, true)
                        ?? ItemXmlDataList.instance.GetCardItem(id.id, true);
                }
                catch { }
                if (card != null && !IsEgoCombatPage(card))
                    return;
                // If XML missing still allow route-tracked ids through.
                if (card == null && !RMRAbnormalityUnlockManager.IsEgoUnlockedForCurrentRoute(id)
                    && !RMRAbnormalityUnlockManager.IsEgoOwnedOnCurrentRoute(id))
                    return;
                ids.Add(id);
            }

            try
            {
                // Route-picked realization EGO.
                // IsEgoUnlockedForCurrentRoute is checked via ownership helper + inventory scan below.
            }
            catch { }

            try
            {
                if (LogueBookModels.cardlist != null)
                {
                    foreach (DiceCardItemModel item in LogueBookModels.cardlist)
                    {
                        if (item?.ClassInfo == null)
                            continue;
                        if (IsEgoCombatPage(item.ClassInfo))
                            TryAdd(item.GetID() ?? item.ClassInfo.id);
                    }
                }
            }
            catch { }

            // Route-unlocked ids even if stripped from decks / not shown in inventory.
            try
            {
                foreach (LorId id in RMRAbnormalityUnlockManager.EnumerateRouteUnlockedEgoPages())
                    TryAdd(id);
            }
            catch { }

            return ids;
        }

        /// <summary>
        /// Vanilla: personal EGO becomes choosable when emotion level is high enough.
        /// RMR previously only AddCard'd EGO into combat inventory and stripped them from decks —
        /// never personalEgoDetail. Grant owned EGO to every living player unit at battle start.
        /// </summary>
        public static void GrantOwnedEgoToBattleUnits()
        {
            if (!LogLikeRoutines.IsRoguelikeBattleSettingContext() && !LogLikeMod.CheckStage(true))
            {
                // Still try if battle units exist (mid-reception).
            }
            List<LorId> egoIds = GetOwnedEgoIdsForBattle();
            if (egoIds.Count == 0)
                return;

            int granted = 0;
            try
            {
                List<BattleUnitModel> players = BattleObjectManager.instance?.GetList(Faction.Player);
                if (players != null)
                {
                    foreach (BattleUnitModel unit in players)
                    {
                        if (unit == null || unit.IsDead() || unit.personalEgoDetail == null)
                            continue;
                        foreach (LorId id in egoIds)
                        {
                            try
                            {
                                // AddCard is idempotent enough for battle; duplicates avoided by Exists checks when present.
                                bool has = false;
                                try
                                {
                                    var cards = unit.personalEgoDetail.GetHand();
                                    if (cards != null)
                                        has = cards.Any(c => c != null && c.GetID() != null
                                            && c.GetID().id == id.id
                                            && (string.IsNullOrEmpty(c.GetID().packageId) || string.IsNullOrEmpty(id.packageId)
                                                || c.GetID().packageId == id.packageId));
                                }
                                catch { has = false; }
                                if (!has)
                                {
                                    unit.personalEgoDetail.AddCard(id);
                                    granted++;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.LogWarning($"[RMR] GrantOwnedEgo AddCard failed id={id}: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR] GrantOwnedEgoToBattleUnits: " + ex.Message);
            }

            if (granted > 0)
                Debug.Log($"[RMR] Granted owned EGO to personalEgo ({granted} add ops, {egoIds.Count} unique ids).");
        }

        public static bool IsMoneyPlaceholderPassive(LorId id)
        {
            return id != null && id.id == 1
                && (id.packageId == LogLikeMod.ModId
                    || id.packageId == RMRCore.packageId
                    || string.IsNullOrEmpty(id.packageId));
        }

        public static string GetFloorShortName(SephirahType floor)
        {
            string key;
            switch (floor)
            {
                case SephirahType.Malkuth: key = "ui_RMR_Floor_Malkuth"; break;
                case SephirahType.Yesod: key = "ui_RMR_Floor_Yesod"; break;
                case SephirahType.Hod: key = "ui_RMR_Floor_Hod"; break;
                case SephirahType.Netzach: key = "ui_RMR_Floor_Netzach"; break;
                case SephirahType.Tiphereth: key = "ui_RMR_Floor_Tiphereth"; break;
                case SephirahType.Gebura: key = "ui_RMR_Floor_Gebura"; break;
                case SephirahType.Chesed: key = "ui_RMR_Floor_Chesed"; break;
                case SephirahType.Binah: key = "ui_RMR_Floor_Binah"; break;
                case SephirahType.Hokma: key = "ui_RMR_Floor_Hokma"; break;
                case SephirahType.Keter: key = "ui_RMR_Floor_Keter"; break;
                default: return string.Empty;
            }
            try
            {
                string t = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText(key);
                if (!string.IsNullOrEmpty(t) && t != key)
                    return t;
            }
            catch { /* fall through */ }
            return floor.ToString();
        }

        /// <summary>A3: toast when inventory list is empty after filters.</summary>
        public static void NotifyInventoryEmptyIfNeeded(bool isBookInventory, int count)
        {
            if (count > 0)
                return;
            if (!LogLikeRoutines.IsRoguelikeBattleSettingContext() && !IsRealizationPrepareContext())
                return;
            string key = isBookInventory ? "ui_RMR_BookInventoryEmpty" : "ui_RMR_InventoryEmpty";
            string msg;
            try { msg = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText(key); }
            catch { msg = key; }
            if (string.IsNullOrEmpty(msg) || msg == key)
                msg = isBookInventory
                    ? "当前层限制：没有可显示的核心书页。"
                    : "当前层限制：没有可显示的战斗书页。";
            try
            {
                if (UIAlarmPopup.instance != null)
                    UIAlarmPopup.instance.SetAlarmText(msg);
                else
                    Debug.Log($"[RMR] {msg}");
            }
            catch
            {
                Debug.Log($"[RMR] {msg}");
            }
        }
    }
}
