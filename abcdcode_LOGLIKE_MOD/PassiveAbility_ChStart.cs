// -----------------------------------------------------------------------------
// Passive ability script: PassiveAbility_ChStart
// Fires run bootstrap at first wave start (Play / Continue / Realization gate).
// Namespace/file: abcdcode_LOGLIKE_MOD/PassiveAbility_ChStart.cs
// -----------------------------------------------------------------------------
using RogueLike_Mod_Reborn;

namespace abcdcode_LOGLIKE_MOD
{
    /// <summary>
    /// Chapter-start passive: routes to <see cref="RMRCore.CurrentGamemode"/> initial event.
    /// </summary>
    public class PassiveAbility_ChStart : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (RMRCore.CurrentGamemode == null)
            {
                UnityEngine.Debug.LogWarning("[RMR] PassiveAbility_ChStart: CurrentGamemode is null — skip OnWaveStartInitialEvent.");
                return;
            }
            RMRCore.CurrentGamemode.OnWaveStartInitialEvent();
        }
    }
}
