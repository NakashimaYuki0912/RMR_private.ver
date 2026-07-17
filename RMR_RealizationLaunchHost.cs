// -----------------------------------------------------------------------------
// RogueLike Mod Reborn (RMR): RMR_RealizationLaunchHost
// Namespace/file: ruina-roguelike-reborn-main\RMR_RealizationLaunchHost.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System.Collections;
using abcdcode_LOGLIKE_MOD;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace RogueLike_Mod_Reborn
{
    /// <summary>
    /// Opens a dedicated full-screen floor-pick UI on an RMR overlay canvas.
    /// Never leaves the player on vanilla BattleSetting / prepare with a panel on top.
    /// </summary>
    public class RMRRealizationLaunchHost : MonoBehaviour
    {
        private static RMRRealizationLaunchHost _running;
        private static GameObject _overlayRoot;
        #region --- UI show / hide / build ---


        /// <summary>
        /// High sorting-order Screen Space Overlay used only for RMR exclusive UIs
        /// (floor pick). Survives scene changes until ForceDestroy.
        /// </summary>
        public static Transform GetOrCreateOverlayRoot()
        {
            if (_overlayRoot != null)
                return _overlayRoot.transform;

            _overlayRoot = new GameObject("RMR_OverlayCanvas");
            Object.DontDestroyOnLoad(_overlayRoot);
            var canvas = _overlayRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 8000;
            // Prefer integer-ish scaling to reduce SDF soft sampling on CN hub text.
            try { canvas.pixelPerfect = true; } catch { /* ignore */ }
            var scaler = _overlayRoot.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f; // match height — LoR is typically 16:9
            try { scaler.referencePixelsPerUnit = 100f; } catch { /* ignore */ }
            _overlayRoot.AddComponent<GraphicRaycaster>();

            var rt = _overlayRoot.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
            }
            return _overlayRoot.transform;
        }
        #endregion

        #region --- Other helpers ---


        public static void DestroyOverlayIfEmpty()
        {
            try
            {
                if (_overlayRoot == null)
                    return;
                // Keep while exclusive RMR UIs still need the overlay canvas.
                if (LogRealizationPanel.Instance != null && LogRealizationPanel.Instance.IsVisible)
                    return;
                if (RMRStartHubPanel.Instance != null && RMRStartHubPanel.Instance.IsVisible)
                    return;
                if (RMRHelpHandbookPanel.Instance != null && RMRHelpHandbookPanel.Instance.IsVisible)
                    return;
                // Compendium from hub: hub may be soft-hidden (IsVisible still true usually), but
                // always keep overlay while atlas host is live.
                try
                {
                    var compendium = Singleton<LogCompendiumPanel>.Instance;
                    if (compendium != null && compendium.IsVisible)
                        return;
                }
                catch { /* atlas type / singleton optional at early boot */ }
                Object.Destroy(_overlayRoot);
                _overlayRoot = null;
            }
            catch { _overlayRoot = null; }
        }

        /// <summary>Always destroy overlay (call when leaving RMR UI / returning to library).</summary>
        public static void DestroyOverlayCompletely()
        {
            try
            {
                if (_overlayRoot != null)
                {
                    Object.Destroy(_overlayRoot);
                    _overlayRoot = null;
                }
            }
            catch { _overlayRoot = null; }
        }
        #endregion

        #region --- Getters / setters / checks ---


        public static void EnsureFloorPanelVisible()
        {
            if (LogRealizationPanel.Instance != null && LogRealizationPanel.Instance.IsVisible)
            {
                try
                {
                    LogRealizationPanel.Instance.transform.SetAsLastSibling();
                    if (_overlayRoot != null)
                        _overlayRoot.transform.SetAsLastSibling();
                }
                catch { }
                LogLikeMod.PauseBool = true;
                return;
            }

            if (_running != null)
                return;

            var host = new GameObject("RMR_RealizationLaunchHost");
            _running = host.AddComponent<RMRRealizationLaunchHost>();
            _running.Begin();
        }
        #endregion

        #region --- UI show / hide / build ---


        /// <summary>
        /// Open floor pick immediately on dedicated overlay (invitation hub path).
        /// Does not send invitation; callback does that after a floor is chosen.
        /// </summary>
        public static void ShowDedicatedFloorPick(System.Action<SephirahType> onFloorPicked)
        {
            try
            {
                LogLikeMod.InvalidateTmpFontCache();
                var _ = LogLikeMod.DefFont_TMP;
            }
            catch { }

            // CRITICAL: do NOT call LogRealizationPanel.ForceCloseStatic() here.
            // ForceCloseStatic destroys the entire RMR_OverlayCanvas; if we create the overlay
            // first (or parent into it), the floor UI is built then wiped the same frame —
            // log says "opened" but the player sees no change (hub still visible).
            try
            {
                if (LogRealizationPanel.Instance != null)
                    LogRealizationPanel.Instance.ForceDestroyUi();
            }
            catch { }

            Transform overlay = GetOrCreateOverlayRoot();
            if (_overlayRoot != null)
            {
                try { _overlayRoot.transform.SetAsLastSibling(); } catch { }
                // Ensure raycasts work even if something stripped the component.
                if (_overlayRoot.GetComponent<GraphicRaycaster>() == null)
                    _overlayRoot.AddComponent<GraphicRaycaster>();
            }

            LogRealizationPanel panel = LogRealizationPanel.Instance;
            if (panel == null || panel.gameObject == null)
            {
                GameObject go = new GameObject("LogRealizationPanel");
                panel = go.AddComponent<LogRealizationPanel>();
            }
            // Invitation-time: pick floor first, then send invite.
            panel.ShowForInvitationPick(overlay, onFloorPicked);
            try
            {
                panel.transform.SetAsLastSibling();
                if (_overlayRoot != null)
                    _overlayRoot.transform.SetAsLastSibling();
            }
            catch { }

            int childCount = 0;
            try { childCount = panel != null ? panel.transform.childCount : -1; } catch { }
            Debug.Log($"[RMR] Dedicated realization floor UI opened (overlay, not vanilla prepare). panelChildren={childCount} overlay={( _overlayRoot != null ? _overlayRoot.name : "null")}");
        }
        #endregion

        #region --- Other helpers ---


        public void Begin()
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(OpenWhenReady());
        }
        #endregion

        #region --- UI show / hide / build ---


        private IEnumerator OpenWhenReady()
        {
            // One frame so any transition UI settles; we still use overlay, not BattlePrepare.
            yield return null;

            try
            {
                LogLikeMod.InvalidateTmpFontCache();
                var _ = LogLikeMod.DefFont_TMP;
                LogLikeMod.PauseBool = true;
                RMRRealizationManager.AwaitingRealizationFloorPick = true;

                // Do NOT OpenBattlePrepare — user must not stay on vanilla team UI.
                // Do NOT ForceCloseStatic (destroys overlay). Only clear prior panel content.
                try
                {
                    if (LogRealizationPanel.Instance != null)
                        LogRealizationPanel.Instance.ForceDestroyUi();
                }
                catch { }

                Transform overlay = GetOrCreateOverlayRoot();
                if (_overlayRoot != null)
                {
                    try { _overlayRoot.transform.SetAsLastSibling(); } catch { }
                    if (_overlayRoot.GetComponent<GraphicRaycaster>() == null)
                        _overlayRoot.AddComponent<GraphicRaycaster>();
                }

                LogRealizationPanel panel = LogRealizationPanel.Instance;
                if (panel == null || panel.gameObject == null)
                {
                    GameObject go = new GameObject("LogRealizationPanel");
                    panel = go.AddComponent<LogRealizationPanel>();
                }
                // Post-invite fallback: no invitation callback — floor click starts battle.
                panel.Show(overlay);
                try
                {
                    panel.transform.SetAsLastSibling();
                    if (_overlayRoot != null)
                        _overlayRoot.transform.SetAsLastSibling();
                }
                catch { }
                Debug.Log("[RMR] Realization floor panel opened on dedicated overlay (awaiting floor pick).");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[RMR] Open realization floor panel failed: " + ex);
            }

            Cleanup();
        }
        #endregion

        #region --- Other helpers ---


        private void Cleanup()
        {
            if (_running == this)
                _running = null;
            Destroy(gameObject);
        }
        #endregion

    }
}
