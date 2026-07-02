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
        public bool IsVisible => _isVisible;

        void Awake()
        {
            Instance = this;
        }

        public void Show(UIBattleSettingEditPanel parentPanel)
        {
            if (parentPanel == null)
                return;
            Show(parentPanel.transform);
        }

        public void Show(Transform parentRoot)
        {
            if (_isVisible || parentRoot == null) return;
            _isVisible = true;

            // Build UI
            BuildPanel(parentRoot);
        }

        public void Hide()
        {
            if (!_isVisible) return;
            _isVisible = false;

            if (_rootObject != null)
                Destroy(_rootObject);
            _rootObject = null;
        }

        public bool TryHandleBack()
        {
            if (!_isVisible)
                return false;
            Hide();
            return true;
        }

        private static string GetPanelText(string key, string zh, string en, string jp, string kr)
        {
            try
            {
                string text = TextDataModel.GetText(key);
                if (!string.IsNullOrEmpty(text) && text != key)
                    return text;
            }
            catch
            {
            }
            string lang = TextDataModel.CurrentLanguage.ToString().ToLowerInvariant();
            if (lang.Contains("en")) return en;
            if (lang.Contains("kr") || lang.Contains("ko")) return kr;
            if (lang.Contains("jp") || lang.Contains("ja")) return jp;
            return zh;
        }
        private void BuildPanel(Transform parentRoot)
        {
            RMRAbnormalityUnlockManager.RefreshRealizationProgress();

            _rootObject = new GameObject("RealizationPanel");
            _rootObject.transform.SetParent(parentRoot, false);

            // Background overlay
            var bg = ModdingUtils.CreateImage(_rootObject.transform, "MysteryPanel_transparent",
                new Vector2(1f, 1f), new Vector2(0f, 0f), new Vector2(1920f, 1080f));

            // Title
            var title = ModdingUtils.CreateText_TMP(_rootObject.transform,
                new Vector2(0f, 310f), 38,
                new Vector2(0.5f, 0.5f), new Vector2(1f, 1f),
                new Vector2(0f, 0f),
                TextAlignmentOptions.Center,
                LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
            title.text = GetPanelText("ui_RMR_RealizationTitle", "\u89e3\u653e\u6218", "Floor Realization Battles", "\u89e3\u653e\u6226", "\ud574\ubc29\uc804");

            // Subtitle
            var subtitle = ModdingUtils.CreateText_TMP(_rootObject.transform,
                new Vector2(0f, 265f), 22,
                new Vector2(0.5f, 0.5f), new Vector2(1f, 1f),
                new Vector2(0f, 0f),
                TextAlignmentOptions.Center,
                LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
            subtitle.text = GetPanelText("ui_RMR_RealizationDesc", "\u80dc\u5229\u540e\u6c38\u4e45\u89e3\u9501\u8be5\u5c42\u5f02\u60f3\u4f53\u4e66\u9875\u4e0eE.G.O\u4e66\u9875\u3002", "Victory permanently unlocks that floor's abnormality and E.G.O pages.", "\u52dd\u5229\u3059\u308b\u3068\u3001\u305d\u306e\u968e\u306e\u5e7b\u60f3\u4f53\u30da\u30fc\u30b8\u3068E.G.O\u30da\u30fc\u30b8\u3092\u6c38\u4e45\u89e3\u653e\u3059\u308b\u3002", "\uc2b9\ub9ac \uc2dc \ud574\ub2f9 \uce35\uc758 \ud658\uc0c1\uccb4 \ucc45\uc7a5\uacfc E.G.O \ucc45\uc7a5\uc744 \uc601\uad6c \ud574\uae08\ud569\ub2c8\ub2e4.");

            // Create floor buttons in a 2-column grid (5 rows x 2 cols)
            var floors = new List<SephirahType>
            {
                SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Hod, SephirahType.Netzach,
                SephirahType.Tiphereth, SephirahType.Gebura, SephirahType.Chesed, SephirahType.Binah,
                SephirahType.Hokma, SephirahType.Keter
            };

            float startX = -350f;
            float startY = 185f;
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
            closeTxt.text = GetPanelText("ui_RMR_RealizationClose", "\u5173\u95ed", "Close", "\u9589\u3058\u308b", "\ub2eb\uae30");
            closeBtn.onClick.AddListener(() => Hide());
        }

        private void CreateFloorButton(SephirahType floor, float x, float y)
        {
            bool completed = RMRAbnormalityUnlockManager.IsFloorRealizationCompleted(floor);
            string displayName = RMRRealizationManager.FloorDisplayNames.TryGetValue(floor, out string name)
                ? name : floor.ToString();
            string statusText = completed
                ? GetPanelText("ui_RMR_RealizationCleared", "[\u5df2\u5b8c\u6210]", "[CLEARED]", "[\u5b8c\u4e86]", "[\uc644\ub8cc]")
                : GetPanelText("ui_RMR_RealizationChallenge", "[\u6311\u6218]", "[CHALLENGE]", "[\u6311\u6226]", "[\ub3c4\uc804]");
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
