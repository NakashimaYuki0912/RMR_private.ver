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
    Get-Content -Raw -Encoding UTF8 $Path
}

function Must([bool]$Condition, [string]$Message) {
    if (-not $Condition) {
        throw "FAIL: $Message"
    }
}

$patches = Read-Text 'abcdcode_Refactored\LogLikePatches.cs'
$realization = Read-Text 'RMR_RealizationManager.cs'
$realizationPanel = Read-Text 'abcdcode_LOGLIKE_MOD\LogRealizationPanel.cs'
$abnormality = Read-Text 'RMR_AbnormalityUnlocks.cs'
$rewarding = Read-Text 'abcdcode_LOGLIKE_MOD\RewardingModel.cs'
$loader = Read-Text 'abcdcode_LOGLIKE_MOD\LogLikeMod.cs'
$upgrade = Read-Text 'abcdcode_LOGLIKE_MOD\ShopGoods_CardUpgrade.cs'

Must ($patches -match 'GetOrCreateRealizationButtonLabel') 'Realization button does not own a stable visible label.'
Must ($realizationPanel -match 'TryHandleBack') 'Realization panel cannot consume the BattleSetting back action safely.'
Must ($realization -match 'ForceReturnAsDefeatPending') 'Realization completion is not routed through the vanilla defeat-return path.'
Must ($patches -match 'StageController_GameOver[\s\S]*ForceReturnAsDefeatPending[\s\S]*iswin\s*=\s*false') 'Realization GameOver is not forced to the invitation-return path.'

Must ($abnormality -match 'RouteUnlockedEgoPages') 'E.G.O ownership is not separated into current-route state.'
Must ($abnormality -match 'StartNewRoute[\s\S]*RouteUnlockedEgoPages\.Clear\(\)') 'Starting a new route does not clear battle-usable E.G.O pages.'
Must ($abnormality -match 'GetUnlockedEgoChoicesForBattle[\s\S]*RouteUnlockedEgoPages\.Contains') 'Battle E.G.O choices still use permanent atlas ownership.'
Must ($patches -match 'UnlockEgoForCurrentRoute\(egoCard\.CardId\)') 'Boss reward selection does not unlock E.G.O for the current route.'

Must ($loader -match 'ApplyVanillaEmotionPresentation') 'Creature reward virtual cards do not reuse vanilla abnormality presentation data.'
Must ($rewarding -notmatch 'GetAbnormalityCard\(emotionCardXmlInfo\.Script\[0\]\)') 'Emotion reward descriptions still resolve AbnormalityCard by custom script instead of vanilla card name.'
Must ($rewarding -notmatch 'GetAbnormalityCard\(info\.script\)') 'Passive reward validation still rejects vanilla realization pages by custom script name.'

Must ($rewarding -match 'GetDropBookRewardSelectionCap') 'Drop-book reward selections have no late-chapter cap.'
Must ($rewarding -match 'GetBattleCardRewardRetentionRate') 'Drop-book retention is not chapter-aware.'
Must ($upgrade -notmatch 'this\.NameText\s*=\s*ModdingUtils\.CreateText_TMP') 'Card-upgrade shop good still renders detached title text.'

Must ($patches -match 'GetCurrentChapterDropValueRewardId') 'Enemy drop books are not normalized to the current chapter CardDropValue IDs.'
Must ($patches -match 'string\.IsNullOrEmpty\(detectedId\.packageId\)[\s\S]*detectedId\.packageId\s*==\s*"@origin"') 'Vanilla Hana drop books are not detected for normalization.'

Write-Output 'PASS: 0621 follow-up runtime regression rules are present.'

