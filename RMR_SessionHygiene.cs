using System;
using System.Collections.Generic;
using System.Reflection;
using abcdcode_LOGLIKE_MOD;
using Spine.Unity;
using UnityEngine;

namespace RogueLike_Mod_Reborn
{
    /// <summary>
    /// Mitigates the classic LoR "gets laggier the longer you play, restart fixes it" pattern
    /// for RMR-amplified leaks (Spine motion cache, orphan UI animations, log sets, GC pressure).
    /// Cannot fully fix vanilla Unity mono / engine accumulation, but reclaims RMR-owned growth
    /// between battles and on a slow background cadence.
    /// </summary>
    public static class RMRSessionHygiene
    {
        private const float LightIntervalSeconds = 120f;
        private const float HeavyIntervalSeconds = 420f;
        private const int MaxDebugLogSetSize = 256;

        private static float _lastLightRealtime = -999f;
        private static float _lastHeavyRealtime = -999f;
        private static bool _hostSpawned;
        private static int _lastSpinePruneRemoved;
        private static int _totalSpinePrunes;

        /// <summary>Install a quiet background host once the game scene is up.</summary>
        public static void EnsureHost()
        {
            if (_hostSpawned)
                return;
            try
            {
                GameObject go = new GameObject("RMR_SessionHygieneHost");
                UnityEngine.Object.DontDestroyOnLoad(go);
                go.hideFlags = HideFlags.HideAndDontSave;
                go.AddComponent<RMRSessionHygieneHost>();
                _hostSpawned = true;
                Debug.Log("[RMR SessionHygiene] host installed (spine prune + light GC between nodes).");
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR SessionHygiene] EnsureHost failed: " + ex.Message);
            }
        }

