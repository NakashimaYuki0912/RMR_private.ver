// -----------------------------------------------------------------------------
// RogueLike Mod Reborn (RMR): RMR_CombatCardHoverPreview
// Namespace/file: ruina-roguelike-reborn-main\RMR_CombatCardHoverPreview.cs
// English comments/regions for maintainability. Do not rename disk save keys.
// -----------------------------------------------------------------------------
using System;
using System.Reflection;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace RogueLike_Mod_Reborn
{
    #region --- RMRCombatCardDetailLayer ---

    /// <summary>
    /// Vanilla combat-bookshelf hover (Assembly-CSharp):
    ///   UIInvenCardSlot.OnPointerEnter
    ///     → list.ShowDetailSlotByInventory(slot)
    ///         → detailSlot.SetData(model)
    ///         → detailSlot active, position = slot.position + targetPosForDetailSlot
    ///         → RevealDetailSlot: localScale 0 → slotDetailOriginScale
    ///     → list slot only SetHighlightedSlot (colors)
    ///
    /// Vanilla does NOT reparent, clone list cards, or raise list-item Canvas.
    /// Detail draws above the list because the prefab places detailSlot after list content
    /// (later sibling draws later) under UIBattleSettingEditPanel.canvas sortingOrder=12.
    ///
    /// RMR regression: RaiseUiPanelCanvas on the whole battle-card panel + leftover experiments
    /// broke that sibling relationship. Fix = restore vanilla draw order for detailSlot only:
    ///   1) SetAsLastSibling under its original parent
    ///   2) optional light overrideSorting only on the detail root (not list slots)
    /// Never clone list cards; never reparent off the list hierarchy; never touch localScale.
    /// </summary>
    public static class RMRCombatCardDetailLayer
    {
        /// <summary>
        /// Slightly above RMR HUD clone (LogUIObjs[100] ≈ base+100) and equip panel raises,
        /// without leaving Screen Space Camera of the prepare UI family when possible.
        /// </summary>
        private const int DetailSortBoost = 250;

        private static UIDetailCardSlot _activeDetail;
        private static Canvas _detailCanvas;
        private static bool _addedCanvas;
        private static bool _addedRaycaster;
        private static bool _hadCanvas;
        private static bool _prevOverride;
        private static int _prevOrder;
        private static bool _prevEnabled;

        /// <summary>
        /// Call after vanilla ShowDetailSlotByInventory has positioned the detail.
        /// </summary>
        public static void ElevateDetailSlot(UIInvenCardListScroll list)
        {
            if (list == null)
                return;

            DestroyLegacyHolderIfAny();

            UIDetailCardSlot detail = GetDetailSlot(list);
            if (detail == null || detail.gameObject == null || !detail.gameObject.activeInHierarchy)
                return;

            _activeDetail = detail;
            Transform t = detail.transform;

            // 1) Prefab-style: last among siblings under the same parent as list content.
            try
            {
                t.SetAsLastSibling();
                // Also push list root last under its parent so detail's branch is late if nested.
                if (list.transform.parent != null)
                    list.transform.SetAsLastSibling();
            }
            catch { /* ignore */ }

            // 2) Light Canvas boost only on detail root — do not reparent, do not touch scale.
            try
            {
                Canvas c = detail.GetComponent<Canvas>();
                if (c == null)
                {
                    c = detail.gameObject.AddComponent<Canvas>();
                    _addedCanvas = true;
                    _hadCanvas = false;
                    _prevOverride = false;
                    _prevOrder = 0;
                    _prevEnabled = true;
                }
                else if (_detailCanvas != c)
                {
                    _hadCanvas = true;
                    _addedCanvas = false;
                    _prevOverride = c.overrideSorting;
                    _prevOrder = c.sortingOrder;
                    _prevEnabled = c.enabled;
                }

                _detailCanvas = c;
                c.enabled = true;
                c.overrideSorting = true;
                // Absolute order high enough to sit above list graphics in the same edit panel.
                if (c.sortingOrder < DetailSortBoost)
                    c.sortingOrder = DetailSortBoost;

                if (detail.GetComponent<GraphicRaycaster>() == null)
                {
                    detail.gameObject.AddComponent<GraphicRaycaster>();
                    _addedRaycaster = true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR] detailSlot SetAsLastSibling/Canvas boost failed: " + ex.Message);
            }

            // 3) Remove leftover experimental canvases on list slots (old WIP).
            try { StripElevatedCanvasesFromListSlots(list); } catch { /* ignore */ }
        }

        public static void Restore()
        {
            try
            {
                if (_detailCanvas != null)
                {
                    if (_addedCanvas)
                    {
                        if (_addedRaycaster)
                        {
                            GraphicRaycaster gr = _detailCanvas.GetComponent<GraphicRaycaster>();
                            if (gr != null)
                                UnityEngine.Object.Destroy(gr);
                        }
                        UnityEngine.Object.Destroy(_detailCanvas);
                    }
                    else if (_hadCanvas)
                    {
                        _detailCanvas.overrideSorting = _prevOverride;
                        _detailCanvas.sortingOrder = _prevOrder;
                        _detailCanvas.enabled = _prevEnabled;
                        if (_addedRaycaster)
                        {
                            GraphicRaycaster gr = _detailCanvas.GetComponent<GraphicRaycaster>();
                            if (gr != null)
                                UnityEngine.Object.Destroy(gr);
                        }
                    }
                }
            }
            catch { /* ignore */ }
            finally
            {
                _activeDetail = null;
                _detailCanvas = null;
                _addedCanvas = false;
                _addedRaycaster = false;
                _hadCanvas = false;
            }

            DestroyLegacyHolderIfAny();
        }

        /// <summary>Remove experimental DontDestroyOnLoad holder from older WIP builds.</summary>
        private static void DestroyLegacyHolderIfAny()
        {
            try
            {
                GameObject legacy = GameObject.Find("RMR_DetailSlotLayerHolder");
                if (legacy == null)
                    return;
                // If detail was left parented under it, reparent is unknown — destroy only empty holder.
                if (legacy.transform.childCount > 0)
                {
                    // Put children back under first available battle card list if possible.
                    UIInvenCardListScroll list = UnityEngine.Object.FindObjectOfType<UIInvenCardListScroll>();
                    Transform fallbackParent = list != null ? list.transform : null;
                    while (legacy.transform.childCount > 0)
                    {
                        Transform child = legacy.transform.GetChild(0);
                        if (fallbackParent != null)
                            child.SetParent(fallbackParent, worldPositionStays: true);
                        else
                            child.SetParent(null, worldPositionStays: true);
                    }
                }
                UnityEngine.Object.Destroy(legacy);
            }
            catch { /* ignore */ }
        }

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
                if (c == null || !c.overrideSorting)
                    continue;
                if (c.sortingOrder < 100)
                    continue;
                GraphicRaycaster gr = slot.GetComponent<GraphicRaycaster>();
                if (gr != null)
                    UnityEngine.Object.Destroy(gr);
                UnityEngine.Object.Destroy(c);
            }
        }

        private static UIDetailCardSlot GetDetailSlot(UIInvenCardListScroll list)
        {
            FieldInfo fi = typeof(UIInvenCardListScroll).GetField("detailSlot",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return fi != null ? fi.GetValue(list) as UIDetailCardSlot : null;
        }
    }
    #endregion

    #region --- RMRCombatCardHoverFollow ---


    /// <summary>RMR type: RMRCombatCardHoverFollow</summary>

    public sealed class RMRCombatCardHoverFollow : MonoBehaviour
    {
        private void OnDisable()
        {
            try { RMRCombatCardDetailLayer.Restore(); } catch { /* ignore */ }
        }
    }
    #endregion

}
