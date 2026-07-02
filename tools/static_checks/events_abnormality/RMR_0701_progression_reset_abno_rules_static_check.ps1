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
$root = $script:RepoRoot

function Read-Text($relativePath) {
    [System.IO.File]::ReadAllText((Join-Path $root $relativePath), [System.Text.Encoding]::UTF8)
}

function Read-Xml($relativePath) {
    [xml](Read-Text $relativePath)
}

function Require-Contains($content, $pattern, $label) {
    if ($content -notmatch [regex]::Escape($pattern)) {
        throw "Missing ${label}: $pattern"
    }
}

function Require-True($condition, $message) {
    if (-not $condition) {
        throw $message
    }
}

$bookModels = Read-Text "abcdcode_LOGLIKE_MOD\LogueBookModels.cs"
Require-Contains $bookModels "RedMistChallengeAppearanceChance = 0.3f" "Grade6 Red Mist route chance"
Require-Contains $bookModels "ApplyGrade6RedMistAppearanceRoll(allReceptions, ref stageLimits, chapter)" "Grade6 Red Mist route roll hook"
Require-Contains $bookModels "UnityEngine.Random.value <= RedMistChallengeAppearanceChance" "Grade6 Red Mist route random gate"
Require-Contains $bookModels "allReceptions.Remove(redMist)" "Grade6 Red Mist route removal"
Require-Contains $bookModels "stageLimits.Elite = 0" "Grade6 Red Mist elite slot removal"
Require-Contains $bookModels "stageLimits.Normal += 1" "Grade6 Red Mist normal replacement"

$router = Read-Text "RMR_AbnormalityBattleRouter.cs"
Require-Contains $router "RegularAbnormalityStageIds" "regular abnormality stage pool"
Require-Contains $router "GetMaxLibrariansForChapter" "chapter librarian-count gate"
Require-Contains $router "GetRequiredLibrarianCount" "abnormality required librarian count"
Require-Contains $router "GetRequiredLibrarianCount(id) <= maxLibrarians" "candidate filter by required librarians"
Require-Contains $router "stage.mapInfo != null" "abnormality stage mapInfo crash guard"
Require-True ($router -notmatch "LowTierStageIds") "Abnormality router must not use old low-tier floor buckets."
Require-True ($router -notmatch "MidTierStageIds") "Abnormality router must not use old mid-tier floor buckets."
Require-True ($router -notmatch "HighTierStageIds") "Abnormality router must not use old high-tier floor buckets."
foreach ($stageId in @("202001", "208003", "210004")) {
    Require-Contains $router $stageId "abnormality stage $stageId"
}

$core = Read-Text "RMR_Core.cs"
Require-Contains $core "DistortedEnsembleStageClearedSaveName" "Distorted Ensemble persistent flag"
Require-Contains $core "RecordDistortedEnsembleStageClear" "Distorted Ensemble first-clear writer"
Require-Contains $core "IsDistortedEnsembleUnlockedForUrbanStar" "Distorted Ensemble Urban Star gate"
Require-Contains $core "EnsureBlueReverberationRewardsForUrbanStar" "Blue Reverberation Urban Star grant"
Require-Contains $core "BlueReverberationCorePageId = 250013" "Blue Reverberation core id"
foreach ($cardId in @("604021", "604022", "604023", "604024", "604025")) {
    Require-Contains $core $cardId "Blue Reverberation battle page $cardId"
}
Require-Contains $core "ResetAllArchiveProgress" "initial-event reset entry"
Require-Contains $core "RMRRealizationManager.SetInitialRelicEntryAvailable(true);" "regular RMR start realization entry gate"
Require-Contains $core "ClearSimpleFlag(BlackSilenceStageClearedSaveName" "Black Silence reset"
Require-Contains $core "ClearSimpleFlag(DistortedEnsembleStageClearedSaveName" "Distorted Ensemble reset"
Require-Contains $core "AtlasUnlockedRoleBooks.Clear()" "atlas role reset"
Require-Contains $core "AtlasUnlockedBattleCards.Clear()" "atlas battle-card reset"
Require-Contains $core "AtlasUnlockedAbnormalityPages.Clear()" "atlas abnormality reset"
Require-Contains $core "AtlasUnlockedEgoPages.Clear()" "atlas ego reset"

$unlock = Read-Text "RMR_AbnormalityUnlocks.cs"
Require-Contains $unlock "ResetAllPermanentProgress" "abnormality permanent reset"
Require-Contains $unlock "DeleteSaveFile(RealizationSaveName)" "realization save reset"
Require-Contains $unlock "DeleteSaveFile(RedMistVictorySaveName)" "Red Mist save reset"
Require-Contains $unlock "RMRCore.RecordDistortedEnsembleStageClear()" "Distorted Ensemble victory persistence"

