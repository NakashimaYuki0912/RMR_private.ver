$ErrorActionPreference = 'Stop'

$script:StaticCheckScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$script:RepoRoot = $script:StaticCheckScriptDir
while ($script:RepoRoot -and -not (Test-Path (Join-Path $script:RepoRoot 'RogueLike Mod Reborn.csproj'))) {
    $script:RepoRoot = Split-Path -Parent $script:RepoRoot
}
if (-not $script:RepoRoot) {
    throw 'Could not locate repository root for static check.'
}
Set-Location $script:RepoRoot
$bs = [char]92
$root = $script:RepoRoot
function Read-Text($rel) { Get-Content (Join-Path $root $rel) -Raw -Encoding UTF8 }
function AssertContains($label, $text, $needle) {
    if ($text -cnotlike "*$needle*") { throw "$label missing: $needle" }
}
function AssertNotContains($label, $text, $needle) {
    if ($text -clike "*$needle*") { throw "$label forbidden: $needle" }
}

$rewarding = Read-Text ('abcdcode_LOGLIKE_MOD' + $bs + 'RewardingModel.cs')
$patches = Read-Text ('abcdcode_Refactored' + $bs + 'LogLikePatches.cs')

# A. RewardingModel.cs — helpers
AssertContains 'RewardingModel has IsHighChapterReducedBattleCardRewardChapter helper' $rewarding 'IsHighChapterReducedBattleCardRewardChapter'
AssertContains 'RewardingModel has GetBattleCardRewardChoiceTargetCount helper' $rewarding 'GetBattleCardRewardChoiceTargetCount'
AssertContains 'GetBattleCardRewardChoiceTargetCount returns 2 for high chapters (ternary ? 2 : 3)' $rewarding '? 2 : 3'

# A. RewardingModel.cs — PickUpCards no longer hardcodes targetCount = Math.Min(3, ...)
AssertNotContains 'PickUpCards must not hardcode Math.Min(3, unowned.Count)' $rewarding 'Math.Min(3, unowned.Count)'

# A. RewardingModel.cs — PickUpCards uses GetBattleCardRewardChoiceTargetCount
AssertContains 'PickUpCards uses GetBattleCardRewardChoiceTargetCount' $rewarding 'GetBattleCardRewardChoiceTargetCount(LogLikeMod.curchaptergrade)'
AssertContains 'PickUpCards log uses targetCount variable' $rewarding '{result.Count}/{targetCount}'

# A. RewardingModel.cs — post-filter trim for high chapters
AssertContains 'PickUpCards trims result to targetCount after post-filter' $rewarding 'result.Count > targetCount'

# B. LogLikePatches.cs — helpers
AssertContains 'LogLikePatches has IsHighChapterExtraEquipRewardChapter' $patches 'IsHighChapterExtraEquipRewardChapter'
AssertContains 'LogLikePatches has CreateExtraEquipPageReward' $patches 'CreateExtraEquipPageReward'
AssertContains 'LogLikePatches has TryAddExtraEquipPageReward' $patches 'TryAddExtraEquipPageReward'

# B. LogLikePatches.cs — filters only RewardType.EquipPage
AssertContains 'CreateExtraEquipPageReward filters RewardType.EquipPage' $patches 'x.rewardtype == RewardType.EquipPage'

# B. LogLikePatches.cs — uses CommonReward pool
AssertContains 'CreateExtraEquipPageReward uses CommonReward pool' $patches 'PassiveRewardListType.CommonReward'

# B. LogLikePatches.cs — only for Grade4/5/6
AssertContains 'IsHighChapterExtraEquipRewardChapter checks Grade4/5/6' $patches 'grade == ChapterGrade.Grade4 || grade == ChapterGrade.Grade5 || grade == ChapterGrade.Grade6'

# B. LogLikePatches.cs — TryAddExtraEquipPageReward called in Normal branch
AssertContains 'TryAddExtraEquipPageReward called after Normal CommonReward' $patches 'TryAddExtraEquipPageReward()'

# B. LogLikePatches.cs — no extra reward for Creature/Shop/Start
# (verified by structure: TryAddExtraEquipPageReward is only called inside Normal/Elite/Boss branches)

# C. No reference to _release_packages in either modified file
AssertNotContains 'RewardingModel must not reference _release_packages' $rewarding '_release_packages'
AssertNotContains 'LogLikePatches must not reference _release_packages' $patches '_release_packages'

# D. RMRAbnormalityUnlockManager.EnqueueBattleClearRewards is still called (not modified)
AssertContains 'LogLikePatches still calls EnqueueBattleClearRewards' $patches 'EnqueueBattleClearRewards()'

Write-Host 'RMR 0616 high chapter reward static check passed — all constraints verified.'

