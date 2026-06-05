// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.ShopBase
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class ShopBase
    {
        public static Dictionary<int, Vector2[]> CardShape;
        public List<ShopGoods> Goods;
        public Dictionary<string, GameObject> FrameObj = new Dictionary<string, GameObject>();

        public ShopBase()
        {
            if (ShopBase.CardShape != null)
                return;
            ShopBase.CardShape = new Dictionary<int, Vector2[]>();
            Vector2[] vector2Array1 = new Vector2[3]
            {
      new Vector2(400f, 425f),
      new Vector2(0.0f, 425f),
      new Vector2(-400f, 425f)
            };
            ShopBase.CardShape.Add(3, vector2Array1);
            Vector2[] vector2Array2 = new Vector2[4]
            {
      new Vector2(450f, 425f),
      new Vector2(150f, 425f),
      new Vector2(-150f, 425f),
      new Vector2(-450f, 425f)
            };
            ShopBase.CardShape.Add(4, vector2Array2);
            Vector2[] vector2Array3 = new Vector2[5]
            {
      new Vector2(500f, 425f),
      new Vector2(250f, 425f),
      new Vector2(0.0f, 425f),
      new Vector2(-250f, 425f),
      new Vector2(-500f, 425f)
            };
            ShopBase.CardShape.Add(5, vector2Array3);
            Vector2[] vector2Array4 = new Vector2[6]
            {
              new Vector2(550f, 425f),
              new Vector2(330f, 425f),
              new Vector2(110f, 425f),
              new Vector2(-110f, 425f),
              new Vector2(-330f, 425f),
              new Vector2(-550f, 425f)
            };
            ShopBase.CardShape.Add(6, vector2Array4);
            Vector2[] vector2Array5 = new Vector2[7]
            {
      new Vector2(450f, 470f),
      new Vector2(150f, 470f),
      new Vector2(-150f, 470f),
      new Vector2(-450f, 470f),
      new Vector2(400f, 270f),
      new Vector2(0.0f, 270f),
      new Vector2(-400f, 270f)
            };
            ShopBase.CardShape.Add(7, vector2Array5);
            Vector2[] vector2Array6 = new Vector2[8]
            {
      new Vector2(450f, 470f),
      new Vector2(150f, 470f),
      new Vector2(-150f, 470f),
      new Vector2(-450f, 470f),
      new Vector2(450f, 270f),
      new Vector2(150f, 270f),
      new Vector2(-150f, 270f),
      new Vector2(-450f, 270f)
            };
            ShopBase.CardShape.Add(8, vector2Array6);
            Vector2[] vector2Array7 = new Vector2[9]
            {
      new Vector2(500f, 470f),
      new Vector2(250f, 470f),
      new Vector2(0.0f, 470f),
      new Vector2(-250f, 470f),
      new Vector2(-500f, 470f),
      new Vector2(450f, 270f),
      new Vector2(150f, 270f),
      new Vector2(-150f, 270f),
      new Vector2(-450f, 270f)
            };
            ShopBase.CardShape.Add(9, vector2Array7);
            Vector2[] vector2Array8 = new Vector2[10]
            {
      new Vector2(500f, 470f),
      new Vector2(250f, 470f),
      new Vector2(0.0f, 470f),
      new Vector2(-250f, 470f),
      new Vector2(-500f, 470f),
      new Vector2(500f, 270f),
      new Vector2(250f, 270f),
      new Vector2(0.0f, 270f),
      new Vector2(-250f, 270f),
      new Vector2(-500f, 270f)
            };
            ShopBase.CardShape.Add(10, vector2Array8);
        }

        public virtual SaveData GetSaveData()
        {
            SaveData saveData1 = new SaveData();
            SaveData data1 = new SaveData();
            int num = 0;
            while (true)
            {
                if (this.FrameObj.ContainsKey("Goods_Card" + num.ToString()))
                {
                    if (!this.FrameObj["Goods_Card" + num.ToString()].activeSelf)
                    {
                        ++num;
                    }
                    else
                    {
                        SaveData saveData2 = this.FrameObj["Goods_Card" + num.ToString()].GetComponent<ShopGoods>().GetSaveData();
                        saveData2.AddData("index", new SaveData(num));
                        data1.AddToList(saveData2);
                        ++num;
                    }
                }
                else
                    break;
            }
            saveData1.AddData("Cards", data1);
            SaveData data2 = new SaveData();
            num = 0;
            while (true)
            {
                if (this.FrameObj.ContainsKey("Goods_Passive" + num.ToString()))
                {
                    if (!this.FrameObj["Goods_Passive" + num.ToString()].activeSelf)
                    {
                        ++num;
                    }
                    else
                    {
                        SaveData saveData3 = this.FrameObj["Goods_Passive" + num.ToString()].GetComponent<ShopGoods>().GetSaveData();
                        saveData3.AddData("index", new SaveData(num));
                        data2.AddToList(saveData3);
                        ++num;
                    }
                }
                else
                    break;
            }
            saveData1.AddData("Passives", data2);
            return saveData1;
        }

        public virtual void LoadFromSaveData(SaveData data)
        {
            foreach (SaveData data1 in data.GetData("Cards"))
                this.Shop_CardCreating(ItemXmlDataList.instance.GetCardItem(ExtensionUtils.LogLoadFromSaveData(data1.GetData("Id")), true), 1, new Vector2(0.0f, 0.0f), data1.GetInt("index")).LoadFromSaveData(data1);
            foreach (SaveData data2 in data.GetData("Passives"))
                this.Shop_PassiveCreating(Singleton<RewardPassivesList>.Instance.GetPassiveInfo(ExtensionUtils.LogLoadFromSaveData(data2.GetData("Id"))), new Vector2(0.0f, 0.0f), data2.GetInt("index")).LoadFromSaveData(data2);
        }

        public static List<RewardPassiveInfo> GetPassiveInList(
          List<RewardPassiveInfo> passiveinfos,
          int count,
          ShopRewardType type)
        {
            List<RewardPassiveInfo> passiveInList = new List<RewardPassiveInfo>();
            do
            {
                RewardPassiveInfo passiveinfo = passiveinfos[UnityEngine.Random.Range(0, passiveinfos.Count)];
                using (List<BattleUnitModel>.Enumerator enumerator = BattleObjectManager.instance.GetList(Faction.Player).GetEnumerator())
                {
                    BattleUnitModel current;
                    ShopPickUpModel pickUp;
                    do
                    {
                        if (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            pickUp = LogLikeMod.FindPickUp(passiveinfo.script) as ShopPickUpModel;
                        }
                        else
                            goto label_7;
                    }
                    while (!pickUp.IsCanPickUp(current.UnitData.unitData) || pickUp.GetShopType() != type || !pickUp.IsCanAddShop());
                }
                if (!passiveInList.Contains(passiveinfo))
                    passiveInList.Add(passiveinfo);
                label_7:
                passiveinfos.Remove(passiveinfo);
            }
            while (passiveInList.Count != count && passiveinfos.Count != 0);
            return passiveInList;
        }

        public Vector2 GetShopShape_Card(int num, int id) => ShopBase.CardShape[num][id];

        public Vector2 GetShopShape_Passive(int num, int id)
        {
            return ShopBase.CardShape[num][id] - new Vector2(0.0f, 450f);
        }

        public static ShopBase FindShop(string script)
        {
            foreach (Assembly assem in LogLikeMod.GetAssemList())
            {
                foreach (System.Type type in assem.GetTypes())
                {
                    if (type.Name == "ShopModel_" + script.Trim())
                        return Activator.CreateInstance(type) as ShopBase;
                }
            }
            return new ShopBase();
        }

        public virtual void Init()
        {
            this.FrameObj = new Dictionary<string, GameObject>();
            this.CreateShop();
            LoguePlayDataSaver.SaveShop(this);
            Singleton<GlobalLogueEffectManager>.Instance.OnEnterShop(this);
        }

        public virtual void RemoveShop()
        {
            if (this.FrameObj == null || this.FrameObj.Count <= 0)
                return;
            foreach (UnityEngine.Object @object in this.FrameObj.Values)
                UnityEngine.Object.Destroy(@object);
            this.FrameObj.Clear();
        }

        public static int CompareCardRarity(DiceCardXmlInfo x, DiceCardXmlInfo y)
        {
            if (x.Rarity > y.Rarity)
                return 1;
            return x.Rarity == y.Rarity ? 0 : -1;
        }

        public ShopGoods_Card Shop_CardCreating(DiceCardXmlInfo card, int num, Vector2 position, int id)
        {
            GameObject gameObject = new GameObject("");
            gameObject.transform.SetParent(this.FrameObj["ShopFrame"].transform);
            gameObject.transform.localScale = new Vector3(1f, 1f);
            ShopGoods_Card shopGoodsCard = gameObject.AddComponent<ShopGoods_Card>();
            shopGoodsCard.gameObject.transform.localPosition = (Vector3)position;
            shopGoodsCard.SetShop(this);
            int goods_count = num;
            shopGoodsCard.SetGoods(card, goods_count);
            this.Goods.Add((ShopGoods)shopGoodsCard);
            this.FrameObj.Add("Goods_Card" + id.ToString(), shopGoodsCard.gameObject);
            return shopGoodsCard;
        }

        public virtual void CreateShop_Card(int num = 5)
        {
            Singleton<CardDropValueList>.Instance.GetData(new LorId(LogLikeMod.ModId, 11001));
            CardDropValueXmlInfo data;
            switch (LogLikeMod.curchaptergrade)
            {
                case ChapterGrade.Grade1:
                    data = Singleton<CardDropValueList>.Instance.GetData(new LorId(LogLikeMod.ModId, 11001));
                    break;
                case ChapterGrade.Grade2:
                    data = Singleton<CardDropValueList>.Instance.GetData(new LorId(LogLikeMod.ModId, 12001));
                    break;
                case ChapterGrade.Grade3:
                    data = Singleton<CardDropValueList>.Instance.GetData(new LorId(LogLikeMod.ModId, 13001));
                    break;
                case ChapterGrade.Grade4:
                    data = Singleton<CardDropValueList>.Instance.GetData(new LorId(LogLikeMod.ModId, 14001));
                    break;
                case ChapterGrade.Grade5:
                    data = Singleton<CardDropValueList>.Instance.GetData(new LorId(LogLikeMod.ModId, 15001));
                    break;
                case ChapterGrade.Grade6:
                    data = Singleton<CardDropValueList>.Instance.GetData(new LorId(LogLikeMod.ModId, 16001));
                    break;
                default:
                    data = Singleton<CardDropValueList>.Instance.GetData(new LorId(LogLikeMod.ModId, 16001));
                    break;
            }
            Singleton<GlobalLogueEffectManager>.Instance.ChangeShopCardList(this, ref data);
            List<DiceCardXmlInfo> diceCardXmlInfoList1 = new List<DiceCardXmlInfo>();
            do
            {
                List<DiceCardXmlInfo> diceCardXmlInfoList2 = new List<DiceCardXmlInfo>();
                DiceCardXmlInfo card = RewardingModel.GetCard(data);
                Singleton<GlobalLogueEffectManager>.Instance.ChangeShopCard(ref card);
                diceCardXmlInfoList2.Add(card);
                if (!diceCardXmlInfoList1.Contains(diceCardXmlInfoList2[0]))
                    diceCardXmlInfoList1.Add(diceCardXmlInfoList2[0]);
            }
            while (diceCardXmlInfoList1.Count != num && diceCardXmlInfoList1.Count != Singleton<CardDropTableXmlList>.Instance.GetData(new LorId(data.workshopID, data.DropTableId)).cardIdList.Count);
            diceCardXmlInfoList1.Sort(new Comparison<DiceCardXmlInfo>(ShopBase.CompareCardRarity));
            for (int index = 0; index < num; ++index)
                this.Shop_CardCreating(diceCardXmlInfoList1[index], this.ShopCardCount(diceCardXmlInfoList1[index].Rarity), this.GetShopShape_Card(num, index), index);
            Singleton<GlobalLogueEffectManager>.Instance.OnShopCardListCreate(this);
        }

        public ShopGoods_Passive Shop_PassiveCreating(
          RewardPassiveInfo passive,
          Vector2 position,
          int id)
        {
            GameObject gameObject = new GameObject("");
            gameObject.transform.SetParent(this.FrameObj["ShopFrame"].transform);
            gameObject.transform.localScale = new Vector3(1f, 1f);
            ShopGoods_Passive shopGoodsPassive = gameObject.AddComponent<ShopGoods_Passive>();
            shopGoodsPassive.gameObject.transform.localPosition = (Vector3)position;
            shopGoodsPassive.SetShop(this);
            RewardPassiveInfo goodinfo = passive;
            shopGoodsPassive.SetGoods(goodinfo);
            this.Goods.Add((ShopGoods)shopGoodsPassive);
            this.FrameObj.Add("Goods_Passive" + id.ToString(), shopGoodsPassive.gameObject);
            return shopGoodsPassive;
        }

        public int OnceCount(int num)
        {
            switch (num)
            {
                case 4:
                    return 2;
                case 5:
                    return 3;
                case 6:
                    return 4;
                case 7:
                    return 4;
                case 8:
                    return 5;
                case 9:
                    return 5;
                case 10:
                    return 6;
                default:
                    return num / 2;
            }
        }

        public virtual void CreateShop_passive(int num = 5)
        {
            List<RewardPassiveInfo> chapterData1 = Singleton<RewardPassivesList>.Instance.GetChapterData(ChapterGrade.GradeAll, PassiveRewardListType.Shop, new LorId(LogLikeMod.ModId, 90000));
            List<RewardPassiveInfo> rewardPassiveInfoList = new List<RewardPassiveInfo>();
            int count1 = this.OnceCount(num);
            int count2 = num - count1;
            rewardPassiveInfoList.AddRange(ShopBase.GetPassiveInList(chapterData1, count1, ShopRewardType.Once));
            List<RewardPassiveInfo> chapterData2 = Singleton<RewardPassivesList>.Instance.GetChapterData(ChapterGrade.GradeAll, PassiveRewardListType.Shop, new LorId(LogLikeMod.ModId, 90000));
            rewardPassiveInfoList.AddRange(ShopBase.GetPassiveInList(chapterData2, count2, ShopRewardType.Eternal));
            for (int index = 0; index < rewardPassiveInfoList.Count; ++index)
                this.Shop_PassiveCreating(rewardPassiveInfoList[index], this.GetShopShape_Passive(num, index), index);
        }

        public virtual int ShopCardCount(Rarity rare)
        {
            int num;
            switch (rare)
            {
                case Rarity.Common:
                    num = 5;
                    break;
                case Rarity.Uncommon:
                    num = 4;
                    break;
                case Rarity.Rare:
                    num = 3;
                    break;
                case Rarity.Unique:
                    num = 1;
                    break;
                default:
                    num = 1;
                    break;
            }
            return (int)((double)num + (double)num * (double)LogLikeMod.curchaptergrade / 5.0);
        }

        public virtual int ShopGoodCount()
        {
            switch (LogLikeMod.curchaptergrade)
            {
                case ChapterGrade.Grade1:
                    return 4;
                case ChapterGrade.Grade2:
                    return 5;
                case ChapterGrade.Grade3:
                    return 6;
                case ChapterGrade.Grade4:
                    return 6;
                case ChapterGrade.Grade5:
                    return 7;
                case ChapterGrade.Grade6:
                    return 8;
                default:
                    return 5;
            }
        }

        public virtual void CreateShop()
        {
            this.RemoveShop();
            this.FrameObj.Add("ShopFrame", ModdingUtils.CreateImage(SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.transform, "ShopFrame", new Vector2(1f, 1f), new Vector2(0.0f, -100f)).gameObject);
            Button button1 = ModdingUtils.CreateButton(SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.transform, "AbCardSelection_Skip", new Vector2(1f, 1f), new Vector2(-590f, -480f));
            button1.onClick.AddListener(new UnityAction(this.HideShop));
            this.FrameObj.Add("ShopHide", button1.gameObject);
            TextMeshProUGUI textTmp1 = ModdingUtils.CreateText_TMP(button1.transform, new Vector2(-30f, 0.0f), 40, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
            textTmp1.text = TextDataModel.GetText("ui_ShopHide");
            textTmp1.transform.Rotate(0.0f, 0.0f, 2.5f);
            this.FrameObj.Add("ShopHideText", textTmp1.gameObject);
            this.Goods = new List<ShopGoods>();
            if (!LoguePlayDataSaver.LoadShop(this))
            {
                int num = this.ShopGoodCount();
                try
                {
                    this.CreateShop_Card(num);
                }
                catch (Exception ex)
                {
                    Debug.Log($"Shop Create error1 : Card{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                }
                try
                {
                    this.CreateShop_passive(num);
                }
                catch (Exception ex)
                {
                    Debug.Log($"Shop Create error2 : Passive{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                }
            }
            this.MoneyChecking();
            Button button2 = ModdingUtils.CreateButton(SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.transform, "AbCardSelection_Skip", new Vector2(1f, 1f), new Vector2(580f, -480f));
            TextMeshProUGUI textTmp2 = ModdingUtils.CreateText_TMP(button2.transform, new Vector2(-30f, 0.0f), 40, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
            this.FrameObj.Add("LeaveButton", button2.gameObject);
            button2.onClick.AddListener(new UnityAction(this.LeaveShop));
            string text = TextDataModel.GetText("ui_ShopLeave");
            textTmp2.text = text;
        }

        public virtual void HideShop()
        {
            GameObject gameObject1 = this.FrameObj["ShopFrame"];
            GameObject gameObject2 = this.FrameObj["LeaveButton"];
            if (gameObject1.activeSelf)
            {
                this.FrameObj["ShopHideText"].GetComponent<TextMeshProUGUI>().text = TextDataModel.GetText("ui_ShopShow");
                gameObject1.SetActive(false);
                gameObject2.SetActive(false);
            }
            else
            {
                this.FrameObj["ShopHideText"].GetComponent<TextMeshProUGUI>().text = TextDataModel.GetText("ui_ShopHide");
                gameObject1.SetActive(true);
                gameObject2.SetActive(true);
                this.MoneyChecking();
            }
            if (LogLikeMod.rewards_InStage.Count <= 0)
                return;
            this.FrameObj["ShopHideText"].GetComponent<TextMeshProUGUI>().text = TextDataModel.GetText("ui_ShopShow");
            gameObject1.SetActive(false);
            gameObject2.SetActive(false);
        }

        public virtual void LeaveShop()
        {
            this.RemoveShop();
            Singleton<StageController>.Instance.GetStageModel().GetWave(Singleton<StageController>.Instance.CurrentWave).Defeat();
            Singleton<StageController>.Instance.EndBattle();
            Singleton<ShopManager>.Instance.CloseShop();
            Singleton<GlobalLogueEffectManager>.Instance.OnLeaveShop(this);
        }

        public virtual void MoneyChecking()
        {
            if (this.Goods == null || this.Goods.Count == 0)
                return;
            this.Goods.FindAll(x => x.CheckEnoughMoney());
            LoguePlayDataSaver.SaveShop(this);
            LoguePlayDataSaver.SavePlayData_Menu();
        }
    }
}
