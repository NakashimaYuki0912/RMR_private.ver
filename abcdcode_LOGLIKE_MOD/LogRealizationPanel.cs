using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using RogueLike_Mod_Reborn;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace abcdcode_LOGLIKE_MOD
{
    /// <summary>
    /// UI panel for selecting a floor to challenge in a realization battle.
    /// Displays all 10 floors with completion status.
    /// </summary>
    public class LogRealizationPanel : MonoBehaviour
    {
        public static LogRealizationPanel Instance { get; private set; }

        private GameObject _rootObject;
        private bool _isVisible;

        void Awake()
        {
            Instance = this;
        }

        public void Show(UIBattleSettingEditPanel parentPanel)
        {
            if (_isVisible) return;
            _isVisible = true;

            // Build UI
            BuildPanel(parentPanel);
        }

        public void Hide()
        {
            if (!_isVisible) return;
            _isVisible = false;

            if (_rootObject != null)
                Destroy(_rootObject);
            _rootObject = null;
        }

        private void BuildPanel(UIBattleSettingEditPanel parentPanel)
        {
            _rootObject = new GameObject("RealizationPanel");
            _rootObject.transform.SetParent(parentPanel.transform, false);

            // Background overlay
            var bg = ModdingUtils.CreateImage(_rootObject.transform, "MysteryPanel_transparent",
                new Vector2(1f, 1f), new Vector2(0f, 0f), new Vector2(1920f, 1080f));

            // Title
            var title = ModdingUtils.CreateText_TMP(_rootObject.transform,
                new Vector2(0f, 400f), 48,
                new Vector2(0.5f, 0.5f), new Vector2(1f, 1f),
                new Vector2(0f, 0f),
                TextAlignmentOptions.Center,
                LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
            title.text = "Floor Realization Battles";

            // Subtitle
            var subtitle = ModdingUtils.CreateText_TMP(_rootObject.transform,
                new Vector2(0f, 350f), 28,
                new Vector2(0.5f, 0.5f), new Vector2(1f, 1f),
                new Vector2(0f, 0f),
                TextAlignmentOptions.Center,
                LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
            subtitle.text = "Complete a floor's realization battle to permanently unlock its exclusive pages.";

            // Create floor buttons in a 2-column grid (5 rows x 2 cols)
            var floors = new List<SephirahType>
            {
                SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Hod, SephirahType.Netzach,
                SephirahType.Tiphereth, SephirahType.Gebura, SephirahType.Chesed, SephirahType.Binah,
                SephirahType.Hokma, SephirahType.Keter
            };

            float startX = -350f;
            float startY = 250f;
            float spacingX = 700f;
            float spacingY = -90f;

            for (int i = 0; i < floors.Count; i++)
            {
                int col = i % 2;
                int row = i / 2;
                float x = startX + col * spacingX;
                float y = startY + row * spacingY;

                var floor = floors[i];
                CreateFloorButton(floor, x, y);
            }

            // Close button
            var closeBtn = ModdingUtils.CreateButton(_rootObject.transform, "MysteryButton_Enable",
                new Vector2(1f, 1f), new Vector2(580f, -480f), new Vector2(200f, 60f));
            var closeTxt = ModdingUtils.CreateText_TMP(closeBtn.transform,
                Vector2.zero, 24, new Vector2(0.5f, 0.5f), new Vector2(1f, 1f),
                Vector2.zero, TextAlignmentOptions.Center,
                LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
            closeTxt.text = "Close";
            closeBtn.onClick.AddListener(() => Hide());
        }

        private void CreateFloorButton(SephirahType floor, float x, float y)
        {
            bool completed = RMRAbnormalityUnlockManager.IsFloorRealizationCompleted(floor);
            string displayName = RMRRealizationManager.FloorDisplayNames.TryGetValue(floor, out string name)
                ? name : floor.ToString();
            string statusText = completed ? "[CLEARED]" : "[CHALLENGE]";
            string fullText = $"{statusText}  {displayName}";

            var btnGo = new GameObject("FloorBtn_" + floor.ToString());
            btnGo.transform.SetParent(_rootObject.transform, false);

            var btnImg = btnGo.AddComponent<Image>();
            btnImg.color = completed ? new Color(0.2f, 0.5f, 0.2f, 0.8f) : new Color(0.3f, 0.3f, 0.5f, 0.8f);

            var btnRect = btnGo.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.5f);
            btnRect.anchorMax = new Vector2(0.5f, 0.5f);
            btnRect.sizeDelta = new Vector2(600f, 70f);
            btnRect.anchoredPosition = new Vector2(x, y);

            var btn = btnGo.AddComponent<Button>();
            btn.targetGraphic = btnImg;

            SephirahType capturedFloor = floor;
            btn.onClick.AddListener(() =>
            {
                UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click);
                if (!completed)
                {
                    Hide();
                    RMRRealizationManager.StartRealizationBattle(capturedFloor);
                }
            });

            // Disable interaction if already completed
            if (completed)
                btn.interactable = false;

            // Button text
            var label = ModdingUtils.CreateText_TMP(btnGo.transform,
                Vector2.zero, 22,
                new Vector2(0.5f, 0.5f), new Vector2(1f, 1f),
                Vector2.zero,
                TextAlignmentOptions.Center,
                completed ? new Color(0.7f, 1f, 0.7f) : Color.white,
                LogLikeMod.DefFont_TMP);
            label.text = fullText;
        }
    }
}
