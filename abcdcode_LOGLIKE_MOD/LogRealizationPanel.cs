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

        /// <summary>
        /// When set, floor click only invokes this callback (invitation-time pick).
        /// When null, floor click starts realization battle immediately (in-run fallback).
        /// </summary>
        private Action<SephirahType> _onFloorPicked;

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
            ShowInternal(parentRoot, null);
        }

        /// <summary>
        /// Invitation-time floor pick: choosing a floor does NOT start battle yet;
        /// the callback stores the floor and continues invitation send.
        /// </summary>
        public void ShowForInvitationPick(Transform parentRoot, Action<SephirahType> onFloorPicked)
        {
            ShowInternal(parentRoot, onFloorPicked);
        }

        private void ShowInternal(Transform parentRoot, Action<SephirahType> onFloorPicked)
        {
            if (parentRoot == null)
                return;
            if (_isVisible)
                Hide();
            _onFloorPicked = onFloorPicked;
            _isVisible = true;
            try
            {
                LogLikeMod.InvalidateTmpFontCache();
                var _ = LogLikeMod.DefFont_TMP;
            }
            catch { }
            BuildPanel(parentRoot);
            if (_rootObject != null)
                _rootObject.transform.SetAsLastSibling();
        }

        public void Hide()
        {
            if (!_isVisible) return;
            _isVisible = false;
            _onFloorPicked = null;

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

            // Fullscreen dim (explicit RectTransform so layout is not zero-size).
            var dimGo = new GameObject("Dim", typeof(RectTransform));
            dimGo.transform.SetParent(_rootObject.transform, false);
            var dimImg = dimGo.AddComponent<Image>();
            dimImg.color = new Color(0.05f, 0.04f, 0.03f, 0.9f);
            var dimRt = dimGo.GetComponent<RectTransform>();
            dimRt.anchorMin = Vector2.zero;
            dimRt.anchorMax = Vector2.one;
            dimRt.offsetMin = Vector2.zero;
            dimRt.offsetMax = Vector2.zero;

            TMP_FontAsset font = LogLikeMod.DefFont_TMP;

            // Title
            var title = ModdingUtils.CreateText_TMP(_rootObject.transform,
                new Vector2(0f, 310f), 38,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0f, 0f),
                TextAlignmentOptions.Center,
                LogLikeMod.DefFontColor, font);
            title.rectTransform.sizeDelta = new Vector2(900f, 50f);
            title.enableWordWrapping = true;
            title.text = GetPanelText("ui_RMR_RealizationTitle", "\u89e3\u653e\u6218", "Floor Realization Battles", "\u89e3\u653e\u6226", "\ud574\ubc29\uc804");

            // Subtitle
            var subtitle = ModdingUtils.CreateText_TMP(_rootObject.transform,
                new Vector2(0f, 255f), 20,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0f, 0f),
                TextAlignmentOptions.Center,
                LogLikeMod.DefFontColor, font);
            subtitle.rectTransform.sizeDelta = new Vector2(1000f, 70f);
            subtitle.enableWordWrapping = true;
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
                ? GetPanelText("ui_RMR_RealizationReplay", "[\u5df2\u5b8c\u6210 \u00b7 \u53ef\u518d\u6218]", "[CLEARED · REPLAY]", "[\u5b8c\u4e86\u30fb\u518d\u6226]", "[\uc644\ub8cc \u00b7 \uc7ac\uc804]")
                : GetPanelText("ui_RMR_RealizationChallenge", "[\u6311\u6218]", "[CHALLENGE]", "[\u6311\u6226]", "[\ub3c4\uc804]");
            string fullText = $"{statusText}  {displayName}";

            var btnGo = new GameObject("FloorBtn_" + floor.ToString());
            btnGo.transform.SetParent(_rootObject.transform, false);

            var btnImg = btnGo.AddComponent<Image>();
            btnImg.color = completed ? new Color(0.25f, 0.4f, 0.28f, 0.85f) : new Color(0.35f, 0.3f, 0.22f, 0.85f);

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
                Action<SephirahType> inviteCb = _onFloorPicked;
                Hide();
                if (inviteCb != null)
                {
                    // Invitation-time: only record floor; battle starts after reception bootstraps.
                    inviteCb(capturedFloor);
                    return;
                }
                // In-run / post-invitation fallback: start boss prepare now.
                RMRRealizationManager.StartRealizationBattle(capturedFloor);
            });

            var label = ModdingUtils.CreateText_TMP(btnGo.transform,
                Vector2.zero, 22,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                Vector2.zero,
                TextAlignmentOptions.Center,
                completed ? new Color(0.75f, 0.9f, 0.75f) : LogLikeMod.DefFontColor,
                LogLikeMod.DefFont_TMP);
            label.rectTransform.sizeDelta = new Vector2(560f, 60f);
            label.enableWordWrapping = false;
            label.overflowMode = TextOverflowModes.Overflow;
            label.text = fullText;
        }
    }
}
