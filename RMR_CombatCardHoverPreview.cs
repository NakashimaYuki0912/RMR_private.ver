using System;
using System.Collections;
using System.Reflection;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace RogueLike_Mod_Reborn
{
    /// <summary>
    /// Vanilla combat hover uses <c>UIInvenCardListScroll.detailSlot</c> (not the list item).
    /// RMR's panel Canvas raise puts list siblings over that detail.
    ///
    /// Fix: reparent the existing vanilla detailSlot under a high-order Overlay holder,
    /// using worldPositionStays only (do not rewrite localScale — that breaks RevealDetailSlot).
    /// On hide, restore original parent + sibling index.
    /// </summary>
    public static class RMRCombatCardDetailLayer
    {
        private const string HolderName = "RMR_DetailSlotLayerHolder";
        private const int HolderSortingOrder = 32000;

        private static GameObject _holderRoot;
        private static RectTransform _holderRt;
        private static Canvas _holderCanvas;

        private static UIDetailCardSlot _detail;
        private static Transform _origParent;
        private static int _origSibling;
        private static bool _active;
        private static MonoBehaviour _coroutineHost;
        private static Coroutine _pendingElevate;

        /// <summary>
        /// After vanilla ShowDetailSlotByInventory (and optionally after BCEV SetData).
        /// Schedules elevate at end of frame so reveal/BCEV layout can finish first.
        /// </summary>
        public static void ElevateDetailSlot(UIInvenCardListScroll list)
        {
            if (list == null)
                return;
            UIDetailCardSlot detail = GetDetailSlot(list);
            if (detail == null || detail.gameObject == null || !detail.gameObject.activeInHierarchy)
                return;

            // Already on holder — only ensure sorting, do not thrash transform.
            if (_active && _detail == detail && detail.transform.parent == _holderRt)
            {
                EnsureHolderTop();
                return;
            }

            MonoBehaviour host = list;
            if (_pendingElevate != null && _coroutineHost != null)
            {
                try { _coroutineHost.StopCoroutine(_pendingElevate); } catch { /* ignore */ }
                _pendingElevate = null;
            }
            _coroutineHost = host;
            _pendingElevate = host.StartCoroutine(ElevateEndOfFrame(list));
        }

        private static IEnumerator ElevateEndOfFrame(UIInvenCardListScroll list)
        {
            // Wait for vanilla RevealDetailSlot + BCEV SetData layout.
            yield return null;
            yield return new WaitForEndOfFrame();
            _pendingElevate = null;
            ElevateNow(list);
        }

        private static void ElevateNow(UIInvenCardListScroll list)
        {
            if (list == null)
                return;
            UIDetailCardSlot detail = GetDetailSlot(list);
            if (detail == null || detail.gameObject == null || !detail.gameObject.activeInHierarchy)
                return;

            if (_active && _detail == detail && detail.transform.parent == _holderRt)
            {
                EnsureHolderTop();
                return;
            }

            if (_active && _detail != null && _detail != detail)
                Restore();

            if (!EnsureHolder())
                return;

            // Strip accidental Canvas on list slots (old broken fixes) so they cannot outrank detail.
            try { StripElevatedCanvasesFromListSlots(list); } catch { /* ignore */ }

            _detail = detail;
            Transform t = detail.transform;
            _origParent = t.parent;
            _origSibling = t.GetSiblingIndex();

            try
            {
                // worldPositionStays keeps screen position; do NOT rewrite localScale
                // (RevealDetailSlot animates localScale from 0 → slotDetailOriginScale).
                t.SetParent(_holderRt, worldPositionStays: true);
                EnsureHolderTop();

                // Force absolute top sorting on detail itself too (nested canvases).
                ForceDetailCanvasStack(detail.gameObject);

                _active = true;
                // One log per successful elevate (avoid SetData spam).
                Debug.Log($"[RMR] detailSlot on top layer parent={t.parent?.name} order={HolderSortingOrder}");
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR] ElevateDetailSlot failed: " + ex.Message);
                try
                {
                    if (_origParent != null)
                        t.SetParent(_origParent, worldPositionStays: true);
                }
                catch { /* ignore */ }
                _active = false;
                _detail = null;
            }
        }

        public static void Restore()
        {
            if (_pendingElevate != null && _coroutineHost != null)
            {
                try { _coroutineHost.StopCoroutine(_pendingElevate); } catch { /* ignore */ }
                _pendingElevate = null;
            }

            if (!_active && _detail == null)
                return;

            try
            {
                if (_detail != null && _detail.transform != null && _origParent != null)
                {
                    Transform t = _detail.transform;
                    t.SetParent(_origParent, worldPositionStays: true);
                    int max = Mathf.Max(0, _origParent.childCount - 1);
                    t.SetSiblingIndex(Mathf.Clamp(_origSibling, 0, max));
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR] detailSlot restore failed: " + ex.Message);
            }
            finally
            {
                _active = false;
                _detail = null;
                _origParent = null;
                _origSibling = 0;
            }
        }

        private static void EnsureHolderTop()
        {
            if (_holderRoot == null || _holderCanvas == null)
                return;
            if (!_holderRoot.activeSelf)
                _holderRoot.SetActive(true);
            _holderCanvas.overrideSorting = true;
            _holderCanvas.sortingOrder = HolderSortingOrder;
            try { _holderRoot.transform.SetAsLastSibling(); } catch { /* ignore */ }
        }

        private static void ForceDetailCanvasStack(GameObject detailRoot)
        {
            if (detailRoot == null)
                return;
            Canvas[] canvases = detailRoot.GetComponentsInChildren<Canvas>(true);
            for (int i = 0; i < canvases.Length; i++)
            {
                Canvas c = canvases[i];
                if (c == null)
                    continue;
                c.overrideSorting = true;
                c.sortingOrder = HolderSortingOrder + 1 + i;
            }
            // Root detail may have no canvas — parent holder is enough; optional root canvas:
            Canvas rootCanvas = detailRoot.GetComponent<Canvas>();
            if (rootCanvas == null)
            {
                rootCanvas = detailRoot.AddComponent<Canvas>();
                rootCanvas.overrideSorting = true;
                rootCanvas.sortingOrder = HolderSortingOrder + 1;
                if (detailRoot.GetComponent<GraphicRaycaster>() == null)
                    detailRoot.AddComponent<GraphicRaycaster>();
            }
            else
            {
                rootCanvas.overrideSorting = true;
                rootCanvas.sortingOrder = HolderSortingOrder + 1;
            }
        }

        /// <summary>
        /// Old experimental fixes may have left Canvas+overrideSorting on list slots.
        /// Those can still paint over detail if order is high enough.
        /// </summary>
        private static void StripElevatedCanvasesFromListSlots(UIInvenCardListScroll list)
        {
            if (list == null)
                return;
            UIInvenCardSlot[] slots = list.GetComponentsInChildren<UIInvenCardSlot>(true);
            for (int i = 0; i < slots.Length; i++)
            {
                UIInvenCardSlot slot = slots[i];
                if (slot == null)
                    continue;
                Canvas c = slot.GetComponent<Canvas>();
                if (c == null)
                    continue;
                // Only remove canvases we likely added (override on the slot root).
                if (c.overrideSorting && c.sortingOrder >= 100)
                {
                    GraphicRaycaster gr = slot.GetComponent<GraphicRaycaster>();
                    if (gr != null)
                        UnityEngine.Object.Destroy(gr);
                    UnityEngine.Object.Destroy(c);
                }
            }
        }

        private static bool EnsureHolder()
        {
            if (_holderRoot != null && _holderRt != null && _holderCanvas != null)
            {
                EnsureHolderTop();
                return true;
            }

            try
            {
                _holderRoot = new GameObject(HolderName);
                UnityEngine.Object.DontDestroyOnLoad(_holderRoot);

                _holderCanvas = _holderRoot.AddComponent<Canvas>();
                _holderCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                _holderCanvas.overrideSorting = true;
                _holderCanvas.sortingOrder = HolderSortingOrder;

                var scaler = _holderRoot.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920f, 1080f);
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scaler.matchWidthOrHeight = 1f;

                _holderRoot.AddComponent<GraphicRaycaster>();

                _holderRt = _holderRoot.GetComponent<RectTransform>();
                if (_holderRt == null)
                    _holderRt = _holderRoot.AddComponent<RectTransform>();
                _holderRt.anchorMin = Vector2.zero;
                _holderRt.anchorMax = Vector2.one;
                _holderRt.offsetMin = Vector2.zero;
                _holderRt.offsetMax = Vector2.zero;
                _holderRt.localScale = Vector3.one;
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR] detail holder create failed: " + ex.Message);
                _holderRoot = null;
                _holderRt = null;
                _holderCanvas = null;
                return false;
            }
        }

        private static UIDetailCardSlot GetDetailSlot(UIInvenCardListScroll list)
        {
            FieldInfo fi = typeof(UIInvenCardListScroll).GetField("detailSlot",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return fi != null ? fi.GetValue(list) as UIDetailCardSlot : null;
        }
    }

    public sealed class RMRCombatCardHoverFollow : MonoBehaviour
    {
        private void OnDisable()
        {
            try { RMRCombatCardDetailLayer.Restore(); } catch { /* ignore */ }
        }
    }
}
