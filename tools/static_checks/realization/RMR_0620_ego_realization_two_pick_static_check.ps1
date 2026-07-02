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
function Assert-Contains([string]$Path, [string]$Pattern, [string]$Message) {
    $text = Get-Content -Raw -Encoding UTF8 $Path
    if ($text -notmatch $Pattern) { throw $Message }
}

function Assert-NotContains([string]$Path, [string]$Pattern, [string]$Message) {
    $text = Get-Content -Raw -Encoding UTF8 $Path
    if ($text -match $Pattern) { throw $Message }
}

Assert-Contains 'abcdcode_LOGLIKE_MOD\LogueBookModels.cs' 'AtlasUnlockedEgoPages' 'EGO must have a dedicated permanent unlock set.'
Assert-Contains 'abcdcode_LOGLIKE_MOD\LogueBookModels.cs' 'data\.AddData\("EgoPages"' 'Dedicated EGO unlocks must be persisted.'
Assert-Contains 'abcdcode_LOGLIKE_MOD\LogueBookModels.cs' 'IsAtlasEgoPageUnlocked' 'Dedicated EGO ownership query is missing.'
Assert-Contains 'abcdcode_LOGLIKE_MOD\RewardingModel.cs' 'TryStartUnlockedEgoChoice' 'Unlocked EGO pages must enter the emotion-level EGO selection flow.'
Assert-Contains 'abcdcode_Refactored\LogLikePatches.cs' 'RecordAtlasEgoPage\(egoCard\.CardId\)' 'EGO reward selection must record a dedicated EGO unlock.'
Assert-NotContains 'abcdcode_Refactored\LogLikePatches.cs' 'AddCard\(egoCard\.CardId\)' 'EGO reward must not be added to normal combat-page inventory.'
Assert-Contains 'abcdcode_Refactored\LogLikePatches.cs' 'LogLikeMod\.rewards\.Add\(reward\)' 'Each dropped book copy must remain a separate combat-page selection.'
Assert-Contains 'abcdcode_LOGLIKE_MOD\MysteryModel_CardReward.cs' 'rewards\.Remove\(LogLikeMod\.rewards\.Find' 'Each combat-page selection must consume only one dropped book copy.'
Assert-Contains 'RMR_AbnormalityUnlocks.cs' 'floors\.IntersectWith\(CompletedRealizations\)' 'Boss rewards must use only completed floors in the exact chapter tier.'
Assert-Contains 'RMR_AbnormalityUnlocks.cs' 'floor == SephirahType\.None \|\| !floors\.Contains\(floor\)' 'Realization abnormality matching must use the configured floor mapping.'
Assert-Contains 'RMR_AbnormalityUnlocks.cs' 'RecordAtlasAbnormalityPage\(info\.id\)' 'Selected realization abnormality pages must become permanent.'
Assert-NotContains 'RMR_AbnormalityUnlocks.cs' 'EnsureCompletedRealizationRewardsRecordedToAtlas|RecordRealizationRewardsToAtlas' 'Completing a realization must open its pool, not auto-grant every reward.'

Write-Output 'PASS: EGO storage, realization-tier rewards, and per-drop-book combat-page selection rules are present.'

