using System;
using System.Collections.Generic;
using System.Linq;
using abcdcode_LOGLIKE_MOD;
using GameSave;
using HarmonyLib;
using LOR_DiceSystem;
using UnityEngine;

namespace RogueLike_Mod_Reborn
{
    public static class RMRRealizationManager
    {
        public static readonly Dictionary<SephirahType, int> RealizationBossStageIds = new Dictionary<SephirahType, int>
        {
            { SephirahType.Malkuth,   201005 },
            { SephirahType.Yesod,     202005 },
            { SephirahType.Hod,       203005 },
            { SephirahType.Netzach,   204005 },
            { SephirahType.Tiphereth, 205005 },
            { SephirahType.Gebura,    206005 },
            { SephirahType.Chesed,    207005 },
            { SephirahType.Binah,     208004 },
            { SephirahType.Hokma,     209004 },
            { SephirahType.Keter,     210009 },
        };

        public static readonly Dictionary<SephirahType, string> FloorDisplayNames = new Dictionary<SephirahType, string>
        {
            { SephirahType.Malkuth,   "\u5386\u53f2\u5c42" },
            { SephirahType.Yesod,     "\u79d1\u6280\u5c42" },
            { SephirahType.Hod,       "\u6587\u5b66\u5c42" },
            { SephirahType.Netzach,   "\u827a\u672f\u5c42" },
            { SephirahType.Tiphereth, "\u81ea\u7136\u5c42" },
            { SephirahType.Gebura,    "\u8bed\u8a00\u5c42" },
            { SephirahType.Chesed,    "\u793e\u4f1a\u5c42" },
            { SephirahType.Binah,     "\u54f2\u5b66\u5c42" },
            { SephirahType.Hokma,     "\u5b97\u6559\u5c42" },
            { SephirahType.Keter,     "\u603b\u7c7b\u5c42" },
        };

        public static bool InRealizationBattle { get; private set; }

        /// <summary>
        /// Set when a realization battle is queued but the current event hasn't finished
        /// transitioning yet. Prevents the EndBattle hook from treating the event end
        /// as a realization battle end.
        /// </summary>
        public static bool PendingRealizationBattle { get; private set; }
        public static bool ForceReturnAsDefeatPending { get; private set; }

        public static bool IsRealizationPreparationActive => PendingRealizationBattle && !InRealizationBattle;

        /// <summary>
        /// Clears the realization battle flag. Called from StageController_EndBattle
        /// AFTER the vanilla EndBattle completes, so that postfix hooks still see
        /// the guard during cleanup.
        /// </summary>
        public static void ClearRealizationFlag()
        {
            InRealizationBattle = false;
        }

        public static void ConsumeForceReturnAsDefeat()
        {
            ForceReturnAsDefeatPending = false;
        }

        /// <summary>
        /// Called from StageController_StartBattle when the pending realization stage
        /// actually begins loading. Activates InRealizationBattle and clears the pending flag.
        /// </summary>
        public static void ActivatePendingRealization()
        {
            if (PendingRealizationBattle)
            {
                InRealizationBattle = true;
                PendingRealizationBattle = false;
                Debug.Log($"[RMRRealizationManager] Activated realization battle: {CurrentRealizationFloor}");
            }
        }
        public static SephirahType CurrentRealizationFloor { get; private set; }
        public static bool InitialRelicEntryAvailable { get; private set; }

        public static void SetInitialRelicEntryAvailable(bool available)
        {
            InitialRelicEntryAvailable = available;
        }

