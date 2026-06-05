// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.GlobalLogueEffectManager
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using HarmonyLib;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RogueLike_Mod_Reborn;


namespace abcdcode_LOGLIKE_MOD
{

    public class GlobalLogueEffectManager : Singleton<GlobalLogueEffectManager>, Savable
    {
        public Button Next;
        public Button Prev;
        public Mask BackGroundMask;
        public bool IsOn;
        public Image BackGround;
        public GlobalLogueEffectManager.LogueEffectOnOff OnOffBtn;
        public List<GlobalLogueEffectBase> effects;
        public List<GlobalLogueEffectManager.LogueEffectImage> sprites;
        public int index;
        public bool First;
        public ShopRewardType curGlobalType;
        public bool isLoadingSave;

        public void LoadFromSaveData(SaveData save)
        {
            this.isLoadingSave = true;
            foreach (SaveData save1 in save)
            {
                GlobalLogueEffectBase globalEffectBySave = GlobalLogueEffectBase.CreateGlobalEffectBySave(save1);
                if (globalEffectBySave != null)
                {
                    globalEffectBySave.LoadFromSaveData(save1);
                    this.AddEffects(globalEffectBySave);
                }
            }
            this.isLoadingSave = false;
        }

        public SaveData GetSaveData()
        {
            SaveData saveData = new SaveData();
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                saveData.AddToList(effect.GetSaveData());
            return saveData;
        }

        public GlobalLogueEffectManager()
        {
            this.effects = new List<GlobalLogueEffectBase>();
            this.sprites = new List<GlobalLogueEffectManager.LogueEffectImage>();
            this.IsOn = true;
            this.First = true;
            this.index = 0;
            int num = SingletonBehavior<BattleTutorialManagerUI>.Instance == null ? 1 : 0;
            GameObject logUiObj = LogLikeMod.LogUIObjs[100];
            Image image1 = ModdingUtils.CreateImage(logUiObj.transform, "MaskSpace", new Vector2(1f, 1f), new Vector2(0.0f, 400f), new Vector2(1920f, 110f));
            Mask mask = image1.gameObject.AddComponent<Mask>();
            mask.showMaskGraphic = false;
            this.BackGroundMask = mask;
            Image image2 = ModdingUtils.CreateImage(image1.transform, "UpBar", new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(1920f, 110f));
            this.BackGround = image2;
            Button button = ModdingUtils.CreateButton(logUiObj.transform, "ShowHideButtonOuter", new Vector2(1f, 1f), new Vector2(0.0f, 325f), new Vector2(70f, 70f));
            GameObject gameObject = ModdingUtils.CreateImage(button.transform, "ShowHideButtonInner", new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(40f, 40f)).gameObject;
            GlobalLogueEffectManager.LogueEffectOnOff onoff = button.gameObject.AddComponent<GlobalLogueEffectManager.LogueEffectOnOff>();
            onoff.manager = this;
            onoff.Init(gameObject, button.gameObject);
            this.OnOffBtn = onoff;
            Button.ButtonClickedEvent buttonClickedEvent1 = new Button.ButtonClickedEvent();
            buttonClickedEvent1.AddListener((UnityAction)(() => onoff.OnClick()));
            onoff.OnClick();
            button.onClick = buttonClickedEvent1;
            for (int index = 0; index < 20; ++index)
            {
                GlobalLogueEffectManager.LogueEffectImage logueEffectImage = ModdingUtils.CreateImage(image2.transform, (Sprite)null, new Vector2(1f, 1f), new Vector2((float)(index * 90 - 860), 11f), new Vector2(80f, 80f)).gameObject.AddComponent<GlobalLogueEffectManager.LogueEffectImage>();
                this.sprites.Add(logueEffectImage);
                logueEffectImage.gameObject.SetActive(false);
            }
            this.Prev = ModdingUtils.CreateButton(image2.transform, "MysteryArrow_LR", new Vector2(1f, 1f), new Vector2(-930f, 15f), new Vector2(40f, 40f));
            Button.ButtonClickedEvent buttonClickedEvent2 = new Button.ButtonClickedEvent();
            buttonClickedEvent2.AddListener((UnityAction)(() => this.OnClickPrev()));
            this.Prev.onClick = buttonClickedEvent2;
            this.Next = ModdingUtils.CreateButton(image2.transform, "MysteryArrow_LR", new Vector2(-1f, 1f), new Vector2(930f, 15f), new Vector2(40f, 40f));
            Button.ButtonClickedEvent buttonClickedEvent3 = new Button.ButtonClickedEvent();
            buttonClickedEvent3.AddListener((UnityAction)(() => this.OnClickNext()));
            this.Next.onClick = buttonClickedEvent3;
            this.curGlobalType = ShopRewardType.Eternal;
        }

