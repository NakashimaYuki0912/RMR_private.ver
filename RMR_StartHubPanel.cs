using System;
using System.Collections;
using System.Collections.Generic;
using abcdcode_LOGLIKE_MOD;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RogueLike_Mod_Reborn
{
    /// <summary>
    /// What the player chose on the invitation-time start hub.
    /// Consumed once the reception actually starts (OnWaveStartInitialEvent).
    /// </summary>
    public enum RMRLaunchIntent
    {
        None = 0,
        NormalPlay = 1,
        Realization = 2,
    }

    /// <summary>
    /// RMR mode select: shown right after clicking the mod entry on the invitation panel,
    /// BEFORE ConfirmSendInvitation / in-run battle prepare.
    /// </summary>
    public class RMRStartHubPanel : MonoBehaviour
    {
        public static RMRStartHubPanel Instance { get; private set; }

        /// <summary>
        /// Mirror of durable intent on <see cref="RMRRealizationManager.PendingLaunchIntent"/>.
        /// Prefer manager APIs; this stays for legacy readers.
        /// </summary>
        public static RMRLaunchIntent LaunchIntent { get; private set; }

        private GameObject _root;
        private GameObject _confirmRoot;
        private bool _visible;
        private UIInvitationRightMainPanel _invitationPanel;
        public bool IsVisible => _visible;

        void Awake()
        {
            Instance = this;
        }

        /// <summary>Called only from RealizationManager so hub/manager stay in sync.</summary>
        public static void SyncLaunchIntentMirror(RMRLaunchIntent intent)
        {
            LaunchIntent = intent;
        }

        public static void ClearLaunchIntent()
        {
            // Intent only — keep PendingRealizationFloor while invite is in flight.
            RMRRealizationManager.ClearLaunchIntentOnly();
        }

        public static void SetLaunchIntent(RMRLaunchIntent intent)
        {
            RMRRealizationManager.SetLaunchIntent(intent);
        }

        /// <summary>
        /// Open hub as a dedicated full-screen page (not nested under the right invitation column).
        /// Still keeps a reference to the invitation panel so ConfirmSendInvitation works.
        /// </summary>
        public static void ShowOnInvitation(UIInvitationRightMainPanel invitation)
        {
            if (invitation == null)
            {
                Debug.LogError("[RMRStartHubPanel] ShowOnInvitation: invitation panel is null.");
                return;
            }
            if (Instance == null)
            {
                GameObject go = new GameObject("RMRStartHubPanel");
                DontDestroyOnLoad(go);
                Instance = go.AddComponent<RMRStartHubPanel>();
            }
            Instance._invitationPanel = invitation;
            // Parent is resolved inside ShowInternal AFTER PrepareNewHubSession
            // (Prepare destroys any prior overlay, so creating it first would attach UI to a dead transform).
            Instance.ShowInternal(null);
        }

        /// <summary>Legacy in-run show — blocked during realization; prefer ShowOnInvitation.</summary>
        public static void ShowOrCreate()
        {
            if (RMRRealizationManager.InRealizationBattle
                || RMRRealizationManager.PendingRealizationBattle
                || RMRRealizationManager.RealizationCombatLive)
            {
                Debug.LogWarning("[RMRStartHubPanel] ShowOrCreate blocked — realization in progress.");
                return;
            }
            // After realization we return to the library main page; do not re-open mode select in-run.
            Debug.LogWarning("[RMRStartHubPanel] ShowOrCreate ignored — hub is invitation-time only.");
        }

        public void Show()
        {
            // Keep for API compatibility; invitation path uses full-screen overlay.
            if (_invitationPanel != null)
                ShowInternal(null);
        }

        private void ShowInternal(Transform ignoredParent)
        {
            // Tear down previous hub UI only (do not DestroyOverlayCompletely here —
            // PrepareNewHubSession already clears stale floor/overlay state).
            if (_root != null)
            {
                Destroy(_root);
                _root = null;
            }
            _confirmRoot = null;
            _visible = false;

            // Fresh hub: wipe leftover intent/floor from a previous cancelled attempt.
            // NOTE: this destroys RMR_OverlayCanvas — must recreate AFTER this call.
            RMRRealizationManager.PrepareNewHubSession();
            EnsureChineseCapableFont();

            Transform parent = RMRRealizationLaunchHost.GetOrCreateOverlayRoot();
            if (parent == null)
            {
                Debug.LogError("[RMRStartHubPanel] Overlay root is null — cannot show hub.");
                return;
            }

            BuildUi(parent);
            _visible = true;
            Debug.Log("[RMRStartHubPanel] Full-screen hub shown on overlay.");
            // Key pages + story TMP after mod entry — prevent Hangul tofu on library equip / PV.
            try { LogLikePatches.RefreshKeypageAndStoryLocalize("StartHub.Show"); } catch { }
            StartCoroutine(RebuildIfFontImproves());
            StartCoroutine(DelayedFontRepairAfterHub());
        }

        private System.Collections.IEnumerator DelayedFontRepairAfterHub()
        {
            yield return null;
            yield return null;
            try { LogLikeMod.EnsureLocalizedFonts("StartHub.delayed", repairActiveUi: true); } catch { }
        }

        private static void EnsureChineseCapableFont()
        {
            try
            {
                LogLikeMod.InvalidateTmpFontCache();
                TMP_FontAsset font = LogLikeMod.DefFont_TMP;
                if (font == null)
                    Debug.LogWarning("[RMRStartHubPanel] No CJK-capable TMP font resolved.");
                else
                    Debug.Log($"[RMRStartHubPanel] Using TMP font '{font.name}'.");
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMRStartHubPanel] EnsureChineseCapableFont: " + ex.Message);
            }
        }

        private IEnumerator RebuildIfFontImproves()
        {
            TMP_FontAsset before = LogLikeMod.DefFont_TMP;
            yield return null;
            yield return null;
            LogLikeMod.InvalidateTmpFontCache();
            TMP_FontAsset after = LogLikeMod.DefFont_TMP;
            if (after != null && !object.ReferenceEquals(after, before) && _visible)
            {
                if (_root != null)
                    Destroy(_root);
                _root = null;
                _confirmRoot = null;
                Transform parent = RMRRealizationLaunchHost.GetOrCreateOverlayRoot();
                if (parent != null)
                    BuildUi(parent);
            }
        }

        public void Hide()
        {
            if (!_visible && _root == null)
                return;
            _visible = false;
            try { RMRHelpHandbookPanel.Instance?.Hide(); } catch { }
            try
            {
                var atlas = Singleton<LogAtlasPanel>.Instance;
                if (atlas != null && atlas.root != null && atlas.root.activeSelf)
                    atlas.CloseFromHub();
            }
            catch { }
            if (_root != null)
                Destroy(_root);
            _root = null;
            _confirmRoot = null;
            // Drop empty overlay so it does not block library clicks after hub close.
            try { RMRRealizationLaunchHost.DestroyOverlayIfEmpty(); } catch { }
        }

        public bool TryHandleBack()
        {
            if (!_visible)
                return false;
            // Atlas open on top of hub → close atlas first.
            try
            {
                var atlas = Singleton<LogAtlasPanel>.Instance;
                if (atlas != null && atlas.TryHandleBack())
                    return true;
            }
            catch { }
            if (_confirmRoot != null)
            {
                Destroy(_confirmRoot);
                _confirmRoot = null;
                return true;
            }
            // Invitation hub: back = close hub only (stay on invitation).
            Hide();
            return true;
        }

        private static string T(string key, string zh, string en = null, string kr = null)
        {
            try
            {
                string text = TextDataModel.GetText(key);
                if (!string.IsNullOrEmpty(text) && text != key)
                    return text;
            }
            catch { }
            string lang = TextDataModel.CurrentLanguage.ToString().ToLowerInvariant();
            if (lang.Contains("en") && !string.IsNullOrEmpty(en))
                return en;
            if ((lang.Contains("kr") || lang.Contains("ko")) && !string.IsNullOrEmpty(kr))
                return kr;
            return zh;
        }

        // Full-screen black page + centered parchment menu card.
        private static readonly Color ColPageBlack = new Color(0f, 0f, 0f, 1f);
        private static readonly Color ColPanel = new Color(0.12f, 0.10f, 0.08f, 0.98f);
        private static readonly Color ColPanelEdge = new Color(0.55f, 0.42f, 0.22f, 0.95f);
        private static readonly Color ColGold = new Color(0.93f, 0.76f, 0.42f, 1f);
        private static readonly Color ColCream = new Color(0.93f, 0.88f, 0.78f, 1f);
        private static readonly Color ColMuted = new Color(0.72f, 0.66f, 0.55f, 1f);
        private static readonly Color ColExit = new Color(0.82f, 0.55f, 0.52f, 1f);

        private void BuildUi(Transform parent)
        {
            if (parent == null)
                parent = RMRRealizationLaunchHost.GetOrCreateOverlayRoot();

            _root = new GameObject("StartHubRoot", typeof(RectTransform));
            _root.transform.SetParent(parent, false);
            _root.transform.SetAsLastSibling();

            RectTransform rootRt = _root.GetComponent<RectTransform>();
            rootRt.anchorMin = Vector2.zero;
            rootRt.anchorMax = Vector2.one;
            rootRt.offsetMin = Vector2.zero;
            rootRt.offsetMax = Vector2.zero;

            // Fullscreen pure-black page (covers invitation / library behind it).
            var dimGo = new GameObject("BlackPage", typeof(RectTransform));
            dimGo.transform.SetParent(_root.transform, false);
            var dimImg = dimGo.AddComponent<Image>();
            dimImg.color = ColPageBlack;
            var dimRt = dimGo.GetComponent<RectTransform>();
            dimRt.anchorMin = Vector2.zero;
            dimRt.anchorMax = Vector2.one;
            dimRt.offsetMin = Vector2.zero;
            dimRt.offsetMax = Vector2.zero;
            dimImg.raycastTarget = true;

            // Centered gold rim + menu card (same options as before).
            var frame = new GameObject("Frame", typeof(RectTransform));
            frame.transform.SetParent(_root.transform, false);
            var frameImg = frame.AddComponent<Image>();
            frameImg.color = ColPanelEdge;
            var frameRt = frame.GetComponent<RectTransform>();
            frameRt.anchorMin = frameRt.anchorMax = new Vector2(0.5f, 0.5f);
            frameRt.sizeDelta = new Vector2(680f, 680f);
            frameRt.anchoredPosition = Vector2.zero;

            var card = new GameObject("Card", typeof(RectTransform));
            card.transform.SetParent(frame.transform, false);
            var cardImg = card.AddComponent<Image>();
            cardImg.color = ColPanel;
            var cardRt = card.GetComponent<RectTransform>();
            cardRt.anchorMin = Vector2.zero;
            cardRt.anchorMax = Vector2.one;
            cardRt.offsetMin = new Vector2(3f, 3f);
            cardRt.offsetMax = new Vector2(-3f, -3f);

            AddRule(card.transform, new Vector2(0f, 232f), 520f);

            AddLabel(card.transform,
                T("ui_RMR_HubTitle", "Roguelike \u5f00\u5c40", "Roguelike Start", "Roguelike \uc2dc\uc791"),
                new Vector2(0f, 200f), 42, new Vector2(560f, 56f), ColGold, true);
            AddLabel(card.transform,
                T("ui_RMR_HubDesc",
                    "\u9009\u62e9\u5f00\u5c40\u65b9\u5f0f\u3002\u89e3\u653e\u6218\u53ef\u6c38\u4e45\u89e3\u9501\u697c\u5c42\u5956\u52b1\uff1b\u6b63\u5e38\u6e38\u73a9\u8fdb\u5165\u8089\u9e3d\u8def\u7ebf\u3002",
                    "Choose how to begin. Realization unlocks permanent rewards; Normal Play starts a run.",
                    "\uc2dc\uc791 \ubc29\uc2dd\uc744 \uc120\ud0dd\ud558\uc138\uc694. \ud574\ubc29\uc804\uc740 \uc601\uad6c \ud574\uae08, \uc77c\ubc18 \ud50c\ub808\uc774\ub294 \ub7f0\uc744 \uc2dc\uc791\ud569\ub2c8\ub2e4."),
                new Vector2(0f, 140f), 20, new Vector2(540f, 72f), ColMuted, false);

            AddRule(card.transform, new Vector2(0f, 92f), 460f);

            // Continue lives here (after RMR entry → mode select), not as a lone invitation icon.
            bool hasSave = false;
            try { hasSave = LoguePlayDataSaver.CheckPlayerData(); } catch { hasSave = false; }

            // Buttons: Play, [Continue], Realization, Help, Atlas, Reset, Exit.
            // Atlas sits above Reset (user request). Grow card height for the extra row.
            int btnCount = hasSave ? 7 : 6;
            float step = 62f;
            float cardH = 520f + btnCount * step;
            frameRt.sizeDelta = new Vector2(680f, cardH);

            // Top action cluster starts below title/desc.
            float y = hasSave ? 78f : 56f;
            AddMenuButton(card.transform,
                T("ui_RMR_Hub_Play", "\u6b63\u5e38\u6e38\u73a9", "Normal Play", "\uc77c\ubc18 \ud50c\ub808\uc774"),
                y, OnClickPlay, primary: true);
            y -= step;
            if (hasSave)
            {
                AddMenuButton(card.transform,
                    T("ui_RMR_ContinueRun", "\u7ee7\u7eed\u65c5\u7a0b", "Continue Run", "\uc774\uc5b4\ud558\uae30"),
                    y, OnClickContinue, primary: true);
                y -= step;
            }
            AddMenuButton(card.transform,
                T("ui_RMR_Hub_Realization", "\u6311\u6218\u89e3\u653e\u6218", "Challenge Realization", "\ud574\ubc29\uc804 \ub3c4\uc804"),
                y, OnClickRealization, primary: false);
            y -= step;
            AddMenuButton(card.transform,
                T("ui_RMR_Hub_Help", "\u73a9\u6cd5\u4ecb\u7ecd", "How to Play", "\ud50c\ub808\uc774 \uc18c\uac1c"),
                y, OnClickHelp, primary: false);
            y -= step;
            // 图鉴 — permanent collection browser (moved out of battle prepare).
            AddMenuButton(card.transform,
                T("ui_RMR_Hub_Atlas", "\u56fe\u9274", "Atlas", "\ub3c4\uac10"),
                y, OnClickAtlas, primary: false);
            y -= step;
            AddMenuButton(card.transform,
                T("ui_RMR_Hub_Reset", "\u91cd\u7f6e\u6c38\u4e45\u8fdb\u5ea6", "Reset Permanent Progress", "\uc601\uad6c \uc9c4\ud589 \ucd08\uae30\ud654"),
                y, OnClickResetProgress, primary: false, exitStyle: true);
            y -= step;
            AddMenuButton(card.transform,
                T("ui_RMR_Hub_Exit", "\u9000\u51fa", "Exit", "\uc885\ub8cc"),
                y, OnClickExit, primary: false, exitStyle: true);

            float footerY = y - 48f;
            AddRule(card.transform, new Vector2(0f, footerY + 18f), 460f);
            AddLabel(card.transform,
                T("ui_RMR_HubFooter", "Library of Ruina \u00b7 Roguelike Mod Reborn",
                    "Library of Ruina \u00b7 Roguelike Mod Reborn",
                    "Library of Ruina \u00b7 Roguelike Mod Reborn"),
                new Vector2(0f, footerY), 14, new Vector2(540f, 28f), ColMuted, false);
        }

        private static void AddRule(Transform parent, Vector2 pos, float width)
        {
            var go = new GameObject("Rule", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var img = go.AddComponent<Image>();
            img.color = new Color(ColPanelEdge.r, ColPanelEdge.g, ColPanelEdge.b, 0.55f);
            img.raycastTarget = false;
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(width, 2f);
            rt.anchoredPosition = pos;
        }

        private static TextMeshProUGUI AddLabel(Transform parent, string text, Vector2 pos, int size, Vector2 box, Color color, bool bold)
        {
            var go = new GameObject("Label", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = box;
            rt.anchoredPosition = pos;
            // Noto CJKsc SDF has no real bold face — FontStyles.Bold is synthetic and turns CN mushy/blurry.
            // Prefer size weight over pseudo-bold; material must match the Noto atlas.
            TMP_FontAsset font = LogLikeMod.DefFont_TMP;
            LogLikeMod.ApplyTmpFontPreservingSharpMaterial(tmp, font);
            tmp.fontSize = bold ? size + 2 : size;
            tmp.color = color;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableWordWrapping = true;
            tmp.overflowMode = TextOverflowModes.Overflow;
            tmp.fontStyle = FontStyles.Normal;
            tmp.richText = false;
            try { tmp.enableAutoSizing = false; } catch { /* older TMP */ }
            tmp.text = text;
            return tmp;
        }

        private void AddMenuButton(Transform parent, string label, float y, UnityAction action, bool primary, bool exitStyle = false)
        {
            Button btn = null;
            // Prefer mod mystery button art (same as event choices / close buttons).
            try
            {
                if (LogLikeMod.ArtWorks != null && LogLikeMod.ArtWorks.ContainsKey("MysteryButton_Enable"))
                {
                    btn = ModdingUtils.CreateButton(parent, "MysteryButton_Enable",
                        Vector2.one, new Vector2(0f, y), new Vector2(460f, 64f));
                }
            }
            catch
            {
                btn = null;
            }

            if (btn == null)
            {
                var go = new GameObject("HubBtn", typeof(RectTransform));
                go.transform.SetParent(parent, false);
                var img = go.AddComponent<Image>();
                img.color = primary
                    ? new Color(0.38f, 0.30f, 0.18f, 0.98f)
                    : new Color(0.28f, 0.23f, 0.17f, 0.98f);
                var rt = go.GetComponent<RectTransform>();
                rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.sizeDelta = new Vector2(460f, 64f);
                rt.anchoredPosition = new Vector2(0f, y);
                btn = go.AddComponent<Button>();
                btn.targetGraphic = img;

                // Gold outline
                var edge = new GameObject("Edge", typeof(RectTransform));
                edge.transform.SetParent(go.transform, false);
                edge.transform.SetAsFirstSibling();
                var eImg = edge.AddComponent<Image>();
                eImg.color = primary ? ColGold : ColPanelEdge;
                eImg.raycastTarget = false;
                var eRt = edge.GetComponent<RectTransform>();
                eRt.anchorMin = Vector2.zero;
                eRt.anchorMax = Vector2.one;
                eRt.offsetMin = new Vector2(-2f, -2f);
                eRt.offsetMax = new Vector2(2f, 2f);
            }

            Color textColor = exitStyle ? ColExit : (primary ? ColGold : ColCream);
            var textGo = new GameObject("Text", typeof(RectTransform));
            textGo.transform.SetParent(btn.transform, false);
            var tmp = textGo.AddComponent<TextMeshProUGUI>();
            var trt = textGo.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero;
            trt.anchorMax = Vector2.one;
            trt.offsetMin = new Vector2(16f, 6f);
            trt.offsetMax = new Vector2(-16f, -6f);
            // No FontStyles.Bold: CJK Noto SDF + synthetic bold = unreadable blur on hub buttons.
            LogLikeMod.ApplyTmpFontPreservingSharpMaterial(tmp, LogLikeMod.DefFont_TMP);
            tmp.fontSize = primary ? 30 : 28;
            tmp.color = textColor;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableWordWrapping = false;
            tmp.overflowMode = TextOverflowModes.Overflow;
            tmp.fontStyle = FontStyles.Normal;
            tmp.richText = false;
            try { tmp.enableAutoSizing = false; } catch { /* older TMP */ }
            tmp.text = label;

            // Hover tint
            try
            {
                var colors = btn.colors;
                colors.normalColor = Color.white;
                colors.highlightedColor = new Color(1.05f, 1.0f, 0.9f, 1f);
                colors.pressedColor = new Color(0.85f, 0.8f, 0.7f, 1f);
                colors.selectedColor = colors.highlightedColor;
                colors.fadeDuration = 0.08f;
                btn.colors = colors;
            }
            catch { }

            btn.onClick.AddListener(() =>
            {
                try { UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click); } catch { }
                try { action(); }
                catch (Exception ex) { Debug.LogError("[RMRStartHubPanel] " + ex); }
            });
        }

        private bool EnsureInvitationReady()
        {
            if (_invitationPanel != null)
                return true;
            Debug.LogError("[RMRStartHubPanel] Invitation panel missing — open hub from mod entry only.");
            return false;
        }

        /// <summary>
        /// Send invitation. Only roll back intent when we fail BEFORE ConfirmSendInvitation.
        /// Never treat mid-launch story exceptions as "failed invite" (that re-opened hub and broke normal play).
        /// </summary>
        private void SendInvitationAndCloseHub()
        {
            if (!EnsureInvitationReady())
            {
                RMRRealizationManager.RollbackLaunchIntentKeepHub();
                return;
            }
            List<string> except;
            if (LogLikeMod.CheckExceptionModExist(out except))
            {
                string text = TextDataModel.GetText("ui_ExceptionWithLog") + Environment.NewLine;
                foreach (string str in except)
                    text = text + "-" + str + Environment.NewLine;
                UIAlarmPopup.instance.SetAlarmText(text);
                RMRRealizationManager.RollbackLaunchIntentKeepHub();
                return;
            }

            bool first = true;
            _invitationPanel.SetCustomInvToggle(true);
            foreach (UIInvitationBookSlot slot in _invitationPanel.invitationbookSlots)
            {
                if (first)
                    slot.ApplySlotid(new LorId(LogLikeMod.ModId, -853), true);
                else
                    slot.SetEmptySlot();
                first = false;
            }
            // New reception: allow OnWaveStartInitialEvent to run once for this invite.
            RMRCore.ResetPostInvitationLaunchGate();
            Hide();
            // Do not wrap ConfirmSendInvitation in catch-all rollback: chapter intro / story
            // may throw non-fatal access exceptions after the run has already started.
            _invitationPanel.ConfirmSendInvitation();
        }

        private void OnClickPlay()
        {
            // Mark intent only here. StartNormalPlayFromHub runs after reception boots
            // (HandlePostInvitationLaunch) so a failed pre-send check does not dirty hub state.
            RMRRealizationManager.ClearPendingRealizationFloor();
            SetLaunchIntent(RMRLaunchIntent.NormalPlay);
            SendInvitationAndCloseHub();
        }

        /// <summary>
        /// Continue a saved run — same -855 recipe as the old invitation-side Continue icon,
        /// but placed in the mode-select hub after RMR entry.
        /// </summary>
        private void OnClickContinue()
        {
            if (!EnsureInvitationReady())
                return;

            bool hasSave = false;
            try { hasSave = LoguePlayDataSaver.CheckPlayerData(); } catch { hasSave = false; }
            if (!hasSave)
            {
                try
                {
                    UIAlarmPopup.instance?.SetAlarmText(
                        TextDataModel.GetText("ui_RMR_ContinueFailed")
                        ?? "No saved Roguelike run to continue.");
                }
                catch { }
                return;
            }

            List<string> except;
            if (LogLikeMod.CheckExceptionModExist(out except))
            {
                string text = TextDataModel.GetText("ui_ExceptionWithLog") + Environment.NewLine;
                foreach (string str in except)
                    text = text + "-" + str + Environment.NewLine;
                try { UIAlarmPopup.instance.SetAlarmText(text); } catch { }
                return;
            }

            // Continue must not leave realization bootstrap flags sticky.
            try { RMRRealizationManager.EndHubSessionToLibrary(); } catch { }
            try { RMRRealizationManager.ClearLaunchIntentOnly(); } catch { }
            RMRCore.ResetPostInvitationLaunchGate();

            bool first = true;
            _invitationPanel.SetCustomInvToggle(true);
            foreach (UIInvitationBookSlot slot in _invitationPanel.invitationbookSlots)
            {
                if (first)
                    slot.ApplySlotid(new LorId(LogLikeMod.ModId, -855), true);
                else
                    slot.SetEmptySlot();
                first = false;
            }
            Hide();
            Debug.Log("[RMRStartHubPanel] Continue run from hub.");
            _invitationPanel.ConfirmSendInvitation();
        }

        private void OnClickRealization()
        {
            // Flow: hub → dedicated floor-pick page → (after pick) invite → team prep → battle.
            // Never skip floor pick; never open vanilla prepare before a floor is chosen.
            Debug.Log("[RMRStartHubPanel] Realization button clicked.");
            RMRRealizationManager.BeginHubSession();
            RMRRealizationManager.ClearPendingRealizationFloor();
            SetLaunchIntent(RMRLaunchIntent.Realization);
            try
            {
                // Soft-hide hub so the floor page reads as a real navigation step.
                // Do not Destroy hub — Close on floor UI must return here.
                if (_root != null)
                    _root.SetActive(false);

                RMRRealizationLaunchHost.ShowDedicatedFloorPick(floor =>
                {
                    RMRRealizationManager.PendingRealizationFloor = floor;
                    RMRRealizationManager.BeginHubSession();
                    SetLaunchIntent(RMRLaunchIntent.Realization);
                    Debug.Log($"[RMRStartHubPanel] Realization floor chosen on dedicated UI: {floor}");
                    // Hide hub after pick; invite sends and boots realization stage for that floor.
                    SendInvitationAndCloseHub();
                });
            }
            catch (Exception ex)
            {
                Debug.LogError("[RMRStartHubPanel] Dedicated floor UI failed: " + ex);
                if (_root != null)
                    _root.SetActive(true);
                // Do not auto-send invite without floor — that skipped the required pick page.
            }
        }

        /// <summary>Re-show hub after invitation-time floor pick is cancelled.</summary>
        public void RestoreHubAfterFloorPickCancel()
        {
            try
            {
                if (_root != null)
                    _root.SetActive(true);
                _visible = true;
            }
            catch { }
        }

        private void OnClickHelp()
        {
            // Full-screen page (same black overlay host) so handbook text is not clipped by invite column.
            Transform parent = RMRRealizationLaunchHost.GetOrCreateOverlayRoot();
            RMRHelpHandbookPanel.ShowOrCreate(parent);
        }

        private void OnClickAtlas()
        {
            // Soft-hide hub menu; atlas “返回” restores it (same pattern as realization floor pick).
            Debug.Log("[RMRStartHubPanel] Atlas button clicked.");
            try
            {
                if (_root != null)
                    _root.SetActive(false);

                LogAtlasPanel atlas = null;
                try { atlas = Singleton<LogAtlasPanel>.Instance; } catch { atlas = null; }
                if (atlas == null)
                {
                    // Singleton may not auto-create outside battle UI — construct explicitly.
                    try
                    {
                        atlas = (LogAtlasPanel)Activator.CreateInstance(typeof(LogAtlasPanel), true);
                        // Reflect-set Instance if needed
                        var prop = typeof(Singleton<LogAtlasPanel>).GetProperty("Instance",
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy);
                        // Some Singleton variants use a private field `_instance`
                        var field = typeof(Singleton<LogAtlasPanel>).GetField("_instance",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy)
                            ?? typeof(Singleton<LogAtlasPanel>).GetField("instance",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy);
                        if (field != null)
                            field.SetValue(null, atlas);
                    }
                    catch (Exception createEx)
                    {
                        Debug.LogError("[RMRStartHubPanel] Create LogAtlasPanel failed: " + createEx);
                        throw;
                    }
                }

                atlas.ShowFromHub(() =>
                {
                    try
                    {
                        if (_root != null)
                            _root.SetActive(true);
                        _visible = true;
                    }
                    catch { }
                });
            }
            catch (Exception ex)
            {
                Debug.LogError("[RMRStartHubPanel] Open atlas failed: " + ex);
                if (_root != null)
                    _root.SetActive(true);
            }
        }

        private void OnClickResetProgress()
        {
            ShowResetProgressConfirm();
        }

        private void OnClickExit()
        {
            ShowExitConfirm();
        }

        private void ShowResetProgressConfirm()
        {
            if (_confirmRoot != null)
                Destroy(_confirmRoot);
            _confirmRoot = new GameObject("ResetConfirm", typeof(RectTransform));
            _confirmRoot.transform.SetParent(_root.transform, false);
            _confirmRoot.transform.SetAsLastSibling();
            var crt = _confirmRoot.GetComponent<RectTransform>();
            crt.anchorMin = Vector2.zero;
            crt.anchorMax = Vector2.one;
            crt.offsetMin = Vector2.zero;
            crt.offsetMax = Vector2.zero;

            var dim = _confirmRoot.AddComponent<Image>();
            dim.color = new Color(0f, 0f, 0f, 0.72f);

            var rim = new GameObject("Rim", typeof(RectTransform));
            rim.transform.SetParent(_confirmRoot.transform, false);
            var rimImg = rim.AddComponent<Image>();
            rimImg.color = ColPanelEdge;
            var rrt = rim.GetComponent<RectTransform>();
            rrt.anchorMin = rrt.anchorMax = new Vector2(0.5f, 0.5f);
            rrt.sizeDelta = new Vector2(560f, 280f);

            var box = new GameObject("Box", typeof(RectTransform));
            box.transform.SetParent(rim.transform, false);
            var boxImg = box.AddComponent<Image>();
            boxImg.color = ColPanel;
            var brt = box.GetComponent<RectTransform>();
            brt.anchorMin = Vector2.zero;
            brt.anchorMax = Vector2.one;
            brt.offsetMin = new Vector2(3f, 3f);
            brt.offsetMax = new Vector2(-3f, -3f);

            AddLabel(box.transform, T("ui_RMR_Hub_ResetConfirm",
                    "\u786e\u8ba4\u91cd\u7f6e\u6240\u6709\u6c38\u4e45\u8fdb\u5ea6\uff1f\n\u5c06\u6e05\u7a7a\u56fe\u9274\u3001\u89e3\u653e\u6218\u8bb0\u5f55\u4e0e\u7279\u6b8a Boss \u9996\u901a\u8bb0\u5f55\u3002\n\u5f53\u524d\u8def\u7ebf\u4e34\u65f6\u5185\u5bb9\u4e0d\u4f1a\u5199\u56de\u56fe\u9274\u3002",
                    "Reset all permanent progress?\nThis clears the atlas, realization clears, and special first-clear records.\nCurrent-route temporary pages will not be written back.",
                    "\ubaa8\ub4e0 \uc601\uad6c \uc9c4\ud589\uc744 \ucd08\uae30\ud654\ud560\uae4c\uc694?\n\ub3c4\uac10\u00b7\ud574\ubc29\uc804 \uae30\ub85d\u00b7\ud2b9\uc218 \uccab \ud1b5\uacfc \uae30\ub85d\uc774 \uc0ad\uc81c\ub429\ub2c8\ub2e4."),
                new Vector2(0f, 48f), 18, new Vector2(500f, 120f), ColCream, false);

            AddConfirmButton(box.transform, T("ui_RMR_Hub_ResetYes", "\u786e\u8ba4\u91cd\u7f6e", "Confirm reset", "\ucd08\uae30\ud654"), -110f, () =>
            {
                try
                {
                    RMRCore.ResetAllArchiveProgress();
                    Debug.Log("[RMRStartHubPanel] Permanent progress reset from hub.");
                }
                catch (Exception ex)
                {
                    Debug.LogError("[RMRStartHubPanel] Reset failed: " + ex);
                }
                if (_confirmRoot != null)
                    Destroy(_confirmRoot);
                _confirmRoot = null;
            }, exitStyle: true);
            AddConfirmButton(box.transform, T("ui_RMR_Hub_ResetNo", "\u53d6\u6d88", "Cancel", "\ucde8\uc18c"), 110f, () =>
            {
                if (_confirmRoot != null)
                    Destroy(_confirmRoot);
                _confirmRoot = null;
            }, exitStyle: false);
        }

        private void ShowExitConfirm()
        {
            if (_confirmRoot != null)
                Destroy(_confirmRoot);
            _confirmRoot = new GameObject("ExitConfirm", typeof(RectTransform));
            _confirmRoot.transform.SetParent(_root.transform, false);
            _confirmRoot.transform.SetAsLastSibling();
            var crt = _confirmRoot.GetComponent<RectTransform>();
            crt.anchorMin = Vector2.zero;
            crt.anchorMax = Vector2.one;
            crt.offsetMin = Vector2.zero;
            crt.offsetMax = Vector2.zero;

            var dim = _confirmRoot.AddComponent<Image>();
            dim.color = new Color(0f, 0f, 0f, 0.62f);

            var rim = new GameObject("Rim", typeof(RectTransform));
            rim.transform.SetParent(_confirmRoot.transform, false);
            var rimImg = rim.AddComponent<Image>();
            rimImg.color = ColPanelEdge;
            var rrt = rim.GetComponent<RectTransform>();
            rrt.anchorMin = rrt.anchorMax = new Vector2(0.5f, 0.5f);
            rrt.sizeDelta = new Vector2(500f, 220f);

            var box = new GameObject("Box", typeof(RectTransform));
            box.transform.SetParent(rim.transform, false);
            var boxImg = box.AddComponent<Image>();
            boxImg.color = ColPanel;
            var brt = box.GetComponent<RectTransform>();
            brt.anchorMin = Vector2.zero;
            brt.anchorMax = Vector2.one;
            brt.offsetMin = new Vector2(3f, 3f);
            brt.offsetMax = new Vector2(-3f, -3f);

            AddLabel(box.transform, T("ui_RMR_Hub_ExitConfirm",
                    "\u662f\u5426\u8fd4\u56de\u56fe\u4e66\u9986\uff1f\n\u4e0d\u4f1a\u4fdd\u7559\u672c\u6b21\u5f00\u5c40\u72b6\u6001\u3002",
                    "Return to the Library?\nThis start menu state will not be kept.",
                    "\ub3c4\uc11c\uad00\uc73c\ub85c \ub3cc\uc544\uac08\uae4c\uc694?\n\uc2dc\uc791 \uba54\ub274 \uc0c1\ud0dc\ub294 \uc720\uc9c0\ub418\uc9c0 \uc54a\uc2b5\ub2c8\ub2e4."),
                new Vector2(0f, 36f), 20, new Vector2(440f, 80f), ColCream, false);

            AddConfirmButton(box.transform, T("ui_RMR_Hub_ExitYes", "\u786e\u8ba4", "OK", "\ud655\uc778"), -100f, () =>
            {
                RMRRealizationManager.EndHubSessionToLibrary();
                Hide();
            }, exitStyle: true);
            AddConfirmButton(box.transform, T("ui_RMR_Hub_ExitNo", "\u53d6\u6d88", "Cancel", "\ucde8\uc18c"), 100f, () =>
            {
                if (_confirmRoot != null)
                    Destroy(_confirmRoot);
                _confirmRoot = null;
            }, exitStyle: false);
        }

        private void AddConfirmButton(Transform parent, string label, float x, UnityAction action, bool exitStyle)
        {
            Button btn = null;
            try
            {
                if (LogLikeMod.ArtWorks != null && LogLikeMod.ArtWorks.ContainsKey("MysteryButton_Enable"))
                {
                    btn = ModdingUtils.CreateButton(parent, "MysteryButton_Enable",
                        Vector2.one, new Vector2(x, -52f), new Vector2(170f, 48f));
                }
            }
            catch { btn = null; }

            if (btn == null)
            {
                var go = new GameObject("Confirm", typeof(RectTransform));
                go.transform.SetParent(parent, false);
                var img = go.AddComponent<Image>();
                img.color = new Color(0.32f, 0.26f, 0.18f, 1f);
                var rt = go.GetComponent<RectTransform>();
                rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.sizeDelta = new Vector2(170f, 48f);
                rt.anchoredPosition = new Vector2(x, -52f);
                btn = go.AddComponent<Button>();
                btn.targetGraphic = img;
            }

            var tgo = new GameObject("T", typeof(RectTransform));
            tgo.transform.SetParent(btn.transform, false);
            var tmp = tgo.AddComponent<TextMeshProUGUI>();
            var trt = tgo.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero;
            trt.anchorMax = Vector2.one;
            trt.offsetMin = trt.offsetMax = Vector2.zero;
            LogLikeMod.ApplyTmpFontPreservingSharpMaterial(tmp, LogLikeMod.DefFont_TMP);
            tmp.fontSize = 20;
            tmp.color = exitStyle ? ColExit : ColCream;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.text = label;
            btn.onClick.AddListener(() =>
            {
                try { UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click); } catch { }
                action();
            });
        }
    }
}