        private static bool AtlasOnlyLoadoutActive;
        private static List<BookModel> RouteBookSnapshot;
        private static List<DiceCardItemModel> RouteCardSnapshot;
        private static List<UnitDataModel> RoutePlayerModelSnapshot;
        private static List<UnitBattleDataModel> RoutePlayerBattleModelSnapshot;
        private static Dictionary<UnitDataModel, List<LorId>> RoutePlayersPickSnapshot;
        private static Dictionary<UnitDataModel, List<LorId>> RoutePlayersPerPassivesSnapshot;
        private static Dictionary<UnitDataModel, List<LogStatAdder>> RoutePlayersStatAddersSnapshot;
        private static abcdcode_LOGLIKE_MOD.StageType RouteStageTypeSnapshot;
        private static LorId RouteStageIdSnapshot;
        private static List<EmotionCardXmlInfo> RouteNextListSnapshot;
        private static List<DropBookXmlInfo> RouteRewardsSnapshot;
        private static List<RewardInfo> RouteRewardsPassiveSnapshot;
        private static List<RewardInfo> RouteRewardsInStageSnapshot;
        private static ChapterGrade RouteChapterGradeSnapshot;
        private static int RouteChStageStepSnapshot;

        // Deep per-unit state for proper restoration after realization battle.
        // Units are identified by their index in the original playerModel list.
        private sealed class UnitDeepState
        {
            public LorId EquippedBookId;
            public List<LorId> DeckCardIds;
            public string CustomName;
        }
        private static List<UnitDeepState> RouteUnitDeepStates;
        /// <summary>
        /// Resolves a floor to a runnable realization StageClassInfo.
        /// Validates against StageClassInfoList (battle runtime data), not just StagesXmlList (UI metadata).
        /// </summary>
        public static bool TryResolveRealizationStage(
            SephirahType floor,
            out LorId stageId,
            out StageClassInfo stageClassInfo,
            out string reason)
        {
            stageId = LorId.None;
            stageClassInfo = null;
            reason = null;

            if (!RealizationBossStageIds.TryGetValue(floor, out int stageIdNum))
            {
                reason = $"No realization stage ID mapping for floor: {floor}";
                return false;
            }

            // Try vanilla (empty packageId) first, then mod packageId as fallback
            var candidates = new List<LorId>
            {
                new LorId(string.Empty, stageIdNum),
                new LorId(LogLikeMod.ModId, stageIdNum),
            };
            // Also try the RMRCore packageId
            if (!string.IsNullOrEmpty(RMRCore.packageId) && RMRCore.packageId != LogLikeMod.ModId)
                candidates.Add(new LorId(RMRCore.packageId, stageIdNum));

            foreach (var candidate in candidates)
            {
                stageClassInfo = Singleton<StageClassInfoList>.Instance.GetData(candidate);
                if (stageClassInfo != null && stageClassInfo.waveList != null && stageClassInfo.waveList.Count > 0)
                {
                    stageId = candidate;
                    reason = null;
                    return true;
                }
            }

            // Detail why it failed
            var sb = new System.Text.StringBuilder();
            sb.Append($"Realization stage {stageIdNum} for floor {floor} has no valid StageClassInfo. ");
            sb.Append("Checked candidates: ");
            foreach (var c in candidates)
            {
                var sci = Singleton<StageClassInfoList>.Instance.GetData(c);
                if (sci == null)
                    sb.Append($"[{c.packageId}:{c.id} → null] ");
                else if (sci.waveList == null)
                    sb.Append($"[{c.packageId}:{c.id} → waveList=null] ");
                else if (sci.waveList.Count == 0)
                    sb.Append($"[{c.packageId}:{c.id} → waveList.Count=0] ");
                else
                    sb.Append($"[{c.packageId}:{c.id} → OK] ");
            }
            reason = sb.ToString();
            return false;
        }