        public void OnAddSubPlayer(UnitDataModel model)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
            {
                try
                {
                    effect.OnAddSubPlayer(model);
                }
                catch (Exception ex)
                {
                    this.Log(ex.Message + Environment.NewLine + ex.StackTrace);
                    break;
                }
            }
        }

        public float CraftCostMultiple(CraftEffect cffect)
        {
            float num = 1f;
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
            {
                try
                {
                    num *= effect.CraftCostMultiple(cffect);
                }
                catch (Exception ex)
                {
                    this.Log(ex.Message + Environment.NewLine + ex.StackTrace);
                    return num;
                }
            }
            return num;
        }

        public LorId InvenAddCardChange(LorId baseid)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
            {
                try
                {
                    baseid = effect.InvenAddCardChange(baseid);
                }
                catch (Exception ex)
                {
                    this.Log(ex.Message + Environment.NewLine + ex.StackTrace);
                    return baseid;
                }
            }
            return baseid;
        }

        public void RewardInStageInterrupt()
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
            {
                try
                {
                    effect.RewardInStageInterrupt();
                }
                catch (Exception ex)
                {
                    this.Log(ex.Message + Environment.NewLine + ex.StackTrace);
                    break;
                }
            }
        }

        public void RewardClearStageInterrupt()
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
            {
                try
                {
                    effect.RewardClearStageInterrupt();
                }
                catch (Exception ex)
                {
                    this.Log(ex.Message + Environment.NewLine + ex.StackTrace);
                    break;
                }
            }
        }

        public void ChangeShopCard(ref DiceCardXmlInfo card)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
            {
                try
                {
                    effect.ChangeShopCard(ref card);
                }
                catch (Exception ex)
                {
                    this.Log(ex.Message + Environment.NewLine + ex.StackTrace);
                    break;
                }
            }
        }

        public void ChangeCardReward(ref List<DiceCardXmlInfo> cardlist)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
            {
                try
                {
                    effect.ChangeCardReward(ref cardlist);
                }
                catch
                {
                    this.Log("Global-ChangeCardReward error");
                    break;
                }
            }
        }

        public int ChangeSuccCostValue()
        {
            int num = 0;
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
            {
                try
                {
                    num += effect.ChangeSuccCostValue();
                }
                catch
                {
                    this.Log("Global-ChangeSuccCostValue error");
                    return 0;
                }
            }
            return num;
        }

        public void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
            {
                try
                {
                    effect.ChangeDiceResult(behavior, ref diceResult);
                }
                catch
                {
                    this.Log("Global-ChangeDiceResult error");
                }
            }
        }

        public void ChangeRestChoice(MysteryBase currest, ref List<RewardPassiveInfo> choices)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.ChangeRestChoice(currest, ref choices);
        }

        public void OnRoundStart(StageController stage)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnRoundStart(stage);
        }

        public float DmgFactor(BattleUnitModel model, int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
        {
            float num = 1f;
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                num *= effect.DmgFactor(model, dmg, type, keyword);
            return num;
        }

        public void OnKillUnit(BattleUnitModel killer, BattleUnitModel target)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnKillUnit(killer, target);
        }

        public void OnDieUnit(BattleUnitModel unit)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnDieUnit(unit);
        }

        public void OnStartBattle_AfterCardSet(BattlePlayingCardDataInUnitModel card)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnStartBattle(card);
        }

        public void BeforeRollDice(BattleDiceBehavior behavior)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.BeforeRollDice(behavior);
        }

        public void OnUseCard(BattlePlayingCardDataInUnitModel cardmodel)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnUseCard(cardmodel);
        }

        public void ChangeShopCardList(ShopBase shop, ref CardDropValueXmlInfo list)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.ChangeShopCardList(shop, ref list);
        }

        public void OnShopCardListCreate(ShopBase shop)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnShopCardListCreate(shop);
        }

        public void OnPickCardReward(List<DiceCardXmlInfo> cardlist, DiceCardXmlInfo pick)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnPickCardReward(cardlist, pick);
        }

        public void OnSkipCardRewardChoose(List<DiceCardXmlInfo> cardlist)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnSkipCardRewardChoose(cardlist);
        }

        public bool CanShopPurchase(ShopBase shop, ShopGoods goods)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
            {
                if (!effect.CanShopPurchase(shop, goods))
                    return false;
            }
            return true;
        }

        public void OnLeaveShop(ShopBase shop)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnLeaveShop(shop);
        }

        public void OnEnterShop(ShopBase shop)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnEnterShop(shop);
        }

        public void OnStartBattle()
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnStartBattle();
        }

        public void OnStartBattleAfter()
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnStartBattleAfter();
        }

        public void OnEndBattle()
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnEndBattle();
        }

        public void OnCreateLibrarian(BattleUnitModel model)
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnCreateLibrarian(model);
        }

        public void OnCreateLibrarians()
        {
            foreach (GlobalLogueEffectBase effect in this.GetEffectList())
                effect.OnCreateLibrarians();
        }

        public void ClearList()
        {
            this.effects = new List<GlobalLogueEffectBase>();
            this.First = true;
            this.index = 0;
            if (this.IsOn)
                this.OnOffBtn.OnClick();
            this.UpdateSprites();
        }

        public void AddEffects(GlobalLogueEffectBase effect)
        {
            if (effect == null)
            {
                this.Log("AddEffect : Effect Null");
            }
            else
            {
                if (this.effects == null)
                    this.effects = new List<GlobalLogueEffectBase>();
                GlobalLogueEffectBase globalLogueEffectBase = this.effects.Find((Predicate<GlobalLogueEffectBase>)(x => x.GetType() == effect.GetType()));
                if (globalLogueEffectBase != null && globalLogueEffectBase.CanDupliacte())
                    globalLogueEffectBase.AddedNew();
                else
                    this.effects.Add(effect);
                if (!this.isLoadingSave)
                    Singleton<LogueSaveManager>.Instance.AddToObtainCount(effect);
                this.UpdateSprites();
            }
        }  

        public void RemoveEffect(GlobalLogueEffectBase effect)
        {
            if (this.effects == null)
                return;
            if (this.effects.Contains(effect))
            {
                effect.OnDestroy();
                this.effects.Remove(effect);
            }
            this.UpdateSprites();
        }

        /// <summary>
        /// Returns the first instance of a GlobalLogueEffectBase in inventory.<br></br>
        /// If it does not exist, returns <see langword="null"/>.
        /// </summary>
        public T GetEffect<T>() where T : GlobalLogueEffectBase
        {
            var effect = this.GetEffectList().Find(x => x is T);
            return (T)(effect ?? null);
        }

        public List<GlobalLogueEffectBase> GetEffectList()
        {
            if (this.effects == null)
                this.effects = new List<GlobalLogueEffectBase>();
            return new List<GlobalLogueEffectBase>(this.effects);
        }

        public void OnClickNext()
        {
            ++this.index;
            this.UpdateSprites();
        }

        public void OnClickPrev()
        {
            --this.index;
            this.UpdateSprites();
        }

        public void UpdateSprites()
        {
            LogLikeMod.LogUIObjs[100].SetActive(true);
            int num1 = 0;
            bool flag = LogLikeMod.CheckStage();
            foreach (Component sprite in this.sprites)
                sprite.gameObject.SetActive(false);
            this.BackGroundMask.gameObject.SetActive(this.IsOn & flag);
            this.BackGround.gameObject.SetActive(flag);
            this.OnOffBtn.gameObject.SetActive(flag);
            if (!flag)
                return;
            List<GlobalLogueEffectBase> once = this.effects.FindAll((Predicate<GlobalLogueEffectBase>)(x => x is OnceEffect));
            List<GlobalLogueEffectBase> all = this.effects.FindAll((Predicate<GlobalLogueEffectBase>)(x => !once.Contains(x)));
            List<GlobalLogueEffectBase> globalLogueEffectBaseList = new List<GlobalLogueEffectBase>();
            foreach (GlobalLogueEffectBase globalLogueEffectBase in all)
            {
                if (globalLogueEffectBase.GetSprite() != null)
                {
                    globalLogueEffectBaseList.Add(globalLogueEffectBase);
                    ++num1;
                }
            }
            foreach (GlobalLogueEffectBase globalLogueEffectBase in once)
            {
                if (globalLogueEffectBase.GetSprite() != null)
                {
                    globalLogueEffectBaseList.Add(globalLogueEffectBase);
                    ++num1;
                }
            }
            int num2 = this.index * this.sprites.Count;
            int index1 = 0;
            for (int index2 = num2; index2 < num2 + 20 && globalLogueEffectBaseList.Count > index2; ++index2)
            {
                this.sprites[index1].Init(globalLogueEffectBaseList[index2]);
                ++index1;
            }
            this.Log(this.index.ToString());
            this.Prev.gameObject.SetActive(this.index > 0);
            this.Next.gameObject.SetActive(globalLogueEffectBaseList.Count > this.index * 20 + 20);
            if (num1 > 0 && this.First && !this.IsOn)
            {
                this.First = false;
                this.OnOffBtn.OnClick();
            }
        }

        public void OnCrit(BattleUnitModel critter, BattleUnitModel target)
        {
            foreach (GlobalLogueEffectBase effect in this.effects)
                effect.OnCrit(critter, target);
        }

        public void AfterClearBossWave()
        {
            foreach (GlobalLogueEffectBase effect in this.effects)
                effect.AfterClearBossWave();
        }

        public class LogueEffectOnOff : MonoBehaviour
        {
            public float curRotValue;
            public bool IsOn;
            public float curValue;
            public bool Playing;
            public bool Start;
            public GlobalLogueEffectManager manager;
            public GameObject Outer;
            public GameObject Inner;

            public void Init(GameObject inner, GameObject outer)
            {
                this.Inner = inner;
                this.Outer = outer;
            }

            public void OnClick()
            {
                if (this.Playing)
                    return;
                if (this.manager.IsOn)
                {
                    this.IsOn = false;
                    this.curValue = 105f;
                    this.curRotValue = 60f;
                }
                else
                {
                    this.IsOn = true;
                    this.curValue = -105f;
                    this.curRotValue = -60f;
                }
                this.Start = true;
            }

            public void Update()
            {
                if (Input.GetKeyDown(KeyCode.E))
                    this.OnClick();
                if (this.Start)
                {
                    this.Playing = true;
                    this.Start = false;
                }
                if (!this.Playing)
                    return;
                if (this.IsOn)
                    this.manager.BackGroundMask.gameObject.SetActive(true);
                float y = (float)((double)this.curValue * 1.0 / 5.0);
                float zAngle = (float)((double)this.curRotValue * 1.0 / 5.0);
                this.curValue -= y;
                this.curRotValue -= zAngle;
                this.transform.localPosition += new Vector3(0.0f, y);
                this.manager.BackGround.transform.localPosition += new Vector3(0.0f, y);
                this.Outer.transform.Rotate(0.0f, 0.0f, zAngle, Space.Self);
                this.Inner.transform.Rotate(0.0f, 0.0f, (float)(-(double)zAngle * 0.25), Space.Self);
                if ((double)Mathf.Abs(this.curValue) <= 0.5)
                {
                    if (this.IsOn)
                    {
                        this.gameObject.transform.localPosition = new Vector3(0.0f, 325f);
                        this.manager.BackGround.transform.localPosition = new Vector3(0.0f, 0.0f);
                    }
                    else
                    {
                        this.gameObject.transform.localPosition = new Vector3(0.0f, 430f);
                        this.manager.BackGround.transform.localPosition = new Vector3(0.0f, 105f);
                    }
                    this.manager.IsOn = this.IsOn;
                    this.manager.UpdateSprites();
                    this.Playing = false;
                }
            }
        }

        public class LogueEffectImage : MonoBehaviour
        {
            public bool update;
            public GlobalLogueEffectBase effect;
            public UILogCustomSelectable selectable;
            public Sprite sprite;
            public Image image;
            public Image baseimage;
            public TextMeshProUGUI text;

            public void Init(GlobalLogueEffectBase effect)
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
                    this.image.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    if (this.text == null)
                        this.text = ModdingUtils.CreateText_TMP(this.gameObject.transform, new Vector2(0.0f, 0.0f), 25, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.BottomRight, Color.white, LogLikeMod.DefFont_TMP);
                    if (effect.GetSprite() == null)
                    {
                        this.Log("effect is null");
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (this.baseimage == null)
                            this.baseimage = ModdingUtils.CreateImage(this.transform, "ShopGoodRewardFrame", new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), new Vector2(70f, 70f));
                        if (this.text != null)
                        {
                            int stack = effect.GetStack();
                            stack.Log("cur Effect Stack : " + stack.ToString());
                            this.text.text = stack >= 0 ? stack.ToString() : string.Empty;
                        }
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

            public void OnClickImage()
            {
                if (Singleton<GlobalLogueEffectManager>.Instance.OnOffBtn.Playing)
                    return;
                this.effect.OnClick();
            }

            public void OnEnterImage()
            {
                if (string.IsNullOrEmpty(this.effect.GetEffectDesc()))
                    return;
                this.image.color = UIColorManager.Manager.GetUIColor(UIColor.Highlighted);
                FieldInfo field = this.effect.GetType().GetField("ItemRarity", BindingFlags.Static | BindingFlags.Public);
                Rarity rare = this.effect.GetRarity();
                SingletonBehavior<UIMainOverlayManager>.Instance.SetTooltip(this.effect.GetEffectName(), $"{this.effect.GetEffectDesc()}\n<color=#{ColorUtility.ToHtmlStringRGB(UIColorManager.Manager.GetEquipRarityColor(rare))}>{rare.ToString()}</color>", this.transform as RectTransform, rare);
                var pivot = SingletonBehavior<UIMainOverlayManager>.Instance.tooltipPositionPivot;
                pivot.anchoredPosition = new Vector2(pivot.anchoredPosition.x - 100f, 440f);
                this.update = true;
            }

            public void OnExitImage()
            {
                this.image.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
                SingletonBehavior<UIMainOverlayManager>.Instance.Close();
                this.update = false;
            }


        }
    }
}
