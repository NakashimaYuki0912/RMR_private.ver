using System.Collections.Generic;
using System.Linq;
using abcdcode_LOGLIKE_MOD;
using UnityEngine;

namespace RogueLike_Mod_Reborn
{
    public static class RMRAbnormalityBattleRouter
    {
        private static readonly int[] LowTierStageIds =
        {
            201001, 201002, 201003, 201004,
            202001, 202002, 202003, 202004,
            203001, 203002, 203003, 203004,
            204001, 204002, 204003, 204004
        };

        private static readonly int[] MidTierStageIds =
        {
            205001, 205002, 205003, 205004,
            206001, 206002, 206003, 206004,
            207001, 207002, 207003, 207004
        };

        private static readonly int[] HighTierStageIds =
        {
            210001, 210002, 210003, 210004,
            208001, 208002, 208003,
            209001, 209002, 209003
        };

        public static IReadOnlyList<int> GetCandidateStageIds(ChapterGrade grade)
        {
            if (grade == ChapterGrade.Grade4 || grade == ChapterGrade.Grade5)
                return MidTierStageIds;
            if (grade >= ChapterGrade.Grade6)
                return HighTierStageIds;
            return LowTierStageIds;
        }

        public static StageClassInfo PickStageForChapter(ChapterGrade grade)
        {
            List<StageClassInfo> candidates = GetCandidateStageIds(grade)
                .Select(id => {
                    StageClassInfo info = GetVanillaStage(id);
                    if (info == null)
                        Debug.Log($"[RMR AbnoRoute] Vanilla stage {id} not found in StageClassInfoList.");
                    return info;
                })
                .Where(IsUsableCreatureStage)
                .ToList();

            Debug.Log($"[RMR AbnoRoute] chapter={grade}, candidateCount={candidates.Count}");

            if (candidates.Count == 0)
            {
                Debug.Log($"[RMR AbnoRoute] No usable abnormality stages for chapter {grade}. Will return null.");
                return null;
            }

            StageClassInfo picked = RandomUtil.SelectOne(candidates);
            Debug.Log($"[RMR AbnoRoute] picked={picked?.id}, type={picked?.stageType}, waves={picked?.waveList?.Count}");
            return picked;
        }

        private static bool IsUsableCreatureStage(StageClassInfo stage)
        {
            return stage != null
                && stage.waveList != null
                && stage.waveList.Count > 0
                && stage.stageType == StageType.Creature;
        }

        private static StageClassInfo GetVanillaStage(int id)
        {
            StageClassInfo info = Singleton<StageClassInfoList>.Instance.GetData(id);
            if (info != null)
                return info;

            string[] packageIds = { string.Empty, "@origin", "BaseGame" };
            foreach (string packageId in packageIds)
            {
                info = Singleton<StageClassInfoList>.Instance.GetData(new LorId(packageId, id));
                if (info != null)
                    return info;
            }
            return null;
        }
    }
}