        public static void StartRealizationBattle(SephirahType floor)
        {
            if (!InitialRelicEntryAvailable)
            {
                Debug.LogWarning("[RMRRealizationManager] Realization battles can only be started from the first initial relic event.");
                return;
            }

            // Resolve and validate against StageClassInfoList BEFORE consuming the entry flag.
            if (!TryResolveRealizationStage(floor, out LorId stageId, out StageClassInfo stageClassInfo, out string reason))
            {
                Debug.LogError($"[RMRRealizationManager] Cannot start realization battle: {reason}");
                return;
            }

            // Only now that we've confirmed a valid stage, consume the flag.
            InitialRelicEntryAvailable = false;
            ForceReturnAsDefeatPending = false;
            // Set PendingRealizationBattle before opening BattleSetting so RMR UI hooks
            // render the atlas-only temporary party instead of the vanilla floor limit.
            PendingRealizationBattle = true;
            CurrentRealizationFloor = floor;
            if (!ApplyAtlasOnlyLoadout())
            {
                PendingRealizationBattle = false;
                InitialRelicEntryAvailable = true;
                return;
            }
            // Keep PendingRealizationBattle instead of InRealizationBattle.
            // InRealizationBattle is only activated in StageController_StartBattle
            // when the realization stage actually loads. This prevents the EndBattle
            // hook from treating the event transition as a realization battle end.
            LogLikeMod.rewards.Clear();
            LogLikeMod.rewards_passive.Clear();

            if (!TryStartVanillaRealizationStage(floor, stageId, stageClassInfo))
            {
                PendingRealizationBattle = false;
                RestoreRouteLoadout();
                InitialRelicEntryAvailable = true;
                Debug.LogError($"[RMRRealizationManager] Failed to start vanilla realization stage: {stageId.packageId}:{stageId.id}");
                return;
            }
            Debug.Log($"[RMRRealizationManager] Started vanilla realization battle: {floor} (stage {stageId.id}) via package {stageId.packageId}");
        }

        private static bool TryStartVanillaRealizationStage(
            SephirahType floor,
            LorId stageId,
            StageClassInfo stageClassInfo)
        {
            if (stageClassInfo == null)
                return false;
            ForceRealizationAvailableUnits(stageClassInfo, 5);

            try
            {
                Singleton<MysteryManager>.Instance.EndMystery();
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMRRealizationManager] Realization transition mystery finish failed: " + ex.Message);
            }

            try
            {
                StageController controller = Singleton<StageController>.Instance;
                if (controller == null)
                    return false;

                // Match vanilla creature/realization stage binding, but stop at
                // BattleSetting so the atlas-only temporary team can be configured.
                controller.SetCurrentSephirah(floor);
                controller.InitStageByCreature(stageClassInfo, false);
                StageModel stageModel = controller.GetStageModel();
                ForceRealizationAvailableUnits(stageModel?.ClassInfo, 5);
                ForceRealizationStageModelAvailableUnits(stageModel, 5);
                LorId currentId = stageModel?.ClassInfo?.id ?? LorId.None;
                if (currentId != stageClassInfo.id)
                {
                    Debug.LogError($"[RMRRealizationManager] Vanilla realization StageModel mismatch. current={currentId.packageId}:{currentId.id}, expected={stageClassInfo.id.packageId}:{stageClassInfo.id.id}");
                    return false;
                }

                LogLikeMod.curstageid = stageId;
                LogLikeMod.curstagetype = abcdcode_LOGLIKE_MOD.StageType.Creature;
                UI.UIController.Instance.OpenBattlePrepare();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("[RMRRealizationManager] Failed to start vanilla realization stage: " + ex);
                return false;
            }
        }

        private static void ForceRealizationAvailableUnits(StageClassInfo stageClassInfo, int count)
        {
            if (stageClassInfo?.waveList == null)
                return;
            foreach (StageWaveInfo wave in stageClassInfo.waveList)
                ForceWaveAvailableUnits(wave, count);
        }

        private static void ForceRealizationStageModelAvailableUnits(StageModel stageModel, int count)
        {
            if (stageModel?.waveList == null)
                return;
            foreach (StageWaveModel wave in stageModel.waveList)
                ForceStageWaveModelAvailableUnits(wave, count);
        }

        private static void ForceWaveAvailableUnits(StageWaveInfo wave, int count)
        {
            if (wave == null)
                return;
            Type type = wave.GetType();
            foreach (string fieldName in new[] { "availableNumber", "availableUnit", "_availableUnit", "AvailableUnit" })
            {
                try
                {
                    var field = type.GetField(fieldName, AccessTools.all);
                    if (field != null && field.FieldType == typeof(int))
                    {
                        field.SetValue(wave, count);
                        return;
                    }
                }
                catch { }
            }
            foreach (string propertyName in new[] { "AvailableUnit", "availableUnit" })
            {
                try
                {
                    var property = type.GetProperty(propertyName, AccessTools.all);
                    if (property != null && property.CanWrite && property.PropertyType == typeof(int))
                    {
                        property.SetValue(wave, count, null);
                        return;
                    }
                }
                catch { }
            }
        }

