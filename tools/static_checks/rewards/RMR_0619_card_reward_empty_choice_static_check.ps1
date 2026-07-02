$script:StaticCheckScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$script:RepoRoot = $script:StaticCheckScriptDir
while ($script:RepoRoot -and -not (Test-Path (Join-Path $script:RepoRoot 'RogueLike Mod Reborn.csproj'))) {
    $script:RepoRoot = Split-Path -Parent $script:RepoRoot
}
if (-not $script:RepoRoot) {
    throw 'Could not locate repository root for static check.'
}
Set-Location $script:RepoRoot
param(
    [string]$Root = (Split-Path -Parent $MyInvocation.MyCommand.Path)
)

$ErrorActionPreference = 'Stop'

function Read-Text([string]$Path) {
    return [System.IO.File]::ReadAllText($Path, [System.Text.Encoding]::UTF8)
}

function Assert-Contains([string]$Text, [string]$Needle, [string]$Message) {
    if ($Text.IndexOf($Needle, [System.StringComparison]::Ordinal) -lt 0) {
        throw $Message
    }
}

$cardRewardPath = Join-Path $Root 'abcdcode_LOGLIKE_MOD\MysteryModel_CardReward.cs'
$rewardingPath = Join-Path $Root 'abcdcode_LOGLIKE_MOD\RewardingModel.cs'

$cardReward = Read-Text $cardRewardPath
$rewarding = Read-Text $rewardingPath

Assert-Contains $cardReward 'HandleEmptyCardChoices' 'MysteryModel_CardReward must explicitly handle drop books that produce zero card choices.'
Assert-Contains $cardReward 'diceCardXmlInfoList == null || diceCardXmlInfoList.Count == 0' 'Card reward UI must guard null/empty PickUpCards results before showing a choice frame.'
Assert-Contains $cardReward '[RMR CardReward] Drop book produced no card choices' 'Empty combat-page reward skips must leave a searchable runtime log.'
Assert-Contains $cardReward 'LogLikeMod.UILogBattleDiceCardUI.Instance.gameObject.SetActive(false)' 'Card reward completion must hide the hover card preview before opening the next reward UI.'
Assert-Contains $cardReward 'this.RemoveCurFrame();' 'Card reward completion must remove its own frame before handing control to the reward queue.'
Assert-Contains $rewarding 'CompleteInterruptReward' 'RewardingModel must keep the interrupt reward completion entrypoint.'

Write-Host 'PASS: card reward empty-choice/static overlay guards are present.'

