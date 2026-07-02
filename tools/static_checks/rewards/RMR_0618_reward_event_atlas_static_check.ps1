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
$root = $script:RepoRoot
function Read-Text([string]$relativePath) {
    Get-Content -LiteralPath (Join-Path $root $relativePath) -Raw -Encoding UTF8
}

function Assert-Contains([string]$label, [string]$text, [string]$needle) {
    if (-not $text.Contains($needle)) {
        throw "$label missing: $needle"
    }
    Write-Host "[PASS] $label"
}

function Assert-NotContains([string]$label, [string]$text, [string]$needle) {
    if ($text.Contains($needle)) {
        throw "$label forbidden: $needle"
    }
    Write-Host "[PASS] $label"
}

$rewarding = Read-Text 'abcdcode_LOGLIKE_MOD\RewardingModel.cs'
$cardReward = Read-Text 'abcdcode_LOGLIKE_MOD\MysteryModel_CardReward.cs'
$event = Read-Text 'abcdcode_LOGLIKE_MOD\MysteryModel_Mystery_Ch1_1.cs'
$atlas = Read-Text 'abcdcode_LOGLIKE_MOD\LogAtlasPanel.cs'

Assert-Contains '1. Reward flow has a centralized interrupt completion helper' $rewarding 'CompleteInterruptReward'
Assert-Contains '2. Reward completion closes the mystery' $rewarding 'EndMystery(mystery)'
Assert-Contains '3. Reward completion advances the queue' $rewarding 'StartPickReward();'
Assert-Contains '4. EGO selection uses its independent queue' $rewarding 'HasQueuedEgoSelections()'
Assert-Contains '4b. EGO selection materializes queued choices independently' $rewarding 'GetQueuedEgoRewards()'

Assert-Contains '5. Empty card reward uses the shared completion path' $cardReward 'HandleEmptyCardChoices'
Assert-Contains '6. Empty final card reward advances the queue' $cardReward 'CompleteRewardFlow()'
Assert-Contains '7. Leaving card rewards uses the same completion path' $cardReward 'LogLikeMod.rewards.Clear();'

Assert-NotContains '8. Mystery current chapter is not hardcoded to 1001' $event 'GetData(new LorId(LogLikeMod.ModId, 1001))'
Assert-NotContains '9. Mystery next chapter is not hardcoded to 2001' $event 'GetData(new LorId(LogLikeMod.ModId, 2001))'
Assert-Contains '10. Mystery derives drop ID from current chapter grade' $event 'GetPlainDropBook'
Assert-Contains '10b. Mystery queues dynamically selected plain drop books' $event 'QueuePlainDropBooks'
Assert-Contains '11. Mystery clamps next chapter to Grade7' $event 'ChapterGrade.Grade7'

Assert-Contains '12. Atlas uses the XML Artwork key' $atlas 'card.Artwork'
Assert-Contains '13. Atlas card images preserve aspect' $atlas 'preserveAspect = true'
Assert-Contains '14. Atlas has category-aware artwork layout' $atlas 'ApplyArtworkLayout'
Assert-Contains '15. Battle and EGO categories use portrait layout' $atlas 'AtlasCategory.EgoPage'

Write-Host ''
Write-Host 'RMR reward/event/atlas static check passed.'

