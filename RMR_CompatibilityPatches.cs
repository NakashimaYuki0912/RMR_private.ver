// -----------------------------------------------------------------------------
// Narrow compatibility guards for third-party framework patches used by RMR.
// -----------------------------------------------------------------------------
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueLike_Mod_Reborn
{
    internal static class RMR_CompatibilityPatches
    {
        private const string AssortedFixesTypeName =
            "Cyaminthe.AssortedFixes.BattleCameraAndUnitPreviewFix";
        private const string AssortedFixesGameOverPrefixName =
            "StageController_GameOver_Prefix";

        private static bool _missingBattleCameraLogged;

        internal static void Install(Harmony harmony)
        {
            if (harmony == null)
                return;

            MethodInfo guardMethod = AccessTools.Method(
                typeof(RMR_CompatibilityPatches),
                nameof(AssortedFixesGameOverPrefix_Guard));
            if (guardMethod == null)
            {
                Debug.LogWarning("[RMR Compat] AssortedFixes title-return guard method not found.");
                return;
            }

            HarmonyMethod guard = new HarmonyMethod(guardMethod)
            {
                priority = Priority.First
            };
            HashSet<MethodInfo> targets = new HashSet<MethodInfo>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    Type type = assembly.GetType(AssortedFixesTypeName, false);
                    MethodInfo target = type?.GetMethod(
                        AssortedFixesGameOverPrefixName,
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                        null,
                        Type.EmptyTypes,
                        null);
                    if (target != null)
                        targets.Add(target);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("[RMR Compat] AssortedFixes scan failed for "
                        + assembly.GetName().Name + ": " + ex.Message);
                }
            }

            int patched = 0;
            foreach (MethodInfo target in targets)
            {
                try
                {
                    harmony.Patch(target, prefix: guard);
                    patched++;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("[RMR Compat] Could not guard "
                        + target.DeclaringType?.Assembly.FullName + ": " + ex.Message);
                }
            }
            Debug.Log("[RMR Compat] AssortedFixes title-return guards installed=" + patched + ".");
        }

        private static bool AssortedFixesGameOverPrefix_Guard()
        {
            try
            {
                BattleCamManager camera = SingletonBehavior<BattleCamManager>.Instance;
                if (camera != null)
                    return true;

                if (!_missingBattleCameraLogged)
                {
                    _missingBattleCameraLogged = true;
                    Debug.LogWarning("[RMR Compat] Skipped AssortedFixes GameOver cleanup because BattleCamManager is absent; vanilla title return will continue.");
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[RMR Compat] AssortedFixes GameOver guard failed safely: " + ex.Message);
                return false;
            }
        }
    }
}
