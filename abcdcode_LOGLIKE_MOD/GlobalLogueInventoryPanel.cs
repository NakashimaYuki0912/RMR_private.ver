// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.GlobalLogueInventoryPanel
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

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
    public class GlobalLogueInventoryPanel : Singleton<GlobalLogueInventoryPanel>
    {
        public GameObject root;
        public List<GlobalLogueInventoryPanel.LogueEffectImage_Inventory> sprites;

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

        public void Init()
        {
            if (this.root == null)
            {
                this.root = GlobalLogueInventoryPanel.GetLogUIObj(1);
                this.sprites = new List<GlobalLogueInventoryPanel.LogueEffectImage_Inventory>();
                ModdingUtils.CreateImage(this.root.transform, "RogueLikeRebornIconAlt", new Vector2(1f, 1f), new Vector2(0.0f, 0.0f));
                for (int index1 = 0; index1 < 8; ++index1)
                {
                    for (int index2 = 0; index2 < 10; ++index2)
                        this.sprites.Add(ModdingUtils.CreateImage(this.root.transform, (Sprite)null, new Vector2(1f, 1f), new Vector2((float)(index2 * 100 - 420), (float)(360 - index1 * 100)), new Vector2(80f, 80f)).gameObject.AddComponent<GlobalLogueInventoryPanel.LogueEffectImage_Inventory>());
                }
            }
            this.root.SetActive(true);
        }

        public void SetActive(bool value)
        {
            if (value)
            {
                this.Init();
                this.UpdateSprites();
            }
            else if (this.root != null)
                this.root.SetActive(false);
        }

        public void UpdateSprites()
        {
            int index = 0;
            foreach (Component sprite in this.sprites)
                sprite.gameObject.SetActive(false);
            List<GlobalLogueEffectBase> once = Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().FindAll((Predicate<GlobalLogueEffectBase>)(x => x is OnceEffect));
            foreach (GlobalLogueEffectBase effect in Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().FindAll((Predicate<GlobalLogueEffectBase>)(x => !once.Contains(x))))
            {
                if (effect.GetSprite() != null)
                {
                    this.sprites[index].Init(effect);
                    ++index;
                }
                if (index == this.sprites.Count)
                    break;
            }
            foreach (GlobalLogueEffectBase effect in once)
            {
                if (effect.GetSprite() != null)
                {
                    this.sprites[index].Init(effect);
                    ++index;
                }
                if (index == this.sprites.Count)
                    break;
            }
        }

        public class LogueEffectImage_Inventory : MonoBehaviour
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
                    bool flag;
                    if (this.text == null)
                    {
                        Color defFontColor = LogLikeMod.DefFontColor;
                        flag = LogLikeMod.DefFont_TMP != null;
                    }
                    else
                        flag = false;
                    if (flag)
                        this.text = ModdingUtils.CreateText_TMP(this.gameObject.transform, new Vector2(0.0f, 0.0f), 30, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.BottomRight, Color.white, LogLikeMod.DefFont_TMP);
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

            public void OnEnterImage()
            {
                if (string.IsNullOrEmpty(this.effect.GetEffectDesc()))
                    return;
                this.image.color = UIColorManager.Manager.GetUIColor(UIColor.Highlighted);
                FieldInfo field = this.effect.GetType().GetField("ItemRarity", BindingFlags.Static | BindingFlags.Public);
                Rarity rare = !(field != (FieldInfo)null) ? Rarity.Special : (Rarity)field.GetValue(null);
                SingletonBehavior<UIMainOverlayManager>.Instance.SetTooltip(this.effect.GetEffectName(), $"{this.effect.GetEffectDesc()}\n<color=#{ColorUtility.ToHtmlStringRGB(UIColorManager.Manager.GetEquipRarityColor(rare))}>{rare.ToString()}</color>", this.transform as RectTransform, rare, UIToolTipPanelType.OnlyContent);
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
