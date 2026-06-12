using System.Collections.Generic;
using abcdcode_LOGLIKE_MOD;
using LOR_DiceSystem;
using UnityEngine;

namespace RogueLike_Mod_Reborn
{
    public static class RMRRealizationManager
    {
        // Vanilla realization/final abnormality stage IDs.
        public static readonly Dictionary<SephirahType, int> RealizationBossStageIds = new Dictionary<SephirahType, int>
        {
            { SephirahType.Malkuth,   201005 },
            { SephirahType.Yesod,     202005 },
            { SephirahType.Hod,       203005 },
            { SephirahType.Netzach,   204005 },
            { SephirahType.Tiphereth, 205005 },
            { SephirahType.Gebura,    206005 },
            { SephirahType.Chesed,    207005 },
            { SephirahType.Binah,     208004 },
            { SephirahType.Hokma,     209004 },
            { SephirahType.Keter,     210009 },
        };

        public static readonly Dictionary<SephirahType, string> FloorDisplayNames = new Dictionary<SephirahType, string>
        {
            { SephirahType.Malkuth,   "历史层 - Malkuth" },
            { SephirahType.Yesod,     "科技层 - Yesod" },
            { SephirahType.Hod,       "文学层 - Hod" },
            { SephirahType.Netzach,   "艺术层 - Netzach" },
            { SephirahType.Tiphereth, "自然层 - Tiphereth" },
            { SephirahType.Gebura,    "语言层 - Gebura" },
            { SephirahType.Chesed,    "社会层 - Chesed" },
            { SephirahType.Binah,     "哲学层 - Binah" },
            { SephirahType.Hokma,     "宗教层 - Hokma" },
            { SephirahType.Keter,     "总类层 - Keter" },
        };

        public static bool InRealizationBattle { get; private set; }
        public static SephirahType CurrentRealizationFloor { get; private set; }

        public static void StartRealizationBattle(SephirahType floor)
        {
            if (!RealizationBossStageIds.TryGetValue(floor, out int stageIdNum))
            {
                Debug.LogError($"[RMRRealizationManager] No stage ID for floor: {floor}");
                return;
            }

            LorId stageId = new LorId(string.Empty, stageIdNum);
            var stageInfo = Singleton<StagesXmlList>.Instance.GetStageInfo(stageId);
            if (stageInfo == null)
            {
                stageId = new LorId(LogLikeMod.ModId, stageIdNum);
                stageInfo = Singleton<StagesXmlList>.Instance.GetStageInfo(stageId);
            }
            if (stageInfo == null)
            {
                Debug.LogError($"[RMRRealizationManager] Stage {stageIdNum} not found in vanilla or mod stage lists.");
                return;
            }

            InRealizationBattle = true;
            CurrentRealizationFloor = floor;
            LogLikeMod.rewards.Clear();
            LogLikeMod.rewards_passive.Clear();

            // Realizations are creature content, not route-ending bosses.
            LogLikeMod.SetNextStage(stageId, abcdcode_LOGLIKE_MOD.StageType.Creature, NextStageSetType.Custom);
            Debug.Log($"[RMRRealizationManager] Queued realization battle: {floor} (stage {stageIdNum})");
        }

        public static void OnRealizationBattleEnded(bool victory)
        {
            if (!InRealizationBattle)
                return;

            if (victory)
            {
                RMRAbnormalityUnlockManager.CompleteFloorRealization(CurrentRealizationFloor);
                Debug.Log($"[RMRRealizationManager] Floor realization completed: {CurrentRealizationFloor}");
            }

            InRealizationBattle = false;
            CurrentRealizationFloor = SephirahType.Keter;
        }
    }
}
