// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.CreaturePickUpModel
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;
using LOR_XML;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class CreaturePickUpModel : PickUpModelBase
    {
        public List<LorId> ids;
        public int level;

        public static List<EmotionCardXmlInfo> GetEmotionListById(List<LorId> ids)
        {
            List<EmotionCardXmlInfo> emotionListById = new List<EmotionCardXmlInfo>();
            foreach (LorId id in ids)
            {
                EmotionCardXmlInfo registeredPickUpXml = LogLikeMod.GetRegisteredPickUpXml(Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id));
                emotionListById.Add(registeredPickUpXml);
            }
            return emotionListById;
        }

        public static void ApplyEmotionCard(
          LorId id,
          EmotionCardAbilityBase cardability,
          BattleUnitModel targetunit,
          bool callOnceEvent = false)
        {
            CreaturePickUpModel.ApplyEmotionCard(LogLikeMod.GetRegisteredPickUpXml(Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id)), cardability, targetunit, callOnceEvent);
        }

        public static void ApplyEmotionCard(
          EmotionCardXmlInfo card,
          EmotionCardAbilityBase cardability,
          BattleUnitModel targetunit,
          bool callOnceEvent = false)
        {
            BattleUnitEmotionDetail emotionDetail = targetunit.emotionDetail;
            BattleEmotionCardModel emotionCard = new BattleEmotionCardModel(card, targetunit);
            emotionCard.AbilityList.Add(cardability);
            cardability.SetEmotionCard(emotionCard);
            emotionDetail.PassiveList.Add(emotionCard);
            if (callOnceEvent)
                emotionCard.OnSelectOnce();
            emotionCard.OnSelect();
            StatBonus statBonus1 = emotionCard.GetStatBonus();
            targetunit.SetHp(Mathf.Max(1, (int)((double)targetunit.hp * (double)(100 + statBonus1.hpRate) / 100.0)));
            targetunit.breakDetail.breakGauge = Mathf.Max(1, (int)((double)(targetunit.breakDetail.breakGauge * (100 + statBonus1.breakRate)) / 100.0));
            StatBonus statBonus2 = new StatBonus();
            foreach (BattleEmotionCardModel passive in emotionDetail.PassiveList)
                statBonus2.AddStatBonus(passive.GetStatBonus());
            LogLikeMod.SetFieldValue(emotionDetail, "_statBonus", statBonus2);
            try
            {
                SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.GetProfileUI(targetunit);
                targetunit.view.dialogUI.TurnOnAbnormalityDlg(card);
                targetunit.view.abCardSelector.DisplaySelectionDialog(card);
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException)
                    return;
                Debug.LogError(ex);
            }
        }

        public static void LoadGetAbnomalityPanel(List<EmotionCardXmlInfo> cards, int level, string name)
        {
            foreach (EmotionCardXmlInfo card in cards)
            {
                AbnormalityCard abnormalityCard = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(card.Script[0]);
                PickUpModelBase pickUp = LogLikeMod.FindPickUp(card.Script[0]);
                if (pickUp != null)
                {
                    abnormalityCard.cardName = pickUp.Name;
                    abnormalityCard.flavorText = pickUp.FlaverText;
                    abnormalityCard.abilityDesc = pickUp.Desc;
                }
            }
            UIGetAbnormalityPanel instance = UIGetAbnormalityPanel.instance;
            FieldInfo field1 = ModdingUtils.GetField("currentFloor", instance);
            GameObject fieldValue1 = ModdingUtils.GetFieldValue<GameObject>("ob_blackbgForKeterCompleterOpen", instance);
            FieldInfo field2 = ModdingUtils.GetField("currentSettinfCardCount", instance);
            FieldInfo field3 = ModdingUtils.GetField("sep", instance);
            FieldInfo field4 = ModdingUtils.GetField("isbinahhokmacompletecheck", instance);
            Image fieldValue2 = ModdingUtils.GetFieldValue<Image>("img_floorIcon", instance);
            TextMeshProUGUI fieldValue3 = ModdingUtils.GetFieldValue<TextMeshProUGUI>("txt_floorname", instance);
            TextMeshProUGUI fieldValue4 = ModdingUtils.GetFieldValue<TextMeshProUGUI>("txt_level", instance);
            GameObject fieldValue5 = ModdingUtils.GetFieldValue<GameObject>("controllerGuide", instance);
            GameObject fieldValue6 = ModdingUtils.GetFieldValue<GameObject>("AbnormalitiesRoot", instance);
            GameObject fieldValue7 = ModdingUtils.GetFieldValue<GameObject>("EgoCardsRoot", instance);
            TextMeshProUGUI fieldValue8 = ModdingUtils.GetFieldValue<TextMeshProUGUI>("txt_getabcardtxt", instance);
            TextMeshProUGUI fieldValue9 = ModdingUtils.GetFieldValue<TextMeshProUGUI>("txt_getegocardtxt", instance);
            List<UIEmotionPassiveCardInven> fieldValue10 = ModdingUtils.GetFieldValue<List<UIEmotionPassiveCardInven>>("AbnormalityList", instance);
            Animator fieldValue11 = ModdingUtils.GetFieldValue<Animator>("anim", instance);
            field1.SetValue(instance, null);
            fieldValue1.SetActive(false);
            field2.SetValue(instance, cards.Count);
            instance.Open();
            field3.SetValue(instance, SephirahType.None);
            field4.SetValue(instance, false);
            fieldValue2.sprite = UISpriteDataManager.instance.GetStoryIcon("Chapter1").icon;
            fieldValue3.text = name;
            switch (level)
            {
                case 1:
                    fieldValue4.text = "I";
                    break;
                case 2:
                    fieldValue4.text = "II";
                    break;
                case 3:
                    fieldValue4.text = "III";
                    break;
                case 4:
                    fieldValue4.text = "IV";
                    break;
                case 5:
                    fieldValue4.text = "V";
                    break;
                case 6:
                    fieldValue4.text = "VI";
                    break;
                default:
                    fieldValue4.text = "X";
                    break;
            }
            instance.SetColor(UIColorManager.Manager.GetSephirahGlowColor(SephirahType.None));
            fieldValue5.SetActive(false);
            fieldValue6.SetActive(true);
            fieldValue7.SetActive(false);
            fieldValue8.gameObject.SetActive(true);
            fieldValue9.gameObject.SetActive(false);
            fieldValue8.text = "";
            instance.selectablePanel.ChildSelectable = instance.abpanelSelectable;
            UIControlManager.Instance.SelectSelectableForcely(instance.selectablePanel.ChildSelectable);
            List<EmotionCardXmlInfo> emotionCardXmlInfoList = cards;
            for (int index = 0; index < emotionCardXmlInfoList.Count; ++index)
            {
                if (index > fieldValue10.Count)
                    return;
                fieldValue10[index].Init(emotionCardXmlInfoList[index]);
            }
            foreach (UIEmotionPassiveCardInven passiveCardInven in fieldValue10)
                passiveCardInven.SetActiveDetail(false);
            instance.GetType().GetMethod("SetDefault", AccessTools.all).Invoke(instance, (object[])null);
            fieldValue11.SetTrigger("Reveal");
        }

        public virtual string GetCreatureName() => "";

        public virtual List<EmotionCardXmlInfo> GetCreatureList() => (List<EmotionCardXmlInfo>)null;

        public virtual Sprite GetSprite() => (Sprite)null;
    }
}
