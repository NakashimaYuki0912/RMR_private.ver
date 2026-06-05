// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.RewardingModel
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using LOR_XML;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class RewardingModel
    {
        public static RewardingModel.RewardFlag rewardFlag;

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
                empty += Singleton<PassiveDescXmlList>.Instance.GetName(bookinfo.EquipEffect.PassiveList[index]);
                if (index < bookinfo.EquipEffect.PassiveList.Count - 1)
                    empty += ", ";
            }
            return $"{str}{abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("Equip_Passive")}: {empty}";
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
            BookXmlInfo data = Singleton<BookXmlList>.Instance.GetData(info.id);
            AbnormalityCard abnormalityCard = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(LogLikeMod.GetRegisteredPickUpXml(info).Name);
            abnormalityCard.cardName = data.InnerName;
            abnormalityCard.flavorText = $"{RewardingModel.GetChapterText(data.Chapter)}, {RewardingModel.GetRaritytext(data.Rarity)}";
            abnormalityCard.abilityDesc = RewardingModel.GetAblilityText(data);
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
                AbnormalityCard abnormalityCard = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(info.script);
                PickUpModelBase pickUp = LogLikeMod.FindPickUp(info.script);
                abnormalityCard.cardName = pickUp.Name;
                abnormalityCard.flavorText = pickUp.FlaverText;
                abnormalityCard.abilityDesc = pickUp.Desc;
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
                DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(cardId, true);
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

        public static List<DiceCardXmlInfo> PickUpCards(CardDropValueXmlInfo info)
        {
            List<DiceCardXmlInfo> cardlist = new List<DiceCardXmlInfo>();
            do
            {
                DiceCardXmlInfo card;
                do
                {
                    card = RewardingModel.GetCard(info);
                }
                while (card == null);
                if (!cardlist.Contains(card))
                    cardlist.Add(card);
            }
            while (cardlist.Count != 3 && cardlist.Count != Singleton<CardDropTableXmlList>.Instance.GetData(new LorId(info.workshopID, info.DropTableId)).cardIdList.Count);
            Singleton<GlobalLogueEffectManager>.Instance.ChangeCardReward(ref cardlist);
            return cardlist;
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
            if (LogLikeMod.rewards_InStage.Count == 0 || LogLikeMod.rewards_InStage.Count <= 0)
                return;
            List<EmotionCardXmlInfo> passiveRewardsInlist = LogueBookModels.GetPassiveRewards_Inlist(LogLikeMod.rewards_InStage[0].rewards);
            RewardingModel.rewardFlag = RewardingModel.RewardFlag.RewardInStage;
            SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.Init(1, passiveRewardsInlist);
        }

        public static void StartPickReward()
        {
            var list = new List<RewardInfo>(LogLikeMod.rewards_passive); // NULL SAFETY CHECKING
            foreach (var passive in list)
            {
                if (passive.rewards == null)
                    LogLikeMod.rewards_passive.Remove(passive);
            }
            if (LogLikeMod.rewardsMystery.Count > 0)
            {
                LorId mysteryid = LogLikeMod.rewardsMystery[0];
                LogLikeMod.rewardsMystery.RemoveAt(0);
                Singleton<MysteryManager>.Instance.StartMystery(mysteryid);
                SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.SetRootCanvas(false);
            }
            else
            {
                if (LogLikeMod.rewards.Count == 0 && LogLikeMod.rewards_passive.Count == 0 && LogLikeMod.nextlist.Count == 0)
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
            if (LogLikeMod.rewards.FindAll(x => x != null).Count == 0 && LogLikeMod.rewards_passive.FindAll(x => x != null).Count == 0 && LogLikeMod.nextlist.FindAll(x => x != null).Count == 0)
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
                return null;
            int level = LogLikeMod.curemotion;
            List<RewardPassiveInfo> rewardPassiveInfoList = new List<RewardPassiveInfo>(LogueBookModels.EmotionCardList);
            rewardPassiveInfoList.RemoveAll(x => (LogLikeMod.FindPickUp(x.script) as CreaturePickUpModel).level != level || LogueBookModels.selectedEmotion.Contains(x));
            foreach (RewardPassiveInfo info in rewardPassiveInfoList)
                infos.Add(LogLikeMod.GetRegisteredPickUpXml(info));
            while (true)
            {
                EmotionCardXmlInfo emotionCardXmlInfo = infos.Find(x => infos.FindAll(y => x == y).Count > 1);
                if (emotionCardXmlInfo != null)
                    infos.Remove(emotionCardXmlInfo);
                else
                    break;
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
                AbnormalityCard abnormalityCard = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(emotionCardXmlInfo.Script[0]);
                PickUpModelBase pickUp = LogLikeMod.FindPickUp(emotionCardXmlInfo.Script[0]);
                if (pickUp != null)
                {
                    abnormalityCard.cardName = pickUp.Name;
                    abnormalityCard.flavorText = pickUp.FlaverText;
                    abnormalityCard.abilityDesc = pickUp.Desc;
                }
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
        }
    }
}
