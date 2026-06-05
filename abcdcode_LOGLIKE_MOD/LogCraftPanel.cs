// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogCraftPanel
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;
using System;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class LogCraftPanel : Singleton<LogCraftPanel>
    {
        public GameObject root;
        public List<LogCraftPanel.LogueEffectImage_Craft> sprites;

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
                this.root = LogCraftPanel.GetLogUIObj(1);
                this.sprites = new List<LogCraftPanel.LogueEffectImage_Craft>();
                for (int index1 = 0; index1 < 7; ++index1)
                {
                    for (int index2 = 0; index2 < 8; ++index2)
                        this.sprites.Add(ModdingUtils.CreateImage(this.root.transform, (Sprite)null, new Vector2(1f, 1f), new Vector2((float)(index2 * 120 - 420), (float)(360 - index1 * 110)), new Vector2(90f, 90f)).gameObject.AddComponent<LogCraftPanel.LogueEffectImage_Craft>());
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
            List<GlobalLogueEffectBase> all1 = Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().FindAll(x => x is CraftEffect);
            List<GlobalLogueEffectBase> all2 = all1.FindAll(x => (x as CraftEffect).IsNormal());
            List<GlobalLogueEffectBase> all3 = all1.FindAll(x => !(x as CraftEffect).IsNormal());
            foreach (GlobalLogueEffectBase globalLogueEffectBase in all2)
            {
                CraftEffect effect = globalLogueEffectBase as CraftEffect;
                if (effect.GetCraftSprite() != null)
                {
                    this.sprites[index].Init(effect);
                    ++index;
                }
                if (index == this.sprites.Count)
                    break;
            }
            foreach (CraftEffect effect in all3)
            {
                if (effect.GetCraftSprite() != null)
                {
                    this.sprites[index].Init(effect);
                    ++index;
                }
                if (index == this.sprites.Count)
                    break;
            }
        }

        public class LogueEffectImage_Craft : MonoBehaviour
        {
            public CraftEffect effect;
            public UILogCustomSelectable selectable;
            public Sprite sprite;
            public Image image;
            public Image baseimage;
            public TextMeshProUGUI text;

            public void Init(CraftEffect effect)
            {
                if (effect == null)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    this.effect = effect;
                    this.image = this.gameObject.GetComponent<Image>();
                    int num;
                    if (this.text == null)
                    {
                        Color defFontColor = LogLikeMod.DefFontColor;
                        num = LogLikeMod.DefFont_TMP != null ? 1 : 0;
                    }
                    else
                        num = 0;
                    if (num != 0)
                        this.text = ModdingUtils.CreateText_TMP(this.gameObject.transform, new Vector2(0.0f, 0.0f), 30, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.BottomRight, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
                    if (effect.GetCraftSprite() == null)
                    {
                        this.Log("effect is null");
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (this.text != null)
                        {
                            int stack = effect.GetStack();
                            this.text.text = stack >= 0 ? stack.ToString() : string.Empty;
                        }
                        this.sprite = effect.GetCraftSprite();
                        this.image.sprite = this.sprite;
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
                        if (!(SingletonBehavior<UIMainOverlayManager>.Instance != null))
                            return;
                        SingletonBehavior<UIMainOverlayManager>.Instance.Close();
                    }
                }
            }

            public void OnClickImage()
            {
                if (this.effect.CanCraft((int)((double)this.effect.GetCraftCost() * (double)Singleton<GlobalLogueEffectManager>.Instance.CraftCostMultiple(this.effect))))
                    this.effect.Crafting();
            }

            public void OnEnterImage()
            {
                if (string.IsNullOrEmpty(this.effect.GetCraftDesc()))
                    return;
                int num = (int)((double)this.effect.GetCraftCost() * (double)Singleton<GlobalLogueEffectManager>.Instance.CraftCostMultiple(this.effect));
                string content = $"{TextDataModel.GetText("ui_CraftCost")}: {num.ToString()} {TextDataModel.GetText("ui_Money")}{Environment.NewLine}{this.effect.GetCraftDesc()}";
                this.SetTooltip(SingletonBehavior<UIMainOverlayManager>.Instance, this.effect.GetCraftName(), content, this.transform as RectTransform, UIToolTipPanelType.OnlyContent);
            }

            public void OnExitImage()
            {
                SingletonBehavior<UIMainOverlayManager>.Instance.Close();
            }

            public void SetTooltip(
              UIMainOverlayManager __instance,
              string name,
              string content,
              RectTransform rectTransform,
              UIToolTipPanelType panelType = UIToolTipPanelType.Normal)
            {
                __instance.GetType().GetMethod("Open", AccessTools.all).Invoke(__instance, (object[])null);
                TextMeshProUGUI fieldValue1 = LogLikeMod.GetFieldValue<TextMeshProUGUI>(__instance, "tooltipName");
                TextMeshProUGUI fieldValue2 = LogLikeMod.GetFieldValue<TextMeshProUGUI>(__instance, "tooltipDesc");
                fieldValue1.text = name;
                fieldValue1.rectTransform.sizeDelta = new Vector2(fieldValue1.rectTransform.sizeDelta.x, 20f);
                Camera camera = (Camera)null;
                if (rectTransform != null)
                {
                    Graphic componentInChildren = rectTransform.GetComponentInChildren<Graphic>();
                    if (componentInChildren != null && componentInChildren.canvas.renderMode == RenderMode.ScreenSpaceCamera)
                        camera = Camera.main;
                }
                string str = content;
                fieldValue2.text = str;
                TextMeshProMaterialSetter fieldValue3 = LogLikeMod.GetFieldValue<TextMeshProMaterialSetter>(__instance, "setter_tooltipname");
                fieldValue3.underlayColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                fieldValue1.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
                fieldValue3.enabled = false;
                fieldValue3.enabled = true;
                fieldValue1.enabled = false;
                fieldValue1.enabled = true;
                __instance.GetType().GetMethod("SetTooltipOverlayBoxSize", AccessTools.all).Invoke(__instance, new object[1]
                {
         panelType
                });
                __instance.GetType().GetMethod("SetTooltipOverlayBoxPosition", AccessTools.all).Invoke(__instance, new object[2]
                {
         camera,
         rectTransform
                });
            }
        }
    }
}
