// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShopGood7
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]

    public class PickUpModel_ShopGood7 : ShopPickUpModel
    {
        public PickUpModel_ShopGood7()
        {
            this.basepassive = Singleton<PassiveXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, 8570007));
            this.Name = Singleton<PassiveDescXmlList>.Instance.GetName(this.basepassive.id);
            this.Desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(this.basepassive.id);
            this.id = new LorId(LogLikeMod.ModId, 90007);
        }

        public override bool IsCanAddShop() => !LogueBookModels.shopPick.Contains(this.id);

        public override void OnPickUp(BattleUnitModel model) => base.OnPickUp(model);

        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new PickUpModel_ShopGood7.FirstIsGood());
        }

        public class FirstIsGood : GlobalLogueEffectBase
        {
            public override Sprite GetSprite() => LogLikeMod.ArtWorks["ShopPassive7"];

            public override string GetEffectName() => TextDataModel.GetText("Shop7_Name");

            public override string GetEffectDesc() => TextDataModel.GetText("Shop7_Desc");

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (behavior.card.owner.faction == Faction.Enemy || LogLikeMod.curchaptergrade - behavior.card.card.XmlData.Chapter + 1 <= ChapterGrade.Grade1)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = 1
                });
            }

            public override void ChangeShopCardList(ShopBase shop, ref CardDropValueXmlInfo list)
            {
                base.ChangeShopCardList(shop, ref list);
                Dictionary<int, int> dic = new Dictionary<int, int>();
                dic.Add(0, -854001);
                dic.Add(1, -854101);
                dic.Add(2, -854201);
                dic.Add(3, -854301);
                dic.Add(4, -854401);
                dic.Add(5, -854501);
                List<int> allDicValueAsKey = ModdingUtils.FindAll_DicValueAsKey<int, int>(dic, (Predicate<int>)(x => (ChapterGrade)x < LogLikeMod.curchaptergrade));
                CardDropValueXmlInfo dropValueXmlInfo = CardDropValueXmlInfo.Copying(list);
                CardDropTableXmlInfo data = Singleton<CardDropTableXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, dic[(int)LogLikeMod.curchaptergrade]));
                CardDropTableXmlInfo newinfo = new CardDropTableXmlInfo();
                newinfo._id = -data._id;
                newinfo.workshopId = data.workshopId;
                newinfo.cardIdList = new List<LorId>((IEnumerable<LorId>)data.cardIdList);
                if (allDicValueAsKey.Count > 0)
                {
                    foreach (int id in allDicValueAsKey)
                        newinfo.cardIdList.AddRange((IEnumerable<LorId>)Singleton<CardDropTableXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, id)).cardIdList);
                }
                this.Log("CardChange5");
                List<CardDropTableXmlInfo> dropTableXmlInfoList = (List<CardDropTableXmlInfo>)typeof(CardDropTableXmlList).GetField("_list", AccessTools.all).GetValue(Singleton<CardDropTableXmlList>.Instance);
                dropTableXmlInfoList.RemoveAll((Predicate<CardDropTableXmlInfo>)(x => x.id == newinfo.id));
                dropTableXmlInfoList.Add(newinfo);
                dropValueXmlInfo.DropTableId = newinfo._id;
                list = dropValueXmlInfo;
            }
        }
    }
}
