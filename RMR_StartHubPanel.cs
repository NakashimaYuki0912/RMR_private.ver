using System;
using System.Collections;
using System.Collections.Generic;
using abcdcode_LOGLIKE_MOD;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
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
            // Never rebuild hub while help/atlas/confirm/floor-pick is on top — destroys soft-hide state.
            if (after == null || object.ReferenceEquals(after, before) || !_visible)
                yield break;
            if (_confirmRoot != null)
                yield break;
            try
            {
                if (RMRHelpHandbookPanel.Instance != null && RMRHelpHandbookPanel.Instance.IsVisible)
                    yield break;
            }
            catch { }
            try
            {
                var atlas = Singleton<LogAtlasPanel>.Instance;
                if (atlas != null && atlas.IsVisible)
                    yield break;
            }
            catch { }
            try
            {
                if (LogRealizationPanel.Instance != null && LogRealizationPanel.Instance.IsVisible)
                    yield break;
            }
            catch { }
            // Soft-hidden hub (_root inactive) must not be rebuilt either.
            if (_root != null && !_root.activeSelf)
                yield break;

            if (_root != null)
                Destroy(_root);
            _root = null;
            _confirmRoot = null;
            Transform parent = RMRRealizationLaunchHost.GetOrCreateOverlayRoot();
            if (parent != null)
                BuildUi(parent);
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
                if (atlas != null && atlas.IsVisible)
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
            // Stack: help → atlas → confirm → hub.
            try
            {
                if (RMRHelpHandbookPanel.Instance != null && RMRHelpHandbookPanel.Instance.IsVisible)
                {
                    RMRHelpHandbookPanel.Instance.Hide();
                    return true;
                }
            }
            catch { }
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

        // Hub A: full-screen invitation page with a left identity block and right action index.
        private static readonly Color ColPageBlack = new Color(0f, 0f, 0f, 1f);
        private static readonly Color ColPanel = new Color(0.082f, 0.071f, 0.055f, 0.98f);
        private static readonly Color ColPanelEdge = new Color(0.376f, 0.278f, 0.157f, 0.95f);
        private static readonly Color ColGold = new Color(0.773f, 0.608f, 0.333f, 1f);
        private static readonly Color ColCream = new Color(0.91f, 0.87f, 0.79f, 1f);
        private static readonly Color ColMuted = new Color(0.725f, 0.667f, 0.557f, 1f);
        private static readonly Color ColExit = new Color(0.812f, 0.557f, 0.522f, 1f);

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

            // Scheme A visual baseline: cover-cropped reception artwork under layered darkness.
            try
            {
                if (LogLikeMod.ArtWorks != null && LogLikeMod.ArtWorks.ContainsKey("\u968f\u673a\u4e8b\u4ef6\u80cc\u666f1"))
                {
                    var artGo = new GameObject("InvitationBackdrop", typeof(RectTransform));
                    artGo.transform.SetParent(_root.transform, false);
                    var art = artGo.AddComponent<Image>();
                    art.sprite = LogLikeMod.ArtWorks["\u968f\u673a\u4e8b\u4ef6\u80cc\u666f1"];
                    art.color = new Color(0.58f, 0.53f, 0.44f, 0.92f);
                    art.preserveAspect = true;
                    art.raycastTarget = false;
                    var artRt = artGo.GetComponent<RectTransform>();
                    artRt.anchorMin = Vector2.zero;
                    artRt.anchorMax = Vector2.one;
                    artRt.offsetMin = Vector2.zero;
                    artRt.offsetMax = Vector2.zero;
                    var fitter = artGo.AddComponent<AspectRatioFitter>();
                    fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
                    fitter.aspectRatio = art.sprite.rect.width / Math.Max(1f, art.sprite.rect.height);
                }
            }
            catch { }

            AddHorizontalShade(_root.transform, "InvitationVeil", Vector2.zero, Vector2.one,
                new Color(0f, 0f, 0f, 0.36f));
            AddHorizontalShade(_root.transform, "InvitationLeftShade", Vector2.zero, new Vector2(0.56f, 1f),
                new Color(0f, 0f, 0f, 0.72f));
            AddHorizontalShade(_root.transform, "InvitationRightShade", new Vector2(0.58f, 0f), Vector2.one,
                new Color(0f, 0f, 0f, 0.54f));
            AddInvitationPageFrame(_root.transform);

            TextMeshProUGUI eyebrow = AddLabel(_root.transform, "LIBRARY INVITATION  \u00b7  RMR",
                new Vector2(-480f, 382f), 12, new Vector2(650f, 30f), ColGold, false);
            eyebrow.alignment = TextAlignmentOptions.Left;

            TextMeshProUGUI title = AddLabel(_root.transform,
                T("ui_RMR_HubTitle", "\u8089\u9e3d\u63a5\u5f85", "Roguelike Reception", "Roguelike \uc811\ub300"),
                new Vector2(-480f, 310f), 70, new Vector2(650f, 94f), ColCream, true);
            title.alignment = TextAlignmentOptions.Left;

            TextMeshProUGUI titleEn = AddLabel(_root.transform, "ROGUELIKE MOD REBORN",
                new Vector2(-480f, 232f), 24, new Vector2(650f, 38f), ColGold, false);
            titleEn.alignment = TextAlignmentOptions.Left;

            AddInvitationSigil(_root.transform, new Vector2(-715f, 105f));
            AddRule(_root.transform, new Vector2(-480f, -2f), 650f);

            TextMeshProUGUI desc = AddLabel(_root.transform,
                T("ui_RMR_HubDesc",
                    "\u9009\u62e9\u8fd9\u6b21\u63a5\u5f85\u7684\u5f00\u59cb\u65b9\u5f0f\u3002\u8def\u7ebf\u5185\u5bb9\u5c5e\u4e8e\u672c\u6b21\u65c5\u7a0b\uff1b\u56fe\u9274\u4e0e\u89e3\u653e\u8bb0\u5f55\u5c06\u6c38\u4e45\u4fdd\u7559\u3002",
                    "Choose how this reception begins. Route resources belong to this journey; Atlas and realization records persist.",
                    "\uc774\ubc88 \uc811\ub300\uc758 \uc2dc\uc791 \ubc29\uc2dd\uc744 \uc120\ud0dd\ud558\uc138\uc694."),
                new Vector2(-480f, -82f), 17, new Vector2(650f, 110f), ColMuted, false);
            desc.alignment = TextAlignmentOptions.TopLeft;

            // Scheme A: right-side reception index plate (ticket stack host).
            var menuRoot = new GameObject("InvitationMenu", typeof(RectTransform));
            menuRoot.transform.SetParent(_root.transform, false);
            var menuRt = menuRoot.GetComponent<RectTransform>();
            menuRt.anchorMin = menuRt.anchorMax = new Vector2(0.5f, 0.5f);
            menuRt.sizeDelta = new Vector2(540f, 760f);
            menuRt.anchoredPosition = new Vector2(566f, 64f);

            var menuPlate = new GameObject("IndexPlate", typeof(RectTransform));
            menuPlate.transform.SetParent(menuRoot.transform, false);
            menuPlate.transform.SetAsFirstSibling();
            var plateImg = menuPlate.AddComponent<Image>();
            plateImg.color = new Color(0.055f, 0.045f, 0.035f, 0.72f);
            plateImg.raycastTarget = false;
            var plateRt = menuPlate.GetComponent<RectTransform>();
            plateRt.anchorMin = plateRt.anchorMax = new Vector2(0.5f, 0.5f);
            plateRt.sizeDelta = new Vector2(520f, 640f);
            plateRt.anchoredPosition = new Vector2(0f, 10f);
            var plateEdge = new GameObject("IndexPlateEdge", typeof(RectTransform));
            plateEdge.transform.SetParent(menuPlate.transform, false);
            plateEdge.transform.SetAsFirstSibling();
            var peImg = plateEdge.AddComponent<Image>();
            peImg.color = new Color(ColPanelEdge.r, ColPanelEdge.g, ColPanelEdge.b, 0.55f);
            peImg.raycastTarget = false;
            var peRt = plateEdge.GetComponent<RectTransform>();
            peRt.anchorMin = Vector2.zero;
            peRt.anchorMax = Vector2.one;
            peRt.offsetMin = new Vector2(-1.5f, -1.5f);
            peRt.offsetMax = new Vector2(1.5f, 1.5f);

            TextMeshProUGUI indexTitle = AddLabel(menuRoot.transform, "RECEPTION INDEX", new Vector2(34f, 354f), 12,
                new Vector2(430f, 26f), ColPanelEdge, false);
            indexTitle.alignment = TextAlignmentOptions.Left;
            AddRule(menuRoot.transform, new Vector2(0f, 332f), 420f);

            TextMeshProUGUI footer = AddLabel(_root.transform,
                T("ui_RMR_HubFooter", "Library of Ruina \u00b7 Roguelike Mod Reborn",
                    "Library of Ruina \u00b7 Roguelike Mod Reborn",
                    "Library of Ruina \u00b7 Roguelike Mod Reborn"),
                new Vector2(0f, -470f), 13, new Vector2(900f, 28f), ColMuted, false);
            footer.alignment = TextAlignmentOptions.Center;

            // Continue lives here (after RMR entry → mode select), not as a lone invitation icon.
            bool hasSave = false;
            try { hasSave = LoguePlayDataSaver.CheckPlayerData(); } catch { hasSave = false; }

            // The recovered A prototype uses seven 67px invitation rows with a 9px gap.
            float step = 76f;
            float y = 304f;
            AddMenuButton(menuRoot.transform, "I",
                T("ui_RMR_Hub_Play", "\u6b63\u5e38\u6e38\u73a9", "Normal Play", "\uc77c\ubc18 \ud50c\ub808\uc774"),
                "BEGIN A NEW ROGUELIKE RECEPTION", y, OnClickPlay, primary: true);
            y -= step;
            if (hasSave)
            {
                AddMenuButton(menuRoot.transform, "II",
                    T("ui_RMR_ContinueRun", "\u7ee7\u7eed\u65c5\u7a0b", "Continue Run", "\uc774\uc5b4\ud558\uae30"),
                    "RESUME THE SAVED ROUTE", y, OnClickContinue, primary: true);
                y -= step;
            }
            AddMenuButton(menuRoot.transform, "III",
                T("ui_RMR_Hub_Realization", "\u6311\u6218\u89e3\u653e\u6218", "Challenge Realization", "\ud574\ubc29\uc804 \ub3c4\uc804"),
                "FLOOR REALIZATION", y, OnClickRealization, primary: false);
            y -= step;
            AddMenuButton(menuRoot.transform, "IV",
                T("ui_RMR_Hub_Help", "\u73a9\u6cd5\u4ecb\u7ecd", "How to Play", "\ud50c\ub808\uc774 \uc18c\uac1c"),
                "HOW TO PLAY", y, OnClickHelp, primary: false);
            y -= step;
            AddMenuButton(menuRoot.transform, "V",
                T("ui_RMR_Hub_Atlas", "\u56fe\u9274", "Atlas", "\ub3c4\uac10"),
                "PERMANENT ATLAS", y, OnClickAtlas, primary: false);
            y -= step;
            AddMenuButton(menuRoot.transform, "VI",
                T("ui_RMR_Hub_Reset", "\u91cd\u7f6e\u6c38\u4e45\u8fdb\u5ea6", "Reset Permanent Progress", "\uc601\uad6c \uc9c4\ud589 \ucd08\uae30\ud654"),
                "RESET ARCHIVE", y, OnClickResetProgress, primary: false, exitStyle: true);
            y -= step;
            AddMenuButton(menuRoot.transform, "VII",
                T("ui_RMR_Hub_Exit", "\u9000\u51fa", "Exit", "\uc885\ub8cc"),
                "RETURN TO INVITATION", y, OnClickExit, primary: false, exitStyle: true);
        }

        private static void AddInvitationSigil(Transform parent, Vector2 pos)
        {
            var glow = new GameObject("InvitationSigilGlow", typeof(RectTransform));
            glow.transform.SetParent(parent, false);
            var glowImage = glow.AddComponent<Image>();
            glowImage.color = new Color(ColGold.r, ColGold.g, ColGold.b, 0.055f);
            glowImage.raycastTarget = false;
            var glowRt = glow.GetComponent<RectTransform>();
            glowRt.anchorMin = glowRt.anchorMax = new Vector2(0.5f, 0.5f);
            glowRt.sizeDelta = new Vector2(136f, 136f);
            glowRt.anchoredPosition = pos;
            glowRt.localRotation = Quaternion.Euler(0f, 0f, 45f);

            var outer = new GameObject("InvitationSigilOuter", typeof(RectTransform));
            outer.transform.SetParent(parent, false);
            var outerImage = outer.AddComponent<Image>();
            outerImage.color = ColGold;
            outerImage.raycastTarget = false;
            var outerRt = outer.GetComponent<RectTransform>();
            outerRt.anchorMin = outerRt.anchorMax = new Vector2(0.5f, 0.5f);
            outerRt.sizeDelta = new Vector2(118f, 118f);
            outerRt.anchoredPosition = pos;
            outerRt.localRotation = Quaternion.Euler(0f, 0f, 45f);

            var outerInner = new GameObject("InvitationSigilOuterInner", typeof(RectTransform));
            outerInner.transform.SetParent(outer.transform, false);
            var outerInnerImage = outerInner.AddComponent<Image>();
            outerInnerImage.color = new Color(0.045f, 0.040f, 0.031f, 1f);
            outerInnerImage.raycastTarget = false;
            var outerInnerRt = outerInner.GetComponent<RectTransform>();
            outerInnerRt.anchorMin = outerInnerRt.anchorMax = new Vector2(0.5f, 0.5f);
            outerInnerRt.sizeDelta = new Vector2(116f, 116f);
            outerInnerRt.anchoredPosition = Vector2.zero;

            var ring = new GameObject("InvitationSigilRing", typeof(RectTransform));
            ring.transform.SetParent(outerInner.transform, false);
            var ringImage = ring.AddComponent<Image>();
            ringImage.color = new Color(ColGold.r, ColGold.g, ColGold.b, 0.66f);
            ringImage.raycastTarget = false;
            var ringRt = ring.GetComponent<RectTransform>();
            ringRt.anchorMin = ringRt.anchorMax = new Vector2(0.5f, 0.5f);
            ringRt.sizeDelta = new Vector2(84f, 84f);
            ringRt.anchoredPosition = Vector2.zero;

            var ringInner = new GameObject("InvitationSigilRingInner", typeof(RectTransform));
            ringInner.transform.SetParent(ring.transform, false);
            var ringInnerImage = ringInner.AddComponent<Image>();
            ringInnerImage.color = outerInnerImage.color;
            ringInnerImage.raycastTarget = false;
            var ringInnerRt = ringInner.GetComponent<RectTransform>();
            ringInnerRt.anchorMin = ringInnerRt.anchorMax = new Vector2(0.5f, 0.5f);
            ringInnerRt.sizeDelta = new Vector2(82f, 82f);
            ringInnerRt.anchoredPosition = Vector2.zero;

            var core = new GameObject("InvitationSigilCore", typeof(RectTransform));
            core.transform.SetParent(ringInner.transform, false);
            var coreImage = core.AddComponent<Image>();
            coreImage.color = new Color(0.56f, 0.20f, 0.18f, 1f);
            coreImage.raycastTarget = false;
            var coreRt = core.GetComponent<RectTransform>();
            coreRt.anchorMin = coreRt.anchorMax = new Vector2(0.5f, 0.5f);
            coreRt.sizeDelta = new Vector2(38f, 38f);
            coreRt.anchoredPosition = Vector2.zero;
        }

        private static void AddHorizontalShade(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var image = go.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        private static void AddInvitationPageFrame(Transform parent)
        {
            var frame = new GameObject("InvitationPageFrame", typeof(RectTransform));
            frame.transform.SetParent(parent, false);
            var rt = frame.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(32f, 32f);
            rt.offsetMax = new Vector2(-32f, -32f);
            AddFrameLine(frame.transform, "FrameTop", new Vector2(0.5f, 1f), new Vector2(0f, -0.5f), new Vector2(0f, 1f), new Vector2(1f, 1f), ColPanelEdge);
            AddFrameLine(frame.transform, "FrameBottom", new Vector2(0.5f, 0f), new Vector2(0f, 0.5f), new Vector2(0f, 0f), new Vector2(1f, 0f), ColPanelEdge);
            AddFrameLine(frame.transform, "FrameLeft", new Vector2(0f, 0.5f), new Vector2(0.5f, 0f), new Vector2(0f, 0f), new Vector2(0f, 1f), ColPanelEdge);
            AddFrameLine(frame.transform, "FrameRight", new Vector2(1f, 0.5f), new Vector2(-0.5f, 0f), new Vector2(1f, 0f), new Vector2(1f, 1f), ColPanelEdge);

            // Prototype's heavy upper-left and lower-right L corners.
            AddCornerBar(frame.transform, "CornerTL_H", new Vector2(39f, -1.5f), new Vector2(78f, 3f), new Vector2(0f, 1f));
            AddCornerBar(frame.transform, "CornerTL_V", new Vector2(1.5f, -39f), new Vector2(3f, 78f), new Vector2(0f, 1f));
            AddCornerBar(frame.transform, "CornerBR_H", new Vector2(-39f, 1.5f), new Vector2(78f, 3f), new Vector2(1f, 0f));
            AddCornerBar(frame.transform, "CornerBR_V", new Vector2(-1.5f, 39f), new Vector2(3f, 78f), new Vector2(1f, 0f));
        }

        private static void AddFrameLine(Transform parent, string name, Vector2 pivot, Vector2 pos,
            Vector2 anchorMin, Vector2 anchorMax, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var image = go.AddComponent<Image>();
            image.color = new Color(color.r, color.g, color.b, 0.46f);
            image.raycastTarget = false;
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.pivot = pivot;
            rt.anchoredPosition = pos;
            rt.sizeDelta = anchorMin.x == anchorMax.x ? new Vector2(1f, 0f) : new Vector2(0f, 1f);
        }

        private static void AddCornerBar(Transform parent, string name, Vector2 pos, Vector2 size, Vector2 anchor)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var image = go.AddComponent<Image>();
            image.color = ColGold;
            image.raycastTarget = false;
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = anchor;
            rt.sizeDelta = size;
            rt.anchoredPosition = pos;
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

        private void AddMenuButton(Transform parent, string roman, string title, string subtitle, float y,
            UnityAction action, bool primary, bool exitStyle = false)
        {
            var go = new GameObject("HubMenuButton", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var graphic = go.AddComponent<RMRInvitationButtonGraphic>();
            graphic.leftColor = primary
                ? new Color(0.20f, 0.16f, 0.10f, 0.98f)
                : new Color(0.082f, 0.071f, 0.055f, 0.96f);
            graphic.rightColor = new Color(0.031f, 0.031f, 0.027f, 0.84f);
            graphic.raycastTarget = true;
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(500f, 67f);
            rt.anchoredPosition = new Vector2(0f, y);
            var btn = go.AddComponent<Button>();
            btn.targetGraphic = graphic;

            var leftAccent = new GameObject("HubButtonAccent", typeof(RectTransform));
            leftAccent.transform.SetParent(go.transform, false);
            var accentImage = leftAccent.AddComponent<Image>();
            accentImage.color = exitStyle ? ColExit : (primary ? ColGold : ColPanelEdge);
            accentImage.raycastTarget = false;
            var accentRt = leftAccent.GetComponent<RectTransform>();
            accentRt.anchorMin = new Vector2(0f, 0f);
            accentRt.anchorMax = new Vector2(0f, 1f);
            accentRt.sizeDelta = new Vector2(primary ? 3f : 2f, 0f);
            accentRt.anchoredPosition = Vector2.zero;

            var bottom = new GameObject("HubButtonRule", typeof(RectTransform));
            bottom.transform.SetParent(go.transform, false);
            var bottomImage = bottom.AddComponent<Image>();
            bottomImage.color = new Color(ColPanelEdge.r, ColPanelEdge.g, ColPanelEdge.b, 0.34f);
            bottomImage.raycastTarget = false;
            var bottomRt = bottom.GetComponent<RectTransform>();
            bottomRt.anchorMin = new Vector2(0f, 0f);
            bottomRt.anchorMax = new Vector2(1f, 0f);
            bottomRt.sizeDelta = new Vector2(0f, 1f);
            bottomRt.anchoredPosition = Vector2.zero;

            Color titleColor = exitStyle ? ColExit : (primary ? ColGold : ColCream);
            TextMeshProUGUI number = AddLabel(go.transform, roman, new Vector2(-212f, 8f), 13,
                new Vector2(46f, 26f), exitStyle ? ColExit : ColGold, false);
            number.name = "HubButtonNumber";
            number.alignment = TextAlignmentOptions.Left;
            TextMeshProUGUI main = AddLabel(go.transform, title, new Vector2(-72f, 11f), 20,
                new Vector2(270f, 29f), titleColor, false);
            main.name = "HubButtonTitle";
            main.alignment = TextAlignmentOptions.Left;
            TextMeshProUGUI sub = AddLabel(go.transform, subtitle, new Vector2(-72f, -15f), 11,
                new Vector2(270f, 20f), new Color(0.60f, 0.55f, 0.47f, 1f), false);
            sub.name = "HubButtonSubtitle";
            sub.alignment = TextAlignmentOptions.Left;

            TextMeshProUGUI arrow = AddLabel(go.transform, "\u203a", new Vector2(218f, 0f), 24,
                new Vector2(28f, 34f), exitStyle ? ColExit : ColPanelEdge, false);
            arrow.name = "HubButtonArrow";

            var hover = go.AddComponent<RMRHubButtonHover>();
            hover.target = rt;
            hover.accent = accentImage;
            hover.graphic = graphic;
            hover.idleAccent = accentImage.color;
            hover.hoverAccent = exitStyle ? new Color(0.91f, 0.40f, 0.36f, 1f) : new Color(0.886f, 0.769f, 0.510f, 1f);
            hover.idleLeft = graphic.leftColor;
            hover.idleRight = graphic.rightColor;
            hover.hoverLeft = new Color(0.329f, 0.231f, 0.110f, 0.98f);
            hover.hoverRight = new Color(0.055f, 0.047f, 0.035f, 0.90f);

            // Hover tint
            try
            {
                var colors = btn.colors;
                colors.normalColor = Color.white;
                colors.highlightedColor = new Color(1.34f, 1.22f, 0.98f, 1f);
                colors.pressedColor = new Color(0.78f, 0.70f, 0.58f, 1f);
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
            var go = new GameObject("Confirm", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var img = go.AddComponent<Image>();
            img.color = new Color(0.10f, 0.085f, 0.065f, 1f);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(170f, 48f);
            rt.anchoredPosition = new Vector2(x, -52f);
            var btn = go.AddComponent<Button>();
            btn.targetGraphic = img;
            var outline = go.AddComponent<Outline>();
            outline.effectColor = exitStyle ? ColExit : ColPanelEdge;
            outline.effectDistance = new Vector2(1f, -1f);
            outline.useGraphicAlpha = false;

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

    /// <summary>
    /// Split-tone invitation ticket button face (left/right fill). Used as Button targetGraphic.
    /// </summary>
    internal sealed class RMRInvitationButtonGraphic : MaskableGraphic
    {
        public Color leftColor = new Color(0.12f, 0.10f, 0.07f, 0.98f);
        public Color rightColor = new Color(0.04f, 0.035f, 0.03f, 0.90f);
        [Range(0.2f, 0.8f)] public float split = 0.42f;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            Rect r = GetPixelAdjustedRect();
            float mid = Mathf.Lerp(r.xMin, r.xMax, Mathf.Clamp01(split));
            Color32 cL = leftColor * color;
            Color32 cR = rightColor * color;
            // Left panel
            vh.AddVert(new Vector3(r.xMin, r.yMin), cL, Vector2.zero);
            vh.AddVert(new Vector3(r.xMin, r.yMax), cL, Vector2.zero);
            vh.AddVert(new Vector3(mid, r.yMax), cL, Vector2.zero);
            vh.AddVert(new Vector3(mid, r.yMin), cL, Vector2.zero);
            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
            // Right panel
            vh.AddVert(new Vector3(mid, r.yMin), cR, Vector2.zero);
            vh.AddVert(new Vector3(mid, r.yMax), cR, Vector2.zero);
            vh.AddVert(new Vector3(r.xMax, r.yMax), cR, Vector2.zero);
            vh.AddVert(new Vector3(r.xMax, r.yMin), cR, Vector2.zero);
            vh.AddTriangle(4, 5, 6);
            vh.AddTriangle(6, 7, 4);
        }

        public void SetSplitColors(Color left, Color right)
        {
            leftColor = left;
            rightColor = right;
            SetVerticesDirty();
        }
    }

    internal sealed class RMRHubButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public RectTransform target;
        public Image accent;
        public Image point;
        public RMRInvitationButtonGraphic graphic;
        public Color idleAccent;
        public Color hoverAccent;
        public Color idlePoint;
        public Color hoverPoint;
        public Color idleLeft;
        public Color idleRight;
        public Color hoverLeft;
        public Color hoverRight;

        private Vector2 _idlePosition;
        private bool _captured;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Capture();
            if (target != null)
                target.anchoredPosition = _idlePosition + new Vector2(7f, 0f);
            if (accent != null)
                accent.color = hoverAccent;
            if (point != null)
                point.color = hoverPoint;
            if (graphic != null)
                graphic.SetSplitColors(hoverLeft, hoverRight);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Restore();
        }

        private void OnDisable()
        {
            Restore();
        }

        private void Capture()
        {
            if (_captured || target == null)
                return;
            _idlePosition = target.anchoredPosition;
            _captured = true;
        }

        private void Restore()
        {
            if (_captured && target != null)
                target.anchoredPosition = _idlePosition;
            if (accent != null)
                accent.color = idleAccent;
            if (point != null)
                point.color = idlePoint;
            if (graphic != null)
                graphic.SetSplitColors(idleLeft, idleRight);
        }
    }
}
