// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogCreatureTabPanel
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class LogCreatureTabPanel : Singleton<LogCreatureTabPanel>
    {
        public bool IsPreload;
        public Dictionary<int, List<LogCreatureTabPanel.LogueImage_CreatureTab>> TabGroups;
        public GameObject root;

        public LogCreatureTabPanel() => this.IsPreload = false;

        public static GameObject GetLogUIObj(int index)
        {
            GameObject gameObject1 = (UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel).EditPanel.BattleCardPanel.gameObject;
            GameObject gameObject2 = UnityEngine.Object.Instantiate<Transform>(gameObject1.transform, gameObject1.transform.parent).gameObject;
            UnityEngine.Object.Destroy(gameObject2.GetComponent<UIBattleSettingPanel>());
            for (int index1 = 0; index1 < gameObject2.transform.childCount; ++index1)
                UnityEngine.Object.Destroy(gameObject2.transform.GetChild(index1).gameObject);
            gameObject2.SetActive(true);
            gameObject2.transform.localPosition = new Vector3(0.0f, 0.0f);
            gameObject2.transform.localScale = gameObject1.transform.localScale;
            gameObject2.GetComponent<Canvas>().enabled = true;
            gameObject2.GetComponent<Canvas>().sortingOrder += index;
            gameObject2.GetComponent<CanvasGroup>().alpha = 1f;
            gameObject2.GetComponent<CanvasGroup>().blocksRaycasts = true;
            gameObject2.GetComponent<CanvasGroup>().interactable = true;
            return gameObject2;
        }

        public IEnumerator PreloadImages()
        {
            this.Log("Start CreatureTab Preload");
            this.TabGroups = new Dictionary<int, List<LogCreatureTabPanel.LogueImage_CreatureTab>>();
            while (true)
            {
                yield return new WaitForEndOfFrame();
                try
                {
                    this.root = LogCreatureTabPanel.GetLogUIObj(1);
                    break;
                }
                catch (Exception ex)
                {
                    Exception e = ex;
                }
            }
            for (int j = 0; j < 4; ++j)
            {
                this.TabGroups[j] = new List<LogCreatureTabPanel.LogueImage_CreatureTab>();
                for (int i = 0; i < (j == 3 ? 8 : 10); ++i)
                {
                    Image image = ModdingUtils.CreateImage(this.root.transform, (Sprite)null, new Vector2(1f, 1f), new Vector2((float)(i * 100 - 420), (float)(360 - j * 150)), new Vector2(80f, 80f));
                    LogCreatureTabPanel.LogueImage_CreatureTab logueimage = image.gameObject.AddComponent<LogCreatureTabPanel.LogueImage_CreatureTab>();
                    this.TabGroups[j].Add(logueimage);
                    yield return new WaitForEndOfFrame();
                    image = (Image)null;
                    logueimage = (LogCreatureTabPanel.LogueImage_CreatureTab)null;
                }
            }
            this.TabGroups[0][0].Init((CreaturePickUpModel)new PickUpModel_BloodBath0());
            this.TabGroups[0][1].Init((CreaturePickUpModel)new PickUpModel_ScorchedGirl0());
            this.TabGroups[0][2].Init((CreaturePickUpModel)new PickUpModel_ForsakenMurderer0());
            this.TabGroups[0][3].Init((CreaturePickUpModel)new PickUpModel_ShyLookToday0());
            this.TabGroups[0][4].Init((CreaturePickUpModel)new PickUpModel_UniverseZogak0());
            yield return new WaitForEndOfFrame();
            this.TabGroups[0][5].Init((CreaturePickUpModel)new PickUpModel_QueenOfHatred0());
            this.TabGroups[0][6].Init((CreaturePickUpModel)new PickUpModel_Redhood0());
            this.TabGroups[0][7].Init((CreaturePickUpModel)new PickUpModel_ScareCrow0());
            this.TabGroups[0][8].Init((CreaturePickUpModel)new PickUpModel_Bigbird0());
            this.TabGroups[0][9].Init((CreaturePickUpModel)new PickUpModel_Bloodytree0());
            yield return new WaitForEndOfFrame();
            this.TabGroups[1][0].Init((CreaturePickUpModel)new PickUpModel_HeartofAspiration0());
            this.TabGroups[1][1].Init((CreaturePickUpModel)new PickUpModel_HappyTeddyBear0());
            this.TabGroups[1][2].Init((CreaturePickUpModel)new PickUpModel_LittleHelper0());
            this.TabGroups[1][3].Init((CreaturePickUpModel)new PickUpModel_RedShoes0());
            this.TabGroups[1][4].Init((CreaturePickUpModel)new PickUpModel_ChildofGalaxy0());
            yield return new WaitForEndOfFrame();
            this.TabGroups[1][5].Init((CreaturePickUpModel)new PickUpModel_KnightOfDespair0());
            this.TabGroups[1][6].Init((CreaturePickUpModel)new PickUpModel_BigBadWolf0());
            this.TabGroups[1][7].Init((CreaturePickUpModel)new PickUpModel_LumberJack0());
            this.TabGroups[1][8].Init((CreaturePickUpModel)new PickUpModel_SmallBird0());
            this.TabGroups[1][9].Init((CreaturePickUpModel)new PickUpModel_Clock0());
            yield return new WaitForEndOfFrame();
            this.TabGroups[2][0].Init((CreaturePickUpModel)new PickUpModel_Pinocchio0());
            this.TabGroups[2][1].Init((CreaturePickUpModel)new PickUpModel_FairyCarnival0());
            this.TabGroups[2][2].Init((CreaturePickUpModel)new PickUpModel_SingingMachine0());
            this.TabGroups[2][3].Init((CreaturePickUpModel)new PickUpModel_SpiderBud0());
            this.TabGroups[2][4].Init((CreaturePickUpModel)new PickUpModel_Porccubus0());
            yield return new WaitForEndOfFrame();
            this.TabGroups[2][5].Init((CreaturePickUpModel)new PickUpModel_Greed0());
            this.TabGroups[2][6].Init((CreaturePickUpModel)new PickUpModel_Mountain0());
            this.TabGroups[2][7].Init((CreaturePickUpModel)new PickUpModel_House0());
            this.TabGroups[2][8].Init((CreaturePickUpModel)new PickUpModel_LongBird0());
            this.TabGroups[2][9].Init((CreaturePickUpModel)new PickUpModel_BlueStar0());
            yield return new WaitForEndOfFrame();
            this.TabGroups[3][0].Init((CreaturePickUpModel)new PickUpModel_TheSnowQueen0());
            this.TabGroups[3][1].Init((CreaturePickUpModel)new PickUpModel_QueenBee0());
            this.TabGroups[3][2].Init((CreaturePickUpModel)new PickUpModel_Butterfly0());
            this.TabGroups[3][3].Init((CreaturePickUpModel)new PickUpModel_Laetitia0());
            this.TabGroups[3][4].Init((CreaturePickUpModel)new PickUpModel_Alriune0());
            yield return new WaitForEndOfFrame();
            this.TabGroups[3][5].Init((CreaturePickUpModel)new PickUpModel_Angry0());
            this.TabGroups[3][6].Init((CreaturePickUpModel)new PickUpModel_Nosferatu0());
            this.TabGroups[3][7].Init((CreaturePickUpModel)new PickUpModel_Ozma0());
            this.IsPreload = true;
            this.Log("End CreatureTab Preload");
        }

        public void Init()
        {
            if (!this.IsPreload)
                return;
            foreach (KeyValuePair<int, int> keyValuePair in LogueBookModels.EmotionSelectDic)
            {
                foreach (LogCreatureTabPanel.LogueImage_CreatureTab imageCreatureTab in this.TabGroups[keyValuePair.Key])
                    imageCreatureTab.Active(false);
                this.TabGroups[keyValuePair.Key][keyValuePair.Value].Active(true);
            }
            this.root.transform.localScale = new Vector3(1.2f, 1f, 1f);
            this.root.SetActive(true);
        }

        public void SetActive(bool value)
        {
            if (value)
                this.Init();
            else if (this.root != null)
                this.root.SetActive(false);
        }

        public List<RewardPassiveInfo> GetCreaturePickUpByIndex(int level, int index)
        {
            List<RewardPassiveInfo> creaturePickUpByIndex = new List<RewardPassiveInfo>();
            foreach (LorId id in this.TabGroups[level][index].effect.ids)
                creaturePickUpByIndex.Add(Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id));
            return creaturePickUpByIndex;
        }

        public void ChangeCreature(CreaturePickUpModel model)
        {
            foreach (KeyValuePair<int, List<LogCreatureTabPanel.LogueImage_CreatureTab>> tabGroup in this.TabGroups)
            {
                if ((bool)tabGroup.Value.Find((Predicate<LogCreatureTabPanel.LogueImage_CreatureTab>)(x => x.effect == model)))
                {
                    foreach (LogCreatureTabPanel.LogueImage_CreatureTab imageCreatureTab in tabGroup.Value)
                    {
                        if (imageCreatureTab.effect == model)
                        {
                            imageCreatureTab.Active(true);
                            LogueBookModels.EmotionSelectDic[tabGroup.Key] = tabGroup.Value.IndexOf(imageCreatureTab);
                        }
                        else
                            imageCreatureTab.Active(false);
                    }
                }
            }
        }

        public class LogueImage_CreatureTab : MonoBehaviour
        {
            public bool update;
            public CreaturePickUpModel effect;
            public UILogCustomSelectable selectable;
            public Sprite sprite;
            public Image image;
            public Image baseimage;
            public TextMeshProUGUI text;

            public void Init(CreaturePickUpModel effect)
            {
                if (effect == null)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    this.effect = effect;
                    this.image = this.gameObject.GetComponent<Image>();
                    this.image.sprite = LogLikeMod.ArtWorks["ShopGoodRewardFrame"];
                    this.image.enabled = false;
                    if (effect.GetSprite() == null)
                    {
                        this.Log("effect is null");
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (this.baseimage == null)
                            this.baseimage = ModdingUtils.CreateImage(this.transform, "ShopGoodRewardFrame", new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(70f, 70f));
                        this.sprite = effect.GetSprite();
                        this.baseimage.sprite = this.sprite;
                        if (this.selectable == null)
                        {
                            this.selectable = this.gameObject.AddComponent<UILogCustomSelectable>();
                            this.selectable.targetGraphic = (Graphic)this.image;
                            Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
                            buttonClickedEvent.AddListener((UnityAction)(() => this.OnClickImage()));
                            this.selectable.onClick = buttonClickedEvent;
                            this.selectable.SelectEvent = new UnityEventBasedata();
                            this.selectable.SelectEvent.AddListener((UnityAction<BaseEventData>)(e => this.OnEnterImage()));
                            this.selectable.DeselectEvent = new UnityEventBasedata();
                            this.selectable.DeselectEvent.AddListener((UnityAction<BaseEventData>)(e => this.OnExitImage()));
                        }
                        this.gameObject.SetActive(true);
                        this.update = false;
                        if (!(SingletonBehavior<UIMainOverlayManager>.Instance != null))
                            return;
                        SingletonBehavior<UIMainOverlayManager>.Instance.Close();
                    }
                }
            }

            public void Active(bool value) => this.image.enabled = value;

            public void OnClickImage()
            {
                if (!this.image.enabled)
                    Singleton<LogCreatureTabPanel>.Instance.ChangeCreature(this.effect);
                else
                    CreaturePickUpModel.LoadGetAbnomalityPanel(this.effect.GetCreatureList(), this.effect.level, this.effect.GetCreatureName());
            }

            public void OnEnterImage() => this.update = true;

            public void OnExitImage()
            {
                SingletonBehavior<UIMainOverlayManager>.Instance.Close();
                this.update = false;
            }

            public void Update()
            {
            }
        }
    }
}