        private static void ForceStageWaveModelAvailableUnits(StageWaveModel wave, int count)
        {
            if (wave == null)
                return;
            try
            {
                var field = wave.GetType().GetField("_availableUnitNumber", AccessTools.all);
                if (field != null && field.FieldType == typeof(int))
                    field.SetValue(wave, count);
            }
            catch { }
        }

        public static void OnRealizationBattleEnded(bool victory)
        {
            if (!InRealizationBattle)
                return;

            if (victory)
            {
                RMRAbnormalityUnlockManager.CompleteFloorRealization(CurrentRealizationFloor);
                Debug.Log($"[RMRRealizationManager] Floor realization completed: {CurrentRealizationFloor}");
            }

            // Clear any rewards that may have been queued during the battle to prevent
            // the Roguelike reward chain from running after realization ends.
            LogLikeMod.rewards?.Clear();
            LogLikeMod.rewards_passive?.Clear();
            LogLikeMod.rewards_InStage?.Clear();
            LogLikeMod.nextlist?.Clear();

            RestoreRouteLoadout();
            ForceReturnAsDefeatPending = true;
            // NOTE: InRealizationBattle is cleared in the StageController_EndBattle hook
            // AFTER orig(self) completes, ensuring ClearBattle/EndBattlePhase postfixes
            // still see the flag during vanilla cleanup.
            CurrentRealizationFloor = SephirahType.Keter;

            // After realization, the vanilla StageController.EndBattle flow will naturally
            // return to the main menu / battle setting screen. No next-stage reward UI
            // will appear because CheckStage() returns false for vanilla realization stages.
            Debug.Log($"[RMRRealizationManager] Realization battle ended. Victory={victory}. State restored; defeat-style return queued.");
        }

        /// <summary>
        /// Returns the player to the main menu after a realization battle.
        /// Called after the vanilla EndBattle completes to ensure clean transition.
        /// </summary>
        public static void ReturnToMainAfterRealization()
        {
            // After realization, the vanilla StageController.EndBattle flow naturally
            // returns to the main menu. This method is a no-op hook for future use
            // if explicit UI transition is needed.
            Debug.Log("[RMRRealizationManager] ReturnToMainAfterRealization called — vanilla EndBattle flow handles UI transition.");
        }

        private static void EnsureDefaultRealizationAtlasUnlocks()
        {
            LogueBookModels.EnsureAtlasUnlocks();
            bool changed = false;

            for (int id = -854; id >= -858; id--)
            {
                LorId bookId = new LorId(LogLikeMod.ModId, id);
                if (Singleton<BookXmlList>.Instance.GetData(bookId) != null)
                    changed |= LogueBookModels.AtlasUnlockedRoleBooks.Add(bookId);
            }

            for (int id = -10; id >= -14; id--)
            {
                LorId cardId = new LorId(LogLikeMod.ModId, id);
                if (ItemXmlDataList.instance.GetCardItem(cardId, true) != null)
                    changed |= LogueBookModels.AtlasUnlockedBattleCards.Add(cardId);
            }

            if (changed)
                // Save only the fallback atlas entries; the broader save path would
                // also sync the current route inventory into the permanent atlas.
                LogueBookModels.SavePermanentAtlasData();
        }


