$ErrorActionPreference = "Stop"

$script:StaticCheckScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$script:RepoRoot = $script:StaticCheckScriptDir
while ($script:RepoRoot -and -not (Test-Path (Join-Path $script:RepoRoot 'RogueLike Mod Reborn.csproj'))) {
    $script:RepoRoot = Split-Path -Parent $script:RepoRoot
}
if (-not $script:RepoRoot) {
    throw 'Could not locate repository root for static check.'
}
Set-Location $script:RepoRoot
$repoRoot = $script:RepoRoot
$source = Get-Content -LiteralPath (Join-Path $repoRoot 'RMR_AbnormalityUnlocks.cs') -Raw
$shopBase = Get-Content -LiteralPath (Join-Path $repoRoot 'abcdcode_LOGLIKE_MOD\ShopBase.cs') -Raw
$rewarding = Get-Content -LiteralPath (Join-Path $repoRoot 'abcdcode_LOGLIKE_MOD\RewardingModel.cs') -Raw

foreach ($needle in @(
    'public static void CompleteFloorRealization(SephirahType floor)',
    'IsRealizationRewardAvailable(RewardPassiveInfo info)',
    'GetShopEligibleAbnormalityPages',
    'GetUnlockedRealizationEgoCardsForRewards',
    'SingingMachine1',
    'Butterfly3',
    'freischutz3',
    '910011', '910012', '910013', '910014', '910015'
)) {
    if ($source -notlike "*$needle*") { throw "RMR_AbnormalityUnlocks.cs missing: $needle" }
}

$completeStart = $source.IndexOf('public static void CompleteFloorRealization')
$completeEnd = $source.IndexOf('public static HashSet<SephirahType> GetFloorsForChapter', $completeStart)
if ($completeStart -lt 0 -or $completeEnd -lt 0) { throw 'CompleteFloorRealization block not found.' }
$completeBlock = $source.Substring($completeStart, $completeEnd - $completeStart)
if ($completeBlock -like '*UnlockPage(*') { throw 'CompleteFloorRealization must mark completion only; it must not directly grant pages.' }
if ($completeBlock -notlike '*SaveRealizationProgress()*') { throw 'CompleteFloorRealization must save realization completion.' }

if ($shopBase -notlike '*GetShopEligibleAbnormalityPages*') { throw 'Shop abnormality page pool must use unlock-manager gating.' }
if ($shopBase -notlike '*CreateShop_EgoPages*') { throw 'Shop must reserve/create EGO page goods.' }
if ($source -notlike '*EnqueueRealizationEgoSelection*' -or $source -notlike '*egoSelectionQueue*') {
    throw 'Realization EGO rewards must use the independent EGO selection queue.'
}
if ($rewarding -notlike '*egoSelectionQueue*' -or $rewarding -notlike '*HasQueuedEgoSelections*') {
    throw 'Reward flow must process and wait for queued realization EGO selections.'
}

$pickTable = Get-Content -LiteralPath (Join-Path $repoRoot 'SpecialStaticInfo\RewardPassiveInfos\CreatureInfo_PickTable.xml') -Raw
foreach ($script in @('SingingMachine1','Butterfly3','freischutz3')) {
    if ($pickTable -notmatch [regex]::Escape('Script="' + $script + '"')) {
        throw "Missing Yesod realization reward page script: $script"
    }
}

Write-Host 'RMR realization unlock static check passed.'

