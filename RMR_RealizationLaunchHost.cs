using System.Collections;
using abcdcode_LOGLIKE_MOD;
using UI;
using UnityEngine;

namespace RogueLike_Mod_Reborn
{
    /// <summary>
    /// Opens the realization floor panel after battle UI is ready.
    /// Singleton-style: only one open attempt at a time; can re-show panel without new combat.
    /// </summary>
    public class RMRRealizationLaunchHost : MonoBehaviour
    {
        private static RMRRealizationLaunchHost _running;

        public static void EnsureFloorPanelVisible()
        {
            // If panel already up, just bring to front.
            if (LogRealizationPanel.Instance != null && LogRealizationPanel.Instance.IsVisible)
            {
                try
                {
                    if (LogRealizationPanel.Instance.transform != null)
                        LogRealizationPanel.Instance.transform.SetAsLastSibling();
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

        public void Begin()
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(OpenWhenReady());
        }

        private IEnumerator OpenWhenReady()
        {
            Transform parent = null;
            for (int i = 0; i < 60; i++)
            {
                parent = ResolveParent();
                if (parent != null)
                    break;
                yield return null;
            }

            if (parent == null)
            {
                Debug.LogError("[RMR] Realization floor panel: no UI parent after wait.");
                Cleanup();
                yield break;
            }

            yield return null;

            try
            {
                LogLikeMod.InvalidateTmpFontCache();
                var _ = LogLikeMod.DefFont_TMP;
                LogLikeMod.PauseBool = true;
                RMRRealizationManager.AwaitingRealizationFloorPick = true;

                try
                {
                    if (UI.UIController.Instance != null)
                        UI.UIController.Instance.OpenBattlePrepare();
                }
                catch { }

                // Prefer parenting under prepare root so the panel covers the start button.
                Transform showParent = parent;
                try
                {
                    var bsp = UI.UIController.Instance?.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel;
                    if (bsp != null)
                        showParent = bsp.transform;
                }
                catch { }

                LogRealizationPanel panel = LogRealizationPanel.Instance;
                if (panel == null)
                {
                    GameObject go = new GameObject("LogRealizationPanel");
                    panel = go.AddComponent<LogRealizationPanel>();
                }
                panel.Show(showParent);
                Debug.Log("[RMR] Realization floor panel opened (awaiting floor pick, dummy combat blocked).");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[RMR] Open realization floor panel failed: " + ex);
            }

            Cleanup();
        }

        private void Cleanup()
        {
            if (_running == this)
                _running = null;
            Destroy(gameObject);
        }

        private static Transform ResolveParent()
        {
            try
            {
                if (LogLikeMod.LogUIObjs != null && LogLikeMod.LogUIObjs.ContainsKey(90) && LogLikeMod.LogUIObjs[90] != null)
                    return LogLikeMod.LogUIObjs[90].transform;
            }
            catch { }

            try
            {
                var panel = UI.UIController.Instance?.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel;
                if (panel != null)
                    return panel.transform;
            }
            catch { }

            try
            {
                if (UI.UIController.Instance != null)
                    return UI.UIController.Instance.transform;
            }
            catch { }

            return null;
        }
    }
}
