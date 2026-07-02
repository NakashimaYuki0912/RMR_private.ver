// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.CraftEffect
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using RogueLike_Mod_Reborn;
using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class CraftEffect : GlobalLogueEffectBase
    {
        public virtual bool IsNormal() => true;

        public virtual string GetCraftName() => "Test";

        public virtual string GetCraftDesc() => "Test";

        public virtual Sprite GetCraftSprite() => (Sprite)null;

        public virtual int GetCraftCost() => 0;

        public virtual bool CanCraft(int costresult) => LogueBookModels.GetMoney() >= costresult;

        public virtual void Crafting()
        {
            LogueBookModels.SubMoney((int)((double)this.GetCraftCost() * (double)Singleton<GlobalLogueEffectManager>.Instance.CraftCostMultiple(this)));
            UIBattleSettingPanel uiPanel = UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel;
            UnitDataModel fieldValue = LogLikeMod.GetFieldValue<UnitDataModel>(uiPanel.InfoRightPanel, "unitdata");
            uiPanel.SetLibrarianProfileData(fieldValue);
        }

        public static List<RewardPassiveInfo> CheckCreaftEquipLimit(ChapterGrade grade)
        {
            List<RewardPassiveInfo> chapterData = Singleton<RewardPassivesList>.Instance.GetChapterData(grade, PassiveRewardListType.CommonReward, LorId.None, true);
            foreach (RewardPassiveInfo rewardPassiveInfo in chapterData.ToArray())
            {
                RewardPassiveInfo rinfo = rewardPassiveInfo;
                if (LogueBookModels.booklist.FindAll(x => x.ClassInfo.id == rinfo.id).Count >= 5)
                    chapterData.Remove(rinfo);
            }
            return chapterData.Count == 0 ? (List<RewardPassiveInfo>)null : chapterData;
        }

        public static void CraftEquipByChapter(ChapterGrade grade)
        {
            RewardPassiveInfo reward = RewardingModel.GetReward(CraftEffect.CheckCreaftEquipLimit(grade));
            BookXmlInfo data = Singleton<BookXmlList>.Instance.GetData(reward.id);
            LogueBookModels.AddBook(reward.id);
            UIAlarmPopup.instance.SetAlarmText(TextDataModel.GetText("CraftEquipResult", data.InnerName));
        }

        public static List<DiceCardXmlInfo> CanCraftExclusiveByChapter(ChapterGrade grade)
        {
            List<DiceCardXmlInfo> cardList = new List<DiceCardXmlInfo>();
            Dictionary<LorId, int> keypageCount = new Dictionary<LorId, int>();
            foreach (var keypage in LogueBookModels.booklist.FindAll(x => x.ClassInfo.Chapter == (((int)grade) + 1)))
            {
                var onlyCards = keypage.GetOnlyCards();
                if (onlyCards != null && onlyCards.Count > 0)
                    foreach (var card in keypage.GetOnlyCards())
                    {
                        var id = card.id.GetOriginalId();
                        if (keypageCount.TryGetValue(id, out _))
                            ++keypageCount[id];
                        else
                            keypageCount[id] = 1;
                        cardList.Add(card);
                    }
            }
            List<DiceCardXmlInfo> cardList2 = new List<DiceCardXmlInfo>();
            cardList2.AddRange(cardList);
            foreach (var card in cardList2)
            {
                keypageCount[card.id] *= card.Limit;
                int obtainCount = LogueBookModels.cardlist.Count(x => x.GetID().GetOriginalId() == card.id.GetOriginalId());
                Debug.Log("CARD ID " + card.id.packageId + " --- " + card.id.id.ToString() + " LIMIT: " + keypageCount[card.id].ToString());
                Debug.Log("CARD OBT COUNT: " + obtainCount.ToString());
                if (obtainCount >= keypageCount[card.id]) {
                    cardList.RemoveAll(x => x.id == card.id);
                    Debug.Log("CARD WAS REMOVED!!");
                }
            }
            return cardList.Count > 0 ? cardList : null;
        }

        public static void CraftExclusiveCardByChapter(ChapterGrade grade)
        {
            List<DiceCardXmlInfo> cards = CraftEffect.CanCraftExclusiveByChapter(grade);
            DiceCardXmlInfo card = cards.SelectOneRandom();
            LogueBookModels.AddCard(card.id);
            UIAlarmPopup.instance.SetAlarmText(TextDataModel.GetText("CraftEquipResult", card.Name));
        }
    }
}
