// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.DiceCardAbility_cryingChildPenaltyLog
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class DiceCardAbility_cryingChildPenaltyLog : DiceCardAbilityBase
    {
        public override void OnLoseParrying()
        {
            if (Singleton<StageController>.Instance.EnemyStageManager is EnemyTeamStageManager_TheCryingLog enemyStageManager)
                enemyStageManager.SetAllWeak();
            this.owner.battleCardResultLog?.SetAfterActionEvent((BattleCardBehaviourResult.BehaviourEvent)(() =>
            {
                BattleCamManager instance1 = SingletonBehavior<BattleCamManager>.Instance;
                CameraFilterPack_TV_BrokenGlass r1 = (instance1 != null ? instance1.EffectCam.gameObject.AddComponent<CameraFilterPack_TV_BrokenGlass>() : (CameraFilterPack_TV_BrokenGlass)null) ?? (CameraFilterPack_TV_BrokenGlass)null;
                if (r1 != null)
                {
                    r1.Broken_High = 20f;
                    r1.Broken_Big = 2f;
                    r1.StartCoroutine(this.BrokenGlassRoutine(r1));
                    BattleCamManager instance2 = SingletonBehavior<BattleCamManager>.Instance;
                    AutoScriptDestruct autoScriptDestruct = (instance2 != null ? instance2.EffectCam.gameObject.AddComponent<AutoScriptDestruct>() : (AutoScriptDestruct)null) ?? (AutoScriptDestruct)null;
                    if (autoScriptDestruct != null)
                    {
                        autoScriptDestruct.targetScript = (MonoBehaviour)r1;
                        autoScriptDestruct.time = 2f;
                    }
                }
                BattleCamManager instance3 = SingletonBehavior<BattleCamManager>.Instance;
                CameraFilterPack_FX_EarthQuake r2 = (instance3 != null ? instance3.EffectCam.gameObject.AddComponent<CameraFilterPack_FX_EarthQuake>() : (CameraFilterPack_FX_EarthQuake)null) ?? (CameraFilterPack_FX_EarthQuake)null;
                if (!(r2 != null))
                    return;
                r2.StartCoroutine(this.EarthQuakeRoutine(r2));
                BattleCamManager instance4 = SingletonBehavior<BattleCamManager>.Instance;
                AutoScriptDestruct autoScriptDestruct1 = (instance4 != null ? instance4.EffectCam.gameObject.AddComponent<AutoScriptDestruct>() : (AutoScriptDestruct)null) ?? (AutoScriptDestruct)null;
                if (autoScriptDestruct1 != null)
                {
                    autoScriptDestruct1.targetScript = (MonoBehaviour)r2;
                    autoScriptDestruct1.time = 0.5f;
                }
            }));
        }

        public IEnumerator EarthQuakeRoutine(CameraFilterPack_FX_EarthQuake r)
        {
            float e = 0.0f;
            while ((double)e < 1.0)
            {
                e += Time.deltaTime * 2f;
                r.Speed = (float)(30.0 * (1.0 - (double)e));
                r.X = (float)(0.019999999552965164 * (1.0 - (double)e));
                r.Y = (float)(0.019999999552965164 * (1.0 - (double)e));
                yield return null;
            }
        }

        public IEnumerator BrokenGlassRoutine(CameraFilterPack_TV_BrokenGlass r)
        {
            float e = 0.0f;
            r.Broken_High = 20f;
            r.Broken_Big = 2f;
            while ((double)e < 2.0)
            {
                e += Time.deltaTime;
                if ((double)e > 1.0)
                {
                    r.Broken_High = (float)(20.0 * (1.0 - ((double)e - 1.0)));
                    r.Broken_Big = (float)(2.0 * (1.0 - ((double)e - 1.0)));
                }
                yield return null;
            }
        }
    }
}