        /// <summary>
        /// Call after a reception / battle / realization node ends (safe, may hitch slightly on heavy).
        /// </summary>
        public static void OnNodeTransition(bool heavy = true)
        {
            try
            {
                LightCleanup("node");
                if (heavy)
                    HeavyCleanup("node");
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR SessionHygiene] OnNodeTransition: " + ex.Message);
            }
        }

        /// <summary>
        /// Periodic light pass from the host MonoBehaviour.
        /// Heavy GC only runs between nodes (OnNodeTransition) or when clearly idle —
        /// never mid-dice to avoid hitch-on-click.
        /// </summary>
        public static void TickBackground()
        {
            float now = Time.realtimeSinceStartup;
            if (now - _lastLightRealtime < LightIntervalSeconds)
                return;
            LightCleanup("bg");
            // Rare idle heavy pass: only when not in an active battle phase.
            if (now - _lastHeavyRealtime >= HeavyIntervalSeconds && !IsInLiveCombat())
                HeavyCleanup("bg-idle");
        }

        public static void LightCleanup(string reason)
        {
            _lastLightRealtime = Time.realtimeSinceStartup;
            int removed = PruneSpineMotions();
            ClearDebugLogSetsIfLarge();
            RewardingModel.ClearLocalizeCachesIfStale();
            if (removed > 0)
            {
                _totalSpinePrunes += removed;
                Debug.Log($"[RMR SessionHygiene] light({reason}): pruned {removed} dead spine entries (total={_totalSpinePrunes}).");
            }
        }

        public static void HeavyCleanup(string reason)
        {
            _lastHeavyRealtime = Time.realtimeSinceStartup;
            // Second prune after any destroy from light pass.
            PruneSpineMotions();
            try
            {
                // Unload unreferenced assets accumulated from card art / VFX / workshop loads.
                // Only outside sensitive combat — this can hitch for a frame or two.
                Resources.UnloadUnusedAssets();
            }
            catch { /* ignore */ }
            try
            {
                // Gen2 collect: long sessions fill the mono heap; this is what "restart helps" often is.
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch { /* ignore */ }
            Debug.Log($"[RMR SessionHygiene] heavy({reason}): UnloadUnusedAssets + GC.");
        }

        /// <summary>
        /// spinemotions grows one SkeletonAnimation per (skin, ActionDetail, CharacterAppearance GO)
        /// and never removes destroyed GOs — a primary RMR progressive-lag source.
        /// </summary>
        public static int PruneSpineMotions()
        {
            int removed = 0;
            try
            {
                var root = LogLikeMod.spinemotions;
                if (root == null || root.Count == 0)
                    return 0;

                var emptyDataNames = new List<string>();
                foreach (var dataKv in root)
                {
                    if (dataKv.Value == null)
                    {
                        emptyDataNames.Add(dataKv.Key);
                        continue;
                    }

                    var emptyDetails = new List<ActionDetail>();
                    foreach (var detailKv in dataKv.Value)
                    {
                        if (detailKv.Value == null)
                        {
                            emptyDetails.Add(detailKv.Key);
                            continue;
                        }

                        var deadGos = new List<GameObject>();
                        foreach (var goKv in detailKv.Value)
                        {
                            GameObject host = goKv.Key;
                            SkeletonAnimation skel = goKv.Value;
                            // Unity fake-null: destroyed GO compares equal to null.
                            bool hostDead = host == null;
                            bool skelDead = skel == null || skel.gameObject == null;
                            if (hostDead || skelDead)
                            {
                                deadGos.Add(host);
                                try
                                {
                                    if (!skelDead && skel != null && skel.gameObject != null)
                                        UnityEngine.Object.Destroy(skel.gameObject);
                                }
                                catch { /* ignore */ }
                            }
                        }

                        foreach (GameObject dead in deadGos)
                        {
                            detailKv.Value.Remove(dead);
                            removed++;
                        }

                        if (detailKv.Value.Count == 0)
                            emptyDetails.Add(detailKv.Key);
                    }

                    foreach (ActionDetail d in emptyDetails)
                        dataKv.Value.Remove(d);

                    if (dataKv.Value.Count == 0)
                        emptyDataNames.Add(dataKv.Key);
                }

                foreach (string name in emptyDataNames)
                    root.Remove(name);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR SessionHygiene] PruneSpineMotions: " + ex.Message);
            }

            _lastSpinePruneRemoved = removed;
            return removed;
        }

        private static void ClearDebugLogSetsIfLarge()
        {
            try
            {
                // Reflection keeps LogLikePatches private HashSets from growing forever.
                Type t = typeof(LogLikePatches);
                foreach (string fieldName in new[] { "_loggedPoorBookNames", "_loggedPoorPassives" })
                {
                    FieldInfo fi = t.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
                    if (fi == null)
                        continue;
                    object setObj = fi.GetValue(null);
                    if (setObj is HashSet<string> set && set.Count > MaxDebugLogSetSize)
                        set.Clear();
                }
            }
            catch { /* ignore */ }
        }

        /// <summary>True while dice/action phases are running (avoid GC.Collect mid-click).</summary>
        private static bool IsInLiveCombat()
        {
            try
            {
                StageController sc = Singleton<StageController>.Instance;
                if (sc == null)
                    return false;
                StageController.StagePhase phase = sc.Phase;
                if (phase == StageController.StagePhase.EndBattle
                    || phase == StageController.StagePhase.EndBattle2)
                    return false;

                // Any non-end phase with battle units still registered ≈ live combat / prepare field.
                var bom = BattleObjectManager.instance;
                if (bom == null)
                    return false;
                var list = bom.GetList();
                return list != null && list.Count > 0
                    && phase != StageController.StagePhase.EndBattle
                    && phase != StageController.StagePhase.EndBattle2;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>DontDestroyOnLoad ticker — cheap interval checks only.</summary>
    public sealed class RMRSessionHygieneHost : MonoBehaviour
    {
        private void Update()
        {
            // ~every second wall-clock via interval inside TickBackground.
            if ((Time.frameCount & 31) != 0)
                return;
            RMRSessionHygiene.TickBackground();
        }

        private void OnApplicationQuit()
        {
            try { RMRSessionHygiene.PruneSpineMotions(); } catch { /* ignore */ }
        }
    }
}
