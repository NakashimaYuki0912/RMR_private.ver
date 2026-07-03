// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.RewardingModel
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using LOR_XML;
using RogueLike_Mod_Reborn;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class RewardingModel
    {
        public static RewardingModel.RewardFlag rewardFlag;
        private const double BattleCardRewardRetentionRate = 0.7;
        private const double NormalBattleCardRewardRetentionRate = 0.49;
        private static readonly HashSet<LorId> NormalizedDropBookRewardIds = new HashSet<LorId>();
        private static bool BossFallbackRewardCheckedThisBattle;

        public static void ResetDropBookRewardNormalization()
        {
            NormalizedDropBookRewardIds.Clear();
            BossFallbackRewardCheckedThisBattle = false;
        }

        public static string GetChapterText(int grade)
        {
            switch (grade)
            {
                case 1:
                    return TextDataModel.GetText("ui_maintitle_citystate_1");
                case 2:
                    return TextDataModel.GetText("ui_maintitle_citystate_2");
                case 3:
                    return TextDataModel.GetText("ui_maintitle_citystate_3");
                case 4:
                    return TextDataModel.GetText("ui_maintitle_citystate_4");
                case 5:
                    return TextDataModel.GetText("ui_maintitle_citystate_5");
                case 6:
                    return TextDataModel.GetText("ui_maintitle_citystate_6");
                case 7:
                    return TextDataModel.GetText("ui_maintitle_citystate_7");
                default:
                    return "Not Found";
            }
        }

        public static string GetResistText(AtkResist resist)
        {
            switch (resist)
            {
                case AtkResist.Weak:
                    return TextDataModel.GetText("ui_resistance_weak");
                case AtkResist.Vulnerable:
                    return TextDataModel.GetText("ui_resistance_vulnerable");
                case AtkResist.Normal:
                    return TextDataModel.GetText("ui_resistance_normal");
                case AtkResist.Endure:
                    return TextDataModel.GetText("ui_resistance_endure");
                case AtkResist.Resist:
                    return TextDataModel.GetText("ui_resistance_resist");
                case AtkResist.Immune:
                    return TextDataModel.GetText("ui_resistance_immune");
                default:
                    return "???";
            }
        }

        public static string GetAblilityText(BookXmlInfo bookinfo)
        {
            string str = $"{$"{$"{$"{$"{$"{$"{string.Empty}{TextDataModel.GetText("ui_ability_hp")}: {bookinfo.EquipEffect.Hp.ToString()}{Environment.NewLine}"}{TextDataModel.GetText("ui_ability_break")}: {bookinfo.EquipEffect.Break.ToString()}{Environment.NewLine}"}{abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Equip_Light")}: {bookinfo.EquipEffect.StartPlayPoint.ToString()}{Environment.NewLine}"}{abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Equip_SpeedArea")}: {bookinfo.EquipEffect.SpeedMin.ToString()}~{bookinfo.EquipEffect.Speed.ToString()}{Environment.NewLine}"}{abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Equip_Slash")}: {RewardingModel.GetResistText(bookinfo.EquipEffect.SResist)}/{RewardingModel.GetResistText(bookinfo.EquipEffect.SBResist)}{Environment.NewLine}"}{abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Equip_Penetrate")}: {RewardingModel.GetResistText(bookinfo.EquipEffect.PResist)}/{RewardingModel.GetResistText(bookinfo.EquipEffect.PBResist)}{Environment.NewLine}"}{abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Equip_Hit")}: {RewardingModel.GetResistText(bookinfo.EquipEffect.HResist)}/{RewardingModel.GetResistText(bookinfo.EquipEffect.HBResist)}{Environment.NewLine}";
            string empty = string.Empty;
            for (int index = 0; index < bookinfo.EquipEffect.PassiveList.Count; ++index)
            {
                empty += RewardingModel.GetPassiveName(bookinfo.EquipEffect.PassiveList[index]);
                if (index < bookinfo.EquipEffect.PassiveList.Count - 1)
                    empty += ", ";
            }
            return $"{str}{abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Equip_Passive")}: {empty}";
        }

        private static bool IsOriginPackage(string packageId)
        {
            return string.IsNullOrEmpty(packageId) || packageId == "@origin";
        }

        private static List<LorId> GetOriginAwareIds(LorId id)
        {
            var result = new List<LorId>();
            if (id == null || id == LorId.None)
                return result;
            result.Add(id);
            if (IsOriginPackage(id.packageId))
            {
                LorId originId = new LorId(id.id);
                if (!result.Contains(originId))
                    result.Add(originId);
                LorId emptyPackageId = new LorId(string.Empty, id.id);
                if (!result.Contains(emptyPackageId))
                    result.Add(emptyPackageId);
            }
            return result;
        }

        public static BookXmlInfo GetBookDataOriginAware(LorId id)
        {
            foreach (LorId candidate in GetOriginAwareIds(id))
            {
                BookXmlInfo book = Singleton<BookXmlList>.Instance.GetData(candidate, false);
                if (book != null)
                    return book;
                if (IsOriginPackage(candidate.packageId))
                {
                    book = Singleton<BookXmlList>.Instance.GetData(candidate.id);
                    if (book != null)
                        return book;
                }
            }
            return null;
        }

        public static DiceCardXmlInfo GetCardItemOriginAware(LorId id)
        {
            foreach (LorId candidate in GetOriginAwareIds(id))
            {
                DiceCardXmlInfo card = ItemXmlDataList.instance.GetCardItem(candidate, false);
                if (card != null)
                    return card;
                if (IsOriginPackage(candidate.packageId))
                {
                    card = ItemXmlDataList.instance.GetCardItem(candidate.id, false);
                    if (card != null)
                        return card;
                }
            }
            return null;
        }

        public static string GetLocalizedBookName(BookXmlInfo book)
        {
            if (book == null)
                return string.Empty;
            foreach (LorId candidate in GetOriginAwareIds(book.id))
            {
                string name = Singleton<BookDescXmlList>.Instance.GetBookName(candidate);
                if (!string.IsNullOrEmpty(name))
                    return name;
                if (IsOriginPackage(candidate.packageId))
                {
                    name = Singleton<BookDescXmlList>.Instance.GetBookName(new LorId(candidate.id));
                    if (!string.IsNullOrEmpty(name))
                        return name;
                }
            }
            return book.InnerName ?? string.Empty;
        }

        public static string GetPassiveName(LorId passiveId)
        {
            foreach (LorId candidate in GetOriginAwareIds(passiveId))
            {
                string name = Singleton<PassiveDescXmlList>.Instance.GetName(candidate);
                if (!string.IsNullOrEmpty(name))
                    return name;
                if (IsOriginPackage(candidate.packageId))
                {
                    name = Singleton<PassiveDescXmlList>.Instance.GetName(candidate.id);
                    if (!string.IsNullOrEmpty(name))
                        return name;
                }
            }
            return passiveId?.ToString() ?? string.Empty;
        }

        public static RewardPassiveInfo FindRewardInfo(EmotionCardXmlInfo card)
        {
            if (card == null || Singleton<RewardPassivesList>.Instance?.infos == null)
                return null;
            foreach (RewardPassivesInfo infoGroup in Singleton<RewardPassivesList>.Instance.infos)
            {
                if (infoGroup?.RewardPassiveList == null)
                    continue;
                foreach (RewardPassiveInfo info in infoGroup.RewardPassiveList)
                {
                    if (info == null || info.passiveid != card.id)
                        continue;
                    EmotionCardXmlInfo registered = LogLikeMod.GetRegisteredPickUpXml(info);
                    if (registered == card)
                        return info;
                }
            }
            return null;
        }

        public static string GetRaritytext(Rarity rarity)
        {
            switch (rarity)
            {
                case Rarity.Common:
                    return TextDataModel.GetText("ui_rarity_common");
                case Rarity.Uncommon:
                    return TextDataModel.GetText("ui_rarity_uncommon");
                case Rarity.Rare:
                    return TextDataModel.GetText("ui_rarity_rare");
                case Rarity.Unique:
                    return TextDataModel.GetText("ui_rarity_unique");
                default:
                    return "Not Found";
            }
        }

        public static void CreateEquipRewardXmlData(RewardPassiveInfo info)
        {
            if (info == null)
                return;
            BookXmlInfo data = RewardingModel.GetBookDataOriginAware(info.id);
            EmotionCardXmlInfo pickUpXml = LogLikeMod.GetRegisteredPickUpXml(info);
            if (data == null || pickUpXml == null)
                return;
            AbnormalityCard abnormalityCard = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(pickUpXml.Name);
            if (abnormalityCard == null)
                return;
            abnormalityCard.cardName = RewardingModel.GetLocalizedBookName(data);
            abnormalityCard.flavorText = $"{RewardingModel.GetChapterText(data.Chapter)}, {RewardingModel.GetRaritytext(data.Rarity)}";
            abnormalityCard.abilityDesc = RewardingModel.GetAblilityText(data);
            if (!string.IsNullOrEmpty(data._bookIcon))
                pickUpXml._artwork = data._bookIcon;
        }

        public static RewardPassiveInfo GetReward(List<RewardPassiveInfo> rewards)
        {
            List<RewardPassiveInfo> rewardPassiveInfoList1 = new List<RewardPassiveInfo>();
            List<RewardPassiveInfo> rewardPassiveInfoList2 = new List<RewardPassiveInfo>();
            List<RewardPassiveInfo> rewardPassiveInfoList3 = new List<RewardPassiveInfo>();
            List<RewardPassiveInfo> rewardPassiveInfoList4 = new List<RewardPassiveInfo>();
            if (rewards == null || rewards.Count == 0)
                return (RewardPassiveInfo)null;
            foreach (RewardPassiveInfo reward in rewards)
            {
                switch (reward.passiverarity)
                {
                    case Rarity.Common:
                        rewardPassiveInfoList1.Add(reward);
                        break;
                    case Rarity.Uncommon:
                        rewardPassiveInfoList2.Add(reward);
                        break;
                    case Rarity.Rare:
                        rewardPassiveInfoList3.Add(reward);
                        break;
                    case Rarity.Unique:
                        rewardPassiveInfoList4.Add(reward);
                        break;
                }
            }
            List<RewardPassiveInfo> rewardPassiveInfoList5 = (List<RewardPassiveInfo>)null;
            List<Rarity> rarityList = new List<Rarity>();
            if (rewardPassiveInfoList1.Count > 0)
            {
                for (int index = 0; index < 50; ++index)
                    rarityList.Add(Rarity.Common);
            }
            if (rewardPassiveInfoList2.Count > 0)
            {
                for (int index = 0; index < 30; ++index)
                    rarityList.Add(Rarity.Uncommon);
            }
            if (rewardPassiveInfoList3.Count > 0)
            {
                for (int index = 0; index < 13; ++index)
                    rarityList.Add(Rarity.Rare);
            }
            if (rewardPassiveInfoList4.Count > 0)
            {
                for (int index = 0; index < 7; ++index)
                    rarityList.Add(Rarity.Unique);
            }
            Rarity rarity = rarityList[UnityEngine.Random.Range(0, rarityList.Count)];
            if (rarity == Rarity.Common)
                rewardPassiveInfoList5 = rewardPassiveInfoList1;
            if (rarity == Rarity.Uncommon)
                rewardPassiveInfoList5 = rewardPassiveInfoList2;
            if (rarity == Rarity.Rare)
                rewardPassiveInfoList5 = rewardPassiveInfoList3;
            if (rarity == Rarity.Unique)
                rewardPassiveInfoList5 = rewardPassiveInfoList4;
            if (rewardPassiveInfoList5 == null || rewardPassiveInfoList5.Count == 0)
                return (RewardPassiveInfo)null;
            RewardPassiveInfo info = rewardPassiveInfoList5[UnityEngine.Random.Range(0, rewardPassiveInfoList5.Count)];
            if (info.rewardtype == RewardType.EquipPage)
            {
                RewardingModel.CreateEquipRewardXmlData(info);
                LogLikeMod.GetRegisteredPickUpXml(info).TargetType = EmotionTargetType.All;
            }
            else if (info.script != string.Empty)
            {
                EmotionCardXmlInfo registeredPickUpXml = LogLikeMod.GetRegisteredPickUpXml(info);
                AbnormalityCard abnormalityCard = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(registeredPickUpXml?.Name ?? info.script);
                PickUpModelBase pickUp = LogLikeMod.FindPickUp(info.script);
                if (abnormalityCard != null && pickUp != null)
                {
                    abnormalityCard.cardName = pickUp.Name;
                    abnormalityCard.flavorText = pickUp.FlaverText;
                    abnormalityCard.abilityDesc = pickUp.Desc;
                }
            }
            return info;
        }

        public static DiceCardXmlInfo GetCard(CardDropValueXmlInfo info)
        {
            if (info == null)
            {
                Debug.Log("info is null");
                return (DiceCardXmlInfo)null;
            }
            CardDropTableXmlInfo data = Singleton<CardDropTableXmlList>.Instance.GetData(new LorId(info.workshopID, info.DropTableId));
            List<DiceCardXmlInfo> diceCardXmlInfoList1 = new List<DiceCardXmlInfo>();
            List<DiceCardXmlInfo> diceCardXmlInfoList2 = new List<DiceCardXmlInfo>();
            List<DiceCardXmlInfo> diceCardXmlInfoList3 = new List<DiceCardXmlInfo>();
            List<DiceCardXmlInfo> diceCardXmlInfoList4 = new List<DiceCardXmlInfo>();
            if (data == null || data.cardIdList.Count == 0)
                return (DiceCardXmlInfo)null;
            foreach (LorId cardId in data.cardIdList)
            {
                DiceCardXmlInfo cardItem = RewardingModel.GetCardItemOriginAware(cardId);
                if (cardItem == null)
                    cardItem.Log($"PickUpCardNull : {cardId.packageId} _ {cardId.id.ToString()}");
                else if (cardItem != null)
                {
                    switch (cardItem.Rarity)
                    {
                        case Rarity.Common:
                            diceCardXmlInfoList1.Add(cardItem);
                            break;
                        case Rarity.Uncommon:
                            diceCardXmlInfoList2.Add(cardItem);
                            break;
                        case Rarity.Rare:
                            diceCardXmlInfoList3.Add(cardItem);
                            break;
                        case Rarity.Unique:
                            diceCardXmlInfoList4.Add(cardItem);
                            break;
                    }
                }
            }
            List<DiceCardXmlInfo> diceCardXmlInfoList5 = (List<DiceCardXmlInfo>)null;
            List<Rarity> rarityList = new List<Rarity>();
            if (diceCardXmlInfoList1.Count > 0)
            {
                for (int index = 0; index < info.CommonValue; ++index)
                    rarityList.Add(Rarity.Common);
            }
            if (diceCardXmlInfoList2.Count > 0)
            {
                for (int index = 0; index < info.UncommonValue; ++index)
                    rarityList.Add(Rarity.Uncommon);
            }
            if (diceCardXmlInfoList3.Count > 0)
            {
                for (int index = 0; index < info.RareValue; ++index)
                    rarityList.Add(Rarity.Rare);
            }
            if (diceCardXmlInfoList4.Count > 0)
            {
                for (int index = 0; index < info.UniqueValue; ++index)
                    rarityList.Add(Rarity.Unique);
            }
            if (rarityList.Count == 0)
                return null;
            Rarity rarity = rarityList[UnityEngine.Random.Range(0, rarityList.Count)];
            if (rarity == Rarity.Common)
                diceCardXmlInfoList5 = diceCardXmlInfoList1;
            if (rarity == Rarity.Uncommon)
                diceCardXmlInfoList5 = diceCardXmlInfoList2;
            if (rarity == Rarity.Rare)
                diceCardXmlInfoList5 = diceCardXmlInfoList3;
            if (rarity == Rarity.Unique)
                diceCardXmlInfoList5 = diceCardXmlInfoList4;
            return diceCardXmlInfoList5 == null || diceCardXmlInfoList5.Count == 0 ? (DiceCardXmlInfo)null : diceCardXmlInfoList5[UnityEngine.Random.Range(0, diceCardXmlInfoList5.Count)];
        }

        /// <summary>
        /// Pick one card from a pre-filtered pool using the same weighted-rarity logic as <see cref="GetCard"/>.
        /// Returns null when the pool is empty or all candidates are excluded.
        /// </summary>
        public static DiceCardXmlInfo GetCardFromFilteredPool(
            List<DiceCardXmlInfo> pool,
            CardDropValueXmlInfo info,
            HashSet<LorId> excludeIds)
        {
            // Separate by rarity, excluding already-picked IDs
            var byRarity = new Dictionary<Rarity, List<DiceCardXmlInfo>>();
            foreach (var card in pool)
            {
                if (excludeIds.Contains(card.id))
                    continue;
                if (!byRarity.ContainsKey(card.Rarity))
                    byRarity[card.Rarity] = new List<DiceCardXmlInfo>();
                byRarity[card.Rarity].Add(card);
            }

            // Build weighted rarity list (same weights as GetCard)
            var rarityList = new List<Rarity>();
            if (byRarity.ContainsKey(Rarity.Common))
                for (int i = 0; i < info.CommonValue; i++) rarityList.Add(Rarity.Common);
            if (byRarity.ContainsKey(Rarity.Uncommon))
                for (int i = 0; i < info.UncommonValue; i++) rarityList.Add(Rarity.Uncommon);
            if (byRarity.ContainsKey(Rarity.Rare))
                for (int i = 0; i < info.RareValue; i++) rarityList.Add(Rarity.Rare);
            if (byRarity.ContainsKey(Rarity.Unique))
                for (int i = 0; i < info.UniqueValue; i++) rarityList.Add(Rarity.Unique);

            if (rarityList.Count == 0)
                return null;

            Rarity rarity = rarityList[UnityEngine.Random.Range(0, rarityList.Count)];
            if (!byRarity.ContainsKey(rarity) || byRarity[rarity].Count == 0)
                return null;

            var rarityPool = byRarity[rarity];
            return rarityPool[UnityEngine.Random.Range(0, rarityPool.Count)];
        }

        public static List<DiceCardXmlInfo> PickUpCards(CardDropValueXmlInfo info)
        {
            List<DiceCardXmlInfo> result = new List<DiceCardXmlInfo>();
            if (info == null)
            {
                Debug.Log("[PickUpCards] Drop value is null");
                return result;
            }

            // Get the full drop table and build the unowned card pool
            CardDropTableXmlInfo dropTable = Singleton<CardDropTableXmlList>.Instance.GetData(
                new LorId(info.workshopID, info.DropTableId));

            if (dropTable == null || dropTable.cardIdList.Count == 0)
            {
                Debug.Log("[PickUpCards] Drop table is empty");
                return result;
            }

            var allCards = new List<DiceCardXmlInfo>();
            foreach (LorId cardId in dropTable.cardIdList)
            {
                DiceCardXmlInfo cardItem = RewardingModel.GetCardItemOriginAware(cardId);
                if (cardItem != null)
                    allCards.Add(cardItem);
            }

            int totalInPool = allCards.Count;

            // Filter: exclude already-owned cards
            var unowned = allCards.Where(c => !LogueBookModels.HasOwnedCombatPage(c.id)).ToList();
            int ownedCount = totalInPool - unowned.Count;

            // Filter: exclude cards that are already upgraded versions
            // (upgraded cards should only come from rest upgrades, not from reward pools)
            int upgradeCount = unowned.RemoveAll(c =>
                c.id.packageId != null && c.id.packageId.Contains(LogCardUpgradeManager.UpgradeKeyword));
            int filteredUpgrade = upgradeCount;

            Debug.Log($"[PickUpCards] Pool total:{totalInPool} owned:{ownedCount} upgradeFiltered:{filteredUpgrade} available:{unowned.Count}");

            // Build the 3-choice list, deduplicating within the selection
            var pickedIds = new HashSet<LorId>();
            int targetCount = Math.Min(3, unowned.Count);

            for (int i = 0; i < targetCount; i++)
            {
                DiceCardXmlInfo card = GetCardFromFilteredPool(unowned, info, pickedIds);
                if (card != null)
                {
                    pickedIds.Add(card.id);
                    result.Add(card);
                    Debug.Log($"[PickUpCards] #{i}: {card.id.packageId}:{card.id.id} ({card.Name})");
                }
            }

            if (result.Count < 3)
                Debug.Log($"[PickUpCards] Only {result.Count}/3 cards available (all owned or pool exhausted)");

            Singleton<GlobalLogueEffectManager>.Instance.ChangeCardReward(ref result);

            // Second filter: after global effects may have injected upgrades or already-owned cards,
            // remove owned cards, upgrade versions, and duplicates by normalized originalId.
            var seenKeys = new HashSet<string>();
            for (int i = result.Count - 1; i >= 0; i--)
            {
                var card = result[i];
                string key = LogueBookModels.NormalizeCardKey(card.id);

                // Remove if already owned (global effect may have swapped in an owned card)
                if (LogueBookModels.HasOwnedCombatPage(card.id))
                {
                    Debug.Log($"[PickUpCards] Post-filter removed owned: {card.id.packageId}:{card.id.id}");
                    result.RemoveAt(i);
                    continue;
                }

                // Remove upgrade versions (upgrades only via rest, not reward pools)
                if (card.id.packageId != null && card.id.packageId.Contains(LogCardUpgradeManager.UpgradeKeyword))
                {
                    Debug.Log($"[PickUpCards] Post-filter removed upgrade: {card.id.packageId}:{card.id.id}");
                    result.RemoveAt(i);
                    continue;
                }

                // Deduplicate by normalized originalId (within the 3-choice set)
                if (!seenKeys.Add(key))
                {
                    Debug.Log($"[PickUpCards] Post-filter removed duplicate originalId: {card.id.packageId}:{card.id.id}");
                    result.RemoveAt(i);
                }
            }

            Debug.Log($"[PickUpCards] After post-filter: {result.Count} cards remain");
            return result;
        }

        public static bool RewardInStage()
        {
            Singleton<GlobalLogueEffectManager>.Instance.RewardInStageInterrupt();
            if (SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.IsEnabled)
                return true;
            if (LogLikeMod.rewards_InStage.Count == 0)
                return false;
            if (BattleObjectManager.instance.GetAliveListWithAvailable(Faction.Player).Count == 0)
            {
                SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.SetRootCanvas(false);
                return false;
            }
            SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.SetRootCanvas(true);
            RewardingModel.StartPickReward_InStage();
            return true;
        }

        public static void StartPickReward_InStage()
        {
            while (LogLikeMod.rewards_InStage.Count > 0)
            {
                List<EmotionCardXmlInfo> passiveRewardsInlist = LogueBookModels.GetPassiveRewards_Inlist(LogLikeMod.rewards_InStage[0].rewards);
                if (passiveRewardsInlist == null || passiveRewardsInlist.Count == 0)
                {
                    Debug.Log("[StartPickReward_InStage] Empty reward, skipping to next");
                    LogLikeMod.rewards_InStage.RemoveAt(0);
                    continue;
                }
                RewardingModel.rewardFlag = RewardingModel.RewardFlag.RewardInStage;
                SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.Init(1, passiveRewardsInlist);
                return;
            }
        }

        public static void StartPickReward()
        {
            var list = new List<RewardInfo>(LogLikeMod.rewards_passive); // NULL SAFETY CHECKING
            foreach (var passive in list)
            {
                if (passive.rewards == null)
                    LogLikeMod.rewards_passive.Remove(passive);
            }
            EnsureBossBattleCardReward();
            NormalizeDropBookRewards();
            if (HasQueuedEgoSelections())
            {
                List<EmotionEgoXmlInfo> egoRewards = GetQueuedEgoRewards();
                if (egoRewards == null || egoRewards.Count == 0)
                {
                    LogLikeMod.egoSelectionQueue.RemoveAt(0);
                    StartPickReward();
                    return;
                }
                RewardingModel.rewardFlag = RewardingModel.RewardFlag.EgoCardReward;
                SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.InitEgo(1, egoRewards);
            }
            else if (LogLikeMod.rewardsMystery.Count > 0)
            {
                LorId mysteryid = LogLikeMod.rewardsMystery[0];
                LogLikeMod.rewardsMystery.RemoveAt(0);
                Singleton<MysteryManager>.Instance.StartMystery(mysteryid);
                SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.SetRootCanvas(false);
            }
            else
            {
                if (LogLikeMod.rewards.Count == 0 && LogLikeMod.rewards_passive.Count == 0 && LogLikeMod.nextlist.Count == 0 && !HasQueuedEgoSelections() && !HasQueuedMysteryRewards())
                    return;
                if (LogLikeMod.rewards.Count > 0)
                {
                    Singleton<MysteryManager>.Instance.StartMystery(new LorId(LogLikeMod.ModId, -4));
                    // start Combat Page card reward event (MysteryModel_CardReward)
                    SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.SetRootCanvas(false);
                }
                else if (LogLikeMod.rewards_passive.Count > 0)
                {
                    List<EmotionCardXmlInfo> passiveRewardsInlist = LogueBookModels.GetPassiveRewards_Inlist(LogLikeMod.rewards_passive.FindAll(x => x !=null)[0].rewards);
                    if (passiveRewardsInlist == null || passiveRewardsInlist.Count == 0)
                    {
                        Debug.Log("[StartPickReward] Empty passive reward, skipping to next");
                        LogLikeMod.rewards_passive.RemoveAt(0);
                        StartPickReward();
                        return;
                    }
                    RewardingModel.rewardFlag = RewardingModel.RewardFlag.PassiveReward;
                    SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.Init(1, passiveRewardsInlist);
                }
                else
                {
                    if (LogLikeMod.nextlist.Count <= 0)
                        return;
                    List<EmotionCardXmlInfo> nextlist = LogLikeMod.nextlist;
                    RewardingModel.rewardFlag = RewardingModel.RewardFlag.NextStageChoose;
                    SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.Init(1, nextlist);
                }
            }
        }

        public static void CompleteInterruptReward()
        {
            MysteryBase mystery = Singleton<MysteryManager>.Instance.curMystery;
            if (mystery != null && mystery.GetType().Name.IndexOf("Reward", StringComparison.OrdinalIgnoreCase) >= 0)
                Singleton<MysteryManager>.Instance.EndMystery(mystery);
            StartPickReward();
        }

        private static void NormalizeDropBookRewards()
        {
            if (LogLikeMod.rewards == null || LogLikeMod.rewards.Count == 0)
                return;
            foreach (var group in LogLikeMod.rewards.FindAll(reward => reward != null && reward.id != null)
                         .GroupBy(reward => reward.id).ToList())
            {
                if (NormalizedDropBookRewardIds.Contains(group.Key))
                    continue;
                NormalizedDropBookRewardIds.Add(group.Key);
                int count = group.Count();
                if (count <= 1)
                    continue;
                int keepCount = Math.Max(1, (int)Math.Ceiling(count * GetBattleCardRewardRetentionRate()));
                keepCount = Math.Min(keepCount, GetDropBookRewardSelectionCap());
                int removeCount = count - keepCount;
                for (int i = LogLikeMod.rewards.Count - 1; i >= 0 && removeCount > 0; i--)
                {
                    DropBookXmlInfo reward = LogLikeMod.rewards[i];
                    if (reward == null || reward.id != group.Key)
                        continue;
                    LogLikeMod.rewards.RemoveAt(i);
                    removeCount--;
                }
            }
        }

        private static double GetBattleCardRewardRetentionRate()
        {
            return LogLikeMod.curstagetype == StageType.Normal
                ? NormalBattleCardRewardRetentionRate
                : BattleCardRewardRetentionRate;
        }

        private static int GetDropBookRewardSelectionCap()
        {
            if (LogLikeMod.curstagetype != StageType.Normal)
                return int.MaxValue;
            if (LogLikeMod.curchaptergrade >= ChapterGrade.Grade6)
                return 2;
            if (LogLikeMod.curchaptergrade >= ChapterGrade.Grade4)
                return 3;
            return int.MaxValue;
        }

        private static void EnsureBossBattleCardReward()
        {
            if (LogLikeMod.curstagetype != StageType.Boss)
                return;
            if (BossFallbackRewardCheckedThisBattle)
                return;
            if (LogLikeMod.rewards == null)
                LogLikeMod.rewards = new List<DropBookXmlInfo>();
            if (LogLikeMod.rewards.FindAll(reward => reward != null).Count > 0)
            {
                BossFallbackRewardCheckedThisBattle = true;
                return;
            }
            BossFallbackRewardCheckedThisBattle = true;
            int chapterNumber = (int)LogLikeMod.curchaptergrade + 1;
            DropBookXmlInfo fallback = Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, chapterNumber * 1000 + 4));
            if (fallback != null)
                LogLikeMod.rewards.Add(fallback);
        }

        public static bool HasQueuedEgoSelections()
        {
            return LogLikeMod.egoSelectionQueue != null
                && LogLikeMod.egoSelectionQueue.Any(choice => choice != null && choice.Count > 0);
        }

        public static bool HasQueuedMysteryRewards()
        {
            return LogLikeMod.rewardsMystery != null && LogLikeMod.rewardsMystery.Count > 0;
        }

        private static List<EmotionEgoXmlInfo> GetQueuedEgoRewards()
        {
            if (!HasQueuedEgoSelections())
                return new List<EmotionEgoXmlInfo>();
            List<LorId> ids = LogLikeMod.egoSelectionQueue[0];
            List<EmotionEgoXmlInfo> result = new List<EmotionEgoXmlInfo>();
            foreach (LorId id in ids)
            {
                if (RMRAbnormalityUnlockManager.IsEgoUnlockedForCurrentRoute(id) || LogueBookModels.IsAtlasEgoPageUnlocked(id))
                    continue;
                DiceCardXmlInfo card = ItemXmlDataList.instance.GetCardItem(id, true);
                if (card == null)
                    continue;
                EmotionEgoXmlInfo ego = LogLikeMod.AddEmotionEgoForReward(card);
                if (ego != null)
                    result.Add(ego);
            }
            return result;
        }

        public static void PickEmotion(List<EmotionCardXmlInfo> emotions)
        {
            RewardingModel.rewardFlag = RewardingModel.RewardFlag.EmtoionChoose;
            SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.Init(emotions.Count, emotions);
        }

        public static bool RewardClearStage(StageController __instance)
        {
            if (LogLikeMod.purpleexcept)
                return true;
            Singleton<GlobalLogueEffectManager>.Instance.RewardClearStageInterrupt();
            if (Singleton<MysteryManager>.Instance.curMystery != null || SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.IsEnabled)
                return false;
            EnsureBossBattleCardReward();
            if (LogLikeMod.rewards.FindAll(x => x != null).Count == 0 && LogLikeMod.rewards_passive.FindAll(x => x != null).Count == 0 && LogLikeMod.nextlist.FindAll(x => x != null).Count == 0 && !HasQueuedEgoSelections() && !HasQueuedMysteryRewards())
                return true;
            if (BattleObjectManager.instance.GetAliveListWithAvailable(Faction.Player).Count == 0)
            {
                SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.SetRootCanvas(false);
                return true;
            }
            if (Singleton<MysteryManager>.Instance.curMystery == null)
                SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.SetRootCanvas(true);
            if (RogueLike_Mod_Reborn.RMRCore.provideAdditionalLogging)
            {
                Debug.Log("REWARDS");
                foreach (var reward in LogLikeMod.rewards)
                {
                    if (reward == null)
                        Debug.Log("NULL REWARD!!");
                    else
                        Debug.Log(reward.id.packageId + " --- " + reward.id.id.ToString());
                }
                Debug.Log("REWARDS PASSIVE");
                for (int i = 0; i < LogLikeMod.rewards_passive.Count; i++)
                {
                    if (LogLikeMod.rewards_passive[i].rewards == null)
                        Debug.Log("NULL REWARD LIST!!");
                    else
                    {
                        Debug.Log($"REWARDS PASSIVE LIST {i}");
                        foreach (var reward in LogLikeMod.rewards_passive[i].rewards)
                        {
                            if (reward == null)
                                Debug.Log("NULL REWARD LIST!!");
                            else
                                Debug.Log(reward.id.packageId + " --- " + reward.id.id.ToString());
                        }
                    }
                }
                Debug.Log("NEXTLIST");
                foreach (var reward in LogLikeMod.nextlist)
                {
                    if (reward == null)
                        Debug.Log("NULL REWARD!!");
                    else
                        Debug.Log(reward.Name + " --- " + reward.id.ToString());
                }
            }
            RewardingModel.StartPickReward();
            return false;
        }

        public static List<EmotionCardXmlInfo> GetCurEmotion()
        {
            List<EmotionCardXmlInfo> infos = new List<EmotionCardXmlInfo>();
            int num1 = LogLikeMod.curemotion + 1;
            int num2 = 0;
            int num3 = 0;
            foreach (UnitBattleDataModel unitBattleDataModel in LogueBookModels.playerBattleModel)
            {
                if (!unitBattleDataModel.isDead)
                {
                    ++num2;
                    num3 += unitBattleDataModel.emotionDetail.EmotionLevel;
                }
            }
            if (num2 == 0 || num3 / num2 < num1)
                return null;
            ++LogLikeMod.curemotion;
            if (LogueBookModels.EmotionCardList == null || LogueBookModels.EmotionCardList.Count == 0)
            {
                EmotionCardXmlInfo fallback = RMRAbnormalityUnlockManager.GetNoAbnormalityFallback(LogLikeMod.curemotion);
                if (fallback != null)
                    infos.Add(fallback);
                return infos;
            }
            int level = LogLikeMod.curemotion;
            List<RewardPassiveInfo> rewardPassiveInfoList = new List<RewardPassiveInfo>(LogueBookModels.EmotionCardList);
            HashSet<string> selectedEmotionIds = new HashSet<string>();
            foreach (RewardPassiveInfo selected in LogueBookModels.selectedEmotion)
            {
                if (selected != null)
                    selectedEmotionIds.Add(RewardingModel.GetRewardPassiveKey(selected));
            }
            rewardPassiveInfoList.RemoveAll(x => x.level != level || selectedEmotionIds.Contains(RewardingModel.GetRewardPassiveKey(x)));
            HashSet<string> emotionCardKeys = new HashSet<string>();
            foreach (RewardPassiveInfo info in rewardPassiveInfoList)
            {
                EmotionCardXmlInfo card = LogLikeMod.GetRegisteredPickUpXml(info);
                if (card == null)
                    continue;
                string key = RewardingModel.GetEmotionCardKey(card);
                if (emotionCardKeys.Add(key))
                    infos.Add(card);
            }
            while (true)
            {
                if (infos.Count > 3)
                {
                    int index = UnityEngine.Random.Range(0, infos.Count);
                    infos.RemoveAt(index);
                }
                else
                    break;
            }
            foreach (EmotionCardXmlInfo emotionCardXmlInfo in infos)
            {
                AbnormalityCard abnormalityCard = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(emotionCardXmlInfo.Name);
                PickUpModelBase pickUp = LogLikeMod.FindPickUp(emotionCardXmlInfo.Script[0]);
                if (abnormalityCard != null && pickUp != null)
                {
                    abnormalityCard.cardName = pickUp.Name;
                    abnormalityCard.flavorText = pickUp.FlaverText;
                    abnormalityCard.abilityDesc = pickUp.Desc;
                }
            }
            if (infos.Count == 0)
            {
                EmotionCardXmlInfo fallback = RMRAbnormalityUnlockManager.GetNoAbnormalityFallback(level);
                if (fallback != null)
                    infos.Add(fallback);
            }
            return infos;
        }

        public static bool EmotionChoice()
        {
            if (SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.IsEnabled)
                return false;
            List<EmotionCardXmlInfo> curEmotion = RewardingModel.GetCurEmotion();
            if (curEmotion == null || curEmotion.Count == 0)
                return true;
            if (BattleObjectManager.instance.GetAliveListWithAvailable(Faction.Player).Count == 0)
            {
                SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.SetRootCanvas(false);
                return true;
            }
            SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.SetRootCanvas(true);
            RewardingModel.PickEmotion(curEmotion);
            return false;
        }

        public enum RewardFlag
        {
            CardReward,
            PassiveReward,
            NextStageChoose,
            EmtoionChoose,
            RewardInStage,
            EgoCardReward,
        }

        private static string GetRewardPassiveKey(RewardPassiveInfo info)
        {
            if (info == null)
                return string.Empty;
            return info.id.packageId + ":" + info.id.id.ToString();
        }

        private static string GetEmotionCardKey(EmotionCardXmlInfo info)
        {
            if (info == null)
                return string.Empty;
            string script = string.Empty;
            if (info.Script != null && info.Script.Count > 0)
                script = info.Script[0];
            string packageId = LogLikeMod.GetPickUpXmlWorkShopId_Passive(info) ?? string.Empty;
            return packageId + ":" + info.id.ToString() + ":" + script;
        }
    }
}
