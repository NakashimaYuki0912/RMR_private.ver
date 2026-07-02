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
$unlockPath = Join-Path $root 'RMR_AbnormalityUnlocks.cs'
$rewardingPath = Join-Path $root 'abcdcode_LOGLIKE_MOD\RewardingModel.cs'
$patchesPath = Join-Path $root 'abcdcode_Refactored\LogLikePatches.cs'
$cardRewardPath = Join-Path $root 'abcdcode_LOGLIKE_MOD\MysteryModel_CardReward.cs'

$unlock = Read-Text $unlockPath
$rewarding = Read-Text $rewardingPath
$patches = Read-Text $patchesPath
$cardReward = Read-Text $cardRewardPath

Assert-NotContains 'Boss realization reward comments must not bypass completion gate' $unlock 'do NOT depend|Does NOT require floor completion|bypass the realization gate'
Assert-Contains 'Boss realization reward pool is filtered to completed floors by tier helper' $unlock 'GetCompletedRealizationFloorsForBossTier\s*\(\s*grade\s*\)'
Assert-Contains 'Boss realization tier helper intersects exact-tier floors with completed floors' $unlock 'floors\.IntersectWith\(CompletedRealizations\)'
Assert-Contains 'Boss realization reward skips cleanly when no completed candidate floor exists' $unlock 'completedFloors\.Count\s*==\s*0'
Assert-Contains 'Boss EGO selection only uses completed realization floors' $unlock 'EnqueueRealizationEgoSelection\s*\(\s*completedFloors\s*\)'

Assert-Contains 'Every enemy drop book is queued without type deduplication' $patches 'QueueDropBookReward\s*\([\s\S]*LogLikeMod\.rewards\.Add\(reward\)'
Assert-Contains 'Drop book normalization retains about seventy percent of each real reward group' $rewarding 'BattleCardRewardRetentionRate\s*=\s*0\.7[\s\S]*GroupBy\(reward\s*=>\s*reward\.id\)'
Assert-Contains 'Combat page reward selection consumes one drop-book copy' $cardReward 'rewards\.Remove\(LogLikeMod\.rewards\.Find'

Assert-Contains 'Skip button is rebound on every reward UI initialization' $patches 'LogLikeMod\.skipPanel\.onClick\.RemoveAllListeners\s*\(\s*\)'
Assert-Contains 'Skip button uses the current LevelUpUI context' $patches 'SkipCurrentRewardSelection\s*\(\s*self\s*\)'
Assert-Contains 'Skip handler advances the reward queue after consuming current reward' $patches 'RewardingModel\.StartPickReward\s*\(\s*\)'
Assert-Contains 'Reward clear completion waits for queued EGO selections' $rewarding 'HasQueuedEgoSelections\s*\(\s*\)'
Assert-Contains 'Reward clear completion waits for queued mystery rewards' $rewarding 'HasQueuedMysteryRewards\s*\(\s*\)'

Write-Host 'PASS: reward queue static checks completed.'

