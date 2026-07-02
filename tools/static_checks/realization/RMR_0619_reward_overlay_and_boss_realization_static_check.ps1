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
function Read-Text([string]$Path) {
    return Get-Content -Raw -Encoding UTF8 -LiteralPath $Path
}

function Assert-Contains([string]$Name, [string]$Text, [string]$Pattern) {
    if ($Text -notmatch $Pattern) {
        throw "FAIL: $Name"
    }
    Write-Host "PASS: $Name"
}

function Assert-NotContains([string]$Name, [string]$Text, [string]$Pattern) {
    if ($Text -match $Pattern) {
        throw "FAIL: $Name"
    }
    Write-Host "PASS: $Name"
}

$root = $script:RepoRoot
$mysteryBase = Read-Text (Join-Path $root 'abcdcode_LOGLIKE_MOD\MysteryBase.cs')
$unlock = Read-Text (Join-Path $root 'RMR_AbnormalityUnlocks.cs')
$rewarding = Read-Text (Join-Path $root 'abcdcode_LOGLIKE_MOD\RewardingModel.cs')
$patches = Read-Text (Join-Path $root 'abcdcode_Refactored\LogLikePatches.cs')

$removeStart = $mysteryBase.IndexOf('public void RemoveCurFrame()')
$removeEnd = $mysteryBase.IndexOf('public void DisableButton', $removeStart)
if ($removeStart -lt 0 -or $removeEnd -lt 0) {
    throw 'FAIL: RemoveCurFrame block not found'
}
$removeBlock = $mysteryBase.Substring($removeStart, $removeEnd - $removeStart)

$bossStart = $unlock.IndexOf('private static void EnqueueBossRealizationTierRewards')
$bossEnd = $unlock.IndexOf('private static List<RewardPassiveInfo> RollRealizationAbnormalityChoices', $bossStart)
if ($bossStart -lt 0 -or $bossEnd -lt 0) {
    throw 'FAIL: EnqueueBossRealizationTierRewards block not found'
}
$bossBlock = $unlock.Substring($bossStart, $bossEnd - $bossStart)

Assert-Contains 'Mystery frames are hidden immediately before Unity deferred destroy' `
    $removeBlock 'gameObject\.SetActive\s*\(\s*false\s*\)[\s\S]*UnityEngine\.Object\.Destroy\s*\(\s*gameObject\s*\)'

$cleanupStart = $patches.IndexOf('public static void HideRewardSelectionImmediately')
if ($cleanupStart -lt 0) {
    throw 'FAIL: synchronous LevelUpUI reward cleanup helper not found'
}
$cleanupEnd = $patches.IndexOf('/// <summary>', $cleanupStart)
if ($cleanupEnd -lt 0) {
    throw 'FAIL: synchronous LevelUpUI reward cleanup helper boundary not found'
}
$cleanupBlock = $patches.Substring($cleanupStart, $cleanupEnd - $cleanupStart)

$rewardStart = $rewarding.IndexOf('public static void StartPickReward()')
$rewardEnd = $rewarding.IndexOf('public static void PickEmotion', $rewardStart)
if ($rewardStart -lt 0 -or $rewardEnd -lt 0) {
    throw 'FAIL: StartPickReward block not found'
}
$rewardBlock = $rewarding.Substring($rewardStart, $rewardEnd - $rewardStart)
$rewardPreludeEnd = $rewardBlock.IndexOf('if (LogLikeMod.rewards_passive == null)')
if ($rewardPreludeEnd -lt 0) {
    throw 'FAIL: StartPickReward prelude boundary not found'
}
$rewardPrelude = $rewardBlock.Substring(0, $rewardPreludeEnd)
Assert-NotContains 'StartPickReward must not globally hide reusable LevelUpUI before choosing the next reward type' `
    $rewardPrelude 'HideRewardSelectionImmediately'

$cleanupCall = $rewardBlock.IndexOf('LogLikeRoutines.HideRewardSelectionImmediately')
$firstMysteryStart = $rewardBlock.IndexOf('Singleton<MysteryManager>.Instance.StartMystery')
if ($cleanupCall -lt 0 -or $firstMysteryStart -lt 0 -or $cleanupCall -gt $firstMysteryStart) {
    throw 'FAIL: LevelUpUI must be synchronously cleaned before StartPickReward opens any mystery UI'
}
Write-Host 'PASS: LevelUpUI is synchronously cleaned before reward mysteries open'

Assert-Contains 'Immediate reward cleanup disables the LevelUpUI root canvas' `
    $cleanupBlock 'SetRootCanvas\s*\(\s*false\s*\)'

Assert-NotContains 'Immediate reward cleanup must not deactivate reusable passive reward candidates' `
    $cleanupBlock 'candidates'

Assert-NotContains 'Immediate reward cleanup must not deactivate reusable EGO reward slots' `
    $cleanupBlock 'egoSlotList'

Assert-Contains 'Reward flow diagnostics are gated by additional logging config' `
    $rewardBlock 'RMRCore.provideAdditionalLogging'

Assert-Contains 'Reward flow diagnostics use a stable marker' `
    $rewardBlock '[RMR RewardFlow]'

Assert-Contains 'Boss realization rewards refresh completed floor save before filtering' `
    $bossBlock 'LoadRealizationProgress\s*\(\s*\)[\s\S]*HashSet<SephirahType>\s+completedFloors'

Assert-Contains 'Boss realization candidate floors are derived by chapter tier' `
    $bossBlock 'GetCompletedRealizationFloorsForBossTier\s*\(\s*grade\s*\)'

Assert-Contains 'Boss realization chapter tier uses only completed floors in the boss reward band' `
    $unlock 'HashSet<SephirahType>\s+floors\s*=\s*GetBossRealizationRewardFloorsForChapter\(grade\)[\s\S]*floors\.IntersectWith\(CompletedRealizations\)'

Assert-Contains 'Low tier floor helper includes Yesod' `
    $unlock 'SephirahType\.Malkuth,\s*SephirahType\.Yesod,\s*SephirahType\.Hod,\s*SephirahType\.Netzach'

Assert-Contains 'Urban Star boss realization floor helper uses Binah Hokma Chesed' `
    $unlock 'return\s+new\s+HashSet<SephirahType>\s*\{\s*SephirahType\.Binah,\s*SephirahType\.Hokma,\s*SephirahType\.Chesed\s*\}'

Assert-NotContains 'Boss realization reward code must not use old hard-coded low-tier candidate set' `
    $bossBlock 'candidateFloors\s*=\s*new\s+HashSet<SephirahType>[\s\S]*SephirahType\.Netzach[\s\S]*SephirahType\.Malkuth[\s\S]*SephirahType\.Binah[\s\S]*SephirahType\.Hod'

Write-Host 'PASS: reward overlay and boss realization static checks completed.'

