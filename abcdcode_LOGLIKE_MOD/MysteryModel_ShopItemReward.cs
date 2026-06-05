// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_ShopItemReward
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class MysteryModel_ShopItemReward : MysteryBase
    {
        public MysteryModel_ShopItemReward.ChoiceResult result;
        public List<RewardPassiveInfo> rewards;
        public static Dictionary<int, Vector2[]> ChoiceShape;

        public override void LoadFromSaveData(SaveData savedata)
        {
        }

        public static MysteryModel_ShopItemReward PopupShopReward(
          List<RewardPassiveInfo> rewards,
          MysteryModel_ShopItemReward.ChoiceResult result = null)
        {
            MysteryModel_ShopItemReward mystery = new MysteryModel_ShopItemReward();
            mystery.rewards = rewards;
            mystery.result = result;
            Singleton<MysteryManager>.Instance.AddInterrupt((MysteryBase)mystery);
            return mystery;
        }

        public MysteryModel_ShopItemReward()
        {
            if (MysteryModel_ShopItemReward.ChoiceShape != null)
                return;
            MysteryModel_ShopItemReward.ChoiceShape = new Dictionary<int, Vector2[]>();
            Vector2[] vector2Array1 = new Vector2[1]
            {
      new Vector2(0.0f, 100f)
            };
            MysteryModel_ShopItemReward.ChoiceShape.Add(1, vector2Array1);
            Vector2[] vector2Array2 = new Vector2[2]
            {
      new Vector2(-200f, 100f),
      new Vector2(200f, 100f)
            };
            MysteryModel_ShopItemReward.ChoiceShape.Add(2, vector2Array2);
            Vector2[] vector2Array3 = new Vector2[3]
            {
      new Vector2(-300f, 100f),
      new Vector2(0.0f, 100f),
      new Vector2(300f, 100f)
            };
            MysteryModel_ShopItemReward.ChoiceShape.Add(3, vector2Array3);
            Vector2[] vector2Array4 = new Vector2[4]
            {
      new Vector2(-360f, 100f),
      new Vector2(-120f, 100f),
      new Vector2(120f, 100f),
      new Vector2(360f, 100f)
            };
            MysteryModel_ShopItemReward.ChoiceShape.Add(4, vector2Array4);
            Vector2[] vector2Array5 = new Vector2[5]
            {
      new Vector2(-400f, 100f),
      new Vector2(-200f, 100f),
      new Vector2(0.0f, 100f),
      new Vector2(200f, 100f),
      new Vector2(400f, 100f)
            };
            MysteryModel_ShopItemReward.ChoiceShape.Add(5, vector2Array5);
            Vector2[] vector2Array6 = new Vector2[6]
            {
      new Vector2(-300f, 250f),
      new Vector2(0.0f, 250f),
      new Vector2(300f, 250f),
      new Vector2(-300f, -50f),
      new Vector2(0.0f, -50f),
      new Vector2(300f, -50f)
            };
            MysteryModel_ShopItemReward.ChoiceShape.Add(6, vector2Array6);
            Vector2[] vector2Array7 = new Vector2[7]
            {
      new Vector2(-360f, 250f),
      new Vector2(-120f, 250f),
      new Vector2(120f, 250f),
      new Vector2(360f, 250f),
      new Vector2(-300f, -50f),
      new Vector2(0.0f, -50f),
      new Vector2(300f, -50f)
            };
            MysteryModel_ShopItemReward.ChoiceShape.Add(7, vector2Array7);
            Vector2[] vector2Array8 = new Vector2[8]
            {
      new Vector2(-360f, 250f),
      new Vector2(-120f, 250f),
      new Vector2(120f, 250f),
      new Vector2(360f, 250f),
      new Vector2(-360f, -50f),
      new Vector2(-120f, -50f),
      new Vector2(120f, -50f),
      new Vector2(360f, -50f)
            };
            MysteryModel_ShopItemReward.ChoiceShape.Add(8, vector2Array8);
            Vector2[] vector2Array9 = new Vector2[9]
            {
      new Vector2(-400f, 250f),
      new Vector2(-200f, 250f),
      new Vector2(0.0f, 250f),
      new Vector2(200f, 250f),
      new Vector2(400f, 250f),
      new Vector2(-360f, -50f),
      new Vector2(-120f, -50f),
      new Vector2(120f, -50f),
      new Vector2(360f, -50f)
            };
            MysteryModel_ShopItemReward.ChoiceShape.Add(9, vector2Array9);
            Vector2[] vector2Array10 = new Vector2[10]
            {
      new Vector2(-400f, 250f),
      new Vector2(-200f, 250f),
      new Vector2(0.0f, 250f),
      new Vector2(200f, 250f),
      new Vector2(400f, 250f),
      new Vector2(-400f, -50f),
      new Vector2(-200f, -50f),
      new Vector2(0.0f, -50f),
      new Vector2(200f, -50f),
      new Vector2(400f, -50f)
            };
            MysteryModel_ShopItemReward.ChoiceShape.Add(10, vector2Array10);
        }

        public override void SwapFrame(int id)
        {
            this.RemoveCurFrame();
            this.FrameObj.Add("Frame", ModdingUtils.CreateImage(LogLikeMod.LogUIObjs[95].transform, "MysteryPanel_transparent", new Vector2(1f, 1f), new Vector2(0.0f, 0.0f)).gameObject);
            Button button = ModdingUtils.CreateButton(LogLikeMod.LogUIObjs[105].transform, "AbCardSelection_Skip", new Vector2(1f, 1f), new Vector2(590f, -480f));
            TextMeshProUGUI textTmp = ModdingUtils.CreateText_TMP(button.transform, new Vector2(-30f, 0.0f), 40, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
            this.FrameObj.Add("LeaveButton", button.gameObject);
            button.onClick.AddListener(new UnityAction(this.LeaveReward));
            textTmp.text = TextDataModel.GetText("ui_selectskip");
            textTmp.transform.Rotate(0.0f, 0.0f, 2.5f);
            this.RewardChoiceCreating(this.rewards);
        }

        public void RewardChoiceCreating(List<RewardPassiveInfo> list)
        {
            int count = list.Count;
            for (int index = 0; index < list.Count; ++index)
                this.RewardChoiceCreating(list[index], this.GetChoiceShape(count, index), index);
        }

        public MysteryModel_ShopItemReward.RewardGood RewardChoiceCreating(
          RewardPassiveInfo passive,
          Vector2 position,
          int id)
        {
            GameObject gameObject = new GameObject("");
            gameObject.transform.SetParent(this.FrameObj["Frame"].transform);
            gameObject.transform.localScale = new Vector3(1f, 1f);
            MysteryModel_ShopItemReward.RewardGood rewardGood = gameObject.AddComponent<MysteryModel_ShopItemReward.RewardGood>();
            rewardGood.gameObject.transform.localPosition = (Vector3)position;
            RewardPassiveInfo goodinfo = passive;
            rewardGood.Set(goodinfo);
            rewardGood.SetParent(this);
            this.FrameObj.Add("RestGoods" + id.ToString(), rewardGood.gameObject);
            return rewardGood;
        }

        public void OnClickGoods(MysteryModel_ShopItemReward.RewardGood goods)
        {
            if (this.result != null)
                this.result(this, goods);
            else
                Singleton<MysteryManager>.Instance.EndMystery((MysteryBase)this);
        }

        public void LeaveReward()
        {
            LogLikeMod.rewards.Clear();
            Singleton<MysteryManager>.Instance.EndMystery((MysteryBase)this);
        }

        public Vector2 GetChoiceShape(int num, int id)
        {
            return MysteryModel_ShopItemReward.ChoiceShape[num][id];
        }

        public class RewardGood : MonoBehaviour
        {
            public Sprite GoodSprite;
            public PickUpModelBase GoodScript;
            public RewardPassiveInfo goodinfo;
            public MysteryModel_ShopItemReward parent;
            public bool update;

            public virtual void SetParent(MysteryModel_ShopItemReward p) => this.parent = p;

            public void Set(RewardPassiveInfo goodinfo)
            {
                this.goodinfo = goodinfo;
                this.GoodScript = LogLikeMod.FindPickUp(goodinfo.script);
                if (goodinfo.id.packageId == LogLikeMod.ModId)
                    this.GoodSprite = LogLikeMod.ArtWorks[goodinfo.artwork];
                else if (LogLikeMod.ModdedArtWorks.ContainsKey((goodinfo.id.packageId, goodinfo.artwork)))
                    this.GoodSprite = LogLikeMod.ModdedArtWorks[(goodinfo.id.packageId, goodinfo.artwork)];
                UILogCustomSelectable logSelectable = null;
                if (goodinfo.id.packageId == LogLikeMod.ModId)
                    logSelectable = ModdingUtils.CreateLogSelectable(this.transform, goodinfo.iconartwork, new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(150f, 150f));
                else if (LogLikeMod.ModdedArtWorks.ContainsKey((goodinfo.id.packageId, goodinfo.iconartwork)))
                    logSelectable = ModdingUtils.CreateLogSelectable(this.transform, LogLikeMod.ModdedArtWorks[(goodinfo.id.packageId, goodinfo.iconartwork)], new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(150f, 150f));
                Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
                buttonClickedEvent.AddListener((UnityAction)(() => this.OnClickGoods()));
                logSelectable.onClick = buttonClickedEvent;
                logSelectable.SelectEvent = new UnityEventBasedata();
                logSelectable.SelectEvent.AddListener((UnityAction<BaseEventData>)(e => this.OnPointerEnter()));
                logSelectable.DeselectEvent = new UnityEventBasedata();
                logSelectable.DeselectEvent.AddListener((UnityAction<BaseEventData>)(e => this.OnPointerExit()));
            }

            public void OnClickGoods()
            {
                if (this.GoodScript is ShopPickUpModel)
                    (this.GoodScript as ShopPickUpModel).OnPickUpShop((ShopGoods)null);
                SingletonBehavior<UIBattleOverlayManager>.Instance.DisableOverlay();
                this.parent.OnClickGoods(this);
            }

            public void OnPointerEnter()
            {
                SingletonBehavior<UIBattleOverlayManager>.Instance.EnableBufOverlay(this.GoodScript.Name, this.GoodScript.Desc, this.GoodSprite, this.gameObject);
                this.update = true;
            }

            public void OnPointerExit()
            {
                SingletonBehavior<UIBattleOverlayManager>.Instance.DisableOverlay();
                this.update = false;
            }

            public void Update()
            {
                if (!this.update)
                    return;
                SingletonBehavior<UIBattleOverlayManager>.Instance.EnableBufOverlay(this.GoodScript.Name, this.GoodScript.Desc, this.GoodSprite, this.gameObject);
            }
        }

        public enum State
        {
            CardList,
            CardChoice,
        }

        public delegate void ChoiceResult(
          MysteryModel_ShopItemReward mystery,
          MysteryModel_ShopItemReward.RewardGood model);
    }
}