        private static bool ApplyAtlasOnlyLoadout()
        {
            if (AtlasOnlyLoadoutActive)
                return true;
            LogueBookModels.EnsureAtlasUnlocks();
            if (LogueBookModels.AtlasUnlockedRoleBooks == null)
                LogueBookModels.AtlasUnlockedRoleBooks = new HashSet<LorId>();
            if (LogueBookModels.AtlasUnlockedBattleCards == null)
                LogueBookModels.AtlasUnlockedBattleCards = new HashSet<LorId>();
            EnsureDefaultRealizationAtlasUnlocks();

            // --- Save full route state ---
            RouteBookSnapshot = LogueBookModels.booklist == null ? new List<BookModel>() : new List<BookModel>(LogueBookModels.booklist);
            RouteCardSnapshot = LogueBookModels.cardlist == null ? new List<DiceCardItemModel>() : new List<DiceCardItemModel>(LogueBookModels.cardlist);
            RoutePlayerModelSnapshot = LogueBookModels.playerModel == null ? new List<UnitDataModel>() : new List<UnitDataModel>(LogueBookModels.playerModel);
            RoutePlayerBattleModelSnapshot = LogueBookModels.playerBattleModel == null ? new List<UnitBattleDataModel>() : new List<UnitBattleDataModel>(LogueBookModels.playerBattleModel);
            RoutePlayersPickSnapshot = LogueBookModels.playersPick == null ? new Dictionary<UnitDataModel, List<LorId>>() : new Dictionary<UnitDataModel, List<LorId>>(LogueBookModels.playersPick);
            RoutePlayersPerPassivesSnapshot = LogueBookModels.playersperpassives == null ? new Dictionary<UnitDataModel, List<LorId>>() : new Dictionary<UnitDataModel, List<LorId>>(LogueBookModels.playersperpassives);
            RoutePlayersStatAddersSnapshot = LogueBookModels.playersstatadders == null ? new Dictionary<UnitDataModel, List<LogStatAdder>>() : new Dictionary<UnitDataModel, List<LogStatAdder>>(LogueBookModels.playersstatadders);
            RouteStageTypeSnapshot = LogLikeMod.curstagetype;
            RouteStageIdSnapshot = LogLikeMod.curstageid;
            RouteNextListSnapshot = LogLikeMod.nextlist == null ? new List<EmotionCardXmlInfo>() : new List<EmotionCardXmlInfo>(LogLikeMod.nextlist);
            RouteRewardsSnapshot = LogLikeMod.rewards == null ? new List<DropBookXmlInfo>() : new List<DropBookXmlInfo>(LogLikeMod.rewards);
            RouteRewardsPassiveSnapshot = LogLikeMod.rewards_passive == null ? new List<RewardInfo>() : new List<RewardInfo>(LogLikeMod.rewards_passive);
            RouteRewardsInStageSnapshot = LogLikeMod.rewards_InStage == null ? new List<RewardInfo>() : new List<RewardInfo>(LogLikeMod.rewards_InStage);
            RouteChapterGradeSnapshot = LogLikeMod.curchaptergrade;
            RouteChStageStepSnapshot = LogLikeMod.curChStageStep;

            // --- Save deep per-unit state BEFORE mutating UnitDataModel objects ---
            RouteUnitDeepStates = new List<UnitDeepState>();
            if (LogueBookModels.playerModel != null)
            {
                foreach (var unit in LogueBookModels.playerModel)
                {
                    var ds = new UnitDeepState();
                    if (unit != null && unit.bookItem != null)
                    {
                        // Save the equipped book ID (the booklist entry that has owner == unit)
                        var ownedBook = LogueBookModels.booklist?.Find(b => b.owner == unit);
                        ds.EquippedBookId = ownedBook?.ClassInfo?.id ?? LorId.None;
                        // Save current deck card IDs
                        var deck = unit.bookItem.GetCardListFromCurrentDeck();
                        ds.DeckCardIds = deck != null ? new List<LorId>(deck.Select(c => c.id)) : new List<LorId>();
                        ds.CustomName = unit.name;
                    }
                    RouteUnitDeepStates.Add(ds);
                }
            }

            // --- Build atlas-only book/card lists ---
            List<BookModel> atlasBooks = new List<BookModel>();
            foreach (LorId id in LogueBookModels.AtlasUnlockedRoleBooks)
            {
                BookXmlInfo book = Singleton<BookXmlList>.Instance.GetData(id);
                if (book == null)
                    continue;
                BookModel model = new BookModel(book);
                model.instanceId = LogueBookModels.nextinstanceid++;
                model.TryGainUniquePassive();
                atlasBooks.Add(model);
            }
            if (atlasBooks.Count == 0)
            {
                Debug.LogError("[RMRRealizationManager] Atlas-only realization loadout has no unlocked core pages, including starter fallback.");
                return false;
            }

            List<DiceCardItemModel> atlasCards = new List<DiceCardItemModel>();
            foreach (LorId id in LogueBookModels.AtlasUnlockedBattleCards)
            {
                DiceCardXmlInfo card = ItemXmlDataList.instance.GetCardItem(id, true);
                if (card == null || card.optionList.Contains(CardOption.NoInventory))
                    continue;
                atlasCards.Add(new DiceCardItemModel(card) { num = LogueBookModels.UNLOCKED_CARD_COUNT });
            }

            LogueBookModels.booklist = atlasBooks;
            LogueBookModels.cardlist = atlasCards;

            // --- Build realization team: ensure 5 librarians ---
            int targetCount = 5;

            // Start with the route player list (will be mutated by EquipNewPage)
            var teamList = new List<UnitDataModel>(RoutePlayerModelSnapshot ?? new List<UnitDataModel>());
            LogueBookModels.playerModel = teamList;
            LogueBookModels.playerBattleModel = new List<UnitBattleDataModel>();
            LogueBookModels.playersPick = new Dictionary<UnitDataModel, List<LorId>>();
            LogueBookModels.playersperpassives = new Dictionary<UnitDataModel, List<LorId>>();
            LogueBookModels.playersstatadders = new Dictionary<UnitDataModel, List<LogStatAdder>>();
            foreach (UnitDataModel unit in teamList)
            {
                if (unit == null)
                    continue;
                LogueBookModels.playersPick[unit] = new List<LorId>();
                LogueBookModels.playersperpassives[unit] = new List<LorId>();
                LogueBookModels.playersstatadders[unit] = new List<LogStatAdder>();
            }

            // If we have fewer than 5, pad using AddSubPlayer
            while (teamList.Count < targetCount)
            {
                try
                {
                    LogueBookModels.AddSubPlayer();
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[RMRRealizationManager] AddSubPlayer failed: {ex.Message}");
                    break;
                }
                // AddSubPlayer appends to LogueBookModels.playerModel
                teamList = LogueBookModels.playerModel;
            }

            // Trim if somehow we have more than 5
            while (teamList.Count > targetCount)
                teamList.RemoveAt(teamList.Count - 1);

            // Equip each librarian with an atlas book
            for (int i = 0; i < teamList.Count; i++)
            {
                var unit = teamList[i];
                // Use atlas book at matching index, or wrap around if fewer books than librarians
                var book = atlasBooks[i % atlasBooks.Count];
                if (unit != null && book != null)
                {
                    LogueBookModels.EquipNewPage(unit, book.ClassInfo, false);
                    // Skip auto-fill only for true built-in decks (e.g. Black Silence, Binah).
                    // OnlyCard pages still need atlas cards filled, otherwise realization
                    // battles can start with empty hands after temporary atlas projection.
                    bool hasBuiltInDeck = false;
                    try
                    {
                        bool editableBlue = LogueBookModels.IsEditableBlueReverberationDeck(unit.bookItem);
                        hasBuiltInDeck = !editableBlue
                            && (unit.bookItem?.IsFixedDeck() == true
                                || unit.bookItem?.IsLockByBluePrimary() == true);
                    }
                    catch { }
                    if (!hasBuiltInDeck)
                    {
                        var deck = unit.bookItem?.GetCardListFromCurrentDeck();
                        if (deck != null)
                        {
                            int deckCount = deck.Count;
                            int needed = 9 - deckCount;
                            int tried = 0;
                            int cardIdx = 0;
                            while (needed > 0 && tried < atlasCards.Count * 2 && cardIdx < atlasCards.Count)
                            {
                                var cardXml = atlasCards[cardIdx].ClassInfo;
                                if (cardXml != null && LogueBookModels.CanAddCardToCurrentDeck(cardXml.id, unit.bookItem))
                                {
                                    unit.AddCardFromInventory(cardXml.id);
                                    needed--;
                                }
                                cardIdx++;
                                if (cardIdx >= atlasCards.Count) cardIdx = 0;
                                tried++;
                            }
                        }
                    }
                    else
                    {
                        Debug.Log($"[RMRRealizationManager] Skipped atlas auto-fill for built-in-deck book: {book.ClassInfo.id.id}");
                    }
                }
            }

            LogueBookModels.playerModel = teamList;
            LogueBookModels.CreatePlayerBattle();
            foreach (UnitBattleDataModel battleModel in LogueBookModels.playerBattleModel)
            {
                if (battleModel != null)
                    battleModel.IsAddedBattle = true;
            }

            AtlasOnlyLoadoutActive = true;
            Debug.Log($"[RMRRealizationManager] Applied atlas-only loadout: books={atlasBooks.Count}, cards={atlasCards.Count}, librarians={teamList.Count}");
            return true;
        }