$startInfo = Read-Xml "SpecialStaticInfo\MysteryXmlInfos\RMR_chstart.xml"
$frames = @($startInfo.MysteryXmlRoot.Mystery.Frame)
foreach ($frameId in @("0", "1", "2", "3", "4")) {
    Require-True ($frames | Where-Object { $_.ID -eq $frameId }) "RMR start mystery must include frame $frameId."
}
$frame0Choices = @($frames | Where-Object { $_.ID -eq "0" } | ForEach-Object { $_.Choice })
foreach ($choiceId in @("6", "7", "8")) {
    Require-True ($frame0Choices | Where-Object { $_.ID -eq $choiceId }) "RMR start frame 0 must include choice $choiceId."
}

$legacyStartInfo = Read-Xml "SpecialStaticInfo\MysteryXmlInfos\chstart.xml"
$legacyMystery = @($legacyStartInfo.MysteryXmlRoot.Mystery) | Where-Object { $_.ID -eq "-1" }
Require-True $legacyMystery "legacy chstart mystery -1 must exist."
$legacyFrames = @($legacyMystery.Frame)
foreach ($frameId in @("0", "1", "2", "3", "4")) {
    Require-True ($legacyFrames | Where-Object { $_.ID -eq $frameId }) "legacy start mystery must include frame $frameId."
}
$legacyFrame0Choices = @($legacyFrames | Where-Object { $_.ID -eq "0" } | ForEach-Object { $_.Choice })
foreach ($choiceId in @("5", "6", "7", "8")) {
    Require-True ($legacyFrame0Choices | Where-Object { $_.ID -eq $choiceId }) "legacy start frame 0 must include choice $choiceId."
}

$events = Read-Text "RMR_MysteryEvents.cs"
Require-Contains $events "case 7:" "reset choice handler"
Require-Contains $events "RMRCore.ResetAllArchiveProgress()" "reset confirmation action"
Require-Contains $events "case 8:" "gameplay guide choice handler"
Require-Contains $events "this.SwapFrame(3)" "gameplay guide frame swap"

$legacyEvents = Read-Text "abcdcode_LOGLIKE_MOD\MysteryModel_ChStart.cs"
Require-Contains $legacyEvents "public void Event5()" "legacy Roadless Camelot start event"
Require-Contains $legacyEvents "public void Event7()" "legacy reset choice event"
Require-Contains $legacyEvents "public void Event8()" "legacy gameplay guide choice event"
Require-Contains $legacyEvents "RMRCore.ResetAllArchiveProgress()" "legacy reset confirmation action"

Require-True ($bookModels -notmatch 'LoadData\("Lastest"\)') "Permanent atlas load must not repopulate from Lastest route save."
Require-True ($bookModels -notmatch 'latest\?\.GetData\("LogueBookModel"\)') "Permanent atlas load must not migrate from Lastest route inventory."

$realization = Read-Text "RMR_RealizationManager.cs"
Require-Contains $realization "PendingRealizationBattle = true;" "realization pending flag before preparation UI"
Require-Contains $realization "if (!ApplyAtlasOnlyLoadout())" "atlas-only loadout failure guard"
Require-Contains $realization "ForceRealizationAvailableUnits(stageClassInfo, 5)" "realization available unit override before stage init"
Require-Contains $realization "ForceRealizationAvailableUnits(stageModel?.ClassInfo, 5)" "realization available unit override after stage init"

foreach ($locale in @("cn", "en", "kr")) {
    $mysteryLoc = Read-Xml "Localize\$locale\MysteryEvents\RMR_chstart.xml"
    $locFrames = @($mysteryLoc.RogueMysteryXmlRoot.Mystery.Frame)
    foreach ($frameId in @("2", "3", "4")) {
        Require-True ($locFrames | Where-Object { $_.ID -eq $frameId }) "$locale start mystery localization must include frame $frameId."
    }
    $startText = Read-Xml "Localize\$locale\Mystery_Start.txt"
    $ids = @($startText.localize.text | ForEach-Object { $_.id })
    foreach ($id in @("MysteryChStartChoice5", "MysteryChStartChoice7", "MysteryChStartChoice8")) {
        Require-True ($ids -contains $id) "$locale Mystery_Start must include $id."
    }
}

"RMR 0701 PROGRESSION RESET ABNO RULES STATIC CHECK PASSED"