        private static void RestoreRouteLoadout()
        {
            if (!AtlasOnlyLoadoutActive)
                return;

            // --- Restore book/card lists ---
            LogueBookModels.booklist = RouteBookSnapshot ?? new List<BookModel>();
            LogueBookModels.cardlist = RouteCardSnapshot ?? new List<DiceCardItemModel>();
            LogueBookModels.playerModel = RoutePlayerModelSnapshot ?? new List<UnitDataModel>();
            LogueBookModels.playerBattleModel = RoutePlayerBattleModelSnapshot ?? new List<UnitBattleDataModel>();
            LogueBookModels.playersPick = RoutePlayersPickSnapshot ?? new Dictionary<UnitDataModel, List<LorId>>();
            LogueBookModels.playersperpassives = RoutePlayersPerPassivesSnapshot ?? new Dictionary<UnitDataModel, List<LorId>>();
            LogueBookModels.playersstatadders = RoutePlayersStatAddersSnapshot ?? new Dictionary<UnitDataModel, List<LogStatAdder>>();
            LogLikeMod.curstagetype = RouteStageTypeSnapshot;
            LogLikeMod.curstageid = RouteStageIdSnapshot;
            LogLikeMod.nextlist = RouteNextListSnapshot ?? new List<EmotionCardXmlInfo>();
            LogLikeMod.rewards = RouteRewardsSnapshot ?? new List<DropBookXmlInfo>();
            LogLikeMod.rewards_passive = RouteRewardsPassiveSnapshot ?? new List<RewardInfo>();
            LogLikeMod.rewards_InStage = RouteRewardsInStageSnapshot ?? new List<RewardInfo>();
            LogLikeMod.curchaptergrade = RouteChapterGradeSnapshot;
            LogLikeMod.curChStageStep = RouteChStageStepSnapshot;

            // --- Restore deep per-unit state ---
            if (LogueBookModels.playerModel != null && RouteUnitDeepStates != null)
            {
                for (int i = 0; i < LogueBookModels.playerModel.Count && i < RouteUnitDeepStates.Count; i++)
                {
                    var unit = LogueBookModels.playerModel[i];
                    var ds = RouteUnitDeepStates[i];
                    if (unit == null || ds == null)
                        continue;
                    // Restore equipped book
                    if (ds.EquippedBookId != LorId.None && LogueBookModels.booklist != null)
                    {
                        var book = LogueBookModels.booklist.Find(b => b != null && b.ClassInfo != null && b.ClassInfo.id == ds.EquippedBookId);
                        if (book != null)
                        {
                            LogueBookModels.EquipNewPage(unit, book.ClassInfo, false);
                        }
                    }
                    // Restore deck cards
                    if (ds.DeckCardIds != null && unit.bookItem != null)
                    {
                        unit.bookItem.GetCardListFromCurrentDeck()?.Clear();
                        foreach (var cardId in ds.DeckCardIds)
                        {
                            try { unit.AddCardFromInventory(cardId); } catch { }
                        }
                    }
                    // Restore custom name
                    if (!string.IsNullOrEmpty(ds.CustomName))
                    {
                        unit.SetCustomName(ds.CustomName);
                    }
                }
            }

            AtlasOnlyLoadoutActive = false;
            Debug.Log("[RMRRealizationManager] Restored route loadout from snapshot.");
        }
    }
}
